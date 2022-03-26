using System.Linq.Expressions;
using System.Reflection;

namespace MultilevelCache
{
    /// <summary>
    /// 委托构造器
    /// </summary>
    internal static class DelegateBuilder
    {
        static Dictionary<string, IMethodDelegate> DelegateDir = new Dictionary<string, IMethodDelegate>();
        /// <summary>
        /// 创建方法委托实例
        /// </summary>
        /// <param name="methodInfo"></param>
        public static void CreateDelegate(MethodInfo methodInfo)
        {
            DelegateDir.Add(Common.GetCachedKey(methodInfo), BuildSenderDelegate(methodInfo));
            IMethodDelegate BuildSenderDelegate(MethodInfo methodInfo)
            {
                return (IMethodDelegate)Activator.CreateInstance(typeof(MethodDelegate<,,>).MakeGenericType(
                    new[] { methodInfo.DeclaringType, methodInfo.ReturnType, methodInfo.ReturnType.BaseType.Equals(typeof(Task)) || methodInfo.ReturnType.BaseType.Equals(typeof(ValueTask)) ? methodInfo.ReturnType.GetGenericArguments()[0] : methodInfo.ReturnType }), methodInfo);
            }
        }
        /// <summary>
        /// 获取方法委托实例
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static IMethodDelegate GetDelegate(MethodInfo methodInfo)
        {
            var key = Common.GetCachedKey(methodInfo);
            return DelegateDir.ContainsKey(key) ? DelegateDir[key] : throw new NullReferenceException();
        }
        /// <summary>
        /// 为委托实例创建委托函数
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Func<TObj, object[], Tout> CreateMethodDelegate<TObj, Tout>(MethodInfo method)
        {
            var exp = CreateLambda(method);
            return Expression.Lambda<Func<TObj, object[], Tout>>(exp.body, exp.parameters).Compile();
            (Expression body, ParameterExpression[] parameters) CreateLambda(MethodInfo methodInfo)
            {
                var mParameter = Expression.Parameter(methodInfo.ReflectedType, "m");
                var parameters = GenerateParameters(methodInfo, out var allParameters);
                var unaryParameters = new List<UnaryExpression>();
                var pindex = 1;
                methodInfo.GetParameters().ToList().ForEach(x =>
                {
                    unaryParameters.Add(Expression.Convert(Expression.Parameter(typeof(object), $"p{pindex}"), x.ParameterType));
                    pindex++;
                });
                var mcExpression = Expression.Call(mParameter, methodInfo, parameters.ToArray());
                if (unaryParameters.Count == 0)
                    mcExpression = Expression.Call(mParameter, methodInfo);
                var reExpression = Expression.Convert(mcExpression, methodInfo.ReturnType);
                return (reExpression, new[] { mParameter, allParameters });
            }
            List<Expression> GenerateParameters(MethodBase method, out ParameterExpression allParameters)
            {
                allParameters = Expression.Parameter(typeof(object[]), "params");
                ParameterInfo[] methodMarameters = method.GetParameters();
                List<Expression> parameters = new List<Expression>();
                for (int i = 0; i < methodMarameters.Length; i++)
                {
                    var indexExpr = Expression.Constant(i);
                    var item = Expression.ArrayIndex(allParameters, indexExpr);
                    var converted = Expression.Convert(item, methodMarameters[i].ParameterType);
                    parameters.Add(converted);
                }
                return parameters;
            }
        }
    }
}
