using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host;
using Host.Modules;
using IApplicationService.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase.AopFilter;
using InfrastructureBase.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.IocModule;
using RPCDapr.Mesh.Dapr;
using RPCDapr.ProxyGenerator.Implements;
using RPCDapr.Server.Kestrel.Implements;
using DTPDapr;
using DTPDapr.PubSub.Dapr;
using DTPDapr.Store.Dapr;

IConfiguration Configuration = default;
var builder = RPCDaprApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseCors = true;
});
RPCDaprActorStartup.ConfigureServices(builder.Services);
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json");
    Configuration = config.Build();
}).ConfigureContainer<ContainerBuilder>(builder =>
{
    //注入RPCDapr依赖
    builder.RegisterRPCDaprModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddHttpClient();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
GoodsAuthenticationHandler.RegisterAllFilter();
//注册自定义Attribute AOP拦截器(需要注册全局拦截器才有效)
AopFilterManager.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});

//builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(Configuration.GetSection("SqlConnectionString").Value));
var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
builder.Services.AddDbContext<EfDbContext>(options => options.UseMySql(Configuration.GetSection("SqlConnectionString").Value, serverVersion));


builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDTPDapr(new DTPDaprConfiguration("RPCDapr-DCMSSample", "GoodsService", null, null, new IApplicationService.DTPDaprs.CreateOrder.Configuration()));

builder.Services.AddDTPDaprStore();

var app = builder.Build();
app.RegisterDTPDaprHandler(async (service, error) =>
{
    //当出现补偿异常的DTPDapr流时，会触发这个异常处理器，需要人工进行处理(持久化消息/告警通知等等)
    //此处作为演示，我将会直接导入到事件异常服务
    await service.GetService<IEventBus>().SendEvent(EventTopicDictionary.Common.EventHandleErrCatch,
                   new EventHandlerErrDto($"DTPDapr流[{error.SourceTopic}]事件补偿异常", error.SourceDataJson, error.SourceException.Message, error.SourceException.StackTrace, false));
});
RPCDaprActorStartup.Configure(app, app.Services);
await app.RunAsync();