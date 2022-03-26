using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{

    public abstract class TopicConfiguration
    {
        public abstract string FlowName { get; set; }
        List<(string, string)> Topics { get; set; } = new List<(string, string)>();
        LinkedList<TopicLinked> TopicLinkeds { get; set; } = new LinkedList<TopicLinked>();
        Func<TopicLinked, string, bool> checkFunc = (linked, topic) => linked.Topic == topic || linked.RollbackTopic == topic;
        public bool ExistsByTopic(string topic)
        {
            return TopicLinkeds.Any(x => checkFunc(x, topic));
        }
        public TopicLinked GetPrev(string topic)
        {
            if (ExistsByTopic(topic))
                return TopicLinkeds.Find(TopicLinkeds.First(x => checkFunc(x, topic))).Previous?.Value;
            return null;
        }
        public TopicLinked GetNext(string topic)
        {
            if (ExistsByTopic(topic))
                return TopicLinkeds.Find(TopicLinkeds.First(x => checkFunc(x, topic))).Next?.Value;
            return null;
        }
        public TopicConfiguration AddNext(string topic, string rollbackTopic = null)
        {
            if (Topics.Any(x => x == (topic, rollbackTopic)))
            {
                throw new ArgumentOutOfRangeException($"当前配置节已包含订阅主题{topic}");
            }
            else
            {
                Topics.Add((topic, rollbackTopic));
            }
            var nextTopic = new TopicLinked(topic, rollbackTopic);
            TopicLinkeds.AddLast(nextTopic);
            return this;
        }
    }
}
