namespace Com.Xuqingkai
{
	/// <summary>
	/// 伪静态，URL重写
	/// </summary>
	public class UrlReWrite : System.Web.IHttpModule
	{
		/// <summary>
		/// 伪静态配置文件路径
		/// </summary>
		public static string FilePath = null;
		public void Init(System.Web.HttpApplication context) { context.BeginRequest += new System.EventHandler(BeginRequest); }
		public void Dispose() { }
		private static string UrlEncode(string url)
		{
			string result = null;
			for (int i = 0; i < url.Length; i++) {
				result += (int)url[i] < 128 ? url[i].ToString() : System.Web.HttpUtility.UrlEncode(url[i].ToString());
			}
			return result;
		}
		private void BeginRequest(object sender, System.EventArgs e)
		{
			System.Web.HttpContext context = ((System.Web.HttpApplication)sender).Context;
			string filePath = FilePath;
			if (filePath == null || filePath.Length == 0) { filePath = System.Configuration.ConfigurationManager.AppSettings["Com.Xuqingkai.UrlReWrite"]; }
			if (filePath == null || filePath.Length == 0) { filePath = "/" + this.GetType().Name + ".txt"; }
			if (!filePath.StartsWith("/")) { filePath = "/" + filePath; }
			filePath = System.Web.Hosting.HostingEnvironment.MapPath(filePath);
			//用空格分开，前面是重写规则，后面是原始文件地址
			if (System.IO.File.Exists(filePath))
			{
				string url = context.Request.RawUrl; string[] urlArr = url.Split('?');
				url = System.Web.HttpUtility.UrlEncode(urlArr[0]).Replace("%2f", "/").Replace("%2F", "/") + (urlArr.Length > 1 ? "?" + urlArr[1] : "");
				string strTxt = System.IO.File.ReadAllText(filePath);
				foreach (string strRole in strTxt.Split('\r'))
				{
					if (strRole.IndexOf(" ") > 0)
					{
						string role = strRole.Trim();
						if (role.IndexOf("//") > 0) { role = role.Substring(0, role.IndexOf("//")); }
						role = role.Trim();
						string path = role.Substring(0, role.Trim().IndexOf(" ")).Replace("数", "(\\d+)").Replace("字", "(.+)");
						if (path.Length > 0) { path = "^" + (path.Substring(0, 1) == "/" ? "" : "/") + path; }
						role = role.Substring(role.Trim().IndexOf(" ") + 1).Trim();
						string file = UrlEncode(role);
						if (System.Text.RegularExpressions.Regex.IsMatch(@url, @path, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
						{
							context.RewritePath(UrlEncode(System.Text.RegularExpressions.Regex.Replace(@url, @path, file, System.Text.RegularExpressions.RegexOptions.IgnoreCase)), true);
							break;
						}
					}
				}
			}
		}


		public static string Search(string search)
		{

			string filePath = System.Web.Hosting.HostingEnvironment.MapPath("/UrlReWrite.txt");
			//用空格分开，前面是重写规则，后面是原始文件地址
			if (search != null && search.Length > 0 && System.IO.File.Exists(filePath))
			{
				string strTxt = System.IO.File.ReadAllText(filePath);
				foreach (string strRole in strTxt.Split('\r'))
				{
					if(strRole.Contains(" ") && (strRole + "&").Contains("=" + search + "&"))
					{
						string path = strRole.Trim().Substring(0, strRole.Trim().Trim().IndexOf(" ")).Trim("$".ToCharArray());
						System.Web.HttpContext.Current.Response.Redirect(path);
					}
				}
			}
			return null;
		}
	}
}