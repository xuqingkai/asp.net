<%@Page Language="C#" Inherits="System.Web.UI.Page"%>
<%
System.Net.WebClient webClient = new System.Net.WebClient();
webClient.Encoding = System.Text.Encoding.UTF8;
string downloadString = webClient.DownloadString("http://cn.bing.com/HPImageArchive.aspx?format=js&n=1");;
System.Web.Script.Serialization.JavaScriptSerializer javascriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
System.Collections.Generic.Dictionary<string, dynamic> deserialize = javascriptSerializer.Deserialize<dynamic>(downloadString);
Response.Redirect("https://www.bing.com" + deserialize["images"][0]["url"]);
%>

