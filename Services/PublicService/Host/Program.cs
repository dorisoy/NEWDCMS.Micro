using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host;
using Host.Modules;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPCDapr.IocModule;
using RPCDapr.Mesh.Dapr;
using RPCDapr.PrRPCDaprerator.Implements;
using RPCDapr.Server.Kestrel.Implements;


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
AuthenticationHandler.RegisterAllFilter();
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
var app = builder.Build();
RPCDaprActorStartup.Configure(app, app.Services);
await app.RunAsync();