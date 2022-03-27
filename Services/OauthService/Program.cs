using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Http;
using OauthService.Modules;
using RPCDapr.IocModule;
using RPCDapr.ProxyGenerator.Implements;
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
RPCDaprStartup.ConfigureServices(builder.Services);
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json");
    Configuration = config.Build();
}).ConfigureContainer<ContainerBuilder>(builder =>
{
    //×¢ÈëRPCDaprÒÀÀµ
    builder.RegisterRPCDaprModule();
    //×¢ÈëÒµÎñÒÀÀµ
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddHttpClient();
//×¢²áÈ«¾ÖÀ¹½ØÆ÷
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
RPCDaprStartup.Configure(app, app.Services);
await app.RunAsync();