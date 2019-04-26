using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Rose.Client
{
    public class RoseAPI
    {
        public delegate void AsyncResultHandler(RoseResult result);
        public int RequestTimeout { get; set; } = 60000;
        private CallbackQueue _workQueue = new CallbackQueue();





        private void WebExceptionHandling(Action action, Action<Exception, int> onException)
        {
            try
            {
                action();
            }
            catch (WebException e)
            {
                onException(e, RoseResult.NetworkError);
            }
            catch (Exception e)
            {
                onException(e, RoseResult.UnknownError);
            }
        }





        public void ProcessQueue()
        {
            _workQueue.Update();
        }





        #region  HTTP Send method
        public RoseResult Send(string url, string request)
        {
            try
            {
                var uri = new Uri(url);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.Method = "POST";
                httpRequest.ServicePoint.Expect100Continue = false;
                httpRequest.ContentType = "application/json; charset=UTF-8";
                httpRequest.Timeout = RequestTimeout;
                //httpRequest.ContentLength = 0;

                byte[] byteArray = Encoding.UTF8.GetBytes(request);
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                using (var response = (HttpWebResponse)httpRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"), true))
                {
                    string result = reader.ReadToEnd();
                    return new RoseResult(result);
                }
            }
            catch (WebException e)
            {
                return new RoseResult(RoseResult.NetworkError, e.Message);
            }
            catch (Exception e)
            {
                return new RoseResult(RoseResult.UnknownError, e.Message);
            }
        }


        public RoseResult Send(string url, JToken requestToken)
        {
            try
            {
                var uri = new Uri(url);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.Method = "POST";
                httpRequest.ServicePoint.Expect100Continue = false;
                httpRequest.Timeout = RequestTimeout;
                //request.ContentLength = 0;

                string request = requestToken.ToString(Newtonsoft.Json.Formatting.None);
                byte[] byteArray = Encoding.UTF8.GetBytes(request);
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                using (var response = (HttpWebResponse)httpRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"), true))
                {
                    string result = reader.ReadToEnd();
                    return new RoseResult(result);
                }
            }
            catch (WebException e)
            {
                return new RoseResult(RoseResult.NetworkError, e.Message);
            }
            catch (Exception e)
            {
                return new RoseResult(RoseResult.UnknownError, e.Message);
            }
        }


        public void SendAsync(string url, string request, AsyncResultHandler callback)
        {
            WebExceptionHandling(() =>
            {
                var uri = new Uri(url);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.Method = "POST";
                httpRequest.ServicePoint.Expect100Continue = false;
                httpRequest.ContentType = "application/json; charset=UTF-8";
                httpRequest.Timeout = RequestTimeout;
                //httpRequest.ContentLength = 0;

                byte[] byteArray = Encoding.UTF8.GetBytes(request);
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                httpRequest.BeginGetResponse(new AsyncCallback(delegate (IAsyncResult result)
                {
                    WebExceptionHandling(() =>
                    {
                        using (var response = (HttpWebResponse)httpRequest.EndGetResponse(result))
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"), true))
                        {
                            string resultString = reader.ReadToEnd();
                            _workQueue.Enqueue(() =>
                            {
                                try
                                {
                                    callback(new RoseResult(resultString));
                                }
                                catch (Exception e)
                                {
                                    callback(new RoseResult(RoseResult.UnknownError, e.Message));
                                }
                            });
                        }
                    }, (e, errorCode) =>
                    {
                        callback(new RoseResult(errorCode, e.Message));
                    });
                }), httpRequest);
            }, (e, errorCode) =>
            {
                callback(new RoseResult(errorCode, e.Message));
            });
        }


        public void SendAsync(string url, JToken requestToken, AsyncResultHandler callback)
        {
            WebExceptionHandling(() =>
            {
                var uri = new Uri(url);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpRequest.Method = "POST";
                httpRequest.ServicePoint.Expect100Continue = false;
                httpRequest.Timeout = RequestTimeout;
                httpRequest.ContentType = "application/json; charset=UTF-8";

                string request = requestToken.ToString(Newtonsoft.Json.Formatting.None);
                byte[] byteArray = Encoding.UTF8.GetBytes(request);
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();


                httpRequest.BeginGetResponse(new AsyncCallback(delegate (IAsyncResult result)
                {
                    WebExceptionHandling(() =>
                    {
                        using (var response = (HttpWebResponse)httpRequest.EndGetResponse(result))
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"), true))
                        {
                            string resultString = reader.ReadToEnd();
                            _workQueue.Enqueue(() =>
                            {
                                try
                                {
                                    callback(new RoseResult(resultString));
                                }
                                catch (Exception e)
                                {
                                    callback(new RoseResult(RoseResult.UnknownError, e.Message));
                                }
                            });
                        }
                    }, (e, errorCode) =>
                    {
                        callback(new RoseResult(errorCode, e.Message));
                    });
                }), httpRequest);
            }, (e, errorCode) =>
            {
                callback(new RoseResult(errorCode, e.Message));
            });
        }
        #endregion





        public void Hello(string url, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "hello" }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void SchemeList(string url, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "schemeList" }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void CollectionList(string url, string scheme, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "collectionList" },
                { "scheme", scheme }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void CreateScheme(string url, string schemeName, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "createScheme" },
                { "scheme", schemeName }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void DropScheme(string url, string schemeName, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "dropScheme" },
                { "scheme", schemeName }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void CreateCollection(string url, string schemeName, string collectionName, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "createCollection" },
                { "collection", $"{schemeName}.{collectionName}" },
                { "justInCache", false }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public void DropCollection(string url, string schemeName, string collectionName, AsyncResultHandler callback)
        {
            JObject jsonRequest = new JObject()
            {
                { "cmd", "dropCollection" },
                { "collection", $"{schemeName}.{collectionName}" }
            };
            SendAsync(url, jsonRequest, callback);
        }


        public enum OrderBy
        {
            Asc, Desc
        }
        public void Select(string url, string collection,
                                 string where, string sortKey,
                                 int rangeStart, int rangeCount,
                                 AsyncResultHandler callback)
        {
            JProperty propWhere = new JProperty("where", where);
            JProperty propSort = new JProperty("sortKey", sortKey);
            JProperty propRange = new JProperty("range", new JArray(rangeStart, rangeCount));
            JObject jsonRequest = new JObject()
            {
                { "cmd", "select" },
                { "collection", collection },
                propWhere, propSort, propRange
            };

            if (where == null)
                propWhere.Remove();
            if (sortKey == null)
                propSort.Remove();

            SendAsync(url, jsonRequest, callback);
        }


        public void Insert(string url, string collection, string uniqueFor, string onDuplicate,
                           string data, AsyncResultHandler callback)
        {
            Insert(url, collection, uniqueFor, onDuplicate, JToken.Parse(data), callback);
        }


        public void Insert(string url, string collection, string uniqueFor, string onDuplicate,
                           JToken data, AsyncResultHandler callback)
        {
            JProperty propUniqueFor = new JProperty("uniqueFor", uniqueFor);
            JProperty propOnDuplicate = new JProperty("onDuplicate", onDuplicate);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "insert" },
                { "collection", collection },
                propUniqueFor, propOnDuplicate,
                { "data", data }
            };

            if (uniqueFor == null)
                propUniqueFor.Remove();
            if (onDuplicate == null)
                propOnDuplicate.Remove();

            SendAsync(url, jsonRequest, callback);
        }


        public void Update(string url, string collection,
                           string where, string data, AsyncResultHandler callback)
        {
            Update(url, collection, where, JToken.Parse(data), callback);
        }


        public void Update(string url, string collection,
                           string where, JToken data, AsyncResultHandler callback)
        {
            JProperty propWhere = new JProperty("where", where);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "update" },
                { "collection", collection },
                { "data", data },
                propWhere
            };

            if (where == null)
                propWhere.Remove();

            SendAsync(url, jsonRequest, callback);
        }


        public void Delete(string url, string collection,
                           string where, AsyncResultHandler callback)
        {
            JProperty propWhere = new JProperty("where", where);
            JObject jsonRequest = new JObject()
            {
                { "cmd", "delete" },
                { "collection", collection },
                propWhere
            };

            if (where == null)
                propWhere.Remove();

            SendAsync(url, jsonRequest, callback);
        }
    }
}
