using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTPDapr
{
    public interface IDTPDaprManager
    {
        Task StartOrNext<T>(string topic, T data);
    }
}
