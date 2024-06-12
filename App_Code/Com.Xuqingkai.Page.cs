using System.Linq;
/// <summary>
/// xuqingkai.com
/// </summary>
namespace Com.Xuqingkai
{
	/// <summary>
	/// 通用页
	/// </summary>
	public partial class Page : Data
    {		
		/// <summary>
        /// 页面载入
        /// </summary>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}
		
		/// <summary>
        /// 网站配置
        /// </summary>
		private System.Collections.Specialized.NameValueCollection _config = null;

		/// <summary>
        /// 网站配置
        /// </summary>
		public string Config(string key)
		{
			if(_config == null){ _config = DataNvc("SELECT * FROM [config]"); }
			if(string.IsNullOrEmpty(key)){
				return null;
			}else if(_config[key] == null && key.Contains(".")){
				string prefix = key.Substring(0, key.IndexOf("."));
				System.Collections.Specialized.NameValueCollection config = DataNvc("SELECT * FROM [" + prefix + "_config]");
				foreach(string key1 in config.Keys){ _config.Set(prefix + "." + key1, config[key1]); }
			}
			return _config[key];
		}

		
		/// <summary>
        /// 登录用户
        /// </summary>
		public System.Collections.Specialized.NameValueCollection _logUser = null;
		
		/// <summary>
        /// 登录用户
        /// </summary>
		public System.Collections.Specialized.NameValueCollection LogUser()
		{
			if(_logUser == null){ _logUser = DataNvc("SELECT * FROM [user] WHERE [user_idkey]='" + SessionGetID("log_user_idkey") + "'");}
			return _logUser;
		}
		
		/// <summary>
        /// 登录用户
        /// </summary>
		public string LogUser(string key)
		{
			_logUser = LogUser();
			return _logUser[key];
		}
				
		/// <summary>
        /// 用户日志
        /// </summary>
		protected void UserLog(object user_idkey, string type_code, string log)
        {
			ExecuteNonQuery("INSERT INTO [user_log] ([user_idkey],[type_code],[contents],[ip],[create_time]) VALUES (" + user_idkey + ",'" + type_code + "','" + log + "','" + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "','" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
		}
		
		/// <summary>
        /// 验证码
        /// </summary>
		protected string UserRegSendCode(string address)
        {
			if(IsMobile(address))
			{
				return null;
			}
			else
			{
				Com.Xuqingkai.Smtp smtp = new Com.Xuqingkai.Smtp("smtp.qq.com:587", "14921995@qq.com", "zntysgboebtlcaff");
				string random = Random(1000, 9999).ToString();
				SessionSet("verity_email", random);
				return smtp.Send(address, "南征支付，商户注册验证码", random);
			}
		}
		
		/// <summary>
        /// 用户登录
        /// </summary>
		protected void SiteMap(string root = "/my/")
        {
			string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";
			xml += "<urlset>\r\n";
			root = root.TrimEnd("/".ToCharArray()) + "/";
			
			foreach (System.IO.FileInfo fileInfo in new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(root)).GetFiles("*.*"))
			{
				if(fileInfo.Extension.Length>0 && ".aspx,html,.htm".Contains(fileInfo.Extension.ToLower()))
				{
					xml += "	<url>\r\n";
					xml += "		<loc>" + Url("/") + root + fileInfo.Name + "</loc>\r\n";
					xml += "		<lastmod>" + System.DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>\r\n";
					xml += "		<changefreq>daily</changefreq>\r\n";
					xml += "		<priority>1.0</priority>\r\n";
					xml += "	</url>\r\n";
				}
			}
			xml += "</urlset>";
			TxtFile("/sitemap.txt", xml);
		}
		
