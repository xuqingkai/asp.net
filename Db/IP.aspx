﻿<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%
System.Configuration.ConnectionStringSettings database = System.Configuration.ConfigurationManager.ConnectionStrings["Database"];
if(database.ProviderName == "System.Data.OleDb")
{
	Response.Redirect("./OleDb/IP.aspx");
}
else if(database.ProviderName == "System.Data.SqlClient")
{
	Response.Redirect("./SqlClient/IP.aspx");
}
else
{
}
%>