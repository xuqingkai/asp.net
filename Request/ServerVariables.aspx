<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>ServerVariables</title>
</head>
<body>
<h1>System.Web.HttpContext.Current.Request.ServerVariables</h1>
<%
System.Collections.Specialized.NameValueCollection serverVariables = System.Web.HttpContext.Current.Request.ServerVariables;
foreach(System.Reflection.PropertyInfo propertyInfo in serverVariables.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
{
    Response.Write("<p>");
    Response.Write("<a title=\"" + propertyInfo.PropertyType + "\">" + propertyInfo.Name + "</a> = ");
    try
    {
        Response.Write(propertyInfo.GetValue(serverVariables, null));
    }
    catch(System.Exception e)
    {
        Response.Write("<i style=\"color:#f00\">" + e.Message + "</i>");
    }
    Response.Write("</p>");
}
foreach(string key in serverVariables.AllKeys)
{
    Response.Write("<p>ServerVariables[\"" + key + "\"] = " + serverVariables[key] + "</p>");
}

%>

</body>
</html>
