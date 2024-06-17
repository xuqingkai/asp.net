<%@ webhandler class="RequestForward"%>

public class RequestForward : System.Web.IHttpHandler
{
    public bool IsReusable 
    { 
        get { return true; } 
    } 
    public void ProcessRequest(System.Web.HttpContext context)
    {
        string name = "callback";
        string url = context.Request.PathInfo;
        string[] headers = new string[]{};
        if(!string.IsNullOrEmpty(url))
        {
            string strHeaders = null;
            if(url.IndexOf("/http:/")>=0 || url.IndexOf("/https:/")>=0)
            {
                strHeaders = url.Substring(0,url.IndexOf("/http")).Replace("/","");
                url = url.Substring(url.IndexOf("/http") + 1);
                if(url.IndexOf("://") < 0){ url = url.Replace(":/","://"); }
                name = url.Split('/')[2];
            }
            else
            {
                strHeaders = url.Substring(1);
                if(strHeaders.IndexOf("/") >= 0) { strHeaders = strHeaders.Substring(0, strHeaders.IndexOf("/")); }
                url = null;
            }
            if(!string.IsNullOrEmpty(strHeaders))
            {
                headers = strHeaders.Replace("|", ",").Split(',');
            }
        }
        string file = context.Server.MapPath("./" + name + ".txt");
        string data = null;
        string response = name;

        if(context.Request.QueryString.ToString() == "view")
        {
            data = System.IO.File.Exists(file) ? context.Server.HtmlEncode(System.IO.File.ReadAllText(file)) : null;
            response = "<!DOCTYPE html><html lang=\"zh\"><head><meta http-equiv=\"Content-type\" content=\"text/html; charset=utf-8\" /><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /><title>callback</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" /><style type=\"text/css\">body{font-size:14px;}textarea{width:99%;height:90vh;font-size:16px;}</style></head><body><form><a style=\"float:right\" href=\"?clear\">清空</a><a href=\"?view\">首页</a><textarea>" + data + "</textarea></form></body></html>";
            context.Response.Write(response);
        }
        else if(context.Request.QueryString.ToString() == "clear")
        {
            System.IO.File.Delete(file);
            context.Response.Redirect("?view");
        }
        else
        {
            System.Collections.Specialized.NameValueCollection header = new System.Collections.Specialized.NameValueCollection();
            foreach(string key in context.Request.Headers.AllKeys)
            {
                foreach(string item in headers)
                {
                    if(key.ToLower().Replace("-","") == (item).ToLower().Replace("-","")){
                      header.Set(key, context.Request.Headers[key]); 
                    }
                }
            }
            string strHeaders = "";
            foreach(string key in header.AllKeys) { strHeaders += key + ":" + header[key] + "\r\n"; }
            //context.Response.Write(strHeaders);context.Response.End();
            
            string method = context.Request.HttpMethod;
            string postString = HttpPost();
        
            string text = "\r\n\r\n";
            text += System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
            text += "-----【URL】------------------------------------------------------------------\r\n";
            text += context.Request.Url.PathAndQuery + "\r\n";
            text += "-----【HEADER】------------------------------------------------------------------\r\n";
            foreach(string key in header.AllKeys)
            {
                text += key + ":" + header[key] + "\r\n";
            }
            text += "-----【" + method + "】------------------------------------------------------------------\r\n";
            text += postString + "\r\n";
            if(!string.IsNullOrEmpty(url)){
                if(string.Join("", headers).IndexOf("Content-Type") < 0){ header.Set("Content-Type", "application/x-www-form-urlencoded"); }
                if(context.Request.HttpMethod == "GET")
                {
                    response = HttpGet(url, header);
                }
                else
                {
                    response = HttpPost(url, header, postString);
                }

                text += "-----【RESPONSE】------------------------------------------------------------------\r\n";
                text += response + "\r\n";
            }
            text += "=======================================================================\r\n";
            text += System.IO.File.Exists(file) ? System.IO.File.ReadAllText(file) : null;
            
            System.IO.File.WriteAllText(file, text);
            context.Response.Write(response);
            context.Response.End();
        }
    }
    public static string HttpPost()
    {
        System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
        stream.Position = 0;
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
        return result;
    }
    public static string HttpGet(string url, System.Collections.Specialized.NameValueCollection headers)
    {
        string result = null;
        System.Net.WebClient webClient = new System.Net.WebClient();
        System.Net.ServicePointManager.Expect100Continue = false;
        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
        //webClient.Encoding = System.Text.Encoding.UTF8;
        try
        {
            if(headers["Content-Type"] == null && headers["ContentType"] != null) { headers.Add("Content-Type", headers["ContentType"]); headers.Remove("ContentType"); }
            if(headers["User-Agent"] == null && headers["UserAgent"] != null) { headers.Add("User-Agent", headers["UserAgent"]); headers.Remove("UserAgent"); }
            foreach(string key in headers.Keys) { webClient.Headers.Add(key,headers[key]); }
            byte[] bytes = webClient.DownloadData(url);
            result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
        }
        catch (System.Net.WebException webException)
        {
            System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
            if(httpWebResponse != null)
            {
                result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
        }
        catch (System.Exception ex)
        {
            result = "HttpGet:" + ex.Message;
            //result = null;
        }
        return result;
    }
    public static string HttpPost(string url, System.Collections.Specialized.NameValueCollection headers, string data)
    {
        string result = null;
        System.Net.WebClient webClient = new System.Net.WebClient();
        System.Net.ServicePointManager.Expect100Continue = false;
        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
        //webClient.Encoding = System.Text.Encoding.UTF8;
        try
        {
            if(headers["Content-Type"] == null && headers["ContentType"] != null) { headers.Add("Content-Type", headers["ContentType"]); headers.Remove("ContentType"); }
            if(headers["User-Agent"] == null && headers["UserAgent"] != null) { headers.Add("User-Agent", headers["UserAgent"]); headers.Remove("UserAgent"); }
            foreach(string key in headers.Keys) { webClient.Headers.Add(key,headers[key]); }
            byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data));
            result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
        }
        catch (System.Net.WebException webException)
        {
            System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
            if(httpWebResponse != null)
            {
                result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
        }
        catch (System.Exception ex)
        {
            result = "HttpPost:" + ex.Message;
        }
        return result;
    }
}
  