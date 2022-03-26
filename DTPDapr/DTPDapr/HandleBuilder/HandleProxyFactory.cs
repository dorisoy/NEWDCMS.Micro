using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.HandleBuilder
{
    /// <summary>
    /// 业务处理代理工厂
    /// </summary>
    public class HandleProxyFactory
    {
        static List<BaseRequestDelegate> delegateHandle = new List<BaseRequestDelegate>();
        public static List<BaseRequestDelegate> GetDelegate() => delegateHandle;
        public static void RegiterAllHandle(Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            //将当前服务的实现注册成委托，通过订阅器以routerkey的方式分发
            foreach (var type in Tools.GetAllTypeFromAttribute())
                foreach (var method in Tools.GetMethodFromAttribute(type))
                    delegateHandle.Add(CreateRequestDelegate(type, method.DTPDaprAttr.Topic, method.DTPDaprAttr.HandleType, method.methodInfo, errorHandle));
        }
        static Type DelegateType = typeof(RequestDelegate<,,>);
        static BaseRequestDelegate CreateRequestDelegate(Type serviceType, string Topic, HandleType handleType, MethodInfo method, Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            var inputType = method.GetParameters().FirstOrDefault()?.ParameterType ?? typeof(object);
            var outputType = method.ReturnType == typeof(Task) ? typeof(Task) : method.ReturnType.GetGenericArguments().FirstOrDefault();
            var genericdelegateType = DelegateType.MakeGenericType(serviceType, inputType, outputType);
            return Activator.CreateInstance(genericdelegateType, new object[] { Topic, handleType, method, errorHandle }) as BaseRequestDelegate;
        }
    }
}
