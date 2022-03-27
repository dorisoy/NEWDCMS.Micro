using DomainBase;
using IApplicationService;
using InfrastructureBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using RPCDapr.Common.Implements;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AopHandlerProvider
    {
        public static void ContextHandler(OxygenHttpContextWapper oxygenHttpContext)
        {
            HttpContextExt.SetCurrent(oxygenHttpContext);//ע��http�����ĸ�����ҵ�������Ķ���
        }
        public static async Task BeforeSendHandler(object param, OxygenHttpContextWapper oxygenHttpContext)
        {
            await new AuthenticationHandler().AuthenticationCheck(HttpContextExt.Current.RoutePath);//��ȨУ��
            //����ǰ�����������У��
            if (param != null)
                CustomModelValidator.Valid(param);
            oxygenHttpContext.Headers.Add("AuthIgnore", "true");
            await Task.CompletedTask;
        }
        public static async Task AfterMethodInvkeHandler(object result)
        {
            await Task.CompletedTask;
        }

        public static async Task<object> ExceptionHandler(Exception exception)
        {
            //�쳣����
            if (exception is ApplicationServiceException || exception is DomainException || exception is InfrastructureException)
            {
                return await ApiResult.Err(exception.Message).Async();
            }
            else
            {
                Console.WriteLine("ϵͳ�쳣��" + exception.Message);
                return await ApiResult.Err().Async();
            }
        }
    }
}
