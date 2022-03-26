using RPCDapr.Client.ServerSymbol.Events;
using RPCDapr.Common.Implements;
using RPCDapr.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Client.ServerSymbol.Events
{
    public static class EventHandleRequestExtension
    {
        static Lazy<ISerialize> serialize = new Lazy<ISerialize>(() =>
        {
            return RPCDaprIocContainer.Resolve<ISerialize>();
        });
        public static T GetData<T>(this EventHandleRequest<T> handleRequest) where T : class
        {
            if (handleRequest == null)
                return default;
            var request = handleRequest as TempDataByEventHandleInput<T>;
            if (request.value != null)
            {
                return request.value;
            }
            else if (!string.IsNullOrEmpty(request.data))
            {
                request.value = serialize.Value.DeserializesJson<T>(request.data);
                return request.value;
            }
            return default;
        }

        public static string GetDataJson<T>(this EventHandleRequest<T> handleRequest) where T : class
        {
            if (handleRequest == null)
                return "";
            var request = handleRequest as TempDataByEventHandleInput<T>;
            if (request.data != null)
                return request.data;
            return "";
        }
    }
}
