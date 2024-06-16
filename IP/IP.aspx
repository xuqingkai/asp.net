<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%=VerifyImage(SqlClientIP())%>
<script runat="server">
public static string SqlClientIP()
{
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlClient"].ConnectionString;
	System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnectionString);
	if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }

	string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
	double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);

	string sql = "SELECT * FROM [xqk_ip_log] WHERE [ip]='" + ip + "' AND [create_timestamp]>" + (timestamp - 60*20)  + " ORDER BY ID DESC;";
	System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.SqlClient.SqlDataAdapter(sqlCommand).Fill(dataTable);
	if (dataTable.Rows.Count == 0) {
		sql = "INSERT INTO [xqk_ip_log] ([ip], [create_date], [create_datetime], [create_timestamp]) VALUES ('" + ip + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + timestamp + ");";
		sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
		int rows = sqlCommand.ExecuteNonQuery();
	}else{
		System.Data.DataRow dr = dataTable.Rows[0];
		ip = dr["ip"].ToString();
	}

	sqlConnection.Close();
	sqlConnection.Dispose();

	return ip;
}
public static string OleDbIP()
{
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlClient"].ConnectionString;
	System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
	if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }

	string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
	double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);

	string sql = "SELECT * FROM [xqk_ip_log] WHERE [ip]='" + ip + "' AND [create_timestamp]>" + (timestamp - 60*20)  + " ORDER BY ID DESC;";
	System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.OleDb.OleDbDataAdapter(oleDbCommand).Fill(dataTable);
	if (dataTable.Rows.Count == 0) {
		sql = "INSERT INTO [xqk_ip_log] ([ip], [create_date], [create_datetime], [create_timestamp]) VALUES ('" + ip + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "', '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + timestamp + ");";
		oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
		int rows = oleDbCommand.ExecuteNonQuery();
	}else{
		System.Data.DataRow dr = dataTable.Rows[0];
		ip = dr["ip"].ToString();
	}

	oleDbConnection.Close();
	oleDbConnection.Dispose();

	return ip;
}
public static string VerifyImage(string text)
{
	string fontColor="#FF0000,#0000FF";
	string backColor = "#FFFFFF";
	System.Web.HttpContext.Current.Response.Buffer = true;
	System.Web.HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
	System.Web.HttpContext.Current.Response.Expires = 0;
	System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
	System.Web.HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");

	string code = text;
	System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(code.Length * 16 + 6, 30);
	System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
	graphics.Clear(System.Drawing.ColorTranslator.FromHtml(backColor));//背景色
	System.Drawing.Font font = new System.Drawing.Font("Arial", 20);
	string[] colorArray = fontColor.Split(",".ToCharArray());
	if(System.DateTime.Now.Second%2 == 0)
	{
		System.Array.Reverse(colorArray);
	}

	char[] charArray = code.ToCharArray();
	int random = new System.Random().Next(0,colorArray.Length);
	for (int i = 0; i < charArray.Length; i++)
	{
		string randomColor = colorArray[(i + random) % colorArray.Length];
		System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml(randomColor));
		graphics.DrawString(charArray[i].ToString(), font, brush, i * 16, 0);
	}
	System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
	bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
	System.Web.HttpContext.Current.Response.Cache.SetNoStore();
	System.Web.HttpContext.Current.Response.ClearContent();
	System.Web.HttpContext.Current.Response.ContentType = "image/Png";
	System.Web.HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
	graphics.Dispose();
	bitmap.Dispose();
	System.Web.HttpContext.Current.Response.End();
	return null;
}
</script>

