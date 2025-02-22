<%@ Page Language="C#" %><%
string name = "callback";
string url = Request.PathInfo;
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
string data = null;
string response = name;

if(Request.QueryString.ToString() == "view")
{
    data = Temp(name);
    response = "<!DOCTYPE html><html lang=\"zh\"><head><meta http-equiv=\"Content-type\" content=\"text/html; charset=utf-8\" /><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /><title>callback</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" /><style type=\"text/css\">body{font-size:14px;}textarea{width:99%;height:90vh;font-size:16px;}</style></head><body><form><a style=\"float:right\" href=\"?clear\">清空</a><a href=\"?view\">首页</a><textarea>" + data + "</textarea></form></body></html>";
    Response.Write(response);
}
else if(Request.QueryString.ToString() == "clear")
{
    Temp(name,"");
    Response.Redirect("?view");
}
else
{
    System.Collections.Specialized.NameValueCollection header = new System.Collections.Specialized.NameValueCollection();
    foreach(string key in Request.Headers.AllKeys)
    {
        foreach(string item in headers)
        {
            if(key.ToLower().Replace("-","") == (item).ToLower().Replace("-","")){
              header.Set(key, Request.Headers[key]); 
            }
        }
    }
    string strHeaders = "";
    foreach(string key in header.AllKeys) { strHeaders += key + ":" + header[key] + "\r\n"; }
    //Response.Write(strHeaders);Response.End();
    
    string method = Request.HttpMethod;
    string postString = HttpPost();

    string text = "\r\n\r\n";
    text += System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
    text += "-----【URL】------------------------------------------------------------------\r\n";
    text += Request.Url.PathAndQuery + "\r\n";
    text += "-----【HEADER】------------------------------------------------------------------\r\n";
    foreach(string key in header.AllKeys)
    {
        text += key + ":" + header[key] + "\r\n";
    }
    text += "-----【" + method + "】------------------------------------------------------------------\r\n";
    text += postString + "\r\n";
    if(!string.IsNullOrEmpty(url)){
        if(string.Join("", headers).IndexOf("Content-Type") < 0){ header.Set("Content-Type", "application/x-www-form-urlencoded"); }
        if(Request.HttpMethod == "GET")
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
    text += Temp(name);
    
    Temp(name, text);
    Response.Write(response);
    Response.End();
}
%>
<script runat="server">
    [System.Web.Services.WebMethod]
    public static string Test()   
    {
        System.Web.HttpContext.Current.Response.ContentType="application/json; charset=utf-8";
        return "{\"name\":\"test\",\"age\":22}";   
    }
    public string Temp(string key, string val=null)
    {
        string model="sql";//sql/io



        
        if(model=="io")
        {
            string file = Server.MapPath("./" + key + ".txt");
            if(val==null)
            {
                if(System.IO.File.Exists(file)) { val = Server.HtmlEncode(System.IO.File.ReadAllText(file)); }
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
        else
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
                command.CommandText="UPDATE [xqk_temp] SET Contents='' WHERE [temp_key]='callback'";
                command.ExecuteNonQuery();  
            }
            else
            {
                command.CommandText="UPDATE [xqk_temp] SET Contents='"+val+"'+Contents WHERE [temp_key]='callback'";
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
</script>