		/// <summary>
        /// 用户登录
        /// </summary>
		protected void UserReg()
        {
			System.Collections.Specialized.NameValueCollection req = Post();
			if(req.Count > 0)
			{
				if(Config("reg_verify") != null)
				{
					if(Config("reg_verify").ToLower().Contains("code") && SessionGet("verity_code") != req["verity_code"])
					{
						Alert(1, "", "验证码错误", "{}", "");
					}
					if(Config("reg_verify").ToLower().Contains("sms") && SessionGet("verity_sms") != req["verity_sms"])
					{
						Alert(1, "", "短信验证码错误", "{}", "");
					}
					if(Config("reg_verify").ToLower().Contains("email") && SessionGet("verity_email") != req["verity_email"])
					{
						Alert(1, "", "邮箱验证码错误", "{}", "");
					}
				}
				string sql = null;
				System.Data.DataRow user = DataRow("SELECT TOP 1 * FROM [user]", true);
				if(user.Table.Columns.Contains("user_name") && IsUserName(req["user_name"])) 
				{
					if(DataNvc("SELECT TOP 1 * FROM [user] WHERE [user_name]=@user_name", req).Count > 0) 
					{
						Alert(1, "", "用户名已经存在", "{}", ""); 
					}
					sql += ",[user_name]"; 
				}
				if(user.Table.Columns.Contains("user_mobile") && IsMobile(req["user_mobile"])) 
				{
					if(DataNvc("SELECT TOP 1 * FROM [user] WHERE [user_mobile]=@user_mobile", req).Count > 0) 
					{
						Alert(1, "", "手机号已经存在", "{}", ""); 
					}
					sql += ",[user_mobile]"; 
				}
				if(user.Table.Columns.Contains("user_email") && IsEmail(req["user_email"])) 
				{
					if(DataNvc("SELECT TOP 1 * FROM [user] WHERE [user_email]=@user_email", req).Count > 0) 
					{
						Alert(1, "", "邮箱已经存在", "{}", ""); 
					}
					sql += ",[user_email]"; 
				}
				
				if(!string.IsNullOrEmpty(sql)) 
				{
					req.Add("user_idkey", MD5(IP() + TimeStamp()));
					int user_id = DataInsert("INSERT INTO [user] (" + sql.Substring(1) + ") VALUES (@" + sql.Substring(1).Replace("[", "").Replace("]", "").Replace(",", ",@") + ")", req);
					if(user_id > 0)
					{
						Alert(0, "", "注册成功", "{}", "");
					}
					else
					{
						Alert(1, "", "注册失败", "{}", "");
					}
				}
			}
		}
		
