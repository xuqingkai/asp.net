<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>Url</title>
</head>
<body>
<h1>System.Web.HttpContext.Current.Request.Url</h1>
<%
System.Uri url = System.Web.HttpContext.Current.Request.Url;
foreach(System.Reflection.PropertyInfo propertyInfo in url.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
{
    Response.Write("<p>");
    Response.Write("<a title=\"" + propertyInfo.PropertyType + "\">" + propertyInfo.Name + "</a> = ");
    try
    {
        Response.Write(propertyInfo.GetValue(url, null));
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
