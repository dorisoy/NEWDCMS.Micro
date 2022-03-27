using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
	public class ConfigurationManager
	{
		private static DTPDaprConfiguration _config = new();
		public static Action<DTPDaprConfiguration> SetConfig = (cfg) => _config = cfg;
		public static Func<DTPDaprConfiguration> GetConfig = () => _config;

	}
}
