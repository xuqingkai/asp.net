<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%Response.ContentType="application/json";%><%=IP()%>
<script runat="server">
public static string IP()
{
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
	System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
	if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); }

	string sql = "SELECT TOP 100 * FROM [xqk_ip_log] ORDER BY ID DESC;";
	System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(sql, connection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.OleDb.OleDbDataAdapter(command).Fill(dataTable);
	
	string result = "";
        foreach(System.Data.DataRow dr in dataTable.Rows){
            result += dr["id"] + ",【" + dr["address"] + "】:" + dr["create_datetime"] + "<br />\r\n";
        }
	connection.Close();
	connection.Dispose();

	return result;
}
</script>

