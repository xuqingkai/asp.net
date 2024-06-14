<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>File</title>
</head>
<body>
<h1>System.Web.HttpContext.Current.Request.Files</h1>
<%
System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
foreach(System.Reflection.PropertyInfo propertyInfo in files.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
{
    Response.Write("<p>");
    Response.Write("<a title=\"" + propertyInfo.PropertyType + "\">" + propertyInfo.Name + "</a> = ");
    try
    {
        Response.Write(propertyInfo.GetValue(files, null));
    }
    catch(System.Exception e)
    {
        Response.Write("<i style=\"color:#f00\">" + e.Message + "</i>");
    }
    Response.Write("</p>");
}
foreach(string key in files.AllKeys)
{
    Response.Write("<p>Files[\"" + key + "\"] = " + files[key] + "</p>");
}

%>

</body>
</html>
