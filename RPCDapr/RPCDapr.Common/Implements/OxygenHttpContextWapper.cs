using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPCDapr.Common.Implements
{
    public class RPCDaprHttpContextWapper
    {
        public string RoutePath { get; set; }
        public ILifetimeScope RequestService { get; set; }
        public HttpContext HttpContext { get; set; }
        public RPCDaprHttpContextWapper(string routePath, ILifetimeScope requestService, HttpContext httpContext)
        {
            RoutePath = routePath;
            RequestService = requestService;
            HttpContext = httpContext;
        }
    }
}
