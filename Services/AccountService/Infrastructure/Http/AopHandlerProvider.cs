using DomainBase;
using IApplicationService;
using InfrastructureBase;
using InfrastructureBase.Object;
using RPCDapr.Common.Implements;
using RPCDapr.ProxyGenerator.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Http;
using InfrastructureBase.Http;
using InfrastructureBase.AopFilter;

namespace Infrastructure.Http
{
    public class AopHandlerProvider
    {
        public static void ContextHandler(RPCDaprHttpContextWapper RPCDaprHttpContext)
        {
            HttpContextExt.SetCurrent(RPCDaprHttpContext);//注入http上下文给本地业务上下文对象
        }
        public static async Task BeforeSendHandler(object param, RPCDaprHttpContextWapper RPCDaprHttpContext)
        {
            await new AccountAuthenticationHandler().AuthenticationCheck(HttpContextExt.Current.RoutePath);//授权校验
            //方法前拦截器，入参校验
            if (param != null)
                CustomModelValidator.Valid(param);
            RPCDaprHttpContext.HttpContext.Request.Headers.Remove("AuthIgnore");
            RPCDaprHttpContext.HttpContext.Request.Headers.Add("AuthIgnore", "true");
            //自定义拦截器
            await AopFilterManager.ExcutingMethodFilter(param);
            await Task.CompletedTask;
        }
        public static async Task AfterMethodInvkeHandler(object result)
        {
            //自定义拦截器
            await AopFilterManager.ExcutedMethodFilter(result);
            await Task.CompletedTask;
        }

        public static async Task<object> ExceptionHandler(Exception exception)
        {
            //异常处理
            if (exception is ApplicationServiceException || exception is DomainException || exception is InfrastructureException)
            {
                return await ApiResult.Err(exception.Message).Async();
            }
            else
            {
                Console.WriteLine("系统异常：" + exception.Message);
                return await ApiResult.Err().Async();
            }
        }
    }
}
