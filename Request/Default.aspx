﻿<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>Request</title>
</head>
<body>
<h1>
System.Web.HttpContext.Current.Request(.<a href="ServerVariables.aspx">ServerVariables</a>|.<a href="Url.aspx">Url</a>|.<a href="Files.aspx">Files</a>|.<a href="Browser.aspx">Browser</a>)
</h1>
<%
System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
foreach(System.Reflection.PropertyInfo propertyInfo in request.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
{
    Response.Write("<p>");
    Response.Write("<a title=\"" + propertyInfo.PropertyType + "\">" + propertyInfo.Name + "</a> = ");
    try
    {
        Response.Write(propertyInfo.GetValue(request, null));
    }
    catch(System.Exception e)
    {
        Response.Write("<i style=\"color:#f00\">" + e.Message + "</i>");
    }
    Response.Write("</p>");
}
%>
</body>
</html>
