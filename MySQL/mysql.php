<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>MYSQL</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
<%
	string databaseConnectionString = "Server=127.0.0.1;Port=3306;DataBase=dbname;User Id=root;Password=123456;";
	MySql.Data.MySqlClient.MySqlConnection mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(databaseConnectionString);
	try
	{
		if (mySqlConnection.State != System.Data.ConnectionState.Open) { mySqlConnection.Open(); }
        
        string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);

        string sql = "SELECT * FROM [xqk_ip_log] WHERE [ip]='" + ip + "' AND [create_timestamp]>" + (timestamp - 60*20)  + " ORDER BY ID DESC;";
        MySql.Data.MySqlClient.MySqlCommand sqlCommand = new MySql.Data.MySqlClient.MySqlCommand(sql, sqlConnection);
        System.Data.DataTable dataTable = new System.Data.DataTable();
        new MySql.Data.MySqlClient.MySqlDataAdapter(sqlCommand).Fill(dataTable);
        if (dataTable.Rows.Count == 0) {
            sql = "INSERT INTO [xqk_ip_log] ([ip], [create_date], [create_datetime], [create_timestamp]) VALUES ('" + ip + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + timestamp + ");";
            sqlCommand = new MySql.Data.MySqlClient.MySqlCommand(sql, sqlConnection);
            int rows = sqlCommand.ExecuteNonQuery();
        }else{
            System.Data.DataRow dr = dataTable.Rows[0];
            ip = dr["ip"].ToString();
        }
	}
	catch(System.Exception exception)
	{
		Response.Write(exception.Message);
	}
	finally
	{
		mySqlConnection.Close();
		mySqlConnection.Dispose();
		Response.Write("<hr />finally");
	}
%>
</body>
</html>
