using Microsoft.AspNetCore.Builder;
using RPCDapr.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCDapr.Server.Kestrel.Interface
{
    public interface IServerHandler
    {
        void BuildHandler(IApplicationBuilder app, ISerialize serialize);
    }
}
