using Domain.Repository;
using Infrastructure.EfDataAccess;
using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.Client.ServerSymbol.Events;

namespace ApplicationService
{
    public class RoleEventHandler : IEventHandler
    {
        private readonly IRoleRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public RoleEventHandler(IRoleRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
