<%@Page Language="C#"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title>ChatGPT</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
<%
string openAISecret = System.Configuration.ConfigurationManager.AppSettings["OpenAI/Secret"];

Response.Clear();
string type=Request.QueryString["type"];
string prompt=Request.QueryString["prompt"];
if(string.IsNullOrEmpty(prompt)){
Response.Write("{\"error\":\"请以Get方式传入prompt参数\"}");
Response.End();
}
string data = "{\"model\":\"text-davinci-003\",\"prompt\":\""+prompt+"\",\"temperature\":0.1,\"max_tokens\":2048,\"top_p\":1,\"frequency_penalty\":0.1,\"presence_penalty\":0.6}";
string url="https://api.openai.com/v1/completions";
if(type=="image/create"){
url="https://api.openai.com/v1/images/generations";
data = "{\"prompt\":\""+prompt+"\",\"n\":3}";

}


System.Net.WebClient webClient = new System.Net.WebClient();
System.Net.ServicePointManager.Expect100Continue = false;
System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
webClient.Headers.Add("Content-Type", "application/json; charset=UTF-8");
webClient.Headers.Add("Authorization", "Bearer " + openAISecret);
byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data));
string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);

Response.Write("" + result);
Response.End();
%>
</body>
</html>








