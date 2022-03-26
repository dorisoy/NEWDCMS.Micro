using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;

namespace MultilevelCache
{
    public class Common
    {
        static Dictionary<MethodInfo, SystemCachedAttribute> systemCachedAttrDir = new Dictionary<MethodInfo, SystemCachedAttribute>();
        static Lazy<IEnumerable<Assembly>> Assemblies = new Lazy<IEnumerable<Assembly>>(() => DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Type != "referenceassembly").Select(lib => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name))));
        static AsyncLocal<IServiceProvider> ServiceProvider { get; set; } = new AsyncLocal<IServiceProvider>();
        internal static IServiceProvider GetServiceProvider()
        {
            if (ServiceProvider == null)
                throw new ArgumentNullException("ServiceProvider没有被实例化");
            return ServiceProvider.Value;
        }

        internal static void SetCachedAttrDir(MethodInfo method, SystemCachedAttribute attr) => systemCachedAttrDir.Add(method, attr);
        internal static SystemCachedAttribute GetCachedAttrDir(MethodInfo method) => systemCachedAttrDir.ContainsKey(method) ? systemCachedAttrDir[method] : default;
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider.Value = serviceProvider;
        }
        /// <summary>
        /// 将包含SystemCachedAttribute服务注册成为代理
        /// </summary>
        /// <returns></returns>
        internal static List<(Type tInterface, Type tImpl)> GetSystemCachedAttributeService()
        {
            var allTypes = Assemblies.Value.Select(x => x.GetTypes().AsEnumerable()).Aggregate((x, y) => x.Concat(y)).ToList();
            return allTypes.Where(x => x.IsClass && x.GetInterfaces().Any() && x.GetMethods().Any(m => m.GetCustomAttribute<SystemCachedAttribute>() != null))
                .Select(x => (x.GetInterfaces().FirstOrDefault(), x)).ToList();
        }

        internal static object DispatchProxyCreate(Type interfaceType, Type implType)
        {
            return typeof(DispatchProxy).GetMethod(nameof(DispatchProxy.Create)).MakeGenericMethod(new[] { interfaceType, typeof(CachedProxy<>).MakeGenericType(implType) }).Invoke(null, null);
        }

        internal static string GetCachedKey(MethodInfo methodInfo)
        {
            return Md5(methodInfo.Name + string.Join("", methodInfo.GetParameters().Select(x => x.Name)));
        }
        internal static string GetCachedKey(MethodInfo methodInfo, object[] args)
        {
            return Md5(methodInfo.Name + string.Join("", methodInfo.GetParameters().Select(x => x.Name)) + System.Text.Json.JsonSerializer.Serialize(args));
        }
        internal static string Md5(string content)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                string md5Str = BitConverter.ToString(result);
                return md5Str.Replace("-", "");
            }
        }
    }
}
