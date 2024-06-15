<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>Cookies</title>
</head>
<body>
<h1>System.Web.HttpContext.Current.Request.Cookies</h1>
<%
System.Web.HttpCookieCollection cookies = System.Web.HttpContext.Current.Request.Cookies;
foreach(System.Reflection.PropertyInfo propertyInfo in cookies.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
{
    Response.Write("<p>");
    Response.Write("<a title=\"" + propertyInfo.PropertyType + "\">" + propertyInfo.Name + "</a> = ");
    try
    {
        Response.Write(propertyInfo.GetValue(cookies, null));
    }
    catch(System.Exception e)
    {
        Response.Write("<i style=\"color:#f00\">" + e.Message + "</i>");
    }
    Response.Write("</p>");
}
foreach(string key in cookies.AllKeys)
{
    Response.Write("<p>cookies[\"" + key + "\"] = " + cookies[key] + "</p>");
}

%>

</body>
</html>
