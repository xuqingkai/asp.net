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
    System.Configuration.ConnectionStringSettingsCollection connectionStringSettingsCollection = System.Configuration.ConfigurationManager.ConnectionStrings;
    for (int i = 0; i < connectionStringSettingsCollection.Count; i++){
        System.Configuration.ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[i];
    %>
    
        <h1>【ConnectionStrings["<%=connectionStringSettings.Name%>"]】： ProviderName=<%=connectionStringSettings.ProviderName%></h1>
        <p><%=connectionStringSettings.ConnectionString%></p>
    
    <%}%>
    
    <hr />
    
    <%
    System.Collections.Specialized.NameValueCollection nameValueCollection = System.Configuration.ConfigurationManager.AppSettings;
    foreach (string key in nameValueCollection.AllKeys){
    %>
    
        <h1>【AppSettings["<%=key%>"]】</h1>
        <p><%=nameValueCollection[key]%></p>
    
    <%}%>
</body>
</html>
