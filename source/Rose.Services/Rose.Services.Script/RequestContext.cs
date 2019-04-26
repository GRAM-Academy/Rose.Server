using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Services.Script
{
    public class RequestContext
    {
        public readonly string Url, RawUrl, RemoteIPv4, RemoteIPv6;
        public readonly HttpListenerContext HttpListenerContext;



        public RequestContext(HttpListenerContext httpListenerContext)
        {
            HttpListenerContext = httpListenerContext;

            Url = httpListenerContext.Request.Url.ToString();
            RawUrl = httpListenerContext.Request.RawUrl;

            RemoteIPv4 = httpListenerContext.Request.RemoteEndPoint.Address.MapToIPv4().ToString();
            RemoteIPv6 = httpListenerContext.Request.RemoteEndPoint.Address.MapToIPv6().ToString();
        }
    }
}
