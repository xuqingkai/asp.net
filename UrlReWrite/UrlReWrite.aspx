<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>Url重写<%=Request.QueryString["keyword"] %></title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
	<a href="/UrlReWrite">/UrlReWrite</a>←重写地址<br />
	<a href="/UrlReWrite/">/UrlReWrite/</a>←真实目录地址<br />
	<a href="/UrlReWrite/List">/UrlReWrite/List</a><br />
	<a href="/UrlReWrite/List/5">/UrlReWrite/List/5</a><br />
	<a href="/UrlReWrite/List/5/2">/UrlReWrite/List/5/2</a><br />
	<a href="/UrlReWrite/Detail/10">/UrlReWrite/Detail/10</a><br />
	<a href="/UrlReWrite/Detail/10.html">/UrlReWrite/Detail/10.html</a><br />
	<a href="/UrlReWrite/Detail/10.html?Tag=<%=Server.UrlEncode("鲜花") %>">/UrlReWrite/Detail/10.html?Tag=<%=Server.UrlEncode("鲜花") %></a><br />
	<a href="/UrlReWrite/Detail/10.html?Tag=<%=Server.UrlEncode("&") %>">/UrlReWrite/Detail/10.html?Tag=<%=Server.UrlEncode("&") %></a><br />
	<a href="/UrlReWrite/yuanchuang">/UrlReWrite/yuanchuang</a><br />
	<a href="/UrlReWrite/Search/<%=Server.UrlEncode("鲜花") %>">/UrlReWrite/Search/鲜花</a><br />
	<a href="/UrlReWrite/Search/<%=Server.UrlEncode("鲜花") %>/3">/UrlReWrite/Search/鲜花/3</a><br />
	<a href="<%=Request.FilePath%>/a/b"><%=Request.FilePath%>/a/b</a><br />
	<a href="<%=Request.FilePath%>/a/b?ClassID=1"><%=Request.FilePath%>/a/b?ClassID=1</a><br />
	<hr />
	File：<%=Request.QueryString["File"] %><br />
	Action：<%=Request.QueryString["Action"] %><br />
	ClassID：<%=Request.QueryString["ClassID"] %><br />
	Page：<%=Request.QueryString["Page"] %><br />
	ID：<%=Request.QueryString["ID"] %><br />
	Search：<%=Request.QueryString["Search"] %><br />
	Tag：<%=Request.QueryString["Tag"] %><br />
	<hr />
	这样写：
	<%=Request.Url.ToString().Substring(0, Request.Url.ToString().IndexOf("/", 10)) + Request.FilePath %><br /><br />
	【请求地址，最全，重写前的虚拟路径，已解码！】<br />
	Request.Url：<%=Request.Url %><br /><br />
	【当前请求地址，重写后的，带附加路径，带参数】<br />
	Request.RawUrl：<%=Request.RawUrl %><br /><br />
	【当前文件虚拟路径，带附加路径，不带参数】<br />
	Request.Path：<%=Request.Path %><br /><br />
	【附加路径，如http://localhost/UrlReWrite.aspx/a/b里的/a/b】<br />
	Request.PathInfo：<%=Request.PathInfo %><br /><br />
	【★当前文件，相对路径，不带附加路径，不带参数，优先用】<br />
	Request.FilePath：<%=Request.FilePath %><br /><br />
	【当前文件虚拟路径，相对根目录，带附加路径，不带参数】<br />
	Request.Url.AbsolutePath：<%=Request.Url.AbsolutePath %><br /><br />
	【当前文件物理路径】<br />
	Request.PhysicalPath：<%=Request.PhysicalPath %><br /><br />
	【当前网站物理路径】<br />
	System.IO.Path.GetDirectoryName：<%=System.IO.Path.GetDirectoryName(Request.PhysicalPath)%><br /><br />
	【★请求地址，未解码】<br />
	Request.Url.AbsoluteUri：<%=Request.Url.AbsoluteUri %><br /><br />
	【当前应用程序】<br />
	Request.ApplicationPath：<%=Request.ApplicationPath %><br /><br />
	【当前虚拟目录】<br />
	Request.VirtualPath：<%=Request.ApplicationPath %><br /><br />
	【主机】<br />
	Request.Url.Host：<%=Request.Url.Host %><br /><br />
	【请求路径，基于操作系统有所区别】<br />
	Request.Url.LocalPath：<%=Request.Url.LocalPath %><br /><br />
	【★路径+请求参数，未解码】<br />
	Request.Url.PathAndQuery：<%=Request.Url.PathAndQuery %><br /><br />
	【非匿名访问下才有返回】<br />
	Request.Url.UserInfo：<%=Request.Url.UserInfo %><br />
	Request.Url.Scheme：<%=Request.Url.Scheme %><br />
	Request.Url.Query：<%=Request.Url.Query %><br />
	Request.Url.Port：<%=Request.Url.Port %><br />
	Request.Url.OriginalString：<%=Request.Url.OriginalString %><br />
	Request.Url.Fragment：<%=Request.Url.Fragment %><br />

</body>
</html>
