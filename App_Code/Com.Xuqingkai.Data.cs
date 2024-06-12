using System.Linq;
/// <summary>
/// xuqingkai.com
/// </summary>
namespace Com.Xuqingkai
{
	/// <summary>
	/// 数据库
	/// </summary>
	public partial class Data : Common
    {	
		/// <summary>
        /// SQL执行错误日志
        /// </summary>
		public string SqlExecuteLog = null;
		
		/// <summary>
        /// SQL执行错误日志
        /// </summary>
        protected void DebugSql (string filePath = null)
        {
			if(filePath == null)
			{
				Clear(SqlExecuteLog);
			}
			else
			{
				TxtFile(filePath, SqlExecuteLog);
			}
		}
		
		/// <summary>
        /// 数据库类型
        /// </summary>
		private string _databaseProviderName = null;

		/// <summary>
        /// 数据库连接字符串
        /// </summary>
		private string _databaseConnectionString = null;

        /// <summary>
        /// 数据库
        /// </summary>
        public Data()
        {
			System.Configuration.ConnectionStringSettings database = System.Configuration.ConfigurationManager.ConnectionStrings["Com.Xuqingkai.Data"];
			if(database != null)
			{
				_databaseProviderName = database.ProviderName.ToLower();
				string connectionString = database.ConnectionString;
				_databaseConnectionString = DatabaseConnectionString(connectionString);
				
				string publicKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjFBZndsLzN0UkN6M2hXMzB4MXdnVjN3eGpXRVZ3N1AxeU9IOU5wVjZtNkdGcXZVay9Xc2MrT1lEbzZMeGx1ejJUVVl4Y3NCT3VnM1Y5b3NlaldZSHRvd2VRODZFN3IvM3lGOFQxVFh4WUJnd0RRcGZsczZHSG55NWdMMC9mU01Lbm5YZ1ZxSml6OTBuVXpBcUVOWStGQXFVbmdocUlrT2NTRXQ0aERad1Qwaz08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+";
				System.Security.Cryptography.RSACryptoServiceProvider publicRSA = new System.Security.Cryptography.RSACryptoServiceProvider();
				publicRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(publicKey)));
				connectionString = RSAEncrypt(publicRSA, connectionString);
			}
			else
			{
				database = System.Configuration.ConfigurationManager.ConnectionStrings["Com.Xuqingkai.Encrypt"];
				if(database != null)
				{
					_databaseProviderName = database.ProviderName.ToLower();
					string connectionString = database.ConnectionString;
					string privateKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjFBZndsLzN0UkN6M2hXMzB4MXdnVjN3eGpXRVZ3N1AxeU9IOU5wVjZtNkdGcXZVay9Xc2MrT1lEbzZMeGx1ejJUVVl4Y3NCT3VnM1Y5b3NlaldZSHRvd2VRODZFN3IvM3lGOFQxVFh4WUJnd0RRcGZsczZHSG55NWdMMC9mU01Lbm5YZ1ZxSml6OTBuVXpBcUVOWStGQXFVbmdocUlrT2NTRXQ0aERad1Qwaz08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjdOaFFwVWhhTWc0akpnRDVyR3Foa08zcUNvUWVMR3FnV0VZZFRrZzJTQytwZDdjWGErRkMvRE44MFlRRGFpbWtPRHpnN0V5T3pVOFZKaWN0Tk5oWlh3PT08L1A+PFE+NVMzZkgreGxTVTBXenF2Y2pnMG5kdmZDaDAyWXVEcklvVzlYanNWcVl2K041dm5VN0QrYTBtL29KaTRoRW1zSUxwMEZ1Wk0vbm8weXBnMVBBU3dRVnc9PTwvUT48RFA+WmFxbFU0MnZTZlZQbVN5cUFCejFwYVM2NWpDNFV4Q2lLOEpOS2lGTlM4ZEowNTNBYkFxU0duaHZoL0JIRnZjeEN3TDIrZmdUQklhQjZaSy9rUU9kdVE9PTwvRFA+PERRPlBtVmQwVUE0VjF1d3NQWWpwMTAzZUhGK09mNUxiU1U4Q0kvYTQ5a2wzT3c5QXp6VDFycCtlWHJVVnNqS1lreFh2dkVyWk5vTlFTcXFOb0Y3R0JPYUl3PT08L0RRPjxJbnZlcnNlUT5LZ2xWR04wU1NBVjJnZE1IOU5semJ3K09INFdTNno3ZW1TbEhVL1BJbG04SzNLNWlNUmd6Ryt1ekhDbXlBUzJBMG5DdDErQXFScjJLU1JrRlMwbXRvUT09PC9JbnZlcnNlUT48RD5yeXZnR2JJdjA2TlMyMmw4VVRoTGYvdWE5TExBeUc1bElSR1RvUVRkeHJZck9KSlBmUTZCWVNDbTRRbzlqZk0zaVovME9sbW1zUSs5TlhLNlN4ZUFSazk0bFJVY3FmQ0ZaeU9BOUNmT3pIOG84NHFscTlnTlRlaHY3MS9KMGRzejZIZS9OSkVrNmxZVHFUdjkxOFk3cnVRVjNtd0szRG5pNW9ZK0xsNHF4Z0U9PC9EPjwvUlNBS2V5VmFsdWU+";
					System.Security.Cryptography.RSACryptoServiceProvider privateRSA = new System.Security.Cryptography.RSACryptoServiceProvider();
					privateRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(privateKey)));
					connectionString = RSADecrypt(privateRSA, connectionString);
					_databaseConnectionString = DatabaseConnectionString(connectionString);
				}
			}
        }
		
		/// <summary>
        /// 数据库：SqlServer://sa:123456@127.0.0.1/database
        /// </summary>
        public Data(string database)
        {
			_databaseConnectionString = DatabaseConnectionString(database);
        }
		
		/// <summary>
        /// DatabaseConnectionString
        /// </summary>
		public string DatabaseConnectionString (string database, string providerName = null)
		{
			string databaseConnectionString = null;
			if(database.ToLower().StartsWith("sqlserver://"))
			{
				//SqlServer://sa:123456@127.0.0.1/database
				System.Uri uri = new System.Uri(database);
				_databaseProviderName = uri.Scheme.ToLower();
				databaseConnectionString += "server=" + uri.Authority + uri.LocalPath.Substring(0, uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/")).Replace("/","\\") + ";";
				databaseConnectionString += "database=" + uri.LocalPath.TrimEnd("/".ToCharArray()).Substring(uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/") + 1) + ";";
				databaseConnectionString += "uid=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(":"))) + ";";
				databaseConnectionString += "pwd=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
				//Clear(databaseConnectionString);
			}
			else if(database.ToLower().StartsWith("sqlsource://"))
			{
				//sqlsource://sa:123456@localhost/sqlexpress/database
				System.Uri uri = new System.Uri(database);
				_databaseProviderName = "sqlserver";
				databaseConnectionString += "Data Source=" + uri.Authority + uri.LocalPath.Substring(0, uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/")).Replace("/","\\") + ";";
				databaseConnectionString += "Initial Catalog=" + uri.LocalPath.TrimEnd("/".ToCharArray()).Substring(uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/") + 1) + ";";
				databaseConnectionString += "User ID=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(":"))) + ";";
				databaseConnectionString += "Password=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
			}
			else if(database.ToLower().StartsWith("sqloledb://"))
			{
				//SqlOleDb://sa:123456@localhost/sqlexpress/database
				System.Uri uri = new System.Uri(database);
				_databaseProviderName = uri.Scheme.ToLower();
				databaseConnectionString += "Provider=SqlOleDb;";
				databaseConnectionString += "Data Source=" + uri.Authority + uri.LocalPath.Substring(0, uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/")).Replace("/","\\") + ";";
				databaseConnectionString += "Initial Catalog=" + uri.LocalPath.TrimEnd("/".ToCharArray()).Substring(uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/") + 1) + ";";
				databaseConnectionString += "User ID=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(":"))) + ";";
				databaseConnectionString += "Password=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
			}
			else if(database.ToLower().StartsWith("jetoledb://"))
			{
				System.Uri uri = new System.Uri(database);
				_databaseProviderName = uri.Scheme.ToLower();
				databaseConnectionString += "Provider=Microsoft.Jet.Oledb.4.0;";
				databaseConnectionString += "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("/") + uri.OriginalString.Substring(uri.OriginalString.IndexOf("/",15) + 1) + ";";
				string password = System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
				if(password != null && password.Length > 0) { databaseConnectionString += "Jet OLEDB:Database Password=" + ";"; }
			}
			else if(database.ToLower().StartsWith("aceoledb://"))
			{
				System.Uri uri = new System.Uri(database);
				_databaseProviderName = uri.Scheme.ToLower();
				databaseConnectionString += "Provider=Microsoft.Ace.Oledb.12.0;";
				databaseConnectionString += "Data Source=" + System.Web.HttpContext.Current.Server.MapPath("/") + uri.OriginalString.Substring(uri.OriginalString.IndexOf("/",15) + 1) + ";";
				string password = System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
				if(password != null && password.Length > 0) { databaseConnectionString += "Database Password=" + ";"; }

			}
			else
			{
				if(providerName != null){ _databaseProviderName = providerName; }
				databaseConnectionString = database;
			}
			return databaseConnectionString;
		}

		/// <summary>
        /// Transaction
        /// </summary>
		protected int Transaction(System.Collections.ArrayList sql, System.Collections.ArrayList parameterArrayList = null)
        {
			return _databaseProviderName == "sqlserver" ? SqlTransaction(sql, parameterArrayList) : OleDbTransaction(sql, parameterArrayList);
		}
		
		/// <summary>
        /// OleDbTransaction
        /// </summary>
		protected int OleDbTransaction(System.Collections.ArrayList sql,System.Collections.ArrayList parameterArrayList = null)
		{
			int rows = 1;
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_databaseConnectionString);
            if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
			System.Data.OleDb.OleDbTransaction oleDbTransaction = oleDbConnection.BeginTransaction();
			try
			{
				for(int i=0; i<sql.Count; i++)
				{
					SqlExecuteLog += sql[i] + "\r\n";
					System.Data.OleDb.OleDbCommand oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.Transaction = oleDbTransaction;
					oleDbCommand.CommandText = sql[i] + "";
					if(parameterArrayList[i] != null)
					{
						if(parameterArrayList[i].GetType().ToString() == "System.Collections.Specialized.NameValueCollection")
						{
							System.Collections.Specialized.NameValueCollection parameters = (System.Collections.Specialized.NameValueCollection)parameterArrayList[i];
							foreach (string key in parameters.AllKeys)
							{
								oleDbCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(key, parameters[key]));
							}	
						}
						else
						{
							System.Collections.Generic.Dictionary<string, string> parameters = (System.Collections.Generic.Dictionary<string, string>)parameterArrayList[i];
							foreach (string key in parameters.Keys)
							{
								oleDbCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(key, parameters[key]));
							}
						}
					}
					oleDbCommand.ExecuteNonQuery();
				}
				oleDbTransaction.Commit();
			}
			catch(System.Exception exception)
            {
				rows = -1;
				oleDbTransaction.Rollback();
				SqlExecuteLog += exception.Message + "\r\n";
            }
			finally
			{
				oleDbConnection.Close();
				oleDbConnection.Dispose();
			}
            return rows;
		}
		
		/// <summary>
        /// SqlTransaction
        /// </summary>
		protected int SqlTransaction(System.Collections.ArrayList sql, System.Collections.ArrayList parameterArrayList = null)
		{
			int rows = 1;
			System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(_databaseConnectionString);			
			if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }
			System.Data.SqlClient.SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
			try
			{
				for(int i=0; i<sql.Count; i++)
				{
					SqlExecuteLog += sql[i] + "\r\n";
					System.Data.SqlClient.SqlCommand sqlCommand = sqlConnection.CreateCommand();
					sqlCommand.Transaction = sqlTransaction;
					sqlCommand.CommandText = sql[i] + "";
					if(parameterArrayList[i] != null)
					{
						if(parameterArrayList[i].GetType().ToString() == "System.Collections.Specialized.NameValueCollection")
						{
							System.Collections.Specialized.NameValueCollection parameters = (System.Collections.Specialized.NameValueCollection)parameterArrayList[i];
							foreach (string key in parameters.AllKeys)
							{
								sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(key, parameters[key]));
							}	
						}
						else
						{
							System.Collections.Generic.Dictionary<string, string> parameters = (System.Collections.Generic.Dictionary<string, string>)parameterArrayList[i];
							foreach (string key in parameters.Keys)
							{
								sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(key, parameters[key]));
							}
						}
					}
					sqlCommand.ExecuteNonQuery();
				}
				sqlTransaction.Commit();
			}
			catch(System.Exception exception)
            {
				rows = -1;
				sqlTransaction.Rollback();
				SqlExecuteLog += exception.Message + "\r\n";
            }
			finally
			{
				sqlConnection.Close();
				sqlConnection.Dispose();
			}
            return rows;
		}

		/// <summary>
        /// OleDbExecuteNonQuery
        /// </summary>
        protected int OleDbExecuteNonQuery(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            int rows = 0;
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_databaseConnectionString);
            if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
			
            System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
			if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { oleDbCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					oleDbCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(key, parameters[key]));
				}
			}
			
			SqlExecuteLog += sql + "\r\n";
            try
            {
                rows = oleDbCommand.ExecuteNonQuery();
            }
			catch(System.Exception exception)
            {
                rows = -1;
				SqlExecuteLog += exception.Message + "\r\n";
            }
            finally
            {
                oleDbConnection.Close();
                oleDbConnection.Dispose();
            }
            return rows;
        }
		
		/// <summary>
		/// OleDbExecuteScalar
		/// </summary>
		protected object OleDbExecuteScalar(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
		{
			object result = null;
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_databaseConnectionString);
			if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
			System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
			if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { oleDbCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					oleDbCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(key, parameters[key]));
				}
			}
			SqlExecuteLog += sql + "\r\n";
			try
			{
				result = oleDbCommand.ExecuteScalar();
			}
			catch(System.Exception exception)
            {
				SqlExecuteLog += exception.Message + "\r\n";
            }
			finally
			{
				oleDbConnection.Close();
				oleDbConnection.Dispose();
			}
			return result;
		}
		
		/// <summary>
        /// OleDbDataAdapter
        /// </summary>
        protected System.Data.DataTable OleDbDataAdapter(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_databaseConnectionString);
			if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
            System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
            if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { oleDbCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					oleDbCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter(key, parameters[key]));
				}
			}
			SqlExecuteLog += sql + "\r\n";
			try
            {
                new System.Data.OleDb.OleDbDataAdapter(oleDbCommand).Fill(dataTable);
            }
            catch(System.Exception exception)
            {
				SqlExecuteLog += exception.Message + "\r\n";
            }
            finally
            {
                oleDbConnection.Close();
                oleDbConnection.Dispose();
            }
            return dataTable;
        }
	
		/// <summary>
        /// SqlExecuteNonQuery
        /// </summary>
        protected int SqlExecuteNonQuery(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
			int rows = 0;
			System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(_databaseConnectionString);
			if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }
			System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
			if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(key, parameters[key])); 
				}
			}
			SqlExecuteLog += sql + "\r\n";
			try
			{
				rows = sqlCommand.ExecuteNonQuery();
			}
			catch(System.Exception exception)
            {
				rows = -1;
				SqlExecuteLog += exception.Message + "\r\n";
            }
			finally
			{
				sqlConnection.Close();
				sqlConnection.Dispose();
			}
            return rows;
        }
		
		/// <summary>
        /// SqlExecuteScalar
        /// </summary>
        protected object SqlExecuteScalar(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            object result = null;
			System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(_databaseConnectionString);
            if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }
			System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
            if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(key, parameters[key])); 
				}
			}
			SqlExecuteLog += sql + "\r\n";
			try
            {
                result = sqlCommand.ExecuteScalar();
            }
            catch(System.Exception exception)
            {
				SqlExecuteLog += exception.Message + "\r\n";
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return result;
        }

		/// <summary>
        /// SqlDataAdapter
        /// </summary>
        protected System.Data.DataTable SqlDataAdapter(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(_databaseConnectionString);
            if (sqlConnection.State != System.Data.ConnectionState.Open) { sqlConnection.Open(); }
			System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql, sqlConnection);
            if(parameters != null)
			{
				if(parameters.Count>0 && parameters.Keys[0].Substring(0, 1) == "@") { sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;}
				foreach (string key in parameters.AllKeys)
				{
					sqlCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(key, parameters[key])); 
				}
			}
			SqlExecuteLog += sql + "\r\n";
			try
            {
                new System.Data.SqlClient.SqlDataAdapter(sqlCommand).Fill(dataTable);
            }
            catch(System.Exception exception)
            {
				SqlExecuteLog += exception.Message + "\r\n";
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return dataTable;
        }
		
		/// <summary>
        /// ExecuteNonQuery
        /// </summary>
		protected int ExecuteNonQuery(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
			return _databaseProviderName == "sqlserver" ? SqlExecuteNonQuery(sql, parameters) : OleDbExecuteNonQuery(sql, parameters);
		}

		/// <summary>
        /// ExecuteScalar
        /// </summary>
		protected object ExecuteScalar(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
			return _databaseProviderName == "sqlserver" ? SqlExecuteScalar(sql, parameters) : OleDbExecuteScalar(sql, parameters);
		}
		
		/// <summary>
        /// DataAdapter
        /// </summary>
		protected System.Data.DataTable DataAdapter(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
			return _databaseProviderName == "sqlserver" ? SqlDataAdapter(sql, parameters) : OleDbDataAdapter(sql, parameters);
		}
        
		/// <summary>
        /// DataTable
        /// </summary>
        protected System.Data.DataTable DataTable(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Data.DataTable dataTable = DataAdapter(sql, parameters);
            return dataTable;
        }
		
		/// <summary>
        /// DataRow
        /// </summary>
        protected System.Data.DataRow DataRow(string sql, System.Collections.Specialized.NameValueCollection parameters = null, bool newRow = false)
        {
            System.Data.DataTable dataTable = DataTable(sql, parameters);
            if (dataTable.Rows.Count == 0) { return newRow ? dataTable.NewRow() : null; }
            return dataTable.Rows[0];
        }
		
		/// <summary>
        /// DataRow
        /// </summary>
        protected System.Data.DataRow DataRow(string sql, bool newRow)
        {
            System.Data.DataTable dataTable = DataTable(sql);
            if (dataTable.Rows.Count == 0) { return newRow ? dataTable.NewRow() : null; }
            return dataTable.Rows[0];
        }
		
		/// <summary>
        /// DataRow
        /// </summary>
        protected System.Collections.Specialized.NameValueCollection DataNvc(string sql, System.Collections.Specialized.NameValueCollection parameters = null, bool newRow = false)
        {
            return ToNvc(DataRow(sql, parameters, newRow)) ?? new System.Collections.Specialized.NameValueCollection();
        }
		
		/// <summary>
        /// DataRows
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="pageSize">每页条数</param>
        protected System.Data.DataRowCollection DataRows(string sql, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            return DataTable(sql, parameters).Rows;
        }
		
		/// <summary>
        /// 插入/新建数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameters">参数</param>
		protected int DataInsert(string sql, System.Collections.Specialized.NameValueCollection parameters)
		{
			string suffix = ";SELECT SCOPE_IDENTITY();";
			int lastID = 0;
			if(parameters != null && parameters.Count > 0)
			{
				if(!sql.ToUpper().StartsWith("INSERT INTO "))
				{
					sql = "INSERT INTO " + sql + " (";
					foreach(string key in parameters.Keys) { sql += "[" + key + "],"; }
					sql = sql.Substring(0, sql.Length - 1) + ") VALUES (";
					foreach(string key in parameters.Keys) { sql += _databaseProviderName == "sqlserver" ? "@" + key + "," : "?"; }
					sql = sql.Substring(0, sql.Length - 1) + ")" + suffix;
				}
				if(!sql.ToUpper().EndsWith(suffix.ToUpper())){ sql += suffix; }
				lastID = System.Convert.ToInt32("0" + ExecuteScalar(sql, parameters));
			}
			return lastID;
		}
		
		/// <summary>
        /// 更新/修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameters">参数</param>
        /// <param name="sqlWhere">条件</param>
		protected int DataUpdate(string sql, System.Collections.Specialized.NameValueCollection parameters, string sqlWhere)
		{
			int count = 0;
			if(parameters != null && parameters.Count > 0)
			{
				if(!sql.ToUpper().StartsWith("UPDATE "))
				{
					sql = "UPDATE " + sql + " SET";
					foreach(string key in parameters.Keys) { sql += "[" + key + "]=" + (_databaseProviderName == "sqlserver" ? "@" + key : "?") + ","; }
					sql = sql.Trim(",".ToCharArray());
					if(!string.IsNullOrEmpty(sqlWhere)){ sql += " WHERE " + sqlWhere;}
				}
				count = System.Convert.ToInt32("0" + ExecuteNonQuery(sql, parameters));
			}
			return count;
		}
		
		/// <summary>
        /// 数据求和
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="field">求和字段</param>
        /// <param name="parameters">检索条件</param>
		protected decimal DataStat(string sql,System.Collections.Specialized.NameValueCollection parameters)
		{
			return ToDecimal("0" + ExecuteScalar(sql, parameters));
		}
	


	
		/// <summary>
        /// 总条数
        /// </summary>
		protected double PagerCount = 0;
		
		/// <summary>
        /// 每页条数
        /// </summary>
		protected int PagerSize = 0;
		
		/// <summary>
        /// DataRows
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="pageSize">每页条数</param>
        protected System.Data.DataRowCollection DataRows(string sql, System.Collections.Specialized.NameValueCollection parameters, int pageSize)
        {
            if (pageSize <= 0) { return DataRows(sql, parameters);}
			
			PagerSize = pageSize;
			string countSql = sql.Substring(0, sql.Contains(" ORDER BY ") ? sql.IndexOf(" ORDER BY ") : sql.Length);
			countSql = "SELECT COUNT(0)" + countSql.Substring(countSql.IndexOf(" FROM "));
			PagerCount = System.Convert.ToDouble("0" + ExecuteScalar(countSql, parameters));
			
			int orderIndexOf = sql.Contains(" ORDER BY ") ? sql.IndexOf(" ORDER BY ") : sql.Length;
			string withSql = sql.Substring(0, orderIndexOf).Replace(" FROM ", ",Row_Number() OVER (" + sql.Substring(orderIndexOf) + ") AS Row_Number_SortID FROM ");
			withSql = "WITH Row_Number_Table AS (" + withSql + ")";
			
            if (pageSize <= 0) { pageSize = 1; }
            int page = 0;
            System.Int32.TryParse("0" + System.Web.HttpContext.Current.Request.QueryString["page"], out page);
            int start = pageSize * (page > 0 ? page - 1 : 0);
            int end = start + pageSize;
			withSql += "SELECT * FROM Row_Number_Table WHERE (Row_Number_SortID BETWEEN " + (start + 1) + " AND " + end + ")";
			//System.Web.HttpContext.Current.Response.Write(withSql);
            return DataRows(withSql, parameters);
        }
		
		/// <summary>
        /// DataRows
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="pageSize">每页条数</param>
        protected System.Data.DataRowCollection DataRows(string fields,string table_where, string orderBy, System.Collections.Specialized.NameValueCollection parameters, int pageSize)
        {
            if (pageSize <= 0) 
			{
				string sql = "SELECT TOP " + System.Math.Abs(pageSize) + " " + fields;
				sql += " FROM " + table_where + " ORDER BY " + orderBy + "";
				return DataRows(sql, parameters);
			}
			else
			{
				PagerSize = pageSize;
				string countSql = "SELECT COUNT(0) FROM " + table_where + "";
				PagerCount = System.Convert.ToDouble("0" + ExecuteScalar(countSql));
				string sql = "SELECT " + fields + ",Row_Number() OVER (ORDER BY " + orderBy + ") AS Row_Number_SortID";
				sql += " FROM " + table_where + "";
				sql = "WITH Row_Number_Table AS (" + sql + ")";
				
				if (pageSize <= 0) { pageSize = 1; }
				int page = 0;
				System.Int32.TryParse("0" + System.Web.HttpContext.Current.Request.QueryString["page"], out page);
				int start = pageSize * (page > 0 ? page - 1 : 0);
				int end = start + pageSize;
				sql += "SELECT * FROM Row_Number_Table WHERE (Row_Number_SortID BETWEEN " + (start + 1) + " AND " + end + ")";
				//System.Web.HttpContext.Current.Response.Write(withSql);
				return DataRows(sql, parameters);
			}
        }
		/// <summary>
		/// 分页
		/// </summary>
		/// <param name="url">地址栏模版，如：/news-{cid}-{page}.html，不设置则为请求参数式分页</param>
		/// <param name="currentClassName">当前页样式类名</param>
		/// <param name="size">当前页左右页码个数</param>
		/// <returns></returns>
		public string DataPage(string url = null, string currentClassName = "current", int size = 4)
		{
			if(currentClassName != null && currentClassName.Length > 0)
			{
				currentClassName = " class=\"" + currentClassName + "\"";
			}
			//生成页码
			string html = "";
			System.Collections.Specialized.NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(Request.QueryString.ToString());
			if(PagerCount < 1)
			{
				PagerCount = 1; 
				return null;
			}
			double max =  System.Math.Ceiling(PagerCount/PagerSize);
			double page = System.Convert.ToDouble("0" + nvc["page"]);
			if(page < 1){page = 1;}
			if (page > max) { page = max; }
			double first = page - System.Math.Abs(size);
			if (first < 1) { first = 1; }
			double last = page + System.Math.Abs(size);
			if (last > max) { last = max; }
			nvc.Remove("page");
			if (url == null || url.Length == 0)
			{
				url = System.Web.HttpContext.Current.Request.FilePath + "?" + ToQueryString(nvc) + (nvc.Count > 0 ? "&" : "");
				if(first > 1)
				{
					html += "　<a href=\"" + url + "page=1\">首页</a>";
				}
				if (page > 1)
				{
					html += "　<a href=\"" + url + "page=" + (page - 1) + "\">上页</a>";
				}
				if (page > System.Math.Abs(size)*2 && size > 0)
				{
					html += "　<a href=\"" + url + "page=" + (page - System.Math.Abs(size)*2 - 1) + "\">...</a>";
				}
				for (double i = first; i <= last; i++)
				{
					html += "　<a href=\"" + url + "page=" + i + "\"" + (i == page ? currentClassName : null) + ">" + i + "</a>";
				}
				if(page < max - System.Math.Abs(size)*2 && size > 0)
				{
					html += "　<a href=\"" + url + "page=" + (page + System.Math.Abs(size)*2 + 1) + "\">...</a>";
				}
				if(page < last)
				{
					html += "　<a href=\"" + url + "page=" + (page + 1) + "\">下页</a>";
				}
				if(last < max)
				{
					html += "　<a href=\"" + url + "page=" + max + "\">末页</a>";
				}
				if(html != null && html.Length > 0)
				{
					html = html.Substring(1);
				}
				html += "　<a>共" + max + "页," + PagerCount + "条</a>";
			}
			else if (url == "li" || url == "li:")
			{
				url = System.Web.HttpContext.Current.Request.FilePath + "?" + nvc.ToString() + (nvc.ToString().Length > 0 ? "&" : "");
				if(first > 1)
				{
					html += "<li><a href=\"" + url + "page=1\">首页</a></li>";
				}
				if (page > 1)
				{
					html += "<li><a href=\"" + url + "page=" + (page - 1) + "\">上页</a></li>";
				}
				if (page > System.Math.Abs(size)*2 && size > 0)
				{
					html += "<li><a href=\"" + url + "page=" + (page - System.Math.Abs(size)*2 - 1) + "\">...</a></li>";
				}
				for (double i = first; i <= last; i++)
				{
					html += "<li" + (i == page ? currentClassName : null) + "><a href=\"" + url + "page=" + i + "\"" + (i == page ? currentClassName : null) + ">" + i + "</a></li>";
				}
				if(page < max - System.Math.Abs(size)*2 && size > 0)
				{
					html += "<li><a href=\"" + url + "page=" + (page + System.Math.Abs(size)*2 + 1) + "\">...</a></li>";
				}
				if(page < last)
				{
					html += "<li><a href=\"" + url + "page=" + (page + 1) + "\">下页</a></li>";
				}
				if(last < max)
				{
					html += "<li><a href=\"" + url + "page=" + last + "\">末页</a></li>";
				}
				html += "<li><a>共" + max + "页," + PagerCount + "条</a></li>";
				html = "<ul>" + html + "</ul>";
			}
			else if (url.StartsWith("li:"))
			{
				foreach (string key in nvc)
				{
					url = url.Replace(("{" + key + "}").ToLower(), System.Web.HttpUtility.UrlEncode(nvc[key]));
				}
				if(first > 1)
				{
					html += "<li><a href=\"" + url + "page=1\">首页</a></li>";
				}
				if (page > 1)
				{
					html += "<li><a href=\"" + url + "page=" + (page - 1) + "\">上页</a></li>";
				}
				if (page > System.Math.Abs(size)*2 && size > 0)
				{
					html += "<li><a href=\"" + url + "page=" + (page - System.Math.Abs(size)*2 - 1) + "\">...</a></li>";
				}
				for (double i = first; i <= last; i++)
				{
					html += "<li" + (i == page ? currentClassName : null) + "><a href=\"" + url.Replace("{page}", i.ToString()) + "\"" + (i == page ? currentClassName : null) + ">" + i + "</a></li>";
				}
				if(page < max - System.Math.Abs(size)*2 && size > 0)
				{
					html += "<li><a href=\"" + url + "page=" + (page + System.Math.Abs(size)*2 + 1) + "\">...</a></li>";
				}
				if(page < last)
				{
					html += "<li><a href=\"" + url + "page=" + (page + 1) + "\">下页</a></li>";
				}
				if(last < max)
				{
					html += "<li><a href=\"" + url + "page=" + last + "\">末页</a></li>";
				}
				html += "<li><a>共" + max + "页," + PagerCount + "条</a></li>";
				html = "<ul>" + html + "</ul>";
			}
			else
			{
				foreach (string key in nvc)
				{
					url = url.Replace(("{" + key + "}").ToLower(), System.Web.HttpUtility.UrlEncode(nvc[key]));
				}
				if(first > 1)
				{
					html += "　<a href=\"" + url + "page=1\">首页</a>";
				}
				if (page > 1)
				{
					html += "　<a href=\"" + url + "page=" + (page - 1) + "\">上页</a>";
				}
				if (page > System.Math.Abs(size)*2 && size > 0)
				{
					html += "　<a href=\"" + url + "page=" + (page - System.Math.Abs(size)*2 - 1) + "\">...</a>";
				}
				for (double i = first; i <= last; i++)
				{
					html += "　<a href=\"" + url.Replace("{page}", i.ToString()) + "\"" + (i == page ? currentClassName : null) + ">" + i + "</a>";
				}
				if(page < max - System.Math.Abs(size)*2 && size > 0)
				{
					html += "　<a href=\"" + url + "page=" + (page + System.Math.Abs(size)*2 + 1) + "\">...</a>";
				}
				if(page < last)
				{
					html += "　<a href=\"" + url + "page=" + (page + 1) + "\">下页</a>";
				}
				if(last < max)
				{
					html += "　<a href=\"" + url + "page=" + last + "\">末页</a>";
				}
				if(html != null && html.Length > 0)
				{
					html = html.Substring(1);
				}
				html += "　<a>共" + max + "页," + PagerCount + "条</a>";
			}
			return html;
		}
    }
}
