<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%IP(Request.QueryString["ConnectionStringName"] ?? "Database");%>
<script runat="server">
public string IP(string connectionStringName)
{
    string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
    //驱动程序：http://www.ch-werner.de/sqliteodbc/
    //DRIVER=SQLite3 ODBC Driver; Database=SQLite3.db;
    System.Data.Odbc.OdbcConnection connection = new System.Data.Odbc.OdbcConnection(databaseConnectionString);
    connection.Open();
    string sql = "SELECT * FROM [xqk_ip_log] WHERE 1=1 ORDER BY ID DESC;";
    System.Data.Odbc.OdbcCommand command = new System.Data.Odbc.OdbcCommand(sql, connection);
    System.Data.DataTable dataTable = new System.Data.DataTable();
    new System.Data.Odbc.OdbcDataAdapter(command).Fill(dataTable);
    
    foreach(System.Data.DataRow dr in dataTable.Rows)
    {
        Response.Write(dr["id"] + "【" + dr["ip"] + "】" + dr["create_datetime"] + "<hr />");
    }
    
    connection.Close();
    connection.Dispose();
    
    return ip;
}
</script>
