using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    /// <summary>
    /// 补偿异常对象
    /// </summary>
    public class ErrorModel
    {
        public ErrorModel(string sourceTopic, string sourceDataJson, string errorDataJson, Exception sourceException)
        {
            SourceTopic = sourceTopic;
            SourceDataJson = sourceDataJson;
            ErrorDataJson = errorDataJson;
            SourceException = sourceException;
        }
        /// <summary>
        /// 订阅的Topic
        /// </summary>
        public string SourceTopic { get; set; }
        /// <summary>
        /// 订阅器原始json格式Data
        /// </summary>
        public string SourceDataJson { get; set; }
        /// <summary>
        /// 订阅器异常抛出json格式Data
        /// </summary>
        public string ErrorDataJson { get; set; }
        /// <summary>
        /// 原始异常
        /// </summary>
        public Exception SourceException { get; set; }
    }
}
