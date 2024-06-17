<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>System.Configuration.ConfigurationManager.AppSettings</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
    <%
    string key = Request.QueryString["key"] ?? "ConnectionString";
    %>
	<h1>【AppSettings["<%=key%>"]】</h1>
    <p><%=System.Configuration.ConfigurationManager.AppSettings[key]%></p>
</body>
</html>
