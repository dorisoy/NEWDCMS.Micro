using RPCDapr.Client.ServerProxyFactory.Interface;
using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.Store.Dapr
{
    public class StoreProviderDaprImpl : IStoreProvider
    {
        private readonly IStateManager stateManager;
        public StoreProviderDaprImpl(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public async Task<DTPDaprData> GetKey(string key)
        {
            return await stateManager.GetState<DTPDaprData>(new DaprStateStoreDTPDaprModel<DTPDaprData>(key));
        }

        public async Task<bool> RemoveKey(string key)
        {
            await stateManager.DelState(new DaprStateStoreDTPDaprModel<DTPDaprData>(key));
            return true;
        }

        public async Task<bool> SetDataByKey(string key, DTPDaprData data, DateTime expireTime)
        {
            await stateManager.SetState(new DaprStateStoreDTPDaprModel<DTPDaprData>(key, data, (expireTime - DateTime.Now).TotalSeconds> int.MaxValue ? -1: (int)((expireTime - DateTime.Now).TotalSeconds)));
            return true;
        }
    }
}
