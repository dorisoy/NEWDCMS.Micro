using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr.HandleBuilder
{
    internal class Tools
    {
        private static Lazy<Assembly[]> Assemblies = new Lazy<Assembly[]>(() =>
        {
            var result = new List<Assembly>();
            foreach (var lib in DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Type != "referenceassembly"))
            {
                try
                {
                    result.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name)));
                }
                catch (Exception)
                {
                    //ingore
                }
            }
            return result.ToArray();
        });
        internal static Assembly[] GetProjectAssembliesArray() => Assemblies.Value;
        internal static IEnumerable<Type> GetAllTypeFromAttribute()
        {
            foreach (var assembly in GetProjectAssembliesArray())
                foreach (var type in assembly.GetTypes().Where(x => x.IsInterface && x.GetMethods().Any(y => y.GetCustomAttribute<DTPDaprLogicHandlerAttribute>() != null)))
                    yield return type;
        }
        internal static IEnumerable<(MethodInfo methodInfo, DTPDaprLogicHandlerAttribute DTPDaprAttr)> GetMethodFromAttribute(Type type)
        {
            return type.GetMethods().Where(x => x.GetCustomAttribute<DTPDaprLogicHandlerAttribute>() != null).Select(x => (x, x.GetCustomAttribute<DTPDaprLogicHandlerAttribute>()));
        }
        internal static Func<TObj, Tin, Tout> CreateMethodDelegate<TObj, Tin, Tout>(MethodInfo method)
        {
            var mParameter = Expression.Parameter(typeof(TObj), "m");
            var pParameter = Expression.Parameter(typeof(Tin), "p");
            var mcExpression = Expression.Call(mParameter, method, Expression.Convert(pParameter, typeof(Tin)));
            var reExpression = Expression.Convert(mcExpression, typeof(Tout));
            return Expression.Lambda<Func<TObj, Tin, Tout>>(reExpression, mParameter, pParameter).Compile();
        }
    }
}
