using Autofac;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Data.Nest;
using RPCDapr.Client.ServerSymbol.Events;
using System.Linq;

namespace Host.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Common.GetProjectAssembliesArray()).Where(x => !new[] { "Microsoft", "System" }.Any(y => x.AssemblyQualifiedName.Contains(y)))
                .AsImplementedInterfaces().Where(x => !(x is IEventHandler))
                .InstancePerLifetimeScope();
            //�¼���������Ҫ����ע����Ϊ��ӿ���ͬ
            Common.RegisterAllEventHandlerInAutofac(Common.GetProjectAssembliesArray(), builder);
            //ע������������ʩ����
            builder.RegisterGeneric(typeof(ElasticSearchRepository<>)).As(typeof(IElasticSearchRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitofWorkManager<EfDbContext>>().As<IUnitofWork>().InstancePerLifetimeScope();
        }
    }
}
