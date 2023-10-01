
using RestSharp;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace PwC.CRM.Share.Util
{
    public class HttpHelperUtil
    {
        static HttpClient httpClient;
        static RestClient restClient;
        static HttpHelperUtil()
        {
            httpClient = new HttpClient(new HttpClientHandler { MaxConnectionsPerServer = 512 });
            restClient = new RestClient();
        }

        public string Post(object p1, object p2)
        {
            throw new NotImplementedException();
        }

        private static string AppName = "Infrastructure";
        #region 模拟GET
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="postDataStr">The post data string.</param>
        /// <param name="contentType">请求类型</param>
        /// <returns>System.String.</returns>
        public static async Task<string> HttpGet(string url, string postDataStr, string contentType = "text/html", Dictionary<string, string> headers = null)
        {
            try
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", contentType);
                return await httpClient.GetStringAsync(url + (string.IsNullOrWhiteSpace(postDataStr) ? string.Empty : "?") + postDataStr);
            }
            catch (Exception ex)
            {

                //await GwHomeLogger.LogErrorAsync(AppName, "HttpGet", ex.Message, ex, ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static  string HttpWebRequestGet(string url, Encoding encode, Dictionary<string, string> headers = null, string contentType = "text/html, application/xhtml+xml, */*")
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = contentType;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream rs = response.GetResponseStream();
            StreamReader sr = new StreamReader(rs, encode);
            var result = sr.ReadToEnd();
            sr.Close();
            rs.Close();
            return result;

        }

        #region 模拟POST
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="posturl">请求url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">请求头</param>
        /// <param name="auth">请求验证字符串</param>
        /// <param name="contentType">请求格式</param>
        /// <returns>System.String.</returns>
        public static async Task<string> HttpPostAsync(string posturl, string postData, Dictionary<string, string> headers = null, AuthenticationHeaderValue auth = null, string contentType = "application/json")
        {
            HttpResponseMessage response = null;
            try
            {
                response = await GetHttpResponseAsync(posturl, postData, headers, auth, contentType);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                //await GwHomeLogger.LogErrorAsync(AppName, "HttpPostAsync", ex.Message, ex, ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }
        }
        /// <summary>
        /// restPost请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpWebRequestPost(string posturl, string postData, Dictionary<string, string> headers = null, int timeOut = 30000)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(posturl);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                var byteData = Encoding.UTF8.GetBytes(postData);
                var length = byteData.Length;
                request.ContentLength = length;
                request.ServicePoint.Expect100Continue = false;
                //ServicePointManager.Expect100Continue = false;
                request.Timeout = timeOut; //是进行后续同步请求时使用 GetResponse 方法等待响应以及 GetRequestStream 方法等待流所允许的毫秒数
                var writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();

                //接收响应内容
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return responseString.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// restPost请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> HttpClientFormDataPost<T>(string posturl, T postBody, Dictionary<string, string> headers = null, Dictionary<string[], byte[]> file = null)
        {
            try
            {
                Dictionary<string, string> body = new Dictionary<string, string>();
                if (postBody != null)
                {
                    var properties = postBody.GetType().GetProperties();
                    foreach (var p in properties)
                    {
                        var value = "";
                        var pvalue = p.GetValue(postBody, null);
                        if (pvalue != null)
                        {
                            value = pvalue.ToString();
                        }
                        body.Add(p.Name, value);
                    }
                }
                HttpClient _httpClient = new HttpClient();
                var postContent = new MultipartFormDataContent();
                string boundary = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
                postContent.Headers.Add("ContentType", $"multipart/form-data,boundary={boundary}");
                var requestUri = posturl;

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        postContent.Headers.Add(item.Key, item.Value);
                    }
                }

                var values = new[]
                {
                    new KeyValuePair<string, string>("ref","苹果")
                };

                foreach (var keyValuePair in body)
                {
                    postContent.Add(new StringContent(keyValuePair.Value),
                    string.Format("\"{0}\"", keyValuePair.Key));
                }
                if (file != null)
                {
                    foreach(var f in file)
                    {
                        var fileContent = new ByteArrayContent(f.Value);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                        postContent.Add(fileContent, name: f.Key[0], fileName:f.Key[1]);
                    }
                }
                var response = await _httpClient.PostAsync(requestUri, postContent);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }





        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string data, string contentType = "application/json")
        {
            try
            {
                byte[] dataByte = Encoding.UTF8.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //下面两个属性有待验证
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;

                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = dataByte.Length;
                request.ServicePoint.Expect100Continue = false;

                using (Stream outStream = request.GetRequestStream())
                {
                    outStream.Write(dataByte, 0, dataByte.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {

                //await GwHomeLogger.LogErrorAsync(AppName, "HttpPostAsync", ex.Message, ex, ex.StackTrace);
                throw ex;
            }
        }

        public static async Task<string> PostFileAsync(string url, string data, string contentType = "application/json")
        {
            try
            {
                byte[] dataByte = Encoding.UTF8.GetBytes(data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //下面两个属性有待验证
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;

                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = dataByte.Length;
                request.ServicePoint.Expect100Continue = false;

                using (Stream outStream = request.GetRequestStream())
                {
                    outStream.Write(dataByte, 0, dataByte.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {

                //await GwHomeLogger.LogErrorAsync(AppName, "HttpPostAsync", ex.Message, ex, ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="posturl">请求url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">请求头</param>
        /// <param name="auth">请求验证字符串</param>
        /// <param name="contentType">请求格式</param>
        /// <returns>System.String.</returns>
        public static Task<HttpResponseMessage> GetHttpResponseAsync(string posturl, string postData, Dictionary<string, string> headers = null, AuthenticationHeaderValue auth = null, string contentType = "application/json")
        {

            try
            {
                Encoding encoding = Encoding.GetEncoding("utf-8");

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> item in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (auth != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = auth;
                }

                return httpClient.PostAsync(posturl, new StringContent(postData, encoding, contentType));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        ///// <summary>
        ///// POST请求
        ///// </summary>
        ///// <param name="posturl">请求url</param>
        ///// <param name="postData">请求数据</param>
        ///// <param name="headers">请求头</param>
        ///// <param name="auth">请求验证字符串</param>
        ///// <param name="contentType">请求格式</param>
        ///// <returns>System.String.</returns>
        //public static string HttpPost(string posturl, string postData, Dictionary<string, string> headers = null, string contentType = "application/json")
        //{
        //    try
        //    {
        //        var request = new RestRequest(posturl, Method.POST)
        //        {
        //            Timeout = 100000
        //        };
        //        if (headers != null)
        //        {
        //            foreach (KeyValuePair<string, string> item in headers)
        //            {
        //                request.AddHeader(item.Key, item.Value);
        //            }
        //        }

        //        request.AddParameter(contentType, postData, ParameterType.RequestBody);
        //        IRestResponse response = restClient.Execute(request);
        //        return response.Content;
        //    }
        //    catch (Exception ex)
        //    {
        //        //await GwHomeLogger.LogErrorAsync(AppName, "HttpPostAsync", ex.Message, ex, ex.StackTrace);
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 上传文件并附带其他的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string UploadFilesToRemoteUrl(string url, IList<byte[]> files, NameValueCollection nvc = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            var postQueue = new ByteArrayCustomQueue();

            var formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            if (nvc != null)
            {
                foreach (string key in nvc.Keys)
                {
                    var formitem = string.Format(formdataTemplate, key, nvc[key]);
                    var formitembytes = Encoding.UTF8.GetBytes(formitem);
                    postQueue.Write(formitembytes);
                }
            }

            var headerTemplate = "\r\n--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/zip\r\n\r\n";

            var i = 0;
            foreach (var file in files)
            {
                var header = string.Format(headerTemplate, "file" + i, "file" + i + ".zip");
                var headerbytes = Encoding.UTF8.GetBytes(header);
                postQueue.Write(headerbytes);
                postQueue.Write(file);
                i++;
            }

            postQueue.Write(Encoding.UTF8.GetBytes("\r\n--" + boundary + "--"));

            request.ContentLength = postQueue.Length;

            using (var requestStream = request.GetRequestStream())
            {
                postQueue.CopyToStream(requestStream);
                requestStream.Close();
            }

            var webResponse2 = request.GetResponse();

            using (var stream2 = webResponse2.GetResponseStream())
            using (var reader2 = new StreamReader(stream2))
            {
                var res = reader2.ReadToEnd();
                webResponse2.Close();
                return res;
            }
        }

        /// <summary>
        /// 模拟httpPost提交表单
        /// </summary>
        /// <param name="url">POS请求的网址</param>
        /// <param name="data">表单里的参数和值</param>
        /// <param name="encoder">页面编码</param>
        /// <returns></returns>
        public static string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoder)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoder.BodyName);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
        #endregion

        /// <summary>
        /// Http请求RESTFUL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="method"></param>
        /// <param name="DctHeaderParams"></param>
        /// <param name="statusCode">请求状态码</param>
        /// <param name="strEncoding"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static string HttpRequset(string url, string postData, string method, Dictionary<string, object> DctHeaderParams, out int statusCode, string strEncoding = "utf-8", string ContentType = "application/json;charset=UTF-8")
        {
            string responseText = ""; // 用于返回值
            Encoding encoding = Encoding.GetEncoding(strEncoding);  // 定义编码格式
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);    // 创建对象
            request.Method = method;            // 请求方式
            request.ContentType = ContentType; // 发送内容格式
            request.Timeout = 30000; // 30秒

            try
            {
                #region  添加头部参数
                if (DctHeaderParams?.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in DctHeaderParams)
                    {
                        if (item.Key == "Date")
                        {
                            request.Date = Convert.ToDateTime(item.Value);
                            CallPrivateMethod(request, "SetSpecialHeaders", "Date", item.Value); // 反射更改Headers条件参数
                        }
                        else
                        {
                            request.Headers.Add(item.Key, StringHelper.ObjectToString(item.Value));
                        }
                    }
                }
                #endregion

                if (method != WebRequestMethods.Http.Get) // Get请求无需写入请求参数
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(postData); // 数据编码UTF-8格式
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //if (response.StatusCode == HttpStatusCode.OK) // http状态码 表示请求接口是否成功
                //    statusCode = true;
                //else
                //    statusCode = false;
                statusCode = (int)response.StatusCode;

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }

                request.Abort();//销毁关闭连接
                response.Close();//销毁关闭响应
            }
            catch (WebException ex)
            {
                throw new Exception("请求异常，可能超时");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseText;
        }
        /// <summary>
        /// 反射更改Headers条件参数
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="param"></param>
        public static void CallPrivateMethod(object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            method.Invoke(instance, param);
        }

        #region 预定义方法或者变更
        //默认的编码
        private Encoding encoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private HttpResult GetHttpRequestData(HttpItem objhttpitem)
        {
            //返回参数
            HttpResult result = new HttpResult();
            try
            {
                #region 得到请求的response
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result.Header = response.Headers;
                    if (response.Cookies != null)
                    {
                        result.CookieCollection = response.Cookies;
                    }
                    if (response.Headers["set-cookie"] != null)
                    {
                        result.Cookie = response.Headers["set-cookie"];
                    }

                    MemoryStream _stream = new MemoryStream();
                    //GZIIP处理
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        //response.GetResponseStream().CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(response.GetResponseStream());
                    }
                    //获取Byte
                    byte[] RawResponse = _stream.ToArray();
                    //是否返回Byte类型数据
                    if (objhttpitem.ResultType == ResultType.Byte)
                    {
                        result.ResultByte = RawResponse;
                    }
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = meta.Groups.Count > 2 ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                    }
                    //得到返回的HTML
                    result.Html = encoding.GetString(RawResponse);
                    //最后释放流
                    _stream.Close();
                }
                #endregion
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                result.Html = "String Error";
                response = (HttpWebResponse)ex.Response;
            }
            if (objhttpitem.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }

        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            byte[] buffer = new byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            // write the required bytes  
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="objhttpItem">参数列表</param>
        /// <param name="_Encoding">读取数据时的编码方式</param>
        private void SetRequest(HttpItem objhttpItem)
        {
            // 验证证书
            SetCer(objhttpItem);
            // 设置代理
            SetProxy(objhttpItem);
            //请求方式Get或者Post
            request.Method = objhttpItem.Method;
            request.Timeout = objhttpItem.Timeout;
            request.ReadWriteTimeout = objhttpItem.ReadWriteTimeout;
            //Accept
            request.Accept = objhttpItem.Accept;
            //ContentType返回类型
            request.ContentType = objhttpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = objhttpItem.UserAgent;
            // 编码
            SetEncoding(objhttpItem);
            //设置Cookie
            SetCookie(objhttpItem);
            //来源地址
            request.Referer = objhttpItem.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = objhttpItem.Allowautoredirect;
            //设置Post数据
            SetPostData(objhttpItem);
            //设置最大连接
            if (objhttpItem.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = objhttpItem.Connectionlimit;
            }
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="objhttpItem"></param>
        private void SetCer(HttpItem objhttpItem)
        {
            if (!string.IsNullOrEmpty(objhttpItem.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.URL));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(objhttpItem.CerPath);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.URL));
            }
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetEncoding(HttpItem objhttpItem)
        {
            if (string.IsNullOrEmpty(objhttpItem.Encoding) || objhttpItem.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                encoding = null;
            }
            else
            {
                //读取数据时的编码方式
                encoding = Encoding.GetEncoding(objhttpItem.Encoding);
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetCookie(HttpItem objhttpItem)
        {
            if (!string.IsNullOrEmpty(objhttpItem.Cookie))
            {
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            }
            //设置Cookie
            if (objhttpItem.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(objhttpItem.CookieCollection);
            }
        }
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetPostData(HttpItem objhttpItem)
        {
            //验证在得到结果时是否有传入数据
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                //写入Byte类型
                if (objhttpItem.PostDataType == PostDataType.Byte)
                {
                    //验证在得到结果时是否有传入数据
                    if (objhttpItem.PostdataByte != null && objhttpItem.PostdataByte.Length > 0)
                    {
                        request.ContentLength = objhttpItem.PostdataByte.Length;
                        request.GetRequestStream().Write(objhttpItem.PostdataByte, 0, objhttpItem.PostdataByte.Length);
                    }
                }//写入文件
                else if (objhttpItem.PostDataType == PostDataType.FilePath)
                {
                    StreamReader r = new StreamReader(objhttpItem.Postdata, encoding);
                    byte[] buffer = Encoding.Default.GetBytes(r.ReadToEnd());
                    r.Close();
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //验证在得到结果时是否有传入数据
                    if (!string.IsNullOrEmpty(objhttpItem.Postdata))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(objhttpItem.Postdata);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="objhttpItem">参数对象</param>
        private void SetProxy(HttpItem objhttpItem)
        {
            if (string.IsNullOrEmpty(objhttpItem.ProxyUserName) && string.IsNullOrEmpty(objhttpItem.ProxyPwd) && string.IsNullOrEmpty(objhttpItem.ProxyIp))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(objhttpItem.ProxyIp, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);
                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }
        #endregion
        #region 普通类型
        /// <summary>    
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>    
        /// <param name="URL">url</param>   
        /// <returns>
        /// </returns>
        public static string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }
        ///<summary>
        ///采用https协议访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="objhttpItem">参数列表</param>
        ///<returns>String类型的数据</returns>
        public HttpResult GetHtml(HttpItem objhttpItem)
        {
            //准备参数
            SetRequest(objhttpItem);
            //调用专门读取数据的类
            return GetHttpRequestData(objhttpItem);
        }
        #endregion
    }
    /// <summary>
    /// Http请求参考类 
    /// </summary>
    public class HttpItem
    {
        string _URL;
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
        string _Method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }
        int _Timeout = 100000;
        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        int _ReadWriteTimeout = 30000;
        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }
        string _Accept = "text/html, application/xhtml+xml, */*";
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }
        string _ContentType = "text/html";
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }
        string _Encoding = string.Empty;
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        private PostDataType _PostDataType = PostDataType.String;
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }
        string _Postdata;
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata
        {
            get { return _Postdata; }
            set { _Postdata = value; }
        }
        private byte[] _PostdataByte = null;
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte
        {
            get { return _PostdataByte; }
            set { _PostdataByte = value; }
        }
        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }
        string _Cookie = string.Empty;
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        string _Referer = string.Empty;
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }
        string _CerPath = string.Empty;
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _CerPath; }
            set { _CerPath = value; }
        }
        private bool isToLower = true;
        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public bool IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }
        private bool allowautoredirect = true;
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public bool Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }
        private int connectionlimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }
        private string proxyusername = string.Empty;
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }
        private string proxypwd = string.Empty;
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }
        private string proxyip = string.Empty;
        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }
        private ResultType resulttype = ResultType.String;
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return resulttype; }
            set { resulttype = value; }
        }

    }
    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        string _Cookie = string.Empty;
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }
        private string html = string.Empty;
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get { return html; }
            set { html = value; }
        }
        private byte[] resultbyte = null;
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {

            get { return resultbyte; }
            set { resultbyte = value; }
        }
        private WebHeaderCollection header = new WebHeaderCollection();
        //header对象
        public WebHeaderCollection Header
        {
            get { return header; }
            set { header = value; }
        }

    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        String,//表示只返回字符串
        Byte//表示返回字符串和字节流
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        String,//字符串
        Byte,//字符串和字节流
        FilePath//表示传入的是文件
    }

    public class ByteArrayCustomQueue
    {

        private LinkedList<byte[]> arrays = new LinkedList<byte[]>();

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Write(byte[] data)
        {
            arrays.AddLast(data);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get { return arrays.Sum(x => x.Length); } }

        /// <summary>
        /// Copies to stream.
        /// </summary>
        /// <param name="requestStream">The request stream.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void CopyToStream(Stream requestStream)
        {
            foreach (var array in arrays)
            {
                requestStream.Write(array, 0, array.Length);
            }
        }
    }
}
