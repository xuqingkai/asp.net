<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>注册机</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        input{font-size:14px;padding:5px;margin-top:10px;width:300px;}
        code{color:#f00;padding:10px;border:1px solid #ccc}
    </style>
</head>
<body>
<%
string org=Request.Form["org"]??"乡镇卫生院";
string date=Request.Form["date"]??DateTime.Now.AddDays(365).ToString("yyyyMMdd");
string code=Request.Form["code"];
%>
<form method="post" action="./">
单位：<input type="text" name="org" value="<%=org%>" /><br />
日期：<input type="text" name="date" value="<%=date%>" /><br />
设备：<input type="text" name="code" value="<%=code%>" placeHolder="可为空" /><br />
<input type="submit" value="提交" />
</form>
<hr /><div>
<%
if(Request.HttpMethod=="POST")
{
    string key="prs@2022";

    string str=org+date+(code.Length>0?"_"+code:"_0");
    if(org=="乡镇卫生院"){ str=org+date; }

    byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
    byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);

    System.Security.Cryptography.DES des = System.Security.Cryptography.DESCryptoServiceProvider.Create();
    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
    System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, des.CreateEncryptor(keyBytes, keyBytes), System.Security.Cryptography.CryptoStreamMode.Write);
    cryptoStream.Write(strBytes, 0, strBytes.Length);
    cryptoStream.FlushFinalBlock();
    string decrypt = Convert.ToBase64String(memoryStream.ToArray());
    cryptoStream.Close();
    memoryStream.Close();

    Response.Write(org+"的注册码为<br /><br /><code>"+decrypt+"</code>");
}
%>
</div>
</body>
</html>
