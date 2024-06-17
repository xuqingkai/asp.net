<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>System.Configuration.ConfigurationManager.ConnectionStrings</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
    <%
    string name = Request.QueryString["name"] ?? "Com.Xuqingkai.Data";
    System.Configuration.ConnectionStringSettings database = System.Configuration.ConfigurationManager.ConnectionStrings[name];
    %>
	<h1>【ConnectionStrings["<%=name%>"]】：ProviderName=<%=database.ProviderName%> </h1>
	<p><%=database.ConnectionString%> </p>
</body>
</html>
