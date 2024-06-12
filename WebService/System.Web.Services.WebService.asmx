<%@ WebService Language="C#" Class="WebService" %>
  /// <summary>
	/// WebService 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
	// [System.Web.Script.Services.ScriptService]
	public class WebService : System.Web.Services.WebService {

		public WebService () {
			//如果使用设计的组件，请取消注释以下行 
			//InitializeComponent(); 
			System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
			stream.Position = 0;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
			System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("./WebService.txt"), result);
		}

		/// <summary>
		/// 方法上头的[WebMethod]是声明一个web服务方法，如果你想写个方法能让客户端调用并返回结果就必须在方法上头标注[WebMethod]
		/// 如果是只负责逻辑运算或私有方法，并不打算给客户端结果，只给类方法内部调用就无需声明[WebMethod]
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>将运算结果转换成字符串返回</returns>
		[WebMethod]
		public string HelloWorld(int a, int b) 
		{

			int result = a + b;
			return result.ToString();
		} 
		
		[WebMethod]
		public string saveLisResult(string json) 
		{
			System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
			stream.Position = 0;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
			System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("./saveLisResult.txt"), result);
			return "success";
		}
		
		[WebMethod]
		public string savePacsResult(string xml) 
		{
			System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
			stream.Position = 0;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
			System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("./savePacsResult.txt"), result);
			return "success";
		}
	}
