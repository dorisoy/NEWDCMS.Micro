using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    public class BaseDTPDaprException : Exception
    {
        public object RollbackModel { get; set; }
        public BaseDTPDaprException()
        {

        }
        public BaseDTPDaprException(string message) : base(message)
        {

        }
    }
    public class DTPDaprException<T> : BaseDTPDaprException
    {
        public DTPDaprException(T rollbackModel)
        {
            RollbackModel = rollbackModel;
        }
        public DTPDaprException(T rollbackModel, string message) : base(message)
        {
            RollbackModel = rollbackModel;
        }
    }
}
