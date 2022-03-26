using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace DTPDapr
{
    public class DTPDaprData
    {
        public DTPDaprData() { }
        public DTPDaprData(string flowName, string topic, object data)
        {
            FlowName = flowName;
            Topic = topic;
            Data = JsonSerializer.SerializeToUtf8Bytes(data);
            StoreKey = Guid.NewGuid().ToString();
            StoreState = DTPDaprDataState.Processing;
        }
        public string StoreKey { get; set; }
        public DTPDaprDataState StoreState { get; set; }
        /// <summary>
        /// 流程名
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 当前主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 事件主体
        /// </summary>
        public byte[] Data { get; set; }

        public void SetState(DTPDaprDataState storeState) => StoreState = storeState;
    }
    public enum DTPDaprDataState
    {
        Processing = 0,
        Done = 1,
        Error = 2
    }
}
