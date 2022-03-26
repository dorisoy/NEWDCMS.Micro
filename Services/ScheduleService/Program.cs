using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using ScheduleService;
using ScheduleService.Modules;
using RPCDapr.IocModule;
using RPCDapr.Server.Kestrel.Implements;

IConfiguration Configuration = default;
var builder = RPCDaprApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
});
RPCDaprStartup.ConfigureServices(builder.Services);
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
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
GlobalConfiguration.Configuration.UseRedisStorage(Configuration.GetSection("RedisConnection").Value);
builder.Services.AddHangfire(x => { });
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<CronScheduleService>();//注册并运行所有的周期作业
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
RPCDaprStartup.Configure(app, app.Services);
await app.RunAsync();