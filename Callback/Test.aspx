<%@ Page Language="C#" %><%
Int64 size = HttpPost().Length;
System.Web.HttpContext.Current.Response.ContentType="application/json; charset=utf-8";
Response.Headers.Add("User-Agent", Request.Headers["User-Agent"]);
Response.Headers.Add("Test", Request.Headers["Test"] + "@" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
Response.Headers.Add("Timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
Response.Write("{\"size\":" + size + "}");
Response.End();
%>
<script runat="server">
    public static string HttpPost()
    {
        System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
        stream.Position = 0;
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
        return result;
    }
    
</script>
