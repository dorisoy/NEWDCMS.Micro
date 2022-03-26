using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    public interface IStoreProvider
    {
        Task<bool> SetDataByKey(string key, DTPDaprData data, DateTime expireTime);
        Task<bool> RemoveKey(string key);
        Task<DTPDaprData> GetKey(string key);
    }
}
