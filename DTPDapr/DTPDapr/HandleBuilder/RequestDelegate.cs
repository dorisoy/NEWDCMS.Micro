using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DTPDapr.HandleBuilder
{
    public class RequestDelegate<Tobj, Tin, Tout> : BaseRequestDelegate where Tin : class, new() where Tout : class
    {
        public RequestDelegate(string topic, HandleType handleType, MethodInfo method, Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            Topic = topic;
            HandleType = handleType;
            ErrorHandle = errorHandle;
            if (typeof(Tout) == typeof(Task))
                MethodDelegateNoReturn = Tools.CreateMethodDelegate<Tobj, Tin, Task>(method);
            else
                MethodDelegate = Tools.CreateMethodDelegate<Tobj, Tin, Task<Tout>>(method);
        }
        internal Func<Tobj, Tin, Task<Tout>> MethodDelegate { get; set; }
        internal Func<Tobj, Tin, Task> MethodDelegateNoReturn { get; set; }
        internal Func<IServiceProvider, ErrorModel, Task> ErrorHandle { get; set; }
        public override async Task Excute(DTPDaprData jsonData, IServiceProvider lifetimeScope)
        {
            var messageobj = JsonSerializer.Deserialize<Tin>(jsonData.Data);
            if (messageobj == default(Tin))
                throw new FormatException($"参数反序列化失败,订阅器主题{Topic},入参类型：{typeof(Tin).Name}");
            try
            {
                if (MethodDelegateNoReturn != null)
                {
                    await MethodDelegateNoReturn(lifetimeScope.GetService<Tobj>(), messageobj);
                }
                else
                {
                    var localRollbackResult = await MethodDelegate(lifetimeScope.GetService<Tobj>(), messageobj);
                    //获取下一个topic
                    if (HandleType == HandleType.Handle)
                    {
                        var next = ConfigurationManager.GetConfig().GetNextByTopic(jsonData.FlowName, jsonData.Topic);
                        if (next != null)
                            await lifetimeScope.GetService<IDTPDaprManager>().StartOrNext(next.Topic, localRollbackResult);
                    }
                    else
                    {
                        var prev = ConfigurationManager.GetConfig().GetPrevTopicByTopic(jsonData.FlowName, jsonData.Topic);
                        if (prev != null)
                            await lifetimeScope.GetService<IDTPDaprManager>().StartOrNext(prev.RollbackTopic, localRollbackResult);
                    }
                }
            }
            catch (BaseDTPDaprException e)
            {
                if (HandleType == HandleType.Handle)
                {
                    var prev = ConfigurationManager.GetConfig().GetPrevTopicByTopic(jsonData.FlowName, jsonData.Topic);
                    if (prev != null)
                        await lifetimeScope.GetService<IDTPDaprManager>().StartOrNext(prev.RollbackTopic, e.RollbackModel);
                    else
                        await ErrorHandle(lifetimeScope, new ErrorModel(jsonData.Topic, JsonSerializer.Serialize(messageobj), "", e));
                }
                else
                {
                    if (ErrorHandle != null)
                        await ErrorHandle(lifetimeScope, new ErrorModel(jsonData.Topic, JsonSerializer.Serialize(messageobj), e.RollbackModel == null ? "" : JsonSerializer.Serialize(e.RollbackModel), e));
                }
            }
            catch (Exception e)
            {
                if (ErrorHandle != null)
                    await ErrorHandle(lifetimeScope, new ErrorModel(jsonData.Topic, JsonSerializer.Serialize(messageobj), "", e));
            }
        }
    }
}
