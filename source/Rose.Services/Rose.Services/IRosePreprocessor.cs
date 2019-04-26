using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Services
{
    public interface IRosePreprocessor
    {
        bool BeforeRequestHandling(HttpListenerContext httpContext, ref string messageBody);
        void AfterRequestHandled(HttpListenerContext httpContext);

        void BeforeResponseHandling(HttpListenerResponse httpResponse, ref string response);
    }
}
