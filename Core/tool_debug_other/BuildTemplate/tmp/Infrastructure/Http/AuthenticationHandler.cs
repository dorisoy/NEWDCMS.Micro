using Autofac;
using IApplicationService.Base.AccessToken;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using RPCDapr.Client.ServerProxyFactory.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AuthenticationHandler : AuthenticationManager
    {
        public static new void RegisterAllFilter()
        {
            AuthenticationManager.RegisterAllFilter();
        }
        public override async Task AuthenticationCheck(string routePath)
        {
            var authMethod = AuthenticationMethods.FirstOrDefault(x => x.Path.Equals(routePath));
            if (authMethod != null)
            {
                var token = HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key == "Authentication").Value;
                var accountInfo = await GetAccountInfo(HttpContextExt.Current.RequestService.Resolve<IStateManager>());
                HttpContextExt.SetUser(accountInfo);
                if (!HttpContextExt.Current.User.IgnorePermission && authMethod.CheckPermission && !HttpContextExt.Current.GetAuthIgnore() && HttpContextExt.Current.User.Permissions != null && !HttpContextExt.Current.User.Permissions.Contains(routePath))
                    throw new InfrastructureException("��ǰ��¼�û�ȱ��ʹ�øýӿڵı�ҪȨ��,������!");
            }
        }

        private async Task<CurrentUser> GetAccountInfo(IStateManager stateManager)
        {
            var token = HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key == "Authentication").Value;
            var usertoken = await stateManager.GetState<AccessTokenItem>(new AccountLoginAccessToken(token));
            if (usertoken == null)
                throw new InfrastructureException("��Ȩ��¼Token�ѹ���,�����µ�¼!");
            var userinfo = await stateManager.GetState<CurrentUser>(new AccountLoginCache(usertoken.Id));
            if (userinfo == null)
                throw new InfrastructureException("��¼�û���Ϣ�ѹ���,�����µ�¼!");
            else if (userinfo.State == 1)
                throw new InfrastructureException("��¼�û��ѱ�����,�����µ�¼!");
            if (!usertoken.LoginAdmin)
                userinfo.Permissions = null;
            return userinfo;
        }
    }
}
