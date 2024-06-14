<%@ Page Language="C#"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>测试</title>
</head>
<body>
<%
HttpBrowserCapabilities browser = Request.Browser;
Response.Write("<p>Browser Capabilities:</p>");
Response.Write("Type = " + browser.Type + "<br />");
Response.Write("Name = " + browser.Browser + "<br />");
Response.Write("Version = " + browser.Version + "<br />");
Response.Write("Major Version = " + browser.MajorVersion + "<br />");
Response.Write("Minor Version = " + browser.MinorVersion + "<br />");
Response.Write("Platform = " + browser.Platform + "<br />");
Response.Write("Is Beta = " + browser.Beta + "<br />");
Response.Write("Is Crawler = " + browser.Crawler + "<br />");
Response.Write("Is AOL = " + browser.AOL + "<br />");
Response.Write("Is Win16 = " + browser.Win16 + "<br />");
Response.Write("Is Win32 = " + browser.Win32 + "<br />");
Response.Write("Supports Frames = " + browser.Frames + "<br />");
Response.Write("Supports Tables = " + browser.Tables + "<br />");
Response.Write("Supports Cookies = " + browser.Cookies + "<br />");
Response.Write("Supports VB Script = " + browser.VBScript + "<br />");
Response.Write("Supports JavaScript = " + browser.JavaScript + "<br />");
Response.Write("Supports Java Applets = " + browser.JavaApplets + "<br />");
Response.Write("Supports ActiveX Controls = " + browser.ActiveXControls + "<br />");
Response.Write("CDF = " + browser.CDF + "<br />");

%
</body>
</html>
