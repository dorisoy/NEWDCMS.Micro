using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    public class ConfigurationManager
    {
        private static DTPDaprConfiguration DTPDaprConfiguration;
        public static Action<DTPDaprConfiguration> SetConfig = (DTPDaprConfiguration) => DTPDaprConfiguration = DTPDaprConfiguration;
        public static Func<DTPDaprConfiguration> GetConfig = () => DTPDaprConfiguration;
    }
}
