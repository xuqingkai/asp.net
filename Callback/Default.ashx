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
        name = name.Replace(":", ".");
        //context.Response.Write(name); context.Response.End();
        
        string data = null;
        string response = name;

        if(context.Request.QueryString.ToString() == "view")
        {
            data = DataStorge(name);
            response = "<!DOCTYPE html><html lang=\"zh\"><head><meta http-equiv=\"Content-type\" content=\"text/html; charset=utf-8\" /><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /><title>callback</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" /><style type=\"text/css\">body{font-size:14px;}textarea{width:99%;height:90vh;font-size:16px;}</style></head><body><form><a style=\"float:right\" href=\"?clear\">清空</a><a href=\"?view\">首页</a><textarea>" + data + "</textarea></form></body></html>";
            context.Response.Write(response);
        }
        else if(context.Request.QueryString.ToString() == "clear")
        {
            DataStorge(name,"");
            context.Response.Redirect("?view");
        }
        else
        {
            System.Collections.Specialized.NameValueCollection header = new System.Collections.Specialized.NameValueCollection();
            foreach(string item in headers)
            {
                string headerKey = item;
                string headerValue = "";
                foreach(string key in context.Request.Headers.AllKeys)
                {
                    if(item.ToLower().Replace("-","") == key.ToLower().Replace("-",""))
                    {
                        headerKey = key;
                        headerValue = context.Request.Headers[key]; 
                    }
                }
                if(!string.IsNullOrEmpty(headerKey)){ header.Set(headerKey, headerValue); }
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
            text += "-----【REQUEST_HEADER】------------------------------------------------------------------\r\n";
            foreach(string key in header.AllKeys)
            {
                text += key + ":" + header[key] + "\r\n";
            }
            text += "-----【" + method + "】------------------------------------------------------------------\r\n";
            text += postString + "\r\n";
            if(!string.IsNullOrEmpty(url)){
                if(string.Join("", headers).ToLower().IndexOf("Content-Type") < 0){ header.Set("Content-Type", "application/x-www-form-urlencoded"); }
                if(context.Request.HttpMethod == "GET")
                {
                    response = HttpGet(url, ref header);
                }
                else
                {
                    response = HttpPost(url, postString, ref header);
                }

                text += "-----【RESPONSE_HEADER】------------------------------------------------------------------\r\n";
                foreach(string key in header.AllKeys)
                {
                    text += key + ":" + header[key] + "\r\n";
                }
                text += "-----【RESPONSE】------------------------------------------------------------------\r\n";
                text += response + "\r\n";
            }
            text += "=======================================================================\r\n";
            text += DataStorge(name);
            
            DataStorge(name, text);
            context.Response.Write(response);
            context.Response.End();
        }
    }
    public string DataStorge(string key, string val=null)
    {
        try
        {
            System.IO.File.CreateText("./test.txt");
            string file = System.Web.HttpContext.Current.Server.MapPath("./" + key + ".txt");
            if(val==null)
            {
                if(System.IO.File.Exists(file)) { val = System.Web.HttpContext.Current.Server.HtmlEncode(System.IO.File.ReadAllText(file)); }
            }
            else if(val=="")
            {
                System.IO.File.Delete(file);
            }
            else
            {
                System.IO.File.WriteAllText(file, val);
            }
        }
        catch
        {
            string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
            if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); }
            System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand();
            command.Connection=connection;

            if(val==null)
            {
                command.CommandText="SELECT Contents FROM [xqk_temp] WHERE [temp_key]='callback'";
                val = command.ExecuteScalar().ToString();
            }
            else if(val=="")
            {
                command.CommandText="UPDATE [xqk_temp] SET [Contents]='' WHERE [temp_key]='callback'";
                command.ExecuteNonQuery();  
            }
            else
            {
                command.CommandText="UPDATE [xqk_temp] SET [Contents]=CONCAT(?,[Contents]) WHERE [temp_key]='callback'";
                command.Parameters.Add(new System.Data.OleDb.OleDbParameter("Contents", val));
                command.ExecuteNonQuery(); 
            }
            connection.Close();
            connection.Dispose();
        }
        return val;

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
    public static string HttpGet(string url, ref System.Collections.Specialized.NameValueCollection headers)
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
            foreach(string key in headers.Keys) { webClient.Headers.Add(key, headers[key]); }
            byte[] bytes = webClient.DownloadData(url);
            result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
            
            System.Collections.Specialized.NameValueCollection responseHeader = new System.Collections.Specialized.NameValueCollection();
            System.Net.WebHeaderCollection responseHeaders = webClient.ResponseHeaders;
            foreach(string key in responseHeaders.AllKeys)
            {
                string headerKey = null;
                string headerValue = "";
                foreach(string item in headers.Keys)
                {
                    if(key.Replace("-","").ToLower() == item.Replace("-","").ToLower())
                    {
                        headerKey = key;
                        headerValue = responseHeaders[key]; 
                    }
                }
                if(!string.IsNullOrEmpty(headerKey)){ responseHeader.Set(headerKey, headerValue); }
            }
            headers = responseHeader;
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
            result = ex.Message;
        }
        return result;
    }
    public static string HttpPost(string url, string data, ref System.Collections.Specialized.NameValueCollection headers)
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
            foreach(string key in headers.Keys) { webClient.Headers.Add(key, headers[key]); }
            byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data));
            result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
            
            System.Collections.Specialized.NameValueCollection responseHeader = new System.Collections.Specialized.NameValueCollection();
            System.Net.WebHeaderCollection responseHeaders = webClient.ResponseHeaders;
            foreach(string key in responseHeaders.AllKeys)
            {
                string headerKey = null;
                string headerValue = "";
                foreach(string item in headers.Keys)
                {
                    if(key.Replace("-","").ToLower() == item.Replace("-","").ToLower())
                    {
                        headerKey = key;
                        headerValue = responseHeaders[key]; 
                    }
                }
                if(!string.IsNullOrEmpty(headerKey)){ responseHeader.Set(headerKey, headerValue); }
            }
            headers = responseHeader;
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
            result = ex.Message;
        }
        return result;
    }
}
  