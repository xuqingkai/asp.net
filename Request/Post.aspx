<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>Post</title>
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
</head>
<body>
<%string url="https://apis.map.qq.com/ws/weather/v1/";%>
<h1><a href="<%=url%>" target="_blank"><%=url%></a></h1>
<%=HttpPost(url,"")%>
<h1>跨域请求，如果正常会显示弹窗。</h1>
<script type="text/javascript">
	$.get('https://apis.map.qq.com/ws/weather/v1/',function(res){
		window.alert(res.message);
	},'json');
</script>
<script runat="server">
    	/// <summary>
		/// POST请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="data">数据</param>
		/// <param name="charset">请求编码</param>
		/// <param name="contentType">响应编码</param>
		/// <returns></returns>
		public string HttpPost(string url, string data, string charset = "UTF-8", string contentType = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			try
			{
				webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding(charset).GetBytes(data));
				result = System.Text.Encoding.GetEncoding(contentType).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpPost:" + ex.Message;
			}
            return result;
		}
 </script>
</body>
</html>