		/// <summary>
        /// 用户登录
        /// </summary>
		protected void UserLogin(string redirectUrl = null)
        {
			System.Collections.Specialized.NameValueCollection req = Post();
			if(req["user"] != null && req["password"] != null)
			{
				if(Config("login_verify") != null)
				{
					if(Config("login_verify").ToLower().Contains("code") && SessionGet("login_verity_code") != req["login_verify_code"])
					{
						Alert(1, "", "网站验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("sms") && SessionGet("login_verify_sms") != req["login_verity_sms"])
					{
						Alert(1, "", "短信验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("email") && SessionGet("verity_email") != req["verity_email"])
					{
						Alert(1, "", "邮箱验证码错误", "{}", "");
					}
				}
				
				string sql = "SELECT TOP 1 * FROM [user] WHERE ";
				if(IsMobile(req["user"])) { sql += "[user_mobile]"; }
				else if(IsEmail(req["user"])) { sql += "[user_email]"; }
				else if(IsNumeric(req["user"])) { sql += "[user_no]"; }
				else { sql += "[user_name]"; }
				sql += "=@user";
				
				System.Collections.Specialized.NameValueCollection logUser = DataNvc(sql, req);
				if(logUser.Count == 0)
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["login_google_key"]) && req["password"] != GoogleAuth(logUser["login_google_key"]))
				{
					Alert(1, "", "账号或谷歌验证码错误", "{}", "");
				}
				else if(logUser["user_password"] != null && logUser["user_password"] != MD5(req["password"]))
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(logUser["user_level"] != null &&logUser["user_level"] == "0")
				{
					Alert(1, "", "账号尚未通过审核", "{}", "");
				}
				if(logUser["user_locked"] != null && logUser["user_locked"] != "0")
				{
					Alert(1, "", "账号已经锁定", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["lock_end_time"]) && System.Convert.ToDateTime(logUser["lock_end_time"]) > System.DateTime.Now)
				{
					Alert(1, "", "账号限制登录到" + logUser["lock_end_time"], "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["safe_ip"]) && !logUser["safe_ip"].Contains(IP()))
				{
					Alert(1, "", "IP地址限制登录", "{}", "");
				}
				
				if(!string.IsNullOrEmpty(logUser["last_login_time"]))
				{
					ExecuteNonQuery("UPDATE [User] SET [last_login_time]='" + Now() + "' WHERE [id]=" + logUser["id"]);
				}
				if(!string.IsNullOrEmpty(logUser["last_login_ip"]))
				{
					ExecuteNonQuery("UPDATE [User] SET [last_login_ip]='" + IP() + "' WHERE [id]=" + logUser["id"]);
				}
				
				SessionSet("log_user_idkey", logUser["user_idkey"]);
				if(!string.IsNullOrEmpty(logUser["agent_level"]) && logUser["agent_level"] != "0")
				{
					if(logUser["agent_locked"] == null || logUser["agent_locked"] != "1")
					{
						SessionSet("log_agent_id", logUser["id"]);
					}
				}
				if(!string.IsNullOrEmpty(logUser["admin_level"]) && logUser["admin_level"].ToString() != "0")
				{
					if(logUser["admin_locked"] == null || logUser["admin_locked"] != "1")
					{
						SessionSet("log_admin_id", logUser["id"]);
					}
				}
				if(IsAjax())
				{
					Alert(0, "login_success", "登录成功", "{}", "");
				}
				else
				{
					if(!string.IsNullOrEmpty(redirectUrl)){ redirectUrl = "./"; }
					Redirect(redirectUrl);
				}
			}
		}
		
		/// <summary>
        /// 用户退出
        /// </summary>
		protected void UserLogout()
        {
			SessionSet("log_user_idkey", "");
			Redirect(Get("HTTP_REFERER") ?? "/");
		}
		
		/// <summary>
        /// 代理登录
        /// </summary>
		protected void AgentLogin(string redirectUrl = null)
        {
			System.Collections.Specialized.NameValueCollection req = Post();
			if(req["user"] != null && req["password"] != null)
			{
				if(Config("login_verify") != null)
				{
					if(Config("login_verify").ToLower().Contains("code") && SessionGet("verity_code") != req["verity_code"])
					{
						Alert(1, "", "网站验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("sms") && SessionGet("verity_sms") != req["verity_sms"])
					{
						Alert(1, "", "短信验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("email") && SessionGet("verity_email") != req["verity_email"])
					{
						Alert(1, "", "邮箱验证码错误", "{}", "");
					}
				}
				
				string sql = "SELECT * FROM [user] WHERE ";
				if(IsMobile(req["user"])) { sql += "[user_mobile]"; }
				else if(IsEmail(req["user"])) { sql += "[user_email]"; }
				else if(IsNumeric(req["user"])) { sql += "[user_no]"; }
				else { sql += "[user_name]"; }
				sql += "=@user";
				
				System.Collections.Specialized.NameValueCollection logUser = DataNvc(sql, req);
				if(logUser.Count == 0)
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["google_login_key"]) && req["password"] != GoogleAuth(logUser["google_login_key"]))
				{
					Alert(1, "", "账号或谷歌验证码错误", "{}", "");
				}
				else if(!string.IsNullOrEmpty(logUser["user_password"]) && logUser["user_password"] != MD5(req["password"]))
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(logUser["agent_level"] != null && logUser["agent_level"] == "0")
				{
					Alert(1, "", "代理尚未通过审核", "{}", "");
				}
				if(logUser["agent_locked"] != null && logUser["agent_locked"] != "0")
				{
					Alert(1, "", "代理已经锁定", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["lock_end_time"]) && System.Convert.ToDateTime(logUser["lock_end_time"]) > System.DateTime.Now)
				{
					Alert(1, "", "账号限制登录到" + logUser["lock_end_time"], "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["login_ip_list"]) && !logUser["login_ip_list"].Contains(IP()))
				{
					Alert(1, "", "IP地址限制登录", "{}", "");
				}
				if(logUser["login_last_time"] != null)
				{
					ExecuteNonQuery("UPDATE [User] SET [login_last_time]='" + Now() + "' WHERE [id]=" + logUser["id"]);
				}
				if(logUser["login_last_ip"] != null)
				{
					ExecuteNonQuery("UPDATE [User] SET [login_last_ip]='" + IP() + "' WHERE [id]=" + logUser["id"]);
				}
				
				SessionSet("log_agent_id", logUser["id"]);
				if(ToInt(logUser["admin_level"]) > 0)
				{
					if(logUser["admin_locked"] == null || logUser["admin_locked"] != "1")
					{
						SessionSet("log_admin_id", logUser["id"]);
					}
				}
				if(IsAjax())
				{
					Alert(0, "login_success", "登录成功", "{}", "");
				}
				else
				{
					if(!string.IsNullOrEmpty(redirectUrl)){ redirectUrl = "./"; }
					Redirect(redirectUrl);
				}
			}
		}
		
		/// <summary>
        /// 代理退出
        /// </summary>
		protected void AgentLogout()
        {
			SessionSet("log_agent_id", "");
			Redirect(Get("HTTP_REFERER") ?? "/");
		}
		
		/// <summary>
        /// 管理员登录
        /// </summary>
		protected void AdminLogin(string redirectUrl = null)
        {
			System.Collections.Specialized.NameValueCollection req = Post();
			if(req["user"] != null && req["password"] != null)
			{

				if(Config("login_verify") != null)
				{
					if(Config("login_verify").ToLower().Contains("code") && SessionGet("login_verity_code") != req["login_verity_code"])
					{
						Alert(1, "", "网站验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("sms") && SessionGet("login_verity_sms") != req["login_verity_sms"])
					{
						Alert(1, "", "短信验证码错误", "{}", "");
					}
					if(Config("login_verify").ToLower().Contains("email") && SessionGet("verity_email") != req["verity_email"])
					{
						Alert(1, "", "邮箱验证码错误", "{}", "");
					}
				}
				
				string sql = "SELECT * FROM [user] WHERE ";
				if(IsMobile(req["user"])) { sql += "[user_mobile]"; }
				else if(IsEmail(req["user"])) { sql += "[user_email]"; }
				else if(IsNumeric(req["user"])) { sql += "[user_no]"; }
				else { sql += "[user_name]"; }
				sql += "=@user";
				
				System.Collections.Specialized.NameValueCollection logUser = DataNvc(sql, req);
				if(logUser.Count == 0)
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["login_verify_google"]) && req["password"] != GoogleAuth(logUser["login_verify_google"]))
				{
					Alert(1, "", "账号或谷歌验证码错误", "{}", "");
				}
				else if(logUser["user_password"] != null && logUser["user_password"] != MD5(req["password"]))
				{
					Alert(1, "", "账号或密码错误", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["admin_level"]) && logUser["admin_level"] == "0")
				{
					Alert(1, "", "管理员尚未通过审核", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["admin_locked"]) && logUser["admin_locked"] != "0")
				{
					Alert(1, "", "管理员已经锁定", "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["lock_end_time"]) && System.Convert.ToDateTime(logUser["lock_end_time"]) > System.DateTime.Now)
				{
					Alert(1, "", "账号限制登录到" + logUser["lock_end_time"], "{}", "");
				}
				if(!string.IsNullOrEmpty(logUser["safe_ip"]) && !logUser["safe_ip"].Contains(IP()))
				{
					Alert(1, "", "IP地址限制登录", "{}", "");
				}
				
				if(logUser["last_login_time"] != null)
				{
					ExecuteNonQuery("UPDATE [User] SET [last_login_time]='" + Now() + "' WHERE [id]=" + logUser["id"]);
				}
				if(logUser["last_login_ip"] != null)
				{
					ExecuteNonQuery("UPDATE [User] SET [last_login_ip]='" + IP() + "' WHERE [id]=" + logUser["id"]);
				} 

				SessionSet("log_admin_id", logUser["id"]);
				if(IsAjax())
				{
					Alert(0, "login_success", "登录成功", "{}", "");
				}
				else
				{
					if(!string.IsNullOrEmpty(redirectUrl)){ redirectUrl = "./"; }
					Redirect(redirectUrl);
				}
			}
		}
		
		/// <summary>
        /// 管理员退出
        /// </summary>
		protected void AdminLogout()
        {
			SessionSet("log_admin_id", "");
			Redirect(Get("HTTP_REFERER") ?? "/");
		}
    }
}
