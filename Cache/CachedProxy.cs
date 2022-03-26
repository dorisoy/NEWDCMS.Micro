using System.Reflection;

namespace MultilevelCache
{
    public class CachedProxy<Timpl> : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return DelegateBuilder.GetDelegate(targetMethod).Excute(args);
        }
    }
}
