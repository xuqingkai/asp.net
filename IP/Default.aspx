<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>IP地址</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
	<h1>
    <%=System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]%>
    </h1>
</body>
</html>
