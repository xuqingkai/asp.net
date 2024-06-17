<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%IP(Request.QueryString["ConnectionStringName"] ?? "Database");%>
<script runat="server">
public string IP(string connectionStringName)
{
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
	System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
	if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); }

	string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
	double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);

	string sql = "SELECT * FROM [xqk_ip_log] WHERE 1=1 ORDER BY ID DESC;";
	System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(sql, connection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.OleDb.OleDbDataAdapter(command).Fill(dataTable);
	
	foreach(System.Data.DataRow dr in dataTable.Rows)
	{
		Response.Write(dr["id"] + "【" + dr["ip"] + "】" + dr["create_datetime"] + "<hr />");
	}

	connection.Close();
	connection.Dispose();

	return ip;
}
</script>
