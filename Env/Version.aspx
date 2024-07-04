<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>Framework Version</title>
</head>
<body>
<h1>System.Environment.Version</h1>
<%=System.Environment.Version.Major%>.
<%=System.Environment.Version.Minor%>.
<%=System.Environment.Version.Build%>.
<%=System.Environment.Version.Revision%>
</body>
</html>
