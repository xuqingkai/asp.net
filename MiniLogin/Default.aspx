<%@ Page Language="C#" Inherits="Com.Xuqingkai.Common"%>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>快捷登录</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
<%
string userName = MiniLogin(new System.Collections.Specialized.NameValueCollection(){
	{"admin", MD5("admin")},
});
%>
恭喜您，登录成功，<%=userName%>，<a href="?MiniLogin=logout">退出</a>
</body>
</html>
