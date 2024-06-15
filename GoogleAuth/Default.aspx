<%@ Page Language="C#" Inherits="Com.Xuqingkai.Common"%>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>谷歌身份验证器</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
<%
string secret = Request.QueryString["secret"] ?? Request.QueryString.ToString();
if(secret == "" && Request.UrlReferrer != null)
{
	secret = Request.UrlReferrer.ToString().ToLower();
	secret = secret.Substring(secret.IndexOf("://") + 3);
	secret = secret.Substring(0, secret.IndexOf("/"));
}
%>
<img src="/qr/?text=otpauth://totp/徐清的老挝:id@<%=Request.Url.Host%>?secret=<%=System.Web.HttpUtility.UrlEncode(GoogleAuth(secret,0))%>&issuer=username" />
<hr /><%=GoogleAuth(secret,0)%>
<hr />
1、直接复制上面的密钥或扫码后再选中复制密钥<br />
2、安装谷歌身份验证器APP（点击下载：<a href="./GoogleAuthenticator.apk" target="_blank">安卓</a>、<a href="https://apps.apple.com/cn/app/google-authenticator/id388497605" target="_blank">苹果</a>）<br />
3、首次安装完成后，点击第一屏的“开始”，第二屏的左下角的“跳过”，进入到下个屏页<br />
4、选择“输入提供的密钥”，进入下一屏页，账号名随便输入（其实就是备注的意思），您的密钥输入上面的密钥字符串<br />
5、添加完成后，就可以看到界面上显示的6个数字，就是谷歌身份验证码了
</body>
</html>
