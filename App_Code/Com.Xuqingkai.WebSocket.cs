namespace Com.Xuqingkai
{
	//服务器win8 2008以上
	public class WebSocket : System.Web.IHttpHandler
	{
        public static System.Collections.Generic.Dictionary<string, System.Net.WebSockets.WebSocket> WebSockets = new System.Collections.Generic.Dictionary<string, System.Net.WebSockets.WebSocket>();

        public void ProcessRequest(System.Web.HttpContext context)
		{
			if (context.IsWebSocketRequest)
			{
				context.AcceptWebSocketRequest(ProcessWebSocket);
			}
			else
			{
				context.Response.Write("【警告提示】<br />");
				context.Response.Write("您看到本文是因为：WebSocket请求无法用浏览器等其他方式访问！！！！<br />");
				context.Response.Write("【使用方法】<br />");
				context.Response.Write("1、Web.Config下system.web标签里增加httpRuntime标签，并配置属性：httpRuntime=\"4.5\"<br />");
				context.Response.Write("2、Web.Config下system.webServer标签里增加" + System.Web.HttpUtility.HtmlEncode("<handlers>") + "标签，包含以下代码：" + System.Web.HttpUtility.HtmlEncode("<add path=\"/WebSocket.html\" verb=\"*\" name=\"WebSocket\" type =\"SH.WebSocket\" />") + "<br />");
				context.Response.Write("3、/WebSocket.html文件只要保证存在即可无需任何代码，至少保证运行无错。");
				context.Response.End();
			}
		}

		private async System.Threading.Tasks.Task ProcessWebSocket(System.Web.WebSockets.AspNetWebSocketContext context)
		{
			System.Net.WebSockets.WebSocket socket = context.WebSocket;
			while (true)
			{
				System.ArraySegment<byte> byteReceive = new System.ArraySegment<byte>(new byte[1024]);
				System.Net.WebSockets.WebSocketReceiveResult webSocketReceiveResult = await socket.ReceiveAsync(byteReceive, System.Threading.CancellationToken.None);
				if (socket.State == System.Net.WebSockets.WebSocketState.Open)
				{
					string jsonReceive = System.Text.Encoding.UTF8.GetString(byteReceive.Array, 0, webSocketReceiveResult.Count);


                    string url = "http://pay.05370.com/pay/callback.aspx/save";
                    string data = "a=1";
                    //string result = null;
                    //byte[] bytes = null;
                    //System.Net.WebClient webClient = new System.Net.WebClient();
                    //try
                    //{
                    //	bytes = webClient.DownloadData(url);
                    //}
                    //catch (Exception ex)
                    //{
                    //	result = ex.Message;
                    //	result = null;
                    //}
                    //if (bytes != null) { result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);}		



                    string result = null;
                    System.Net.CookieContainer cookie = new System.Net.CookieContainer();
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors) { return true; });
                    System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    System.IO.Stream requestStream = null;
                    webRequest.Method = "POST";
                    webRequest.Referer = System.Web.HttpContext.Current.Request.Url.ToString();
                    webRequest.UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["User-Agent"];
                    webRequest.ContentType = "application/x-www-form-urlencoded";//"text/plain","text/html","text/xml","text/javascript","application/javascript","application/json"
                    webRequest.CookieContainer = cookie;
                    byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);
                    System.Net.HttpWebResponse response = null;
                    try
                    {
                        requestStream = webRequest.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        response = (System.Net.HttpWebResponse)webRequest.GetResponse();
                        cookie = new System.Net.CookieContainer();
                        cookie.Add(response.Cookies);
                        string responseCharset = System.Web.HttpUtility.ParseQueryString(response.Headers.Get("Content-Type").Replace(";", "&").Replace(" ", ""))["charset"];
                        if (responseCharset == null || responseCharset.Length == 0) { responseCharset = "UTF-8"; }
                        result = new System.IO.StreamReader(webRequest.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding(responseCharset)).ReadToEnd() + "";
                    }
                    catch (System.Exception ex)
                    {
                        result = ex.Message;
                        result = null;
                    }
                    finally
                    {
                        webRequest.Abort();
                        if (response != null) { response.Close(); }
                    }
                    string jsonReply = "您于" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "向我们发送了：" + jsonReceive;
                    jsonReply += "<br />[" + result + "]";
                    string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    if (!WebSockets.ContainsKey(ip))
                    {
                        WebSockets.Add(ip, socket);
                    }
                    else
                    {
                        WebSockets[ip] = socket;
                    }


                    System.ArraySegment<byte> byteReply = new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(jsonReply));
					await socket.SendAsync(byteReply, System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

                }
				else
				{
					break;
				}
			}
		}

		public bool IsReusable { get { return false; } }
	}
}
