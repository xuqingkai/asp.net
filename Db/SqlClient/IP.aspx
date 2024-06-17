<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%IP(Request.QueryString["ConnectionStringName"] ?? "Database");%>
<script runat="server">
public string IP(string connectionStringName)
{
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
	System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnectionString);
	if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }

	string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
	double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);

	string sql = "SELECT * FROM [xqk_ip_log] WHERE 1=1 ORDER BY ID DESC;";
	System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.SqlClient.SqlDataAdapter(sqlCommand).Fill(dataTable);
	
	foreach(System.Data.DataRow dr in dataTable.Rows)
	{
		Response.Write(dr["id"] + "【" + dr["ip"] + "】" + dr["create_datetime"] + "<hr />");
	}

	sqlConnection.Close();
	sqlConnection.Dispose();

	return ip;
}
</script>
