using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host.Modules;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPCDapr.IocModule;
using RPCDapr.Mesh.Dapr;
using RPCDapr.ProxyGenerator.Implements;
using RPCDapr.Server.Kestrel.Implements;
using System.IO;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }
        static async Task Main(string[] args)
        {
            await CreateDefaultHost(args).Build().RunAsync();
        }

        static IHostBuilder CreateDefaultHost(string[] args) => new HostBuilder()
                .ConfigureWebHostDefaults(webhostbuilder =>
                {
                    //ע���Ϊoxygen����ڵ�
                    webhostbuilder.StartOxygenServer<OxygenActorStartup>((config) =>
                    {
                        config.Port = 80;
                        config.PubSubCompentName = "pubsub";
                        config.StateStoreCompentName = "statestore";
                        config.TracingHeaders = "Authentication";
                    });
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                    _configuration = config.Build();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    //ע�RPCDaprn����
                    builder.RegisterOxygenModule();
                    //ע��ҵ������
                    builder.RegisterModule(new ServiceModule());
                })
                .ConfigureServices((context, services) =>
                {
                    //ע���Զ���HostService
                    services.AddHostedService<CustomerService>();
                    //ע��ȫ��������
                    LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
                    //ע���Ȩ������
                    AuthenticationHandler.RegisterAllFilter();
                    services.AddLogging(configure =>
                    {
                        configure.AddConfiguration(_configuration.GetSection("Logging"));
                        configure.AddConsole();
                    });
                    services.AddDbContext<EfDbContext>(options => options.UseNpgsql(_configuration.GetSection("SqlConnectionString").Value));
                    services.AddAutofac();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
