using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    public class TopicLinked
    {
        public TopicLinked(string topic, string RollbackTopic) { this.Topic = topic; this.RollbackTopic = RollbackTopic; }
        /// <summary>
        /// 当前主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 补偿主题
        /// </summary>
        public string RollbackTopic { get; set; }
    }
}
