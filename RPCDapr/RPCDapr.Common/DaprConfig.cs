using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Common
{

    /// <summary>
    /// 用于表示Dapr配置对象单例
    /// </summary>
    public class DaprConfig
    {
        static DaprConfig Currnet { get; set; }
        public int Port { get; set; }
        public string PubSubCompentName { get; set; }
        public string StateStoreCompentName { get; set; }
        public string TracingHeaders { get; set; }
        public bool UseStaticFiles { get; set; }
        public bool UseCors { get; set; }
        public static DaprConfig GetCurrent()
        {
            if (Currnet == null)
                Currnet = new DaprConfig();
            return Currnet;
        }
    }
}
