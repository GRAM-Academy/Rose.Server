using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data;
using Aegis.Data.Json;
using Aegis.Calculate;
using Aegis.Network;
using Rose.Engine;
using Newtonsoft.Json.Linq;

namespace Rose.Services
{
    public class RequestHandlerArgument
    {
        public HttpListenerContext HttpContext { get; internal set; }
        public string MessageBody { get; internal set; }
        public HighResolutionTimer ProcessingTime { get; internal set; } = HighResolutionTimer.StartNew();


        //  Response
        public delegate void ResponseHandler(JToken resultToken);
        public ResponseHandler Response { get; internal set; }


        // ResponseWithResultCode
        public delegate void ResponseWithCodeHandler(int resultCode, string message, JToken resultToken);
        public ResponseWithCodeHandler ResponseWithResultCode { get; internal set; }
    }





    public class RequestHandler : HttpRequestHandler
    {
        private DispatchMethodSelector<RequestHandlerArgument> _methodSelector;
        public TreeNode<string> Config { get; set; }
        public Encoding ContentEncoding { get; private set; }
        public int MaxDataSize
        {
            get
            {
                return Config.GetValue("maxDataSize", "10485760").ToInt32();
            }
        }





        protected RequestHandler()
        {
            _methodSelector = new DispatchMethodSelector<RequestHandlerArgument>(this, (ref RequestHandlerArgument source, out string key) =>
            {
                try
                {
                    JToken jobject = JToken.Parse(source.MessageBody);
                    key = ((string)(jobject["cmd"])).Trim();
                }
                catch (Exception)
                {
                    throw new AegisException(RoseResult.InvalidReqest);
                }

                if (_methodSelector.HasMethodWithKey(key) == false)
                    throw new AegisException(RoseResult.UnknownCommand);
            });
        }


        protected override bool PreprocessRequest(HttpRequestData request)
        {
            ContentEncoding = request.ContentEncoding;

            //  Response 데이터의 크기 확인
            if (request.ContentEncoding.GetByteCount(request.MessageBody) > MaxDataSize)
            {
                var resultString = Error_TooLargeData();
                Response(request, resultString);
            }
            else
            {
                //  APIHandler에 전달할 Argument 생성
                //  APIHandler에서는 작업이 끝난 후 Response 혹은 ResponseWithResultCode를 호출해야 한다.
                RequestHandlerArgument arg = new RequestHandlerArgument();
                arg.HttpContext = request.Context;
                arg.MessageBody = request.MessageBody;
                arg.Response = (token) =>
                {
                    var result = ResponseString(arg, RoseResult.Ok, "Ok", token);
                    Response(request, result);
                };
                arg.ResponseWithResultCode = (resultCode, message, token) =>
                {
                    var result = ResponseString(arg, resultCode, message, token);
                    Response(request, result);
                };


                //  해당 Method 호출 및 예외처리
                try
                {
                    bool handleRequest = true;
                    if (Starter.Preprocessor != null)
                    {
                        //  전처리기 호출
                        string messageBody = arg.MessageBody;
                        handleRequest = Starter.Preprocessor.BeforeRequestHandling(arg.HttpContext, ref messageBody);

                        if (handleRequest == true)
                        {
                            arg.MessageBody = messageBody;
                            _methodSelector.Invoke(arg);
                        }
                    }
                    else
                        _methodSelector.Invoke(arg);


                    if (handleRequest == true)
                        Starter.Preprocessor?.AfterRequestHandled(arg.HttpContext);
                }
                catch (System.Reflection.TargetInvocationException e) when (e.InnerException is AegisException)
                {
                    AegisException innerException = (e.InnerException as AegisException);
                    string resultString;
                    if (innerException.ResultCodeNo == RoseResult.UnknownCommand)
                        resultString = Error_UnknownCommand(request.MessageBody);

                    else if (innerException.ResultCodeNo == RoseResult.InvalidReqest)
                        resultString = Error_InvalidRequest(request.MessageBody);

                    else
                        resultString = Error_ServerException(innerException, false);

                    Response(request, resultString);
                }
                catch (AegisException e)
                {
                    string resultString;
                    if (e.ResultCodeNo == RoseResult.UnknownCommand)
                        resultString = Error_UnknownCommand(request.MessageBody);

                    else if (e.ResultCodeNo == RoseResult.InvalidReqest)
                        resultString = Error_InvalidRequest(request.MessageBody);

                    else
                        resultString = Error_ServerException(e, false);

                    Response(request, resultString);
                }
                catch (Exception e)
                {
                    string resultString;
                    if (e.InnerException != null)
                        resultString = Error_ServerException(e.InnerException, true);
                    else
                        resultString = Error_ServerException(e, true);

                    Response(request, resultString);
                }
            }

            return true;
        }


        protected string ResponseString(RequestHandlerArgument arg, int resultCode, string message, JToken resultToken)
        {
            JObject result = new JObject()
            {
                { "resultCode", resultCode },
                { "message", message },
                { "processingTime", string.Format("{0:0.###}", arg.ProcessingTime.ElapsedSeconds) },
                { "result", resultToken ?? "{}" }
            };

            return result.ToString(Newtonsoft.Json.Formatting.None);
        }


        private void Response(HttpRequestData requestData, string result)
        {
            requestData.AppendResponseHeader("Access-Control-Allow-Origin", "*");
            requestData.Response(result, "application/json; charset=UTF-8");
        }


        protected virtual string Error_ServerException(Exception e, bool printDetail)
        {
            JObject jsonObject = null;
            if (e is AegisException)
            {
                jsonObject = new JObject()
                {
                    { "resultCode", (e as AegisException).ResultCodeNo },
                    { "message", e.Message },
                    { "exception", e.ToString() }
                };
            }
            else
            {
                jsonObject = new JObject()
                {
                    { "resultCode", RoseResult.ServerError },
                    { "message", e.Message },
                    { "exception", e.ToString() }
                };
            }

            if (printDetail == false)
                jsonObject.GetProperty("exception").Remove();

            return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
        }


        protected virtual string Error_TooLargeData()
        {
            JObject jsonObject = new JObject()
            {
                { "resultCode", RoseResult.DataSizeTooLarge },
                { "message", string.Format("The requested data size too large. It must be under {0} bytes.", MaxDataSize) }
            };
            return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
        }


        protected virtual string Error_UnknownCommand(string request)
        {
            JObject jobject = JObject.Parse(request);
            string key = (string)jobject["cmd"];
            JObject jsonObject = new JObject()
            {
                { "resultCode", RoseResult.UnknownCommand },
                { "message", string.Format("'{0}' is not defined command.", key) }
            };
            return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
        }


        protected virtual string Error_InvalidRequest(string request)
        {
            JObject jsonObject = new JObject()
            {
                { "resultCode", RoseResult.InvalidReqest },
                { "message", "Invalid request." }
            };
            return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
