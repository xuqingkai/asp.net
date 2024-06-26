<%@ Page Language="C#"%>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>首页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
<script runat="server">
    string UrlEncode(string data, string charset = "UTF-8")
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        foreach (char c in data)
        {
            string a = c.ToString();
            string b = System.Web.HttpUtility.UrlEncode(a, System.Text.Encoding.GetEncoding(charset));
            stringBuilder.Append(a == b ? a : b.ToUpper());
        }
        return stringBuilder.ToString();
    }
</script>
<%
if(Request.QueryString["action"] == "sendsms")
{
	System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
	dict.Add("AccessKeyId", Request.Form["AccessKeyId"]);
	dict.Add("Timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T") + "Z");
	dict.Add("SignatureMethod", "HMAC-SHA1");
	dict.Add("SignatureVersion", "1.0");
	dict.Add("SignatureNonce", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
	dict.Add("Action", "SendSms");
	dict.Add("Version", "2017-05-25");
	dict.Add("RegionId", "cn-hangzhou");
	dict.Add("PhoneNumbers", Request.Form["PhoneNumbers"]);
	dict.Add("SignName", Request.Form["SignName"]);
	dict.Add("TemplateCode", Request.Form["TemplateCode"]);
	dict.Add("TemplateParam", "{\"code\":\"" + DateTime.Now.ToString("mmss") + "\"}");

	string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
    System.Array.Sort(keys,string.CompareOrdinal);
	string data = null;
	foreach (string key in keys)
	{
		data += "&" + key + "=" + UrlEncode(dict[key]).Replace("+", "%20").Replace("*", "%2A").Replace("%7E", "~");
	}
	data = data.TrimStart("&".ToCharArray());
	data = "GET&%2F&" + UrlEncode(data);
	byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(Request.Form["SHA1Key"] + "&");
    System.Security.Cryptography.HMACSHA1 hmacSHA1 = new System.Security.Cryptography.HMACSHA1(bytes);
    bytes = hmacSHA1.ComputeHash(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data));
    string signature = System.Convert.ToBase64String(bytes);

	string queryString = "";
	foreach (string key in keys) { queryString += "&" + key + "=" + UrlEncode(dict[key]); }
	queryString = queryString.TrimStart("&".ToCharArray());
	queryString += "&Signature=" + UrlEncode(signature);
	string url = "http://dysmsapi.aliyuncs.com/?" + queryString;
	string result = null;
	try
	{
		result = System.Text.Encoding.GetEncoding("UTF-8").GetString(new System.Net.WebClient().DownloadData(url));
	}
	catch (System.Exception ex)
	{
		result = ex.Message;
	}
	Response.Write(System.Web.HttpUtility.HtmlEncode(result));
}
%>
<form method="post" action="?action=sendsms">
    <p>AccessKeyId=<input type="text" name="AccessKeyId" /></p>
    <p>SHA1Key=<input type="text" name="SHA1Key" /></p>
    <p>PhoneNumbers=<input type="text" name="PhoneNumbers" /></p>
    <p>SignName=<input type="text" name="SignName" /></p>
    <p>TemplateCode=<input type="text" name="TemplateCode" /></p>
    <p><input type="submit" value="提交" /></p>

</form>
</body>
</html>
