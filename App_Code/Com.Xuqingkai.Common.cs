/// <summary>
/// xuqingkai.com
/// </summary>
namespace Com.Xuqingkai
{
	/// <summary>
	/// 通用
	/// </summary>
	public partial class Common : System.Web.UI.Page
    {	
		/// <summary>
		/// AppSettings
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection AppSettings()
		{
			return System.Configuration.ConfigurationManager.AppSettings;
		}
		/// <summary>
		/// 获取AppSettings
		/// </summary>
		/// <param name="key">键名</param>
		/// <returns></returns>
		public static string AppSettings(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings[key];
		}
		/// <summary>
		/// 设置AppSettings
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="val">键值</param>
		/// <returns></returns>
		public static string AppSettings(string key, string val)
		{
			if(System.Configuration.ConfigurationManager.AppSettings[key] == null)
			{
				System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
				config.AppSettings.Settings.Add(key, val);
				config.Save();
				System.Configuration.ConfigurationManager.RefreshSection("appSettings");
			}
			else if(val == null)
			{
				val = System.Configuration.ConfigurationManager.AppSettings[key];
			}
			else
			{
				System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
				config.AppSettings.Settings[key].Value = val;
				config.Save();
				System.Configuration.ConfigurationManager.RefreshSection("appSettings");
			}
			return val;
		}

		
		/// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <param name="mode">运算模式（1:CBC,2:ECB,3:OFB,4:CFB,5:CTS）</param>
        /// <param name="padding">填充模式（1:None,2:PKCS7,3:Zeros,4:ANSIX923,5:ISO10126）</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
		public static string AesEncrypt(object data, object key = null, object iv = null, int mode = 1, int padding = 1, string charset = "UTF-8")
		{
			if (string.IsNullOrEmpty(key + "")) { key = "ABCDefgh1234%^&*"; }
			if (key.ToString().Length<16) { return "error:key.length<16"; }
			if (string.IsNullOrEmpty(iv + "")) { iv = key; }
			if (iv.ToString().Length < 16) { return "error:iv.length<16"; }
			
			byte[] bytes = System.Text.Encoding.GetEncoding(charset).GetBytes(data + "");
			System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
			rijndaelManaged.Key = System.Text.Encoding.GetEncoding(charset).GetBytes(key.ToString());
			rijndaelManaged.IV = System.Text.Encoding.GetEncoding(charset).GetBytes(iv.ToString());
			rijndaelManaged.Mode = (System.Security.Cryptography.CipherMode)mode;
			rijndaelManaged.Padding = (System.Security.Cryptography.PaddingMode)padding;
			
			//byte paddingBytes = (byte)(rijndaelManaged.BlockSize - (bytes.Length % rijndaelManaged.BlockSize));
			//byte[] finalBytes = new byte[bytes.Length + paddingBytes];
			//System.Buffer.BlockCopy(bytes, 0, finalBytes, 0, bytes.Length);
			//for (int i = bytes.Length; i<finalBytes.Length; i++){ finalBytes[i] = (byte)paddingBytes;}
			
			bytes = rijndaelManaged.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			string result = System.Convert.ToBase64String(bytes, 0, bytes.Length);
			//result = "";
			//foreach (byte b in bytes){ result += System.Convert.ToString(b, 16);}
			
			return result;
		}
		/// <summary>
        /// AES解密
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <param name="mode">运算模式（1:CBC,2:ECB,3:OFB,4:CFB,5:CTS）</param>
        /// <param name="padding">填充模式（1:None,2:PKCS7,3:Zeros,4:ANSIX923,5:ISO10126）</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
		public static string AesDecrypt(object data, object key = null, object iv = null, int mode = 1, int padding = 2, string charset = "UTF-8")
		{
			if (string.IsNullOrEmpty(key + "")) { key = "ABCDefgh1234%^&*"; }
			if (key.ToString().Length<16) { return "error:key.length<16"; }
			if (string.IsNullOrEmpty(iv + "")) { iv = key; }
			if (iv.ToString().Length < 16) { return "error:iv.length<16"; }

			byte[] bytes = System.Convert.FromBase64String(data + "");
			System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
			rijndaelManaged.Key = System.Text.Encoding.GetEncoding(charset).GetBytes(key.ToString());
			rijndaelManaged.IV = System.Text.Encoding.GetEncoding(charset).GetBytes(iv.ToString());
			rijndaelManaged.Mode = (System.Security.Cryptography.CipherMode)mode;
			rijndaelManaged.Padding = (System.Security.Cryptography.PaddingMode)padding;
			bytes = rijndaelManaged.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			return System.Text.Encoding.GetEncoding(charset).GetString(bytes);
		}
		
		

		/// <summary>
		/// 响应信息
		/// </summary>
		/// <param name="id">消息号，0为无错误，</param>
		/// <param name="code">消息码</param>
		/// <param name="message">消息内容</param>
		/// <param name="data">返回数据</param>
		/// <param name="url">跳转链接</param>
		/// <returns></returns>
		public static string Alert(int id, string code, object message = null, object data = null, object url = null)
		{
			return IsAjax() ? AlertJSON(id, code, message, data, url) : AlertJs(message, null, url);
		}
		/// <summary>
		/// 响应信息
		/// </summary>
		/// <param name="message">消息内容</param>
		/// <param name="title">标题</param>
		/// <param name="url">跳转链接</param>
		/// <returns></returns>
		public static string AlertJs(object message, object title = null, object url = null)
		{
			if(title == null){title = "提示信息";}
			string html = null;
			if(message != null){html += "<script type=\"text/javascript\">window.alert('" + message + "');</script>";}
			if(url == null) { }
			else if(url.ToString() == "") { html += "<script type=\"text/javascript\">window.history.go(-1);window.close();</script>"; }
			else { html += "<script type=\"text/javascript\">window.location.href='" + url + "';</script>"; }
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write(HtmlPage(title + "", html));
			System.Web.HttpContext.Current.Response.End();
			return null;
		}
		/// <summary>
		/// 响应信息
		/// </summary>
		/// <param name="id">消息号，0为无错误，</param>
		/// <param name="code">消息码</param>
		/// <param name="message">消息内容</param>
		/// <param name="data">返回数据</param>
		/// <param name="url">跳转链接</param>
		/// <returns></returns>
		public static string AlertJSON(int id, string code, object message = null, object data = null, object url = null)
		{
			string json = null;
			json += ",\"id\":" + id;
			json += ",\"code\":\"" + code + "\"";
			json += ",\"message\":\"" + message + "\"";
			if(url != null){json += ",\"url\":\"" + url + "\"";}
			if (data != null) 
			{
				if(data is string && !data.ToString().StartsWith("{") && !data.ToString().StartsWith("["))
				{
					json += ",\"data\":\"" + data + "\""; 
				}
				else
				{
					json += ",\"data\":" + data + ""; 
				}
			}
			json = "{" + json.Substring(1) + "}";
			
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write(json);
			System.Web.HttpContext.Current.Response.End();
			return null;
		}
		
		/// <summary>
		///支付宝生活号支付js代码
		/// </summary>
		/// <returns></returns>
		public static string AlipayShhJs(string tradeNO, string successUrl = "./", string failUrl = null)
		{
			if(failUrl == null){failUrl = successUrl;}
			string html = null;
			html += "<script type=\"text/javascript\">";
			html += "function alipayJSAPI(){AlipayJSBridge.call('tradePay',{tradeNO:'" + tradeNO + "'},function(result){if(result.resultCode=='9000'||result.resultCode=='8000'||result.resultCode=='6004'){window.alert('支付完成');window.location.href='" + successUrl + "'}else{window.alert('支付失败');window.location.href='" + failUrl + "';}});}";
			html += "if(window.AlipayJSBridge){alipayJSAPI && alipayJSAPI();}else{document.addEventListener('AlipayJSBridgeReady',alipayJSAPI,false);}";
			html += "</script>";
			return html;
		}
		
		/// <summary>
		/// 当前物理路径
		/// </summary>
		/// <returns></returns>
		public string AppPath()
		{
			return System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
		}
		
		/// <summary>
		/// BASE64编码
		/// </summary>
		/// <param name="source">要编码的明文</param>
		/// <returns></returns>
		public static string Base64Encode(object source)
		{
			return System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(source + ""));
		}
		/// <summary>
		/// BASE64编码
		/// </summary>
		/// <param name="source">要编码的明文</param>
		/// <param name="charset">编码格式</param>
		/// <returns></returns>
		public static string Base64Encode(object source, string charset)
		{
			return System.Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(source + ""));
		}

		/// <summary>
		/// Base64解码
		/// </summary>
		/// <param name="source">待解密的密文</param>
		/// <returns></returns>
		public static string Base64Decode(object source)
		{
			return System.Text.Encoding.GetEncoding("UTF-8").GetString(System.Convert.FromBase64String(source + ""));
		}
		/// <summary>
		/// Base64解码
		/// </summary>
		/// <param name="source">待解密的密文</param>
		/// <param name="charset">编码格式</param>
		/// <returns></returns>
		public static string Base64Decode(object source, string charset)
		{
			return System.Text.Encoding.GetEncoding(charset).GetString(System.Convert.FromBase64String(source + ""));
		}

		/// <summary>
		/// 获取缓存
		/// </summary>
		/// <param name="key">键名</param>
		/// <returns></returns>
		public static object CacheGet(string key)
		{
			return System.Web.HttpRuntime.Cache[key];
		}
		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="val">键值</param>
		public static object CacheSet(string key, object val)
		{
			if (val == null) { System.Web.HttpRuntime.Cache.Remove(key); }
			else { System.Web.HttpRuntime.Cache.Insert(key, val); }
			return val;
		}
		/// <summary>
		/// 设置缓存
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="value">键值</param>
		/// <param name="seconds">过期秒数</param>
		public static object CacheSet(string key, object val, int seconds)
		{
            System.Web.HttpRuntime.Cache.Insert(key, val, null, System.DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration);
			return val;
		}

		/// <summary>
		/// 清除
		/// </summary>
		public static void Clear()
		{
			System.Web.HttpContext.Current.Response.Clear();
		}
		/// <summary>
		/// 清除（然后输出，并终止后续后续程序）
		/// </summary>
		/// <param name="obj">要输出的对象</param>
		public static void Clear(object obj)
		{
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write(obj);
			System.Web.HttpContext.Current.Response.End();
		}

		/// <summary>
		/// 清除页面缓存
		/// </summary>
		public static void ClearPageCache()
		{
			System.Web.HttpContext.Current.Response.Buffer = true;
			System.Web.HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
			System.Web.HttpContext.Current.Response.Expires = 0;
			System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
			System.Web.HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");
		}

		/// <summary>
		/// COOKIE读取
		/// </summary>
		/// <param name="key">Cookie名</param>
		/// <returns></returns>
		public static string Cookie(string key)
		{
			string result = System.Web.HttpContext.Current.Request.Cookies[key] == null ? null : System.Web.HttpContext.Current.Request.Cookies[key].Value;
			return result + "";
		}
		/// <summary>
		/// COOKIE设置
		/// </summary>
		/// <param name="key">Cookie名</param>
		/// <param name="obj">Cookie值，为空则代表取值</param>
		/// <returns></returns>
		public static string Cookie(string key, object val)
		{
			string result = "";
			if (val != null)
			{
				result = val.ToString();
				System.Web.HttpContext.Current.Response.Cookies[key].Value = result;
			}
			return result;
		}
		/// <summary>
		/// COOKIE设置
		/// </summary>
		/// <param name="key">Cookie名</param>
		/// <param name="obj">Cookie值，为空则代表取值</param>
		/// <param name="seconds">过期秒数</param>
		/// <returns></returns>
		public static string Cookie(string key, object val, int seconds)
		{
			//2592000 = 60 * 60 * 24 * 30;
			string result = "";
			if (val != null)
			{
				result = val.ToString();
				System.Web.HttpContext.Current.Response.Cookies[key].Value = result;
				System.Web.HttpContext.Current.Response.Cookies[key].Expires = System.DateTime.Now.AddSeconds(seconds);
			}
			return result;
		}

		/// <summary>
		/// 创建对象实例
		/// </summary>
		/// <param name="className">类名（带命名空间）</param>
		/// <param name="objects">构造参数</param>
		/// <returns></returns>
		public static object CreateInstance(string className, object[] objects = null)
		{
			object instance = null;
			if(objects == null){ objects = new object[]{};}
			System.Type type = System.Type.GetType(className);
			if(type != null){instance = System.Activator.CreateInstance(type, objects);}
			return instance;
		}
		
		/// <summary>
		/// 创建对象实例
		/// </summary>
		/// <param name="className">类名（带命名空间）</param>
		/// <param name="objects">构造参数</param>
		/// <returns></returns>
		public static T CreateInstance<T>(string className, object[] objects = null)
		{	
			T instance = default(T);
			if(objects == null){ objects = new object[]{};}
			System.Type type = System.Type.GetType(className);
			if(type != null){instance = (T)System.Activator.CreateInstance(type, objects);}
			return instance;
		}
		
		/// <summary>
		/// 数据行转动态实体
		/// </summary>
		/// <param name="dataRow">数据行</param>
		/// <returns></returns>
		public static dynamic DataRowToModel(System.Data.DataRow dataRow)
		{
			dynamic model = null;
			if (dataRow != null)
			{
				model = new System.Dynamic.ExpandoObject();
				foreach (System.Data.DataColumn dataColumn in dataRow.Table.Columns)
				{
					((System.Collections.Generic.IDictionary<string, object>)model).Add(dataColumn.ColumnName, dataRow[dataColumn]);
				}
			}
			return model;
		}
		
		/// <summary>
		/// 数据行转实体
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="dataRow">数据行</param>
		/// <returns></returns>
		public static T DataRowToModel<T>(System.Data.DataRow dataRow) where T : class, new()
		{
			T model = null;
			if (dataRow != null)
			{
				model = new T();
				foreach (System.Reflection.PropertyInfo property in model.GetType().GetProperties())
				{
					System.Type type = System.Type.GetType(property.PropertyType.ToString().Replace("System.Nullable`1[", "").TrimEnd(']'));
					if (type.ToString().StartsWith("System.") && dataRow.Table.Columns.Contains(property.Name.ToString()))
					{
						property.SetValue(model, dataRow.IsNull(property.Name.ToString()) ? null : System.Convert.ChangeType(dataRow[property.Name.ToString()], type), null);
					}
				}
			}
			return model;
		}
		
		/// <summary>
		/// 目录复制
		/// </summary>
		/// <param name="src">源目录</param>
		/// <param name="tgt">目标目录</param>
		/// <returns></returns>
		public static void DirectoryCopy(string src, string tgt) 
		{ 
			System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(src.IndexOf(":\\")>0 ? src : System.Web.HttpContext.Current.Server.MapPath(src)); 
			System.IO.DirectoryInfo target = new System.IO.DirectoryInfo(tgt.IndexOf(":\\")>0 ? tgt : System.Web.HttpContext.Current.Server.MapPath(tgt)); 

			if (!target.FullName.StartsWith(source.FullName, System.StringComparison.CurrentCultureIgnoreCase) && source.Exists) 
			{ 
				if (!target.Exists) { target.Create(); } 
				System.IO.FileInfo[] files = source.GetFiles(); 

				for (int i = 0; i < files.Length; i++) { System.IO.File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true); } 
				System.IO.DirectoryInfo[] directories = source.GetDirectories(); 

				for (int j = 0; j < directories.Length; j++) { DirectoryCopy(directories[j].FullName, target.FullName + @"\" + directories[j].Name); } 			
			} 
		}

		/// <summary>
		/// 目录是否存在
		/// </summary>
		/// <param name="path">目录路径</param>
		/// <returns></returns>
		public static bool DirectoryExist(string path) 
		{ 
			path = path.IndexOf(":\\")>0 ? path : System.Web.HttpContext.Current.Server.MapPath(path); 
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
			return directoryInfo.Exists;
		}

		/// <summary>
		/// 调试显示
		/// </summary>
		/// <param name="content">内容</param>
		/// <returns></returns>
		public static object Debug(object content)
		{
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write(content);
			System.Web.HttpContext.Current.Response.End();
			return content;
		}
		/// <summary>
		/// 调试保存
		/// </summary>
		/// <param name="content">内容</param>
		/// <param name="filePath">生成文件名</param>
		/// <returns></returns>
		public static string Debug(object content, string filePath)
		{		
			string log = null;
			log += "----------------------------\r\n";
			log += "时间：" + System.DateTime.Now.ToString() + "\r\n";
			log += "来源：" + System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + "\r\n";
			log += "路径：" + System.Web.HttpContext.Current.Request.Url.ToString() + "\r\n";
			log += "内容：" + content + "\r\n";
			
			if(string.IsNullOrEmpty(filePath)){filePath = "/debug.txt"; }
			if(!filePath.ToLower().EndsWith(".txt")){ filePath += "/" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt"; }
			filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
			System.IO.File.AppendAllText(filePath, log, System.Text.Encoding.UTF8);
			return null;
		}
	
		/// <summary>
		/// 地图上两点距离
		/// </summary>
		/// <param name="lngA">A点经度</param>
		/// <param name="latA">A点纬度</param>
		/// <param name="lngB">B点经度</param>
		/// <param name="latB">B点纬度</param>
		/// <param name="radius">地球半径，默认为百度的</param>
		/// <returns></returns>
		public static string Distance(object lngA, object latA, object lngB, object latB, double radius = 6370996.81)
		{
			double lng0 = 0; double.TryParse(lngA + "", out lng0);
			double lat0 = 0; double.TryParse(latA + "", out lat0);
			double lng1 = 0; double.TryParse(lngB + "", out lng1);
			double lat1 = 0; double.TryParse(latB + "", out lat1);
			//radius = 6378136.49;腾讯地图半径
			double temp = 0; string result = "";

			if (lng0 > 0 && lat0 > 0)
			{
				temp = 180 / System.Math.PI; lng0 = lng0 / temp; lat0 = lat0 / temp; lng1 = lng1 / temp; lat1 = lat1 / temp;
				temp = System.Math.Cos(lat0) * System.Math.Cos(lng0) * System.Math.Cos(lat1) * System.Math.Cos(lng1);
				temp += System.Math.Cos(lat0) * System.Math.Sin(lng0) * System.Math.Cos(lat1) * System.Math.Sin(lng1);
				temp += System.Math.Sin(lat0) * System.Math.Sin(lat1);
				temp = radius * System.Math.Acos(temp);//乘以地球半径，这是百度地图采用的值
				result = temp + "";
			}
			else
			{
				result = "COS(" + latA + "/180*PI())*COS(" + lngA + "/180*PI())*COS(" + latB + "/180*PI())*COS(" + lngB + "/180*PI())";
				result += "+COS(" + latA + "/180*PI())*SIN(" + lngA + "/180*PI())*COS(" + latB + "/180*PI())*SIN(" + lngB + "/180*PI())";
				result += "+SIN(" + latA + "/180*PI())*SIN(" + latB + "/180*PI())";
				result = radius + "*ACOS(" + result + ")";
			}
			return result;
		}
		
		/// <summary>
		/// DLL编译
		/// </summary>
		/// <param name="dllFileName">DLL文件名</param>
		/// <returns></returns>
		public static string DllCreate(string dllFileName = null)
		{
			if(dllFileName == null || dllFileName.Length == 0){dllFileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".dll";}
			string batCode = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc.exe /target:library /out:" + System.Web.HttpContext.Current.Server.MapPath("/") + dllFileName;
			foreach (System.IO.FileInfo file in new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("/App_Code/")).GetFiles("*.cs"))
			{
				batCode += " " + file.DirectoryName + "\\" + file.Name;
			}
			batCode += "\r\n" + "del .\\生成DLL.bat";
			System.IO.File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("./生成DLL.bat"), batCode, System.Text.Encoding.Default);
			return batCode;
		}
		
		/// <summary>
		/// 网址
		/// </summary>
		public static string DomainUrl()
		{
			string url = System.Web.HttpContext.Current.Request.Url.ToString();
			url = url.Substring(0, url.IndexOf('/', 10));
			return url;
		}

		/// <summary>
		/// 下载
		/// </summary>
		/// <param name="url">文件地址（网络）</param>
		/// <param name="filePath">保存路径</param>
		/// <returns></returns>
		public static string Download(string url, string filePath)
		{
			System.Net.WebClient webClient = new System.Net.WebClient();
			try { webClient.DownloadFile(url, System.Web.HttpContext.Current.Server.MapPath("/") + filePath); }
			catch { }
			return filePath;
		}

        /// <summary>
        /// 输出终止
        /// </summary>
        public static void End()
		{
			System.Web.HttpContext.Current.Response.End();
		}
		/// <summary>
		/// 输出当前参数值，并终止后续程序
		/// <param name="obj">输出内容</param>
		/// </summary>
		public static void End(object obj)
		{
			System.Web.HttpContext.Current.Response.Write(obj);
			System.Web.HttpContext.Current.Response.End();
		}

		/// <summary>
		/// Unicode编码
		/// </summary>
		/// <param name="obj">待编码字符</param>
		/// <returns></returns>
		public static string Escape(object obj)
		{
			return System.Text.RegularExpressions.Regex.Escape(obj + "");
		}
		
		/// <summary>
		/// 文件复制
		/// </summary>
		/// <param name="src">待复制文件</param>
		/// <param name="tgt">文件目的地</param>
		/// <param name="replace">是否覆盖</param>
		/// <returns></returns>
		public static void FileCopy(string src, string tgt, bool replace = true) 
		{ 
			src = src.IndexOf(":\\")>0 ? src : System.Web.HttpContext.Current.Server.MapPath(src); 
			tgt = tgt.IndexOf(":\\")>0 ? tgt : System.Web.HttpContext.Current.Server.MapPath(tgt); 
			if (System.IO.File.Exists(src)) 
			{
				if (replace || !System.IO.File.Exists(tgt)) 
				{
					System.IO.File.Copy(src, tgt, true);
				}
			} 
		} 

		/// <summary>
		/// 文件是否存在
		/// </summary>
		/// <param name="path">文件路径</param>
		/// <returns></returns>
		public static bool FileExist(string path) 
		{ 
			path = path.IndexOf(":\\")>0 ? path : System.Web.HttpContext.Current.Server.MapPath(path); 
			return System.IO.File.Exists(path);
		}

		/// <summary>
		/// 文件迁移
		/// </summary>
		/// <param name="src">待移动文件</param>
		/// <param name="tgt">文件目的地</param>
		/// <param name="replace">是否覆盖</param>
		/// <returns></returns>
		public static bool FileMove(string src, string tgt, bool replace = true) 
		{ 
			bool result = false;
			src = src.IndexOf(":\\")>0 ? src : System.Web.HttpContext.Current.Server.MapPath(src); 
			tgt = tgt.IndexOf(":\\")>0 ? tgt : System.Web.HttpContext.Current.Server.MapPath(tgt); 
			if (System.IO.File.Exists(src)) 
			{
				if(replace){System.IO.File.Delete(tgt);}
				try { System.IO.File.Move(src, tgt); result = true; }
				catch{ }
			} 
			return result;
		}
		
		/// <summary>
		/// 文件MD5值
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <returns></returns>
		public static string FileMD5(string filePath)
		{
			if(filePath.IndexOf(":\\")<0){filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
			System.IO.FileStream fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (byte b in md5.ComputeHash(fileStream)) { stringBuilder.Append(b.ToString("x2")); }
			fileStream.Close();
            return stringBuilder.ToString();

		}
		
		/// <summary>
		/// 文件相对路径，不带域名和参数
		/// </summary>
		/// <returns></returns>
		public static string FilePath()
		{
			return System.Web.HttpContext.Current.Request.FilePath;
		}
		/// <summary>
		/// 文件相对路径
		/// </summary>
		/// <param name="path">地址，为空则为当前页</param>
		/// <returns></returns>
		public static string FilePath(string path)
		{
			string filePath = System.Web.HttpContext.Current.Request.FilePath;
			if (path != null && !path.StartsWith("/"))
			{
				filePath = filePath.Substring(0, filePath.LastIndexOf('/') + 1) + path;
			}
			return filePath;
		}
		
		/// <summary>
		/// 表单字符串
		/// </summary>
		public static string FormString()
		{
			return System.Web.HttpContext.Current.Request.Form.ToString();
		}

		/// <summary>
		/// GET请求集合
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection Get()
		{
			return new System.Collections.Specialized.NameValueCollection(System.Web.HttpContext.Current.Request.QueryString);
		}
		
		/// <summary>
		/// GET值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string Get(string key)
		{
			return System.Web.HttpContext.Current.Request.QueryString[key];
		}
		
		/// <summary>
		/// Get整数值
		/// </summary>
		/// <returns></returns>
		public static double GetID()
		{
			double result = 0;
            double.TryParse(System.Web.HttpContext.Current.Request.QueryString["ID"] + "", out result);
			return System.Math.Floor(result);
		}
		
		/// <summary>
		/// GET整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static double GetID(string key)
		{
			double result = 0;
            double.TryParse(System.Web.HttpContext.Current.Request.QueryString[key] + "", out result);
			return System.Math.Floor(result);
		}
		
		/// <summary>
		/// GET整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <param name="defaultID">默认值</param>
		/// <returns></returns>
		public static double GetID(string key, double defaultID)
		{
			double result = 0;
			if (!double.TryParse(System.Web.HttpContext.Current.Request.QueryString[key] + "", out result)) { result = defaultID; }
			return System.Math.Floor(result);
		}

		/// <summary>
		/// GET同参数名的值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string[] Gets(string key)
		{
			return System.Web.HttpContext.Current.Request.QueryString.GetValues(key);
		}
		
		/// <summary>
		/// 请求(先GET后POST)
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection GetPost()
		{
			System.Collections.Specialized.NameValueCollection request = System.Web.HttpContext.Current.Request.QueryString;
			if (request == null || request.Count == 0) { request = System.Web.HttpContext.Current.Request.Form; }
			return new System.Collections.Specialized.NameValueCollection(request);
		}

		/// <summary>
		/// Request同参名的值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string[] GetPosts(string key)
		{
			string[] result = System.Web.HttpContext.Current.Request.QueryString.GetValues(key);
			if (result == null) { result = System.Web.HttpContext.Current.Request.Form.GetValues(key); }
			return result;
		}
		
		/// <summary>
		/// 谷歌验证码
		/// </summary>
		/// <param name="secret">密钥</param>
		/// <param name="digits">密钥长度</param>
		/// <param name="seconds">失效描述</param>
		/// <returns></returns>
		public static string GoogleAuth(string secret = null, int digits = 6, int seconds = 30)
		{
			if(secret == null || secret.Length == 0){secret = "GoogleAuth." + System.Web.HttpContext.Current.Request.Url.Host.ToLower();}
			if(digits > 0)
			{
				byte[] counter = System.BitConverter.GetBytes((long)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds / seconds);
				if (System.BitConverter.IsLittleEndian){System.Array.Reverse(counter);}
				byte[] hash = new System.Security.Cryptography.HMACSHA1(System.Text.Encoding.ASCII.GetBytes(secret), true).ComputeHash(counter);
				int offset = hash[hash.Length - 1] & 0xf;
				int binary = ((hash[offset] & 0x7f) << 24) | ((hash[offset + 1] & 0xff) << 16) | ((hash[offset + 2] & 0xff) << 8) | (hash[offset + 3] & 0xff);
				int password = binary % (int)System.Math.Pow(10, digits); // 6 digits
				return password.ToString(new string('0', digits));
			}
			else
			{
				byte[] input = System.Text.Encoding.UTF8.GetBytes(secret);
				int charCount = (int)System.Math.Ceiling(input.Length / 5d) * 8;
				char[] returnArray = new char[charCount];

				byte nextChar = 0, bitsRemaining = 5;
				int arrayIndex = 0;

				foreach (byte b in input)
				{
					nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
					if (nextChar < 26) 
					{
						returnArray[arrayIndex++] = (char)(nextChar + 65); 
					}
					else if (nextChar < 32) 
					{
						returnArray[arrayIndex++] = (char)(nextChar + 24);
					}

					if (bitsRemaining < 4)
					{
						nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
						if (nextChar < 26) 
						{
							returnArray[arrayIndex++] = (char)(nextChar + 65); 
						}
						else if (nextChar < 32) 
						{
							returnArray[arrayIndex++] = (char)(nextChar + 24);
						}
						bitsRemaining += 5;
					}

					bitsRemaining -= 3;
					nextChar = (byte)((b << bitsRemaining) & 31);
				}

				if (arrayIndex != charCount)
				{
					//returnArray[arrayIndex++] = GoogleAuthToChar(nextChar);
					if (nextChar < 26) 
					{
						returnArray[arrayIndex++] = (char)(nextChar + 65); 
					}
					else if (nextChar < 32) 
					{
						returnArray[arrayIndex++] = (char)(nextChar + 24);
					}
					while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; 
				}
				secret = new string(returnArray).TrimEnd('=');
				if(digits == 0) { return secret; }
				if(digits == -1) { return "otpauth://totp/google@" + System.Web.HttpContext.Current.Request.Url.Host + "?secret=" + secret; }
				
				long expire_seconds = (seconds - (long)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds % seconds);
				return expire_seconds.ToString();
			}
		}
		
		/// <summary>
		/// 服务器变量
		/// </summary>
		/// <param name="name">输出全部信息</param>
		/// <returns></returns>
		public static string Headers(string name)
		{
			return System.Web.HttpContext.Current.Request.ServerVariables[name];
		}
		
		/// <summary>
		/// Html页
		/// </summary>
		/// <param name="title">标题</param>
		/// <returns></returns>
		public static string HtmlPage(object title)
		{

			return HtmlPage(title, title);
		}

		/// <summary>
		/// Html页
		/// </summary>
		/// <param name="title">标题</param>
		/// <param name="body">内容</param>
		/// <returns></returns>
		public static string HtmlPage(object title, object body)
		{
			string html = "<title>" + title + "</title>";
			html = "\r\n<head>\r\n<meta charset=\"utf-8\">\r\n" + html + "\r\n<meta name=\"viewport\" content=\"width=device-width\">\r\n</head>";
			html = "<!DOCTYPE html><html>" + html + "\r\n<body>\r\n" + body + "\r\n</body>\r\n</html>";
			return html;
		}

		/// <summary>
		/// HTML编码
		/// </summary>
		/// <param name="html">待编码html</param>
		/// <returns></returns>
		public static string HtmlEncode(object html) { return System.Web.HttpUtility.HtmlEncode(html + ""); }

		/// <summary>
		/// HTML解码
		/// </summary>
		/// <param name="html">待解码html</param>
		/// <returns></returns>
		public static string HtmlDecode(object html) { return System.Web.HttpUtility.HtmlDecode(html + ""); }
		
		/// <summary>
		/// GET请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="charset">编码</param>
		/// <returns></returns>
		public static string HttpGet(string url, string charset = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			try
			{
				byte[] bytes = webClient.DownloadData(url);
				result = System.Text.Encoding.GetEncoding(charset).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpGet:" + ex.Message;
				//result = null;
			}
            return result;
		}
		
		/// <summary>
		/// GET请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="headers">头</param>
		/// <param name="charset">编码</param>
		/// <returns></returns>
		public static string HttpGet(string url, System.Collections.Specialized.NameValueCollection headers, string charset = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			if(headers != null)
			{
				if(headers["Content-Type"] == null && headers["ContentType"] != null) { headers.Add("Content-Type", headers["ContentType"]); headers.Remove("ContentType"); }
				if(headers["User-Agent"] == null && headers["UserAgent"] != null) { headers.Add("User-Agent", headers["UserAgent"]); headers.Remove("UserAgent"); }
				foreach(string key in headers.Keys) { webClient.Headers.Add(key,headers[key]); }
			}
			try
			{
				byte[] bytes = webClient.DownloadData(url);
				result = System.Text.Encoding.GetEncoding(charset).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpGet:" + ex.Message;
			}
            return result;
		}

		/// <summary>
		/// 接收数据
		/// </summary>
		/// <param name="charset">编码</param>
		/// <returns></returns>
		public static string HttpPost(string charset = "UTF-8")
		{
			System.IO.Stream stream = System.Web.HttpContext.Current.Request.InputStream;
			stream.Position = 0;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			string result = System.Text.Encoding.GetEncoding(charset).GetString(bytes);
			return result;
		}
		
		/// <summary>
		/// POST请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="data">数据</param>
		/// <param name="charset">请求编码</param>
		/// <param name="contentType">响应编码</param>
		/// <returns></returns>
		public static string HttpPost(string url, string data, string charset = "UTF-8", string contentType = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			try
			{
				webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding(charset).GetBytes(data));
				result = System.Text.Encoding.GetEncoding(contentType).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpPost:" + ex.Message;
			}
            return result;
		}
		
		/// <summary>
		/// POST请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="data">数据</param>
		/// <param name="header">请求头</param>
		/// <param name="charset">请求编码</param>
		/// <param name="contentType">响应编码</param>
		/// <returns></returns>
		public static string HttpPost(string url, string data, System.Collections.Specialized.NameValueCollection headers, string charset="utf-8", string contentType="utf-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			if(headers != null)
			{
				if(headers["Content-Type"] == null && headers["ContentType"] != null) { headers.Add("Content-Type", headers["ContentType"]); headers.Remove("ContentType"); }
				if(headers["User-Agent"] == null && headers["UserAgent"] != null) { headers.Add("User-Agent", headers["UserAgent"]); headers.Remove("UserAgent"); }
				foreach(string key in headers.Keys) { webClient.Headers.Add(key,headers[key]); }
			}
			try
			{
				byte[] bytes = webClient.UploadData(url, System.Text.Encoding.GetEncoding(charset).GetBytes(data));
				result = System.Text.Encoding.GetEncoding(contentType).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpPost:" + ex.Message;
			}
            return result;
		}
		
		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="filePath">文件</param>
		/// <param name="headers">头</param>
		/// <param name="charset">请求编码</param>
		/// <returns></returns>
		public static string HttpUpload(string url, string filePath, System.Collections.Specialized.NameValueCollection headers, string charset="utf-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			if(headers != null)
			{
				if(headers["Content-Type"] == null && headers["ContentType"] != null) { headers.Add("Content-Type", headers["ContentType"]); headers.Remove("ContentType"); }
				if(headers["User-Agent"] == null && headers["UserAgent"] != null) { headers.Add("User-Agent", headers["UserAgent"]); headers.Remove("UserAgent"); }
				foreach(string key in headers.Keys) { webClient.Headers.Add(key,headers[key]); }
			}
			try
			{
				byte[] bytes = webClient.UploadFile(url, filePath);
				result = System.Text.Encoding.GetEncoding(charset).GetString(bytes);
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpUpload:" + ex.Message;
			}
            return result;
		}
		
		/// <summary>
		/// Http请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="data">数据</param>
		/// <param name="files">上传文件，URL键值对</param>
		/// <param name="headers">请求消息头，Nvc格式</param>
		/// <param name="extra">额外参数，Nvc格式</param>
		/// <returns></returns>
		public static string HttpRequest(string url, string data = null, string files = null, System.Collections.Specialized.NameValueCollection headers = null, System.Collections.Specialized.NameValueCollection extra = null)
		{
			string result = null;
			System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			System.Net.ServicePointManager.Expect100Continue = false;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			
			httpWebRequest.AllowAutoRedirect = false;
			httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
			
			if(headers != null)
			{
				if(headers["Content-Type"] != null) { httpWebRequest.ContentType = headers["Content-Type"]; headers.Remove("Content-Type"); }
				if(headers["ContentType"] != null) { httpWebRequest.ContentType = headers["ContentType"]; headers.Remove("ContentType"); }
				if(headers["Host"] != null) { httpWebRequest.Host = headers["Host"]; headers.Remove("Host"); }
				if(headers["Referer"] != null) { httpWebRequest.Referer = headers["Referer"]; headers.Remove("Referer"); }
				if(headers["Refererr"] != null) { httpWebRequest.Referer = headers["Refererr"]; headers.Remove("Refererr"); }
				if(headers["User-Agent"] != null) { httpWebRequest.UserAgent = headers["User-Agent"];  headers.Remove("User-Agent"); }
				if(headers["UserAgent"] != null) { httpWebRequest.UserAgent = headers["UserAgent"]; headers.Remove("UserAgent"); }
				foreach(string key in headers.Keys) { httpWebRequest.Headers.Add(key,headers[key]); }
			}
			
			if(extra == null){ extra = new System.Collections.Specialized.NameValueCollection(); }
			string requestCharset = extra["RequestCharset"] ?? "UTF-8";
			string responseCharset = extra["ResponseCharset"] ?? "UTF-8";
			
			if (data == null)//GET请求
            {
                httpWebRequest.Method = "GET";
            }
            else if(files == null)//POST数据，如JSON、XML
			{
				httpWebRequest.Method = "POST";
				System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
                byte[] bytes = System.Text.Encoding.GetEncoding(requestCharset).GetBytes(data);
                requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
            }
            else//POST上传
			{
				httpWebRequest.Method = "POST";
				System.IO.Stream requestStream = httpWebRequest.GetRequestStream();
				
				string boundary = "---------------------------" + System.DateTime.Now.Ticks.ToString("x");
				httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
				byte[] startBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
				
				System.Collections.Specialized.NameValueCollection nvcData = System.Web.HttpUtility.ParseQueryString(data);
				if (nvcData != null && nvcData.Count > 0)
				{
					foreach (string key in nvcData.AllKeys)
					{
						requestStream.Write(startBytes, 0, startBytes.Length);
						byte[] inputBytes = System.Text.Encoding.GetEncoding(requestCharset).GetBytes("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n" + nvcData[key]);
						requestStream.Write(inputBytes, 0, inputBytes.Length);
					}
				}
				
				System.Collections.Specialized.NameValueCollection nvcFile = System.Web.HttpUtility.ParseQueryString(files);
				if (nvcFile != null && nvcFile.Count > 0)
				{
					foreach (string key in nvcFile.AllKeys)
					{
						string filePath = System.Web.HttpContext.Current.Server.MapPath(nvcFile[key]);
						if (System.IO.File.Exists(filePath))
						{
							string fileName = filePath.Replace("\\", "/");
							if (fileName.Contains("/")) { fileName = fileName.Substring(fileName.LastIndexOf("/") + 1); }
							requestStream.Write(startBytes, 0, startBytes.Length);
							//此处可不指定Content-Type
							string fileData = "Content-Disposition: form-data; name=\"" + key + "\"; filename=\"" + fileName + "\"\r\nContent-Type: {2}\r\n\r\n";
							byte[] fileByte = System.Text.Encoding.GetEncoding(requestCharset).GetBytes(fileData);
							requestStream.Write(fileByte, 0, fileByte.Length);

							System.IO.FileStream fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
							byte[] fileBytes = new byte[System.Math.Min(4096, (int)fileStream.Length)]; int fileBytesRead = 0;
							while ((fileBytesRead = fileStream.Read(fileBytes, 0, fileBytes.Length)) != 0) { requestStream.Write(fileBytes, 0, fileBytesRead); }
							fileStream.Close();
						}
					}
				}
				
				byte[] endBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
				requestStream.Write(endBytes, 0, endBytes.Length);
				requestStream.Close();
			}	
			
			if(extra["CertPath"] != null)
			{
				string certPath = extra["CertPath"];
				if (certPath.IndexOf(":\\")<0) { certPath = System.Web.HttpContext.Current.Server.MapPath(certPath); }
				string certPassword = extra["CertPassword"];
				
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = null;
				x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(System.IO.File.ReadAllBytes(certPath), certPassword, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
				//DefaultKeySet 使用默认的密钥集。用户密钥集通常为默认值。 
				//Exportable 导入的密钥被标记为可导出。 
				//MachineKeySet 私钥存储在本地计算机存储区而不是当前用户存储区。 
				//PersistKeySet 导入证书时会保存与 PFX 文件关联的密钥。 
				//UserKeySet 私钥存储在当前用户存储区而不是本地计算机存储区。 既使证书指定密钥应存储在本地计算机存储区，私钥也会存储到当前用户存储区。  
				//UserProtected 通过对话框或其他方法，通知用户密钥被访问。使用的加密服务提供程序 (CSP) 定义确切的行为。 
				httpWebRequest.ClientCertificates.Add(x509Certificate2);
			}
			
			try
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
				result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding(responseCharset)).ReadToEnd();
			}
			catch (System.Net.WebException webException)
			{
				System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webException.Response;
				if(httpWebResponse != null)
				{
					result = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")).ReadToEnd();
				}
			}
			catch (System.Exception ex)
			{
				result = "HttpRequest:" + ex.Message;
			}
            return result;
		}
		
		/// <summary>
		/// 包含页面
		/// </summary>
		/// <param name="files">页面文件路径，可多个参数，也可以一个参数，用逗号分割</param>
		/// <returns></returns>
		public static string Include(params string[] files)
		{
			string result = null;
			foreach (string filePath in string.Join(",", files).Replace("|", ",").Split(','))
			{
				if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath)))
				{
					System.IO.TextWriter stringWriter = new System.IO.StringWriter();
					System.Web.HttpContext.Current.Server.Execute(filePath, stringWriter);
					result += stringWriter.ToString();
					stringWriter = null;
				}
			}
			return result;
		}

		/// <summary>
		/// 反射执行某方法
		/// </summary>
		/// <param name="methodName">方法名</param>
		public static void Invoke<T>(string methodName) where T : new()
		{
			System.Type type = new T().GetType();
			object obj = System.Activator.CreateInstance(type);
			System.Reflection.MethodInfo method = type.GetMethod(methodName);
			method.Invoke(obj, null);
		}
		/// <summary>
		/// 反射执行某方法
		/// </summary>
		/// <param name="methodName">方法名</param>
		public static void Invoke<T>(T obj, string methodName) where T : new()
		{
			System.Reflection.MethodInfo method = obj.GetType().GetMethod(methodName);
			method.Invoke(obj, null);
		}

		/// <summary>
		/// IP
		/// </summary>
		/// <returns></returns>
		public static string IP()
		{
			return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
		
		/// <summary>
		/// 判断是不是64位系统
		/// </summary>
		/// <returns></returns>
		public static bool Is64bit()
		{
			return System.IntPtr.Size == 8;
        }
		/// <summary>
		/// 是否在支付宝客户端访问
		/// </summary>
		/// <returns></returns>
		public static bool IsAlipayBrowser()
		{
			return (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] + "").Contains("AlipayClient");
		}

		/// <summary>
		/// 判断是不是AJAX请求
		/// </summary>
		/// <returns></returns>
		public static bool IsAjax()
		{
			return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REQUESTED_WITH"] != null;;
		}
		/// <summary>
		/// 判断是不是AJAX请求
		/// </summary>
		/// <param name="key">参数</param>
		/// <returns></returns>
		public static bool IsAjax(string key)
		{
			bool isAjax = false;
			if (key != null && key.Length > 0)
			{
				isAjax = System.Web.HttpContext.Current.Request.QueryString[key] != null;
			}
			isAjax = isAjax || System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REQUESTED_WITH"] != null;
			return isAjax;
		}

		/// <summary>
		/// 是否在APP客户端访问
		/// </summary>
		/// <returns></returns>
		public static bool IsAppBrowser(string userAgentName = "AppBrowser")
		{
			string userAgent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
			return userAgent.Contains(userAgentName);
		}

		/// <summary>
		/// 是否中文
		/// </summary>
		/// <param name="obj">字符串</param>
		/// <returns></returns>
		public static bool IsChinese(object obj)
		{
			return IsMatch(obj + "", "^[\u4E00-\u9FA5]+$");
		}
		
		/// <summary>
		/// 是否日期格式
		/// </summary>
		/// <param name="obj">字符串</param>
		/// <returns></returns>
		public static bool IsDateTime(object obj)
		{
			System.DateTime dateTime = System.DateTime.Now;
			return System.DateTime.TryParse(obj + "", out dateTime);
		}

		/// <summary>
		/// Email格式验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsEmail(object obj)
		{
			return IsMatch(obj + "", "^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$");
		}

        /// <summary>
        /// 身份证格式验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsIdCardNo(object obj)
        {
            string idCardNo = (obj + "").ToLower();

            if (idCardNo.Length != 18) { return false; }
            if (idCardNo.Contains("x") && !idCardNo.EndsWith("x")) { return false; }
            if (!IsMatch(idCardNo.Replace("x", ""), "^\\d{17,18}$")) { return false; }

            string address = ",11,22,35,44,53,12,23,36,45,54,13,31,37,46,61,14,32,41,50,62,15,33,42,51,63,21,34,43,52,64,65,71,81,82,91,";
            if (!address.Contains("," + idCardNo.Substring(0, 2) + ",")) { return false;}

            string birthday = idCardNo.Substring(6, 4) + "-" + idCardNo.Substring(10, 2) + "-" + idCardNo.Substring(12,2);
            System.DateTime outBirthday;
            if (!System.DateTime.TryParse(birthday, out outBirthday)) { return false; }

            int sum = 0;
            for (int i = 0; i < 17; i++) { sum += new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 }[i] * int.Parse(idCardNo.Substring(i, 1).ToString()); }

            int remainder = -1;
            System.Math.DivRem(sum, 11, out remainder);

            if (!idCardNo.EndsWith("10x98765432".Substring(remainder, 1))) { return false; }

            return true;
        }

		/// <summary>
		/// 正则表达式验证
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="patern">表达式</param>
		/// <returns></returns>
		public static bool IsMatch(object str, string patern)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(str + "", patern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// 手机号验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsMobile(object obj)
		{
			return IsMatch(obj + "", "^1(3|4|5|6|7|8|9)\\d{9}$");
		}

		/// <summary>
		/// 是否手机浏览器
		/// </summary>
		/// <returns></returns>
		public static bool IsMobileBrowser()
		{
			string user_agent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] + "";
			bool result = false;
			foreach (string key in "iPhone,iPod,Android,ios".Split(','))
			{
				if (user_agent.Contains(key))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// 自动跳转手机页
		/// </summary>
		public static void IsMobileRedirect()
		{
			IsMobileRedirect(null);
		}
		/// <summary>
		/// 自动跳转手机页
		/// </summary>
		/// <param name="url">转向地址</param>
		public static void IsMobileRedirect(string url)
		{
			string host = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower();
			string path = System.Web.HttpContext.Current.Request.FilePath;
			//如果不是访问手机站目录，但又是手机浏览器访问或用了手机站域名，则转向
			if (!path.ToLower().StartsWith("/mobile/") && (IsMobileBrowser() || host.StartsWith("m.")))
			{
				if (url == null)
				{
					path += System.Web.HttpContext.Current.Request.Url.Query;
					url = "/mobile" + path;
				}
				System.Web.HttpContext.Current.Response.Redirect(url);
			}
		}

		/// <summary>
		/// 昵称验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsNickName(object obj)
		{
			return IsMatch(obj + "", "^[\u4E00-\u9FA5a-zA-Z0-9]+$");
		}

		/// <summary>
		/// 数字验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsNumber(object obj)
		{
			return IsMatch(obj + "", "^\\d{1,100}$");
		}
		
		/// <summary>
		/// 数字验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsNumberic(object obj)
		{
			return IsMatch(obj + "", "^\\d{1,100}$");
		}
		/// <summary>
		/// 数字验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsNumeric(object obj)
		{
			return IsMatch(obj + "", "^\\d{1,100}$");
		}

		/// <summary>
		/// 密码格式验证
		/// </summary>
		/// <param name="obj">字符串</param>
		public static bool IsPassword(object obj, int min = 6)
		{
			return (obj + "").Length >= min;
		}

		/// <summary>
		/// 是否在QQ客户端访问
		/// </summary>
		/// <returns></returns>
		public static bool IsQQBrowser()
		{
			return (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] + "").Contains("QQ/");
		}

		/// <summary>
		/// 是否在微信客户端访问
		/// </summary>
		/// <returns></returns>
		public static bool IsWeixinBrowser()
		{
			return (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] + "").Contains("MicroMessenger");
		}

		/// <summary>
		/// 是不是POST请求 
		/// </summary>
		/// <returns></returns>
		public static bool IsPost() { return System.Web.HttpContext.Current.Request.HttpMethod == "POST"; }

		/// <summary>
		/// 用户名验证，默认4-18位字母开头且和数字的字符串
		/// </summary>
		/// <param name="obj">待验证字符串</param>
		/// <returns></returns>
		public static bool IsUserName(object obj)
		{
			return IsUserName(obj, 4, 18);
		}
		/// <summary>
		/// 用户名验证
		/// </summary>
		/// <param name="obj">待验证字符串</param>
		/// <param name="min">最低长度</param>
		/// <param name="max">最大长度</param>
		/// <returns></returns>
		public static bool IsUserName(object obj, int min, int max)
		{
			return IsMatch(obj, "^[a-zA-Z][a-zA-Z0-9]{" + (min - 1) + "," + (max - 1) + "}$");
		}
		
		/// <summary>
		/// 判断是不是winform模式
		/// </summary>
		public static bool IsWinform()
		{
			return System.Environment.CurrentDirectory == System.AppDomain.CurrentDomain.BaseDirectory;
		}
		
		public static string LogUserId(object val = null)
		{
			if(val == null)
			{
				return SessionGet("log_user_id");	
			}
			else
			{
				return SessionSet("log_user_id", val);	
			}
		}
		public static string LogAgentId(object val = null)
		{
			if(val == null)
			{
				return SessionGet("log_agent_id");	
			}
			else
			{
				return SessionSet("log_agent_id", val);	
			}
		}
		
		
		public static string LogAdminId(object val = null)
		{
			if(val == null)
			{
				return SessionGet("log_admin_id");	
			}
			else
			{
				return SessionSet("log_admin_id", val);	
			}
		}
		
		/// <summary>
		/// 当前文件路径
		/// </summary>
		/// <returns></returns>
		public string MapPath()
		{
			string filePath = System.Web.HttpContext.Current.Request.FilePath;
			filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
			return filePath;
		}
		
		/// <summary>
		/// 获取路径
		/// </summary>
		/// <param name="path">路径</param>
		/// <returns></returns>
		public string MapPath(object path)
		{
			string realPath = null;
			if(path != null) 
			{
				realPath = path.ToString();
				if(realPath.IndexOf(":\\") != 1){ realPath = System.Web.HttpContext.Current.Server.MapPath(realPath); }
			}
			return realPath;
		}

		/// <summary>
		/// MD5
		/// </summary>
		/// <param name="data">要加密的明文</param>
		/// <returns></returns>
		public static string MD5(object data)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (byte b in md5.ComputeHash(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data + ""))) { stringBuilder.Append(b.ToString("x2")); }
			return stringBuilder.ToString();
		}
		
		/// <summary>
		/// HMACMD5
		/// </summary>
		/// <param name="data">要加密的明文</param>
		/// <param name="key">密码</param>
		/// <returns></returns>
		public static string MD5(object data, object key)
		{
			System.Security.Cryptography.HMACMD5 hmacMD5 = new System.Security.Cryptography.HMACMD5(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(key + ""));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (byte b in hmacMD5.ComputeHash(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data + ""))) { stringBuilder.Append(b.ToString("x2")); }
			return stringBuilder.ToString();
		}
		
		/// <summary>
		/// 简易登录验证
		/// </summary>
		/// <returns></returns>
		public static string MiniLogin(System.Collections.Specialized.NameValueCollection users = null, string redirectUrl = null)
		{
			if(users == null){return SessionGet("MiniLoginKey");}
			//登录
			string miniLoginUser = System.Web.HttpContext.Current.Request["MiniLoginUser"];
			string miniLoginPassword = System.Web.HttpContext.Current.Request["MiniLoginPassword"];
			string miniLoginKey = System.Web.HttpContext.Current.Request["MiniLoginKey"];
			if(miniLoginKey != null)
			{
				miniLoginKey = System.Text.Encoding.GetEncoding("UTF-8").GetString(System.Convert.FromBase64String(miniLoginKey));
				if(miniLoginKey.IndexOf("=") > 0)
				{
					miniLoginUser = miniLoginKey.Substring(0, miniLoginKey.IndexOf("="));
					if(miniLoginKey.IndexOf("=") < miniLoginKey.Length-1)
					{
						miniLoginPassword = miniLoginKey.Substring(miniLoginKey.IndexOf("=")+1);
					}
				}
			}
			if(miniLoginUser != null && miniLoginPassword != null)
			{
				string miniLoginValue = users[miniLoginUser];
				if(miniLoginValue == null)
				{
					if(IsAjax()){Alert(1, "fail","快捷账号不存在");}
					Clear("<script type=\"text/javascript\">window.alert('快捷账号不存在');window.history.back();</script>");
				}
				else if(miniLoginValue == "")
				{
					if(IsAjax()){Alert(1, "fail","暂未初始化密码");}
					Clear("<script type=\"text/javascript\">window.alert('暂未初始化密码');window.history.back();</script>");
				}
				else if(miniLoginValue == "google")
				{
					if(miniLoginPassword != GoogleAuth(null))
					{
						if(IsAjax()){Alert(1, "fail","谷歌验证码错误");}
						Clear("<script type=\"text/javascript\">window.alert('谷歌验证码错误');window.history.back();</script>");
					}
				}
				else if(miniLoginValue.Length == 32)
				{
					if(MD5(miniLoginPassword) != miniLoginValue)
					{
						if(IsAjax()){Alert(1, "fail","固定密码错误");}
						Clear("<script type=\"text/javascript\">window.alert('固定密码错误');window.history.back();</script>");
					}
				}
				else
				{
					if(miniLoginPassword == System.DateTime.Now.AddMinutes(-1).ToString(miniLoginValue)){}
					else if(miniLoginPassword == System.DateTime.Now.ToString(miniLoginValue)){}
					else if(miniLoginPassword == System.DateTime.Now.AddMinutes(1).ToString(miniLoginValue)){}
					else if(miniLoginPassword == MD5(System.DateTime.Now.AddMinutes(-1).ToString(miniLoginValue))){}
					else if(miniLoginPassword == MD5(System.DateTime.Now.ToString(miniLoginValue))){}
					else if(miniLoginPassword == MD5(System.DateTime.Now.AddMinutes(1).ToString(miniLoginValue))){}
					else
					{
						if(IsAjax()){Alert(1, "fail","动态密码错误");}
						Clear("<script type=\"text/javascript\">window.alert('动态密码错误');window.history.back();</script>");
					}
				}
				
				SessionSet("MiniLoginKey", miniLoginUser);
				if(IsAjax())
				{
					Alert(0, "success", "登录成功");
				}
				string http_referer = System.Web.HttpContext.Current.Request.Form["HTTP_REFERER"];
				if(http_referer == null || http_referer.Length == 0){http_referer = redirectUrl;}
				if(http_referer == null || http_referer.Length == 0){http_referer = "./";}
				System.Web.HttpContext.Current.Response.Redirect(http_referer);
			}
			//退出
			if(System.Web.HttpContext.Current.Request["MiniLogin"] == "logout")
			{
				SessionSet("MiniLoginKey", "");
				string http_referer = System.Web.HttpContext.Current.Request["HTTP_REFERER"];
				if(http_referer == null || http_referer.Length == 0){http_referer = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];}
				if(http_referer == null || http_referer.Length == 0){http_referer = "./";}
				System.Web.HttpContext.Current.Response.Redirect(http_referer);
			}
			//鉴权
			miniLoginKey = SessionGet("MiniLoginKey");
			if(users.Count > 0)
			{
				if(miniLoginKey == null || miniLoginKey == "")
				{
					if(IsAjax()){Alert(1, "MiniLogin_Has_Logout","登录状态已退出");}
					Clear("<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>快捷登录</title><meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,user-scalable=0\" /><style type=\"text/css\">.login-form{max-width:240px;margin:0 auto;}.login-form input{width:100%;height:30px;line-height:20px;padding:5px;border:1px solid #ddd;border-radius:3px;margin:0;margin-bottom:24px;font-size:14px;box-sizing:content-box;-moz-box-sizing:content-box;-webkit-box-sizing:content-box;-webkit-appearance:none;	}.login-form .submit{height:30px;line-height:30px;border:none;;overflow:hidden;font-size:16px;background:#009688;color:#fff;}</style></head><body><form method=\"post\" action=\"" + System.Web.HttpContext.Current.Request.FilePath + "\" class=\"login-form\"><br /><input autoComplete=\"off\" required type=\"text\" name=\"MiniLoginUser\" placeHolder=\"账号名称\" /><input autoComplete=\"off\" required type=\"password\" name=\"MiniLoginPassword\" placeHolder=\"登录密码\" /><input class=\"submit\" type=\"submit\" value=\"快捷登录\" /><input type=\"hidden\" name=\"HTTP_REFERER\" value=\"" + System.Web.HttpContext.Current.Request.Url.ToString() + "\"></form></body></html>");
				}
			}
			return miniLoginKey;
		}

		
		/// <summary>
		/// 日期时间
		/// </summary>
		/// <returns></returns>
		public static string Now()
		{
			return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// 日期时间
		/// </summary>
		/// <param name="format">格式</param>
		/// <returns></returns>
		public static string Now(string format)
		{
			return System.DateTime.Now.ToString(format);
		}

		/// <summary>
		/// 日期时间
		/// </summary>
		/// <param name="seconds">秒数</param>
		/// <returns></returns>
		public static string Now(int seconds)
		{
			return System.DateTime.Now.AddSeconds(seconds).ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// 日期时间
		/// </summary>
		/// <param name="format">格式</param>
		/// <param name="seconds">秒数</param>
		/// <returns></returns>
		public static string Now(string format, int seconds)
		{
			if (format == null || format.Length == 0) { format = "yyyy-MM-dd HH:mm:ss"; }
			return System.DateTime.Now.AddSeconds(seconds).ToString(format);
		}
								
		/// <summary>
		/// 生成流水号
		/// </summary>
		/// <param name="prefix">前缀</param>
		/// <returns></returns>
		public static string SerialNumber(object prefix = null)
		{
			//new System.Threading.Thread(new System.Threading.ThreadStart(delegate{
				
			//})).Start();
			
			string serialNumber = prefix + System.DateTime.Now.ToString("yyyyMMddHHmmssffffff");
			serialNumber += System.Threading.Thread.CurrentThread.ManagedThreadId;
			return serialNumber;
		}
		
		/// <summary>
		/// 格式化请求字符串
		/// </summary>
		/// <param name="queryString">请求字符串</param>
		/// <param name="charset">编码</param>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection ParseQueryString(string queryString, string charset = "UTF-8")
		{
			return System.Web.HttpUtility.ParseQueryString(queryString + "", System.Text.Encoding.GetEncoding(charset));
		}
		
		/// <summary>
		/// 载入Xml
		/// </summary>
		/// <param name="strXml"></param>
		/// <param name="root"></param>
		/// <returns></returns>
		public static System.Xml.XmlNode ParseXml(string strXml, string root = "xml")
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Xml.XmlNode xml = null;
			try
			{
				xmlDocument.LoadXml(strXml.ToString().EndsWith("</" + root + ">") ? strXml.ToString() : "<" + root + ">" + strXml + "</" + root + ">");
				xml = xmlDocument[root];
			}
			catch(System.Exception exception){UrlEncode(exception.Message);}
			return xml;
		}
		
		/// <summary>
		/// POST值
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection Post()
		{
			return new System.Collections.Specialized.NameValueCollection(System.Web.HttpContext.Current.Request.Form);;
		}
		/// <summary>
		/// POST值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string Post(string key)
		{
			return System.Web.HttpContext.Current.Request.Form[key];
		}
		/// <summary>
		/// POST整数值
		/// </summary>
		/// <returns></returns>
		public static double PostID()
		{
			double result = 0;
            double.TryParse(System.Web.HttpContext.Current.Request.Form["ID"] + "", out result);
			return System.Math.Floor(result);
		}
		
		/// <summary>
		/// POST整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static double PostID(string key)
		{
			double result = 0;
            double.TryParse(System.Web.HttpContext.Current.Request.Form[key] + "", out result);
			return System.Math.Floor(result);
		}
		/// <summary>
		/// POST整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static double PostID(string key, double defaultID)
		{
			double result = 0;
			if (!double.TryParse(System.Web.HttpContext.Current.Request.Form[key] + "", out result)) { result = defaultID; }
			return System.Math.Floor(result);
		}

		/// <summary>
		/// POST同参名的值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string[] Posts(string key)
		{
			return System.Web.HttpContext.Current.Request.Form.GetValues(key);
		}
		
		/// <summary>
		/// 请求字符串
		/// </summary>
		public static string QueryString()
		{
			return System.Web.HttpContext.Current.Request.QueryString.ToString();
		}
		
		/// <summary>
		/// 过滤请求字符串
		/// </summary>
		/// <param name="removeKeys">要去除的参数名</param>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection QueryString(string removeKeys)
		{
			System.Collections.Specialized.NameValueCollection request = System.Web.HttpUtility.ParseQueryString(System.Web.HttpContext.Current.Request.QueryString.ToString());
			if(removeKeys != null && removeKeys.Length > 0)
			{
				foreach(string key in removeKeys.Split(",".ToCharArray())) { request.Remove(key); }
			}
			return request;
		}

        /// <summary>
        /// 10进制转换
        /// </summary>
        /// <param name="number"></param>
        /// <param name="radix"></param>
        /// <returns></returns>
        public static string Radix(int number, int radix)
        {
            string result = null;
            if (radix >= 2 && radix <= 62)
            {
                int num = System.Math.Abs(number);
                if (num == 0)
                {
                    result = "0";
                }
                else
                {
                    result = "";
                    double remainer = 0;
                    int i = 0;
                    while (remainer < num)
                    {
                        i++;
                        string n = (num - remainer) % System.Math.Pow(radix, i) / System.Math.Pow(radix, i - 1) + "";
                        for (int j = 10;j < 62; j++) {
                            n = n.Replace(j.ToString(), "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(j - 10,1));
                        }
                        result = n + result;
                        remainer = num % System.Math.Pow(radix, i);
                    }
                }
                result = string.Join("", result.ToCharArray());
                if (number < 0) { result = "-" + result; }

            }
            return result;
        }

		/// <summary>
		/// 随机数
		/// </summary>
		/// <returns></returns>
		public static int Random()
		{
			System.Random random = new System.Random(System.BitConverter.ToInt32(System.Guid.NewGuid().ToByteArray(), 0));
			return random.Next();
		}
		
		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="start">最小值</param>
		/// <param name="end">最大值</param>
		/// <returns></returns>
		public static int Random(int start, int end)
		{
			System.Random random = new System.Random(System.BitConverter.ToInt32(System.Guid.NewGuid().ToByteArray(), 0));
			return random.Next(System.Math.Min(start, end+1), System.Math.Max(start, end+1));
		}

		/// <summary>
		/// 请求URL：重写后的相对根路径，不带域名，带参数
		/// </summary>
		/// <returns></returns>
		public static string RawUrl()
		{
			return System.Web.HttpContext.Current.Request.RawUrl;
		}
		
		/// <summary>
		/// 当前URL，带协议和域名
		/// </summary>
		/// <param name="path">可取空，空字符串，斜杠，问号，路径</param>
		public static string RawUrl(string path)
		{
			string url = System.Web.HttpContext.Current.Request.Url.ToString();
			string rawUrl =  System.Web.HttpContext.Current.Request.RawUrl;
			if (path == "/")
			{
				url = url.Substring(0, url.IndexOf("/", 10));
			}
			else if (path == "./")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + rawUrl.Substring(0, rawUrl.LastIndexOf("/") + 1);
			}
			else if (path == "?")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + (rawUrl.Contains("?") ? rawUrl.Substring(0, rawUrl.IndexOf("?")) : rawUrl);
			}
			else if (path == "")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + rawUrl;
			}
			else if (path.StartsWith("/"))
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + path;
			}
			else
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + rawUrl.Substring(0, rawUrl.LastIndexOf("/") + 1) + path;
			}
			return url;
		}

		/// <summary>
		/// 转向
		/// </summary>
		public static void Redirect()
		{
			string url = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
			if (url == null || url.Length == 0) { url = "/"; }
			System.Web.HttpContext.Current.Response.Redirect(url);
		}
		/// <summary>
		/// 转向
		/// </summary>
		/// <param name="url">页面地址，默认为来源页，空字符串为首页</param>
		public static void Redirect(string url)
		{
			if (url != null && url.StartsWith("?")) { url = System.Web.HttpContext.Current.Request.FilePath + url; }
			System.Web.HttpContext.Current.Response.Redirect(url);
		}
		/// <summary>
		/// 301重定向
		/// </summary>
		/// <param name="domainName">唯一用来显示的域名</param>
		public static void Redirect301(string domainName)
		{
			string serverIP = ServerVariables("local_ADDR"), customIP = ServerVariables("REMOTE_ADDR");
			string host = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower();
			if (serverIP != customIP && host != (domainName + "").ToLower())
			{
				System.Web.HttpContext.Current.Response.StatusCode = 301;
				System.Web.HttpContext.Current.Response.Status = "301 Moved Permanently";
				System.Web.HttpContext.Current.Response.AddHeader("Location", "http://" + domainName + System.Web.HttpContext.Current.Request.RawUrl);
				System.Web.HttpContext.Current.Response.End();
			}
		}
		/// <summary>
		/// 转向
		/// </summary>
		/// <param name="url">页面地址，默认为来源页，空字符串为首页</param>
		public static void RedirectDebug(string url)
		{
			if (url != null && url.StartsWith("?")) { url = System.Web.HttpContext.Current.Request.FilePath + url; }
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write("<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" /></head><body>");
			System.Web.HttpContext.Current.Response.Write(System.Web.HttpContext.Current.Request.Url + "<br />");
			System.Web.HttpContext.Current.Response.Write("<a href=\"" + url + "\">" + url + "</a>");
			System.Web.HttpContext.Current.Response.Write("</body></html>");
			System.Web.HttpContext.Current.Response.End();
		}

		/// <summary>
		/// 来源页
		/// </summary>
		/// <param name="defaultUrl">备用地址，当来源页不存在时启用</param>
		/// <param name="key">地址栏参数名</param>
		/// <returns></returns>
		public static string Referer()
		{
			return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
		}
		/// <summary>
		/// 来源页
		/// </summary>
		/// <param name="key">地址栏参数名</param>
		/// <returns></returns>
		public static string Referer(string key)
		{
			string url = System.Web.HttpContext.Current.Request.QueryString[key];
			if (url == null || url.Length == 0) { url = System.Web.HttpContext.Current.Request.Form[key]; }
			if (url == null || url.Length == 0) { url = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; }
			if (url == null || url.Length == 0) { url = "/"; }
			return url;
		}

		/// <summary>
		/// 字符串替换
		/// </summary>
		/// <param name="obj0">要做替换的字符串</param>
		/// <param name="obj1">被替换掉的字符串</param>
		/// <param name="obj2">被替换后的字符串</param>
		/// <returns></returns>
		public static string Replace(object obj0, object obj1, object obj2)
		{
			return System.Text.RegularExpressions.Regex.Replace(obj0 + "", obj1 + "", obj2 + "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// Request整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static double RequestID(string key)
		{
			string id = System.Web.HttpContext.Current.Request.QueryString[key];
			if (id == null) { id = System.Web.HttpContext.Current.Request.Form[key]; }
			double result = 0;
            double.TryParse(id + "", out result);
			return System.Math.Floor(result);
		}
		/// <summary>
		/// Request整数值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <param name="defaultID">默认ID</param>
		/// <returns></returns>
		public static double RequestID(string key, double defaultID)
		{
			string id = System.Web.HttpContext.Current.Request.QueryString[key];
			if (id == null) { id = System.Web.HttpContext.Current.Request.Form[key]; }
			double result = 0; if (!double.TryParse(id + "", out result)) { result = defaultID; }
			return System.Math.Floor(result);
		}

		/// <summary>
		/// 请求(先GET后POST)
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection Requests()
		{
			System.Collections.Specialized.NameValueCollection request = System.Web.HttpContext.Current.Request.QueryString;
			if (request == null || request.Count == 0) { request = System.Web.HttpContext.Current.Request.Form; }
			return new System.Collections.Specialized.NameValueCollection(request);
		}

		/// <summary>
		/// Request同参名的值
		/// </summary>
		/// <param name="key">参数名</param>
		/// <returns></returns>
		public static string[] Requests(string key)
		{
			string[] result = System.Web.HttpContext.Current.Request.QueryString.GetValues(key);
			if (result == null) { result = System.Web.HttpContext.Current.Request.Form.GetValues(key); }
			return result;
		}
			
		/// <summary>
		/// 强制下载（另存为）
		/// </summary>
		/// <param name="filePath">文件名</param>
		public static void SaveAs(string filePath)
		{
			SaveAs(filePath, null);
		}
		/// <summary>
		/// 强制下载（另存为）
		/// </summary>
		/// <param name="filePath">文件名</param>
		/// <param name="contentType">文件MIME类型</param>
		public static void SaveAs(string filePath, string fileExt)
		{
			string fileName = filePath.Replace("\\", "/").Replace(" ", "");
			if (fileName.Contains("/")) { fileName = fileName.Substring(fileName.LastIndexOf("/") + 1); }
			string contentType = "application/octet-stream";
			if (fileExt != null && fileName.Contains("/"))
			{
				System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
				dict.Add(".html", "text/html");
				dict.Add(".htm", "text/html");
				dict.Add(".ico", "image/x-icon");
				dict.Add(".bmp", "image/bmp");
				dict.Add(".gif", "image/gif");
				dict.Add(".jpg", "image/jpeg");
				dict.Add(".png", "image/png");
				dict.Add(".js", "application/x-javascript");
				dict.Add(".mp3", "audio/mpeg");
				dict.Add(".mp4", "video/mp4");
				dict.Add(".pdf", "application/pdf");
				dict.Add(".doc", "application/vnd.ms-word");
				dict.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
				dict.Add(".xls", "application/vnd.ms-excel");
				dict.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				dict.Add(".ppt", "application/vnd.ms-powerpoint");
				dict.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
				dict.Add(".txt", "text/plain");
				dict.Add(".xml", "text/xml");
				string ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
				if (dict.ContainsKey(ext)) { contentType = dict[ext]; }
			}
			System.Web.HttpContext.Current.Response.Clear();
			filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
				System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
				//不指明Content-Length用Flush的话不会显示下载进度   
				System.Web.HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
				System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
				System.Web.HttpContext.Current.Response.ContentType = contentType;
				System.Web.HttpContext.Current.Response.TransmitFile(filePath, 0, fileInfo.Length);
			}
			else
			{
				System.Web.HttpContext.Current.Response.Write(fileName + " is not found");
			}
			System.Web.HttpContext.Current.Response.End();
		}

		/// <summary>
		/// 服务器变量
		/// </summary>
		/// <returns></returns>
		public static string ServerVariables()
		{
			System.Collections.Specialized.NameValueCollection serverVariables = System.Web.HttpContext.Current.Request.ServerVariables;
			string result = "";
			foreach (string key in serverVariables) { result += "<br />【" + key + "】：" + serverVariables[key] + "\n"; };
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Write(result);
			System.Web.HttpContext.Current.Response.End();
			return null;
		}
		/// <summary>
		/// 服务器变量
		/// </summary>
		/// <param name="name">输出全部信息</param>
		/// <returns></returns>
		public static string ServerVariables(string name)
		{
			System.Collections.Specialized.NameValueCollection serverVariables = System.Web.HttpContext.Current.Request.ServerVariables;
			string result = serverVariables[name];
			return result;
		}

		/// <summary>
		/// SESSION操作
		/// </summary>
		/// <param name="key">SESSION名</param>
		/// <returns></returns>
		public static string SessionGet(string key)
		{
			object result = System.Web.HttpContext.Current.Session == null ? "" : System.Web.HttpContext.Current.Session[key];
			return result + "";
		}
		
		/// <summary>
		/// SESSION操作
		/// </summary>
		/// <param name="key">SESSION名</param>
		/// <returns></returns>
		public static double SessionGetID(string key)
		{
			object result = System.Web.HttpContext.Current.Session == null ? null : System.Web.HttpContext.Current.Session[key];
			double id = 0;
            double.TryParse(result + "", out id);
			return System.Math.Floor(id);
		}
		
		/// <summary>
		/// SESSION操作
		/// </summary>
		/// <param name="key">SESSION名</param>
		/// <param name="obj">SESSION值</param>
		/// <returns></returns>
		public static string SessionSet(string key, object val)
		{
			System.Web.HttpContext.Current.Session[key] = val + "";
			return val + "";
		}

		/// <summary>
		/// SHA1模式加密
		/// </summary>
		/// <param name="str">待加密文</param>
		/// <returns></returns>
		public static string SHA1(object obj, object mode = null,string charset = "UTF-8")
		{
			System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			if(string.IsNullOrEmpty(charset)) { charset = "UTF-8"; }
			byte[] bytes = sha1.ComputeHash(System.Text.Encoding.GetEncoding(charset).GetBytes(obj + ""));
			if(mode == null) { mode = 10; }
			if(mode.ToString() == "10" || mode.ToString().ToLower() == "hex")
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				foreach (byte b in bytes) { stringBuilder.Append(b.ToString("x2")); }
				return stringBuilder.ToString();
			}
			else if(mode.ToString() == "64" || mode.ToString().ToLower() == "base64")
			{
				return System.Convert.ToBase64String(bytes);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// SHA1模式加密
		/// </summary>
		/// <param name="obj">待加密文</param>
		/// <param name="key">密码</param>
		/// <returns></returns>
		public static string HMACSHA1(object obj, object key, object mode = null,string charset = "UTF-8")
		{
			System.Security.Cryptography.HMACSHA1 hmacSHA1 = new System.Security.Cryptography.HMACSHA1(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(key + ""));
			if(string.IsNullOrEmpty(charset)) { charset = "UTF-8"; }
			byte[] bytes = hmacSHA1.ComputeHash(System.Text.Encoding.GetEncoding(charset).GetBytes(obj + ""));
			if(mode == null) { mode = 10; }
			if(mode.ToString() == "10" || mode.ToString().ToLower() == "hex")
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				foreach (byte b in bytes) { stringBuilder.Append(b.ToString("x2")); }
				return stringBuilder.ToString();
			}
			else if(mode.ToString() == "64" || mode.ToString().ToLower() == "base64")
			{
				return System.Convert.ToBase64String(bytes);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// SHA256模式加密
		/// </summary>
		/// <param name="obj">待加密文</param>
		/// <returns></returns>
		public static string SHA256(object obj, object mode = null,string charset = "UTF-8")
		{
			System.Security.Cryptography.SHA256CryptoServiceProvider sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
			if(string.IsNullOrEmpty(charset)) { charset = "UTF-8"; }
			byte[] bytes = sha256.ComputeHash(System.Text.Encoding.GetEncoding(charset).GetBytes(obj + ""));
			if(mode == null) { mode = 10; }
			if(mode.ToString() == "10" || mode.ToString().ToLower() == "hex")
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				foreach (byte b in bytes) { stringBuilder.Append(b.ToString("x2")); }
				return stringBuilder.ToString();
			}
			else if(mode.ToString() == "64" || mode.ToString().ToLower() == "base64")
			{
				return System.Convert.ToBase64String(bytes);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// SHA256模式加密
		/// </summary>
		/// <param name="obj">待加密文</param>
		/// <param name="key">密码</param>
		public static string HMACSHA256(object obj, object key, object mode = null,string charset = "UTF-8")
		{
			System.Security.Cryptography.HMACSHA256 hmacSHA256 = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(key + ""));
			if(string.IsNullOrEmpty(charset)) { charset = "UTF-8"; }
			byte[] bytes = hmacSHA256.ComputeHash(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj + ""));
			
			if(mode == null) { mode = 10; }
			if(mode.ToString() == "10" || mode.ToString().ToLower() == "hex")
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				foreach (byte b in bytes) { stringBuilder.Append(b.ToString("x2")); }
				return stringBuilder.ToString();
			}
			else if(mode.ToString() == "64" || mode.ToString().ToLower() == "base64")
			{
				return System.Convert.ToBase64String(bytes);
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 测试SOCKET状态
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static bool SocketConnect(string ip, int port)
        {
            bool result = true;
            try
            {
                System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                socket.Connect(ip, port);
                socket.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// SOCKET发送数据
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <param name="port">端口</param>
        /// <param name="message">数据</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string SocketSend(string ip, int port, string message, string charset = "UTF-8")
		{
			string result = "222";
			System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
			socket.BeginConnect(ip, port, delegate (System.IAsyncResult iar1)
			{
				System.Net.Sockets.Socket socket1 = (System.Net.Sockets.Socket)iar1.AsyncState;
				try
				{
					socket1.EndConnect(iar1);
					byte[] data = System.Text.Encoding.GetEncoding(charset).GetBytes(message);
					socket1.BeginSend(data, 0, data.Length, System.Net.Sockets.SocketFlags.None, delegate (System.IAsyncResult iar2)
					{
						System.Net.Sockets.Socket socket2 = (System.Net.Sockets.Socket)iar2.AsyncState;
						try
						{
							int endSendResult = socket2.EndSend(iar2);//返回消息长度
							socket2.Shutdown(System.Net.Sockets.SocketShutdown.Both);
							result = endSendResult + "";
                        }
						finally
						{
							System.Threading.Thread.Sleep(50000);
							socket2.Close();
						}
					}, socket1);
				}
				catch (System.Exception exception)
				{
					result = exception.Message;
					socket1.Close();
				}

			}, socket);

			return result;
		}
		
		/// <summary>
        /// SOCKET发送数据
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <param name="port">端口</param>
        /// <param name="message">数据</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string SocketReceive(string ip, int port, string message, string charset = "UTF-8")
		{
			System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
			socket.Connect(ip, port);
			byte[] sendByte = System.Text.Encoding.GetEncoding(charset).GetBytes(message);
			socket.Send(sendByte, sendByte.Length, 0);
			byte[] receiveByte = new byte[4096];
            int resv = socket.Receive(receiveByte,receiveByte.Length,0);
            string result = System.Text.Encoding.GetEncoding(charset).GetString(receiveByte, 0, resv);
			return result;
		}
		
		/// <summary>
        /// TcpClient发送数据
        /// </summary>
        /// <param name="ip">目标IP</param>
        /// <param name="port">端口</param>
        /// <param name="message">数据</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
		public static string TcpClient(string ip, int port, string message, string charset = "UTF-8")
		{
			System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(ip, port);
			System.Net.Sockets.NetworkStream networkStream  = tcpClient.GetStream();
			byte[] sendByte = System.Text.Encoding.GetEncoding(charset).GetBytes(message);
			networkStream.Write(sendByte,0,sendByte.Length);
			byte[] receiveByte = new byte[4096];
			networkStream.Read(receiveByte, 0, receiveByte.Length); 
            string result = System.Text.Encoding.GetEncoding(charset).GetString(receiveByte, 0, receiveByte.Length);
			return result;
		}

		/// <summary>
		/// RSA专用
		/// </summary>
		/// <returns></returns>
		private static int rsaGetPosition(System.IO.BinaryReader binaryReader)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binaryReader.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binaryReader.ReadByte();

            if (bt == 0x81)
                count = binaryReader.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
                {
                    highbyte = binaryReader.ReadByte();	// data size in next 2 bytes
                    lowbyte = binaryReader.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = System.BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }

            while (binaryReader.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binaryReader.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
		
		/// <summary>
		/// RSA：公钥加密，私钥解密，私钥签名，公钥验签
		/// </summary>
		/// <param name="cert">密钥证书或密钥文本</param>
		/// <param name="password">密钥密码</param>
		/// <param name="dwKeySize">-1公钥，0私钥，其余代表密钥长度</param>
		/// <returns></returns>
		public static System.Security.Cryptography.RSACryptoServiceProvider RSA(string cert, object password = null, int dwKeySize = 0)
		{
			if (cert == null)
			{
				string help = "一般来说：公钥加密，私钥解密，私钥签名，公钥验签，以实现只有本人能接收加密过的消息和发布带签名的消息，反之是没有意义的";
				help += "<br />System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();";

				help += "<br />string privateKey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rsa.ToXmlString(true)));";
				help += "<br />System.Security.Cryptography.RSACryptoServiceProvider privateRSA = new System.Security.Cryptography.RSACryptoServiceProvider();";
				help += "<br />privateRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(privateKey)));";

				help += "<br />string publicKey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rsa.ToXmlString(false)));";
				help += "<br />System.Security.Cryptography.RSACryptoServiceProvider publicRSA = new System.Security.Cryptography.RSACryptoServiceProvider();";
				help += "<br />publicRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(publicKey)));";

				help += "<br />System.Convert.ToBase64String(publicRSA.Encrypt(System.Text.Encoding.GetEncoding(charset).GetBytes(cert), false));";
				help += "<br />System.Text.Encoding.GetEncoding(charset).GetString(privateRSA.Decrypt(System.Convert.FromBase64String(cert), false));";
				help += "<br />System.Convert.ToBase64String(privateRSA.SignData(System.Text.Encoding.GetEncoding(charset).GetBytes(cert), signType));";
				help += "<br />publicRSA.VerifyData(System.Text.Encoding.GetEncoding(charset).GetBytes(cert), signType, System.Convert.FromBase64String(signData));";
			}

			string filePath = (cert + "").ToLower();
			if (filePath.EndsWith(".xml"))
			{
				if (filePath.IndexOf(":\\")<0)  { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);  }
				string xml = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
				System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
				rsa.FromXmlString(xml);
				return rsa;
			}
			else if (filePath.EndsWith(".pfx") || filePath.EndsWith(".p12"))
			{
				if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = null;
				if (password == null)
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				}
				else
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath, password.ToString(), System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
				}
				if (1==1)
				{
					return (System.Security.Cryptography.RSACryptoServiceProvider)x509Certificate2.PrivateKey;
				}
				else
				{
					return (System.Security.Cryptography.RSACryptoServiceProvider)x509Certificate2.PublicKey.Key;
				}
			}
			else if (filePath.EndsWith(".cer"))
			{
				if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 rsa = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				return (System.Security.Cryptography.RSACryptoServiceProvider)rsa.PublicKey.Key;
			}
			else 
			{
				if (filePath.EndsWith(".key") || filePath.EndsWith(".pem") || filePath.EndsWith(".txt"))
				{
					if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
					cert = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
				}
				cert = cert.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r", "");
				cert = cert.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "");

				byte[] keyBytes = System.Convert.FromBase64String(cert);
				if(keyBytes.Length == 162)
				{
					//公钥
					System.Security.Cryptography.RSAParameters rsaParameters = new System.Security.Cryptography.RSAParameters();
					
					byte[] modulus = new byte[128];
					System.Array.Copy(keyBytes, 29, modulus, 0, 128);
					rsaParameters.Modulus = modulus;
					
					byte[] exponent = new byte[3];
					System.Array.Copy(keyBytes, 159, exponent, 0, 3);
					rsaParameters.Exponent = exponent;
					
					System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
					rsa.ImportParameters(rsaParameters);
					return rsa;
				}
				else if(keyBytes.Length == 294)
				{
					//公钥
					System.Security.Cryptography.RSAParameters rsaParameters = new System.Security.Cryptography.RSAParameters();
					
					byte[] modulus = new byte[256];
					System.Array.Copy(keyBytes, 33, modulus, 0, 256);
					rsaParameters.Modulus = modulus;
					
					byte[] exponent = new byte[3];
					System.Array.Copy(keyBytes, 291, exponent, 0, 3);
					rsaParameters.Exponent = exponent;
					
					System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
					rsa.ImportParameters(rsaParameters);
					return rsa;
				}
				else
				{
					//私钥
					System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(new System.IO.MemoryStream(keyBytes));
					try
					{
						byte[] modulus, e, d, p, q, dp, dq, inverseQ;
						ushort twoBytes = binaryReader.ReadUInt16();
						if (twoBytes == 0x8130)
						{
							binaryReader.ReadByte();
						}
						else if (twoBytes == 0x8230)
						{
							binaryReader.ReadInt16();
						}
						else
						{
							return null;
						}
						twoBytes = binaryReader.ReadUInt16();
						if (twoBytes != 0x0102)
						{
							return null;
						}
						byte readByte = binaryReader.ReadByte();
						if (readByte != 0x00)
						{
							return null;
						}
						int elems = rsaGetPosition(binaryReader);
						modulus = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						e = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						d = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						p = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						q = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						dp = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						dq = binaryReader.ReadBytes(elems);

						elems = rsaGetPosition(binaryReader);
						inverseQ = binaryReader.ReadBytes(elems);

						System.Security.Cryptography.CspParameters cspParameters = new System.Security.Cryptography.CspParameters();
						cspParameters.Flags = System.Security.Cryptography.CspProviderFlags.UseMachineKeyStore;
						System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(0, cspParameters);
						System.Security.Cryptography.RSAParameters rsaParameters = new System.Security.Cryptography.RSAParameters();
						rsaParameters.Modulus = modulus;
						rsaParameters.Exponent = e;
						rsaParameters.D = d;
						rsaParameters.P = p;
						rsaParameters.Q = q;
						rsaParameters.DP = dp;
						rsaParameters.DQ = dq;
						rsaParameters.InverseQ = inverseQ;
						rsa.ImportParameters(rsaParameters);
						return rsa;
					}
					catch (System.Exception ex)
					{
						if(ex.Message.Length > 0)
						{
							return null;
						}
						return null;
					}
					finally
					{
						binaryReader.Close();
					}
				}
			}
			return null;
		}
		/// <summary>
		/// RSA：公钥加密，私钥解密，私钥签名，公钥验签
		/// </summary>
		/// <param name="cert">密钥证书或密钥文本</param>
		/// <param name="password">密钥密码</param>
		/// <param name="certType">-1公钥，0私钥，其余代表密钥长度</param>
		/// <returns></returns>
		public static System.Security.Cryptography.RSACryptoServiceProvider PrivateRSA(string cert, object password = null, string certType = null)
		{
			if (cert == null)
			{
				string help = "一般来说：公钥加密，私钥解密，私钥签名，公钥验签，以实现只有本人能接收加密过的消息和发布带签名的消息，反之是没有意义的";
				help += "<br />System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();";

				help += "<br />string privateKey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rsa.ToXmlString(true)));";
				help += "<br />System.Security.Cryptography.RSACryptoServiceProvider privateRSA = new System.Security.Cryptography.RSACryptoServiceProvider();";
				help += "<br />privateRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(privateKey)));";

				help += "<br />string publicKey = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rsa.ToXmlString(false)));";
				help += "<br />System.Security.Cryptography.RSACryptoServiceProvider publicRSA = new System.Security.Cryptography.RSACryptoServiceProvider();";
				help += "<br />publicRSA.FromXmlString(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(publicKey)));";

				help += "<br />System.Convert.ToBase64String(publicRSA.Encrypt(System.Text.Encoding.GetEncoding(charset).GetBytes(content), false));";
				help += "<br />System.Text.Encoding.GetEncoding(charset).GetString(privateRSA.Decrypt(System.Convert.FromBase64String(content), false));";
				help += "<br />System.Convert.ToBase64String(privateRSA.SignData(System.Text.Encoding.GetEncoding(charset).GetBytes(content), signType));";
				help += "<br />publicRSA.VerifyData(System.Text.Encoding.GetEncoding(charset).GetBytes(content), signType, System.Convert.FromBase64String(signData));";
			}

			string filePath = (cert + "").ToLower();
			if (filePath.EndsWith(".xml"))
			{
				if (filePath.IndexOf(":\\")<0)  { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);  }
				string xml = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
				System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
				rsa.FromXmlString(xml);
				return rsa;
			}
			else if (filePath.EndsWith(".pfx") || filePath.EndsWith(".p12"))
			{
				if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = null;
				if (password == null)
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				}
				else
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath, password.ToString(), System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
				}
				return (System.Security.Cryptography.RSACryptoServiceProvider)x509Certificate2.PrivateKey;
				//return (System.Security.Cryptography.RSACryptoServiceProvider)x509Certificate2.PublicKey.Key;
			}
			else if (filePath.EndsWith(".cer"))
			{
				if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 rsa = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				return (System.Security.Cryptography.RSACryptoServiceProvider)rsa.PublicKey.Key;
			}
			else
			{
				if (filePath.EndsWith(".key") || filePath.EndsWith(".pem") || filePath.EndsWith(".txt"))
				{
					if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
					cert = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
				}
				cert = cert.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r", "");
				cert = cert.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "");
				
				byte[] keyBytes = System.Convert.FromBase64String(cert);
				System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(new System.IO.MemoryStream(keyBytes));
				try
				{
					byte[] modulus, e, d, p, q, dp, dq, inverseQ;
					ushort twoBytes = binaryReader.ReadUInt16();
					if (twoBytes == 0x8130)
					{
						binaryReader.ReadByte();
					}
					else if (twoBytes == 0x8230)
					{
						binaryReader.ReadInt16();
					}
					else
					{
						return null;
					}
					twoBytes = binaryReader.ReadUInt16();
					if (twoBytes != 0x0102)
					{
						return null;
					}
					byte readByte = binaryReader.ReadByte();
					if (readByte != 0x00)
					{
						return null;
					}
					int elems = rsaGetPosition(binaryReader);
					modulus = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					e = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					d = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					p = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					q = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					dp = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					dq = binaryReader.ReadBytes(elems);

					elems = rsaGetPosition(binaryReader);
					inverseQ = binaryReader.ReadBytes(elems);

					System.Security.Cryptography.CspParameters cspParameters = new System.Security.Cryptography.CspParameters();
					cspParameters.Flags = System.Security.Cryptography.CspProviderFlags.UseMachineKeyStore;
					System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(0, cspParameters);
					System.Security.Cryptography.RSAParameters rsaParameters = new System.Security.Cryptography.RSAParameters();
					rsaParameters.Modulus = modulus;
					rsaParameters.Exponent = e;
					rsaParameters.D = d;
					rsaParameters.P = p;
					rsaParameters.Q = q;
					rsaParameters.DP = dp;
					rsaParameters.DQ = dq;
					rsaParameters.InverseQ = inverseQ;
					rsa.ImportParameters(rsaParameters);
					return rsa;
				}
				catch (System.Exception ex)
				{
					if(ex.Message.Length > 0)
					{
						return null;
					}
					return null;
				}
				finally
				{
					binaryReader.Close();
				}
			}
			return null;
		}

		/// <summary>
		/// 公钥加密
		/// </summary>
		/// <param name="publicRSA">RSA对象</param>
		/// <param name="content">待加密数据</param>
		/// <returns></returns>
		public static string RSAEncrypt(System.Security.Cryptography.RSACryptoServiceProvider publicRSA, string content, string charset = "UTF-8")
		{
			byte[] byteText = System.Text.Encoding.GetEncoding(charset).GetBytes(content);
			int maxSize = publicRSA.KeySize / 8 - 11;
			System.IO.MemoryStream source = new System.IO.MemoryStream(byteText);
			System.IO.MemoryStream target = new System.IO.MemoryStream();
			byte[] buffer = new byte[maxSize];
			int size = source.Read(buffer, 0, maxSize);
			while (size > 0)
			{
				byte[] temp = new byte[size];
				System.Array.Copy(buffer, 0, temp, 0, size);
				
				byte[] encrypt = publicRSA.Encrypt(temp, false);
				target.Write(encrypt, 0, encrypt.Length);
				size = source.Read(buffer, 0, maxSize);
			}
			return System.Convert.ToBase64String(target.ToArray(), System.Base64FormattingOptions.None);
		}

		/// <summary>
		/// 私钥解密
		/// </summary>
		/// <param name="privateRSA">RSA对象</param>
		/// <param name="content">待解密字符串</param>
		/// <returns></returns>
		public static string RSADecrypt(System.Security.Cryptography.RSACryptoServiceProvider privateRSA, string content, string charset = "UTF-8")
		{
			byte[] byteText = System.Convert.FromBase64String(content);
			int keySize = privateRSA.KeySize / 8;
			System.IO.MemoryStream source = new System.IO.MemoryStream(byteText);
			System.IO.MemoryStream target = new System.IO.MemoryStream();
			byte[] buffer = new byte[keySize];
			int size = source.Read(buffer, 0, keySize);
			while (size > 0)
			{
				byte[] temp = new byte[size];
				System.Array.Copy(buffer, 0, temp, 0, size);
				
				byte[] decrypt = privateRSA.Decrypt(temp, false);
				target.Write(decrypt, 0, decrypt.Length);
				size = source.Read(buffer, 0, keySize);
			}
			return System.Text.Encoding.GetEncoding(charset).GetString(target.ToArray());
		}

		/// <summary>
		/// 私钥签名
		/// </summary>
		/// <param name="privateRSA">RSA对象</param>
		/// <param name="bytes">待签数据</param>
		/// <param name="hashType">哈希摘要模式</param>
		/// <returns></returns>
		public static string RSASign(System.Security.Cryptography.RSACryptoServiceProvider privateRSA, byte[] bytes, string hashType = "SHA1")
		{
			return System.Convert.ToBase64String(privateRSA.SignData(bytes, hashType));
		}

		/// <summary>
		/// 私钥签名
		/// </summary>
		/// <param name="privateRSA">RSA对象</param>
		/// <param name="content">待签数据</param>
		/// <param name="hashType">哈希摘要模式</param>
		/// <returns></returns>
		public static string RSASign(System.Security.Cryptography.RSACryptoServiceProvider privateRSA, string content, string hashType = "SHA1")
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
			return System.Convert.ToBase64String(privateRSA.SignData(bytes, hashType));
		}

		/// <summary>
		/// 公钥验签
		/// </summary>
		/// <param name="rsa">RSA对象</param>
		/// <param name="bytes">待签数据</param>
		/// <param name="signature">签名结果</param>
		/// <param name="hashType">哈希摘要模式</param>
		/// <returns></returns>
		public static bool RSAVerify(System.Security.Cryptography.RSACryptoServiceProvider publicRSA, byte[] bytes, string signature, string hashType = "SHA1")
		{
			return publicRSA.VerifyData(bytes, hashType, System.Convert.FromBase64String(signature));
		}
		
		/// <summary>
		/// 公钥验签
		/// </summary>
		/// <param name="rsa">RSA对象</param>
		/// <param name="content">待签数据</param>
		/// <param name="signature">签名结果</param>
		/// <param name="hashType">哈希摘要模式</param>
		/// <returns></returns>
		public static bool RSAVerify(System.Security.Cryptography.RSACryptoServiceProvider publicRSA, string content, string signature, string hashType = "SHA1")
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
			return publicRSA.VerifyData(bytes, hashType, System.Convert.FromBase64String(signature));
		}

		/// <summary>
		/// 字符串分割
		/// </summary>
		/// <param name="strAll">要被分割的字符串</param>
		/// <param name="strChar">用来分割的字符串</param>
		/// <returns></returns>
		public static string[] Split(string strAll, string strChar)
		{
			return (new System.Text.RegularExpressions.Regex(strChar)).Split(strAll);
		}
		
		/// <summary>
		/// Xml字符串截取
		/// </summary>
		/// <param name="strXml"></param>
		/// <returns></returns>
		public static string SubXml(string strXml, string tag)
		{
			string xml = null;
			if(strXml.Contains("<" + tag + ">") && strXml.LastIndexOf("</" + tag + ">")>strXml.IndexOf("<" + tag + ">"))
			{
				xml = strXml.Substring(strXml.IndexOf("<" + tag + ">") + tag.Length + 2);
				xml = xml.Substring(0, xml.LastIndexOf("</" + tag + ">"));
			}
			return xml;
		}
		
		/// <summary>
		/// 字符串截取
		/// </summary>
		/// <param name="str"></param>
		/// <param name="fromTag"></param>
		/// <param name="endTag"></param>
		/// <returns></returns>
		public static string SubStr(string source, string fromTag, string endTag, bool endTagMust = true)
		{
			string res = null;
			if(source.Contains(fromTag))
			{
				source = source.Substring(source.IndexOf(fromTag) + fromTag.Length);
				
				if(string.IsNullOrEmpty(endTag))
				{
					res = source;
				}
				else if(source.Contains(endTag))
				{
					res = source.Substring(0, source.IndexOf(endTag));
				}
				else if(!endTagMust)
				{
					res = source;
				}
			}
			return res;
		}
		
		/// <summary>
		/// 载入模板
		/// </summary>
		/// <param name="templatePath">模板路径</param>
		/// <returns></returns>
		public static string Template(string templatePath)
		{
			string filePath = System.Web.HttpContext.Current.Request.FilePath.Substring(System.Web.HttpContext.Current.Request.FilePath.LastIndexOf("/"));
			templatePath = templatePath.TrimEnd("/".ToCharArray()) + filePath;
			if(System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
			{
				System.Web.HttpContext.Current.Server.Transfer(templatePath);
			}
			return templatePath;
		}
		
		/// <summary>
		/// 多线程DEMO
		/// </summary>
		public static void ThreadStart()
		{
			new System.Threading.Thread(new System.Threading.ThreadStart(delegate{
				HttpGet("http://www.yigebucunzaideyuming.com");
			})).Start();
		}
		
		/// <summary>
		/// 数据行转Dict
		/// </summary>
		/// <param name="dataRow">数据行</param>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, string> ToDict(System.Data.DataRow dataRow)
		{
			System.Collections.Generic.Dictionary<string, string> dict = null;
			if (dataRow != null)
			{
				dict = new System.Collections.Generic.Dictionary<string, string>();
				foreach (System.Data.DataColumn dataColumn in dataRow.Table.Columns)
				{
					dict.Add(dataColumn.ColumnName, dataRow[dataColumn].ToString());
				}
			}
			return dict;
		}
		
		/// <summary>
		/// NVC转Dict
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, string> ToDict(System.Collections.Specialized.NameValueCollection nvc)
		{
			System.Collections.Generic.Dictionary<string, string> dict = null;
			if(nvc != null)
			{
				dict = new System.Collections.Generic.Dictionary<string, string>();
				foreach(string key in nvc.AllKeys) 
				{
					if(!string.IsNullOrEmpty(key)){ dict.Add(key, nvc.GetValues(key)[0]);  }
				}
			}
			return dict;
		}

		/// <summary>
		/// JSON/XML转为DICT对象
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, dynamic> ToDict(object obj, string root = null)
		{
			string str = obj + "";
			System.Collections.Generic.Dictionary<string, dynamic> dict = null;
			if(string.IsNullOrEmpty(root))
			{
				try
				{
					System.Web.Script.Serialization.JavaScriptSerializer javascriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
					dict = javascriptSerializer.Deserialize<dynamic>(str);
				}
				catch{}
			}
			else
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				try
				{
					dict = new System.Collections.Generic.Dictionary<string, dynamic>();
					xmlDocument.LoadXml(str.EndsWith("</" + root + ">") ? str : "<" + root + ">" + str + "</" + root + ">");
					foreach(System.Xml.XmlNode xmlNode in xmlDocument[root]) { dict.Add(xmlNode.Name, xmlNode.InnerText); }
				}
				catch{dict = null;}
			}
			return dict;
		}

		/// <summary>
		/// Dict转请求表单
		/// </summary>
		/// <param name="dict">Dict字典集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		/// <param name="action">表单指向</param>
		/// <param name="submit">是否自动提交</param>
		public static string ToForm(System.Collections.Generic.Dictionary<string, string> dict, bool asciiSort = false, string action = null, bool submit = true, string target = null)
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal); }
			
			string formHtml = null;
			foreach(string key in keys) { formHtml += key + "=<input type=\"text\" name=\"" + key + "\" value=\"" + HtmlEncode(dict[key]) + "\"><br />\r\n"; }
			if(action != null)
			{
				if(target != null && target.Trim() == ""){target = "_blank";}
				formHtml = "<form action=\"" + action +  "\" method=\"post\" id=\"pay_post_form\" style=\"display:" + (submit ? "none" : "") + ";\"" + (target == null ? "" : " target=\"" + target + "\"") + ">\r\n" + formHtml;
				formHtml += "<input type=\"submit\" value=\"正在提交\"><br /></form>\r\n";
				if(submit) { formHtml += "\r\n<script>document.getElementById('pay_post_form').submit();</script>"; }
			}
			return formHtml;
		}
		
		/// <summary>
		/// NVC转请求表单
		/// </summary>
		/// <param name="nvc">Nvc集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		/// <param name="action">表单指向</param>
		/// <param name="submit">是否自动提交</param>
		public static string ToForm(System.Collections.Specialized.NameValueCollection nvc, bool asciiSort = false, string action = null, bool submit = true, string target = null)
		{
			string[] keys = nvc.AllKeys;
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal); }
			
			string formHtml = null;
			foreach(string key in keys) 
			{
				foreach (string val in nvc.GetValues(key))
				{
					formHtml += key + "=<input type=\"text\" name=\"" + key + "\" value=\"" + HtmlEncode(val) + "\"><br />\r\n"; 
				}
			}
			if(action != null)
			{
				if(target != null && target.Trim() == ""){target = "_blank";}
				formHtml = "<form action=\"" + action +  "\" method=\"post\" id=\"pay_post_form\" style=\"display:" + (submit ? "none" : "") + ";\"" + (target == null ? "" : " target=\"" + target + "\"") + ">\r\n" + formHtml;
				formHtml += "<input type=\"submit\" value=\"正在提交\"><br /></form>\r\n";
				if(submit) { formHtml += "\r\n<script>document.getElementById('pay_post_form').submit();</script>"; }
			}
			return formHtml;
		}
		/// <summary>
		/// 数据行转JSON
		/// </summary>
		/// <param name="dataRow">数据行</param>
		/// <returns></returns>
		public static string ToJSON(System.Data.DataRow dataRow)
		{
			string json = null;
			if (dataRow != null)
			{
				foreach (System.Data.DataColumn dataColumn in dataRow.Table.Columns)
				{
					json += ",\"" + dataColumn.ColumnName + "\":\"" + dataRow[dataColumn] + "\"";
				}
				json = "{" + json.Substring(1) + "}";
			}
			return json;
		}
		
		/// <summary>
		/// Dict转Json字符串
		/// </summary>
		/// <param name="dict">Dict字典集合</param>
		/// <param name="asciiSort">是否排序</param>
		/// <returns></returns>
		public static string ToJSON(System.Collections.Generic.Dictionary<string, string> dict, bool asciiSort = false)
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal); }
			
			string json = null;
			foreach(string key in keys) { json += ",\"" + key + "\":\"" + dict[key] + "\""; }
			
			if(json != null && json.Length > 0){json = json.Substring(1);}
			json = "{" + json + "}";
			return json;
		}
		
		/// <summary>
		/// Nvc转Json字符串
		/// </summary>
		/// <param name="nvc">请求字符串</param>
		/// <param name="asciiSort">是否排序</param>
		/// <returns></returns>
		public static string ToJSON(System.Collections.Specialized.NameValueCollection nvc, bool asciiSort = false)
		{
			string[] keys = nvc.AllKeys;
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal); }
			
			string json = null;
			foreach(string key in keys) { json += ",\"" + key + "\":\"" + nvc[key] + "\""; }
			
			if(json != null && json.Length > 0){json = json.Substring(1);}
			json = "{" + json + "}";
			return json;
		}
		
		/// <summary>
		/// JSON字符串转为dynamic对象
		/// </summary>
		/// <param name="strJSON">JSON字符串</param>
		/// <returns></returns>
		public static dynamic ParseJSON(object strJSON)
		{
			dynamic json = null;
			try
			{
				System.Web.Script.Serialization.JavaScriptSerializer javascriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
				json = javascriptSerializer.Deserialize<dynamic>(strJSON + "");
			}
			catch { }
			return json;
		}
		
		
		/// <summary>
		/// 格式化货币格式
		/// </summary>
		/// <param name="val">原始金额</param>
		/// <param name="type">最终货币格式</param>
		/// <returns></returns>
		public static string ToMoney(object val, string type = "fen")
		{
			double result = 0;
            if (!double.TryParse(val + "", out result)){};
			return type == "fen" ? (result*100).ToString() : result.ToString("F2");
		}
		
		/// <summary>
		/// 分转元，/100
		/// </summary>
		/// <param name="val">原始金额</param>
		public static string ToYuan(object val)
		{
			double result = 0;
            if (!double.TryParse(val + "", out result)){};
			return (result/100).ToString("F2");
		}
		
		/// <summary>
		/// 元转分，*100
		/// </summary>
		/// <param name="val">原始金额</param>
		public static string ToFen(object val)
		{
			double result = 0;
            if (!double.TryParse(val + "", out result)){};
			return (result*100).ToString("F2");
		}
		
		/// <summary>
		/// DataRow转NVC
		/// </summary>
		/// <param name="dataRow">数据行</param>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection ToNvc(System.Data.DataRow dataRow)
		{
			System.Collections.Specialized.NameValueCollection nvc = null;
			if (dataRow != null)
			{
				nvc = new System.Collections.Specialized.NameValueCollection();
				foreach (System.Data.DataColumn dataColumn in dataRow.Table.Columns)
				{
					nvc.Add(dataColumn.ColumnName, dataRow[dataColumn].ToString());
				}
			}
			return nvc;
		}
		
		/// <summary>
		/// Dict转NVC
		/// </summary>
		/// <param name="dict">Dict字典集合</param>
		public static System.Collections.Specialized.NameValueCollection ToNvc(System.Collections.Generic.Dictionary<string, string> dict)
		{
			System.Collections.Specialized.NameValueCollection nvc = null;
			if(dict != null)
			{
				nvc = new System.Collections.Specialized.NameValueCollection();
				foreach(string key in dict.Keys) { nvc.Add(key, dict[key]); }
			}
			return nvc;
		}
		
		/// <summary>
		/// JSON/XML转为NVC对象
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection ToNvc(object obj, string root = "xml")
		{
			string str = obj + "";
			System.Collections.Specialized.NameValueCollection nvc = null;
			if(str.StartsWith("{") && str.EndsWith("}"))
			{
				try
				{
					System.Web.Script.Serialization.JavaScriptSerializer javascriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
					System.Collections.Generic.Dictionary<string, dynamic> dict = javascriptSerializer.Deserialize<dynamic>(str);
					nvc = new System.Collections.Specialized.NameValueCollection();
					foreach(string key in dict.Keys){ nvc.Add(key, dict[key].ToString()); }
				}
				catch{}
			}
			else if(str.StartsWith("<") && str.EndsWith(">"))
			{
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				try
				{
					xmlDocument.LoadXml(str.EndsWith("</" + root + ">") ? str : "<" + root + ">" + str + "</" + root + ">");
					nvc = new System.Collections.Specialized.NameValueCollection();
					foreach(System.Xml.XmlNode xmlNode in xmlDocument[root]) { nvc.Add(xmlNode.Name, xmlNode.InnerXml); }
				}
				catch{}
			}
			else
			{
				nvc = System.Web.HttpUtility.ParseQueryString(str);
			}
			return nvc;
		}
		
		/// <summary>
		/// Dict转请求字符串
		/// </summary>
		/// <param name="dict">Dict字典集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		public static string ToQueryString(System.Collections.Generic.Dictionary<string, string> dict, bool asciiSort = false)
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal); }
			
			string requestString = null;
			foreach(string key in keys) { requestString += "&" + key + "=" + System.Web.HttpUtility.UrlEncode(dict[key]) + ""; }
			if(requestString != null && requestString.Length > 0){requestString = requestString.Substring(1);}
			return requestString;
		}
		
		/// <summary>
		/// Nvc转签名字符串
		/// </summary>
		/// <param name="nvc">NVC集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		public static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc, bool asciiSort = false)
		{
			string[] keyArray = nvc.AllKeys;
			if(asciiSort) { System.Array.Sort(keyArray,string.CompareOrdinal); }
			string requestString = null;
			foreach(string key in keyArray) 
			{
				string[] values = nvc.GetValues(key);
				if(values != null){
					foreach (string val in values) { requestString += "&" + key + "=" + System.Web.HttpUtility.UrlEncode(val) + ""; }
				}
			}
			if(requestString != null && requestString.Length > 0){requestString = requestString.Substring(1);}
			return requestString;
		}
		
		/// <summary>
		/// Dict转签名字符串
		/// </summary>
		/// <param name="dict">Dictionary字典集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		/// <param name="inKeys">指定字段</param>
		/// <param name="outKeys">例外字段</param>
		public static string ToSign(System.Collections.Generic.Dictionary<string, string> dict, bool asciiSort, bool emptyValue = true, string inKeys = null, string outKeys = null)
		{
			string[] keyArray = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(inKeys != null && inKeys.Length > 0) { keyArray = inKeys.Split(",".ToCharArray()); }
			if(asciiSort) { System.Array.Sort(keyArray,string.CompareOrdinal); }
			inKeys = "," + System.String.Join(",", keyArray).ToLower() + ",";
			outKeys = ("," + outKeys + ",").ToLower();
			string signString = null;
			foreach(string key in keyArray)
			{
				if(inKeys.Contains("," + key.ToLower() + ",") && !outKeys.Contains("," + key.ToLower() + ","))
				{
					if(emptyValue || !string.IsNullOrEmpty(dict[key])){signString += "&" + key + "=" + dict[key] + "";}
				}
			}
			
			if(signString != null && signString.Length > 0){signString = signString.Substring(1);}
			return signString;
		}

		/// <summary>
		/// Nvc转签名字符串
		/// </summary>
		/// <param name="nvc">NVC集合</param>
		/// <param name="asciiSort">是否ASCII排序</param>
		/// <param name="inKeys">指定字段</param>
		/// <param name="outKeys">例外字段</param>
		public static string ToSign(System.Collections.Specialized.NameValueCollection nvc, bool asciiSort, bool emptyValue = true, string inKeys = null, string outKeys = null)
		{
			string[] keyArray = nvc.AllKeys;
			if(inKeys != null && inKeys.Length > 0) { keyArray = inKeys.Split(",".ToCharArray()); }
			if(asciiSort) { System.Array.Sort(keyArray,string.CompareOrdinal); }
			inKeys = "," + System.String.Join(",", keyArray).ToLower() + ",";
			outKeys = ("," + outKeys + ",").ToLower();
			string signString = null;
			foreach(string key in keyArray)
			{
				if(key!= null && inKeys.Contains("," + key.ToLower() + ",") && !outKeys.Contains("," + key.ToLower() + ","))
				{
					if(emptyValue || !string.IsNullOrEmpty(nvc[key])){signString += "&" + key + "=" + nvc[key] + "";}	
				}
			}
			
			if(signString != null && signString.Length > 0){signString = signString.Substring(1);}
			return signString;
		}
		
		/// <summary>
		/// Dict转XML字符串
		/// </summary>
		/// <param name="dict">Dict字典集合</param>
		/// <param name="asciiSort">是否排序</param>
		/// <returns></returns>
		public static string ToXml(System.Collections.Generic.Dictionary<string, string> dict, bool asciiSort = false, string root = "xml")
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal);}
			
			string xml = null;
			foreach(string key in keys) { xml += "<" + key + "><![CDATA[" + dict[key] + "]]></" + key + ">"; }
			xml = "<" + root + ">" + xml + "</" + root + ">";
			return xml;
		}		
		/// <summary>
		/// Nvc转XML字符串
		/// </summary>
		/// <param name="nvc">NVC集合</param>
		/// <param name="asciiSort">是否排序</param>
		/// <returns></returns>
		public static string ToXml(System.Collections.Specialized.NameValueCollection nvc, bool asciiSort = false, string root = "xml")
		{
			string[] keys = nvc.AllKeys;
			if(asciiSort) { System.Array.Sort(keys,string.CompareOrdinal);}
			
			string xml = null;
			foreach(string key in keys) 
			{ xml += "<" + key + "><![CDATA[" + nvc[key] + "]]></" + key + ">"; }
			xml = "<" + root + ">" + xml + "</" + root + ">";
			return xml;
		}
		
		

		/// <summary>
		/// 3DES加密，返回BASE64编码密文字符串
		/// </summary>
		/// <param name="key">密钥，至少24个字符串，多了截取前24位</param>
		/// <param name="encryptString">待加密明文字符串</param>
		/// <param name="key">模式mode，ECB或者CBC</param>
		/// <param name="key">扩充iv，ECB模式下无需</param>
		/// <param name="key">编码格式，默认UTF-8</param>
		/// <returns></returns>
		public static string TripleDesEncode(string key, string encryptString, string mode = "ECB", string iv = null, string charset = "UTF-8")
		{
			System.Security.Cryptography.TripleDESCryptoServiceProvider provider = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
			provider.Key = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(key.Substring(0,24));
			provider.Mode = mode.ToUpper() == "ECB" ? System.Security.Cryptography.CipherMode.ECB : System.Security.Cryptography.CipherMode.CBC;
			//provider.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
			if(mode.ToUpper() == "CBC" && iv != null){provider.IV = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(iv);}
			
			System.Security.Cryptography.ICryptoTransform encryptor = provider.CreateEncryptor();
			byte[] encryptBuffer = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(encryptString);
			string encryptResult = System.Convert.ToBase64String(encryptor.TransformFinalBlock(encryptBuffer, 0, encryptBuffer.Length));
			return encryptResult;
		}
		
		/// <summary>
		/// 3DES解密，返回明文字符串
		/// </summary>
		/// <param name="key">密钥，至少24个字符串，多了截取前24位</param>
		/// <param name="encryptString">待解密BASE64编码密文字符串</param>
		/// <param name="key">模式mode，ECB或者CBC</param>
		/// <param name="key">扩充iv，ECB模式下无需</param>
		/// <param name="key">编码格式，默认UTF-8</param>
		/// <returns></returns>
		public static string TripleDesDecode(string key, string decryptString, string mode = "ECB", string iv = null, string charset = "UTF-8")
		{
			System.Security.Cryptography.TripleDESCryptoServiceProvider provider = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
			provider.Key = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(key.Substring(0,24));
			provider.Mode = mode.ToUpper() == "ECB" ? System.Security.Cryptography.CipherMode.ECB : System.Security.Cryptography.CipherMode.CBC;
			//provider.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
			if(mode.ToUpper() == "CBC" && iv != null){provider.IV = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(iv);}

			System.Security.Cryptography.ICryptoTransform decryptor = provider.CreateDecryptor();
			byte[] decryptBuffer = System.Convert.FromBase64String(decryptString);
			string decryptResult = System.Text.Encoding.GetEncoding("UTF-8").GetString(decryptor.TransformFinalBlock(decryptBuffer, 0, decryptBuffer.Length));
			return decryptResult;
		}
		
		/// <summary>
		/// 缩略图
		/// </summary>
		/// <param name="filePath">源图路径</param>
		/// <param name="width">最大宽度</param>
		/// <param name="height">最大高度</param>
		/// <param name="fileName">输出文件名，仅限jpg,png,gif，否则返回null</param>
		/// <returns></returns>
		public string Thumb(string filePath, double width, double height, string fileName = null)
		{
			filePath = filePath + "";
			if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
			System.Drawing.Image image = System.Drawing.Image.FromFile(filePath, true);

			double sourceWidth = image.Width;
			double targetWidth = sourceWidth;
			width = System.Math.Abs(width);

			double sourceHeight = image.Height;
			double targetHeight = sourceHeight;
			height = System.Math.Abs(height);

			if (targetWidth > width && width > 0)//太宽
			{
				targetHeight = targetHeight * (width / targetWidth);
				targetWidth = width;
			}

			if (targetHeight > height && height > 0)//太高
			{
				targetWidth = targetWidth * (height / targetHeight);
				targetHeight = height;
			}
			if (width == 0) { width = targetWidth; }
			if (height == 0) { height = targetHeight; }

			//System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)width, (int)height);
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)targetWidth, (int)targetHeight);
			System.Drawing.Graphics fromImage = System.Drawing.Graphics.FromImage(bitmap);
			fromImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			fromImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
			fromImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			fromImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
			fromImage.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			fromImage.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
			System.Drawing.Rectangle source = new System.Drawing.Rectangle(0, 0, (int)sourceWidth, (int)sourceHeight);
			//System.Drawing.Rectangle target = new System.Drawing.Rectangle((int)((width - targetWidth) / 2), (int)((height - targetHeight) / 2), (int)targetWidth, (int)targetHeight);
			System.Drawing.Rectangle target = new System.Drawing.Rectangle(0, 0, (int)targetWidth, (int)targetHeight);
			fromImage.DrawImage(image, target, source, System.Drawing.GraphicsUnit.Pixel);
			System.Drawing.Image newImage = (System.Drawing.Image)bitmap.Clone();
			bitmap.Dispose();
			image.Dispose();


			if (fileName != null) { filePath = fileName; }
			if (filePath.IndexOf(":\\")<0) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
			fileName = filePath.Replace(System.Web.HttpContext.Current.Server.MapPath("/"), "/").Replace("\\", "/");

			string path = "";
			foreach (string v in fileName.Substring(0, fileName.LastIndexOf("/")).Split('/'))
			{
				path += v + "/";
				if (path.Length > 1 && v.Length > 0)
				{
					string realPath = System.Web.HttpContext.Current.Server.MapPath(path);
					if (!System.IO.Directory.Exists(realPath)) { System.IO.Directory.CreateDirectory(realPath); }
				}
			}
			fileName += "";
			switch (fileName.ToLower().Substring(fileName.LastIndexOf('.')))
			{
				case ".jpg":
					newImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
					break;
				case ".png":
					newImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
					break;
				case ".gif":
					newImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif);
					break;
				default:
					fileName = null;
					break;
			}
			return fileName;
		}

		/// <summary>
		/// 时间戳
		/// </summary>
		/// <returns></returns>
		public static double TimeStamp()
		{
			return System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);
		}

		/// <summary>
		/// 时间转时间戳
		/// </summary>
		/// <param name="dateTime">时间</param>
		/// <returns></returns>
		public static double TimeStamp(System.DateTime dateTime)
		{
			return System.Math.Floor((dateTime - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);
		}

		/// <summary>
		/// 字符串转时间戳
		/// </summary>
		/// <param name="dateTime">时间</param>
		/// <returns></returns>
		public static double TimeStamp(string dateTime)
		{
			return System.Math.Floor((System.Convert.ToDateTime(dateTime) - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);
		}

		/// <summary>
		/// 时间戳转时间
		/// </summary>
		/// <param name="seconds">秒数</param>
		/// <returns></returns>
		public static System.DateTime TimeStamp(double seconds)
		{
			return System.Convert.ToDateTime("1970-01-01 00:00:00").AddSeconds(seconds);
		}
		
		/// <summary>
		/// 时间戳转时间
		/// </summary>
		/// <param name="seconds">秒数</param>
		/// <returns></returns>
		public static System.DateTime TimeStamp(int seconds)
		{
			return System.Convert.ToDateTime("1970-01-01 00:00:00").AddSeconds(seconds);
		}
		
		/// <summary>
		/// 转数字
		/// </summary>
		/// <param name="obj">要处理的对象</param>
		/// <returns></returns>
		public static decimal ToDecimal(object obj)
		{
			decimal result = ToDecimal(obj, 0);
			return result;
		}
		
		/// <summary>
		/// 转数字
		/// </summary>
		/// <param name="obj">要处理的对象</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static decimal ToDecimal(object obj, decimal defaultValue)
		{
			decimal result = 0;
			if (!decimal.TryParse(obj + "", out result)) { result = defaultValue; }
			return result;
		}
		
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj">要处理的对象</param>
        /// <returns></returns>
        public static double ToDouble(object obj)
        {
            double result = ToDouble(obj, 0);
            return result;
        }
		
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj">要处理的对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(object obj, double defaultValue)
        {
            double result = 0;
            if (!double.TryParse(obj + "", out result)) { result = defaultValue; }
            return result;
        }
		
		
		/// <summary>
		/// 转整数
		/// </summary>
		/// <param name="obj">要转换的字符串或者数字等</param>
		/// <returns></returns>
		public static int ToInt(object obj)
		{
			double result = 0;
            double.TryParse(obj + "", out result);
			return System.Convert.ToInt32(System.Math.Floor(result));
		}
		
		/// <summary>
		/// 转整数
		/// </summary>
		/// <param name="obj">要转换的字符串或者数字等</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static int ToInt(object obj, double defaultValue = 0)
		{
			double result = 0;
			if (!double.TryParse(obj + "", out result)) { result = defaultValue; }
			return System.Convert.ToInt32(System.Math.Floor(result));
		}
		
		
		/// <summary>
		/// 读TXT文件
		/// </summary>
		/// <param name="filePath">文件名</param>
		/// <returns></returns>
		public static string TxtFile(string filePath)
		{
			filePath = filePath.IndexOf(":\\")>0 ? filePath : System.Web.HttpContext.Current.Server.MapPath(filePath); 
			if(!System.IO.File.Exists(filePath)){ return null; }
			
			string content = System.IO.File.ReadAllText(filePath);
			return content + "";
		}
		/// <summary>
		/// 写TXT文件
		/// </summary>
		/// <param name="filePath">文件名</param>
		/// <param name="content">文本内容</param>
		/// <returns></returns>
		public static string TxtFile(string filePath, object content)
		{
			filePath = filePath.IndexOf(":\\")>0 ? filePath : System.Web.HttpContext.Current.Server.MapPath(filePath); 
			System.IO.File.AppendAllText(filePath, content + "", System.Text.Encoding.UTF8);
			return content + "";
		}
		/// <summary>
		/// 写TXT文件
		/// </summary>
		/// <param name="filePath">文件名</param>
		/// <param name="content">文本内容</param>
		/// <param name="replace">是否替换</param>
		/// <returns></returns>
		public static string TxtFile(string filePath, object content, bool replace)
		{
			filePath = filePath.IndexOf(":\\")>0 ? filePath : System.Web.HttpContext.Current.Server.MapPath(filePath); 
			if(replace)
			{
				System.IO.File.WriteAllText(filePath, content + "", System.Text.Encoding.UTF8);
			}
			else
			{
				System.IO.File.AppendAllText(filePath, content + "", System.Text.Encoding.UTF8);
			}
			return content + "";
		}

		/// <summary>
		/// 输出
		/// </summary>
		/// <param name="obj">要输出的对象</param>
		public static void Write(object obj)
		{
			System.Web.HttpContext.Current.Response.Write(obj);
		}
		
		/// <summary>
		/// 输出
		/// </summary>
		/// <param name="obj">要输出的对象</param>
		public static void Writeln(object obj)
		{
			System.Web.HttpContext.Current.Response.Write(obj + "\r\n");
		}
		
		/// <summary>
		/// 输出
		/// </summary>
		/// <param name="obj">要输出的对象</param>
		public static void Writebr(object obj)
		{
			System.Web.HttpContext.Current.Response.Write(obj + "<br />\r\n");
		}

		/// <summary>
		/// 输出js
		/// </summary>
		/// <param name="obj">js代码</param>
		public static void WriteScript(object js)
		{
			js = "<script language=\"javascript\">(function(){" + js + "})();</script>";
			System.Web.HttpContext.Current.Response.Write(js);
		}

		/// <summary>
		/// Unicode解码
		/// </summary>
		/// <param name="obj">待解码字符</param>
		/// <returns></returns>
		public static string Unescape(object obj)
		{
			return System.Text.RegularExpressions.Regex.Unescape(obj + "");
		}
		
		/// <summary>
        /// Unicode编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnicodeEncrypt(string str)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    stringBuilder.Append("\\u");
                    stringBuilder.Append(((int)str[i]).ToString("x"));
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnicodeDecrypt(string str)
        {
            //最直接的方法Regex.Unescape(str);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return regex.Replace(str, delegate(System.Text.RegularExpressions.Match match) { return ((char)System.Convert.ToInt32(match.Groups[1].Value, 16)).ToString(); });
        }
		
		/// <summary>
		/// 上传：成功会返回“/”开头的字符串，否则就是错误描述
		/// </summary>
		/// <param name="field">字段名</param>
		/// <param name="dir">保存根目录：/Upload/{yyyy}/{MM}/{dd}</param>
		/// <param name="k">大小，单位K</param>
		/// <param name="ext">后缀：media,image,file,rar</param>
		/// <param name="count">上传个数，最小1个</param>
		/// <returns></returns>
		public static string Upload(string field = null, string dir = null, int k = 0, string ext = null, int count = 0)
		{
			if (System.Web.HttpContext.Current.Request.HttpMethod != "POST") { return "请POST方式请求"; }

			if (field == null || field.Length == 0) { field = "file"; }
			if (dir == null || dir.Length == 0) { dir = "/Upload/{yyyy}{MM}/{dd}"; }
			if (k < 1) { k = 10240; }
			if (ext == null || ext.Length == 0) { ext = "*"; }
			ext = ext.Replace("*", "media,image,file");

			if (count < 1) { count = 1; }


			System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
			if (files.Count < 1) { return "请选择上传文件"; }

			if (!dir.StartsWith("/")) { dir = "/" + dir; }

			int max = k * 1024;

			ext = ext.ToLower();
			ext = ext.Replace(".", "").Trim(',');
			ext = ext.Replace("media", "audio,video,flash");
			ext = ext.Replace("audio", "mp3");
			ext = ext.Replace("video", "mp4");
			ext = ext.Replace("flash", "swf,flv");
			ext = ext.Replace("image", "gif,jpg,jpeg,png,bmp");
			ext = ext.Replace("file", "doc,docx,xls,xlsx,ppt,pptx,zip,rar,txt");
			ext = "," + ext + ",";

			string result = null;
			int fileCount = 0;
			System.Collections.ArrayList list = new System.Collections.ArrayList();

			for (int i = 0; i < files.Count; i++)
			{
				if (files.AllKeys[i].Length == 0 || files.AllKeys[i].ToLower() != field.ToLower()) { continue; }
				fileCount++;
				if (fileCount > count)
				{
					result = "上传文件数" + fileCount + "高于限制：" + count + "个";
					break;
				}
				System.Web.HttpPostedFile file = files[i];
				if (file.InputStream.Length == 0)
				{
					continue;
				}

				if (file.InputStream.Length > max && max > 0)
				{
					result = "文件大小" + System.Convert.ToInt32(file.InputStream.Length/1024) + "K高于限制" + k + "K";
					break;
				}

				string fileExt = System.IO.Path.GetExtension(file.FileName).ToLower() + "";
				if (fileExt.Length == 0 || ext.Contains("," + fileExt.Substring(1) + ",") == false)
				{
					result = "上传文件类型" + fileExt + "不属于" + ext.Trim(',') + "";
					break;
				}

				dir = dir.Replace("{yyyy}", System.DateTime.Now.ToString("yyyy"));
				dir = dir.Replace("{MM}", System.DateTime.Now.ToString("MM"));
				dir = dir.Replace("{dd}", System.DateTime.Now.ToString("dd"));
				dir = dir.Replace("{HH}", System.DateTime.Now.ToString("HH"));
				dir = dir.Replace("{mm}", System.DateTime.Now.ToString("mm"));
				dir = dir.TrimEnd('/');
				string path = "";
				foreach (string v in dir.Split('/'))
				{
					path += v + "/";
					if (path.Length > 1 && v.Length > 0)
					{
						string realPath = System.Web.HttpContext.Current.Server.MapPath(path);
						if (!System.IO.Directory.Exists(realPath)) { System.IO.Directory.CreateDirectory(realPath); }
					}
				}
				string url = dir + "/" + System.DateTime.Now.ToString("yyyyMMddHHmmss_fff_") + i + fileExt;
				list.Add(url);
				file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(url));
			}

			if (result != null && result.Length > 0)
			{
				foreach (string file in list)
				{
					System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(file));
				}
				list.Clear();
			}
			else
			{
				foreach (string file in list)
				{
					result += "," + file;
				}
				if (result != null && result.Length > 0) { result = result.TrimStart(','); }
			}
			return result;
		}
		
		/// <summary>
		/// 上传：成功会返回不带Message字段的NVC，否则带Message
		/// </summary>
		/// <param name="dir">保存根目录：/Upload/{yyyy}/{MM}/{dd}</param>
		/// <param name="k">大小，单位K</param>
		/// <param name="ext">后缀：media,image,file,rar</param>
		/// <returns></returns>
		public static System.Collections.Specialized.NameValueCollection UploadFiles(string dir = null, int k = 0, string ext = null, bool rename = true)
		{
			System.Collections.Specialized.NameValueCollection ret =  new System.Collections.Specialized.NameValueCollection();
			if (System.Web.HttpContext.Current.Request.HttpMethod != "POST") 
			{
				ret.Add("Message", "请POST方式请求");
				return ret; 
			}

			if (dir == null || dir.Length == 0) { dir = "/Upload/{yyyy}{MM}/{dd}"; }
			if (k < 1) { k = 10240; }
			if (ext == null || ext.Length == 0) { ext = "*"; }
			ext = ext.Replace("*", "media,image,file");


			System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
			if (files.Count < 1) 
			{
				ret.Add("Message", "请选择上传文件，表单添加：enctype=\"multipart/form-data\"");
				return ret;
			}

			if (!dir.StartsWith("/")) { dir = "/" + dir; }

			int max = k * 1024;

			ext = ext.ToLower();
			ext = ext.Replace(".", "").Trim(',');
			ext = ext.Replace("media", "audio,video,flash");
			ext = ext.Replace("audio", "mp3");
			ext = ext.Replace("video", "mp4");
			ext = ext.Replace("flash", "swf,flv");
			ext = ext.Replace("image", "gif,jpg,png,bmp");
			ext = ext.Replace("file", "doc,docx,xls,xlsx,ppt,pptx,zip,rar,txt");
			ext = "," + ext + ",";

			string message = null;
			System.Collections.ArrayList list = new System.Collections.ArrayList();

			for (int i = 0; i < files.Count; i++)
			{
				string field = files.AllKeys[i];
				if (field.Length == 0) { continue; }
				System.Web.HttpPostedFile file = files[i];
				if (file.InputStream.Length == 0)
				{
					continue;
				}

				if (file.InputStream.Length > max && max > 0)
				{
					message = "文件" + file.FileName + "超过最大限制" + (k/1024) + "M";
					break;
				}

				string fileExt = System.IO.Path.GetExtension(file.FileName).ToLower() + "";
				if (fileExt.Length == 0 || ext.Contains("," + fileExt.Substring(1) + ",") == false)
				{
					message = "仅支持" + ext.Trim(',') + "类型文件:" + file.FileName;
					break;
				}

				dir = dir.Replace("{yyyy}", System.DateTime.Now.ToString("yyyy"));
				dir = dir.Replace("{MM}", System.DateTime.Now.ToString("MM"));
				dir = dir.Replace("{dd}", System.DateTime.Now.ToString("dd"));
				dir = dir.Replace("{HH}", System.DateTime.Now.ToString("HH"));
				dir = dir.Replace("{mm}", System.DateTime.Now.ToString("mm"));
				dir = dir.TrimEnd('/');
				string path = "";
				foreach (string v in dir.Split('/'))
				{
					path += v + "/";
					if (path.Length > 1 && v.Length > 0)
					{
						string realPath = System.Web.HttpContext.Current.Server.MapPath(path);
						if (!System.IO.Directory.Exists(realPath)) { System.IO.Directory.CreateDirectory(realPath); }
					}
				}
				string filePath = dir + "/" + (rename ? System.DateTime.Now.ToString("yyyyMMddHHmmss_fff_") + i : field) + fileExt;
				list.Add(filePath);
				ret.Add(field, filePath);
				file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(filePath));
			}

			if (message != null && message.Length > 0)
			{
				foreach (string file in list) { System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(file)); }
				list.Clear();
				ret.Clear();
				ret.Add("Message", message);
			}
			return ret;
		}

		/// <summary>
		/// 当前URL
		/// </summary>
		/// <returns></returns>
		public static string Url()
		{
			string url = System.Web.HttpContext.Current.Request.Url.ToString();
			return url;
		}
		/// <summary>
		/// 当前URL，带协议和域名
		/// </summary>
		/// <param name="path">可取空，空字符串，斜杠，问号，路径</param>
		public static string Url(string path)
		{
			string url = System.Web.HttpContext.Current.Request.Url.ToString();
			if (path == "/")
			{
				url = url.Substring(0, url.IndexOf("/", 10));
			}
			else if (path == "./" )
			{
				string filePath = System.Web.HttpContext.Current.Request.FilePath;
				url = url.Substring(0, url.IndexOf("/", 10)) + filePath.Substring(0, filePath.LastIndexOf("/") + 1);
			}
			else if (path == "?")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + System.Web.HttpContext.Current.Request.FilePath;
			}
			else if (path == "")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + System.Web.HttpContext.Current.Request.RawUrl;
			}
			else if (path.StartsWith("/"))
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + path;
			}
			else
			{
				string filePath = System.Web.HttpContext.Current.Request.FilePath;
				url = url.Substring(0, url.IndexOf("/", 10)) + filePath.Substring(0, filePath.LastIndexOf("/") + 1) + path;
			}
			return url;
		}

		/// <summary>
		/// Url解码
		/// </summary>
		/// <param name="url">Url地址</param>
		/// <returns></returns>
		public static string UrlDecode(object url)
		{
			return System.Web.HttpUtility.UrlDecode(url + "", System.Text.Encoding.GetEncoding("UTF-8"));
		}
		/// <summary>
		/// Url解码
		/// </summary>
		/// <param name="url">Url地址</param>
		/// <returns></returns>
		public static string UrlDecode(object url, string charset)
		{
			return System.Web.HttpUtility.UrlDecode(url + "", System.Text.Encoding.GetEncoding(charset));
		}

		/// <summary>
		/// Url编码
		/// </summary>
		/// <param name="url">地址</param>
		/// <returns></returns>
		public static string UrlEncode(object url)
		{
			url = System.Web.HttpUtility.UrlEncode(url + "", System.Text.Encoding.GetEncoding("UTF-8"));
			url = url.ToString().Replace("+", "%20");
			return url.ToString();
		}
		
		public static string UrlEncode(string data, string charset)
		{
			if(charset == null || charset.Length == 0){charset = "UTF-8";}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (char c in data)
			{
				string charString = c.ToString();
				string finalString = System.Web.HttpUtility.UrlEncode(charString, System.Text.Encoding.GetEncoding(charset));
				stringBuilder.Append(charString == finalString ? charString : finalString.ToUpper());
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 获取验证码
		/// </summary>
		/// <param name="sessionName">Session名</param>
		/// <returns></returns>
		public static string VerifyCode(string sessionName)
		{
			string code = System.Web.HttpContext.Current.Session[sessionName] + "";
			return code;
		}

		/// <summary>
		/// 设置验证码
		/// </summary>
		/// <param name="sessionName">Session名</param>
		/// <param name="length">验证码长度</param>
		/// <returns></returns>
		public static string VerifyCode(string sessionName, int length)
		{
			if(length < 0)
			{
				System.Web.HttpContext.Current.Session[sessionName] = "";
				return null;
			}
			string code = null;
			string[] chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z".Split(',');
			for (int i = 0; i < length; i++) { code += chars[Random(0, chars.Length - 1)]; }
			System.Web.HttpContext.Current.Session[sessionName] = code;
			return code;
		}

		/// <summary>
		/// 校验验证码
		/// </summary>
		/// <param name="sessionName">Session名</param>
		/// <param name="requestCode">输入的验证码</param>
		/// <returns></returns>
		public static bool VerifyCode(string sessionName, string requestCode)
		{
			string sessionCode = (System.Web.HttpContext.Current.Session[sessionName] + "").ToLower();
			requestCode = (requestCode + "").ToLower();
			return requestCode.Length > 0 && sessionCode == requestCode;
		}

		/// <summary>
		/// 验证码图片
		/// </summary>
		/// <param name="sessionName">Session名</param>
		/// <param name="length">验证码长度</param>
		public static string VerifyImage(string sessionName, int length, string fontColor="#FF0000,#0000FF", string backColor = "#FFFFFF")
		{
			System.Web.HttpContext.Current.Response.Buffer = true;
			System.Web.HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
			System.Web.HttpContext.Current.Response.Expires = 0;
			System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
			System.Web.HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");

			string code = VerifyCode(sessionName, length);
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(code.Length * 24 + 6, 30);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.ColorTranslator.FromHtml(backColor));//背景色
			System.Drawing.Font font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);
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
				graphics.DrawString(charArray[i].ToString(), font, brush, i * 24, 0);
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
		
		/// <summary>
		/// .NET框架
		/// </summary>
		/// <returns></returns>
		public static string VersionFramework()
		{
			string result = null;
			//主版本号.副版本号
			result = System.Environment.Version.Major + "." + System.Environment.Version.Minor;
			//第三版本号.第四版本号
			result += "." + System.Environment.Version.Build + "." + System.Environment.Version.Revision;
			return result;
		}

		/// <summary>
		/// .NET框架
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string VersionIIS(string type = null)
		{
			string result = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
			return result;
		}

		/// <summary>
		/// 操作系统
		/// </summary>
		/// <returns></returns>
		public static string VersionOS()
		{
			string result = System.Environment.OSVersion + "";
			return result;
		}
		
		/// <summary>
		/// 微信公众号支付js代码
		/// </summary>
		/// <returns></returns>
		public static string WeixinPayGzhJs(string json, string successUrl = "./", string failUrl = null)
		{
			if(failUrl == null){failUrl = successUrl;}
			string html = null;
			html += "<script type=\"text/javascript\">";
			html += "function weixinPayJSAPI(){WeixinJSBridge.invoke('getBrandWCPayRequest'," + json + ",function(res){if(res.err_msg=='get_brand_wcpay_request:ok'){window.alert('支付完成');window.location.href='" + successUrl + "';}else{window.alert('支付失败');window.location.href='" + failUrl + "';}});}";
			html += "if(typeof(WeixinJSBridge)=='undefined'){if(document.addEventListener){document.addEventListener('WeixinJSBridgeReady',weixinPayJSAPI,false);}else if(document.attachEvent){document.attachEvent('WeixinJSBridgeReady',weixinPayJSAPI);document.attachEvent('onWeixinJSBridgeReady',weixinPayJSAPI);}}else{weixinPayJSAPI();}";
			html += "</script>";
			return html;
		}
		
		/// <summary>
		/// 生成EXCEL文档
		/// </summary>
		/// <param name="fileName">文件名，可为空</param>
		/// <param name="contents">文档内容，一般为HTML里的Table代码</param>
		public static void ToFile(string fileName, object contents)
		{
			string contentType = "text/plain";
			if(string.IsNullOrEmpty(fileName)) { fileName = "文件名为空.txt"; }
			else if(fileName.ToLower().EndsWith(".pdf")) { contentType = "application/pdf"; }
			else if(fileName.ToLower().EndsWith(".doc")) { contentType = "application/vnd.ms-word"; }
			else if(fileName.ToLower().EndsWith(".docx")) { contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; }
			else if(fileName.ToLower().EndsWith(".xls")) { contentType = "application/vnd.ms-excel"; }
			else if(fileName.ToLower().EndsWith(".xlsx")) { contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
			else if(fileName.ToLower().EndsWith(".ppt")) { contentType = "application/vnd.ms-powerpoint"; }
			else if(fileName.ToLower().EndsWith(".pptx")) { contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; }
			else if(fileName.ToLower().EndsWith(".txt")) { contentType = "text/plain"; }
			else if(fileName.ToLower().EndsWith(".xml")) { contentType = "text/xml"; }
			else if(fileName.ToLower().EndsWith(".pdf")) { contentType = ""; }
			else if(fileName.ToLower().EndsWith(".pdf")) { contentType = ""; }
			else { fileName = "不支持的文件格式.txt"; }
			
			if(fileName.StartsWith(".")) { fileName = "*" + fileName; }
			fileName = fileName.Replace("*", System.DateTime.Now.ToString("yyyyMMddHHmmss"));	
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
			System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
			System.Web.HttpContext.Current.Response.ContentType = contentType;
			System.Web.HttpContext.Current.Response.BinaryWrite(new byte[] { 0xFF, 0xFE }); //防止中文乱码
			System.Web.HttpContext.Current.Response.Write(contents);
			System.Web.HttpContext.Current.Response.End();
		}

		/// <summary>
		/// 测试
		/// </summary>
		public static void Test()
		{
			
		}
				
		/// <summary>
		/// 定时器委托
		/// </summary>		
		public delegate int TimerEventHandler();
		
		/// <summary>
		/// 定时器
		/// </summary>
		/// <param name="seconds"></param>
		/// <param name="callback"></param>
		public static string Timer(int seconds, TimerEventHandler callback = null)
		{
			System.Version v = new System.Version();
			string filePath = System.Web.HttpContext.Current.Server.MapPath(System.DateTime.Now.ToString("HHmmss") + ".txt");
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = seconds * 1000;
			timer.Elapsed += new System.Timers.ElapsedEventHandler(delegate (object source, System.Timers.ElapsedEventArgs e)
			{
				new TimerEventHandler(callback)();
			});
			timer.AutoReset = true;//是否重复执行
			timer.Enabled = true;
			return null;
		}
		
		public static System.Tuple<int, string> TupleTest(string name)
		{
			int len = name.Length;
			string newName = name + "test";

			System.Tuple<int, string> tuple = new System.Tuple<int, string>(len, newName);
			return tuple;
		}
		//public static (string name, int age, uint height) ValueTupleTest(string name)
		//{
		//	return ("Bob", 28, 175);
		//}
		
		/// <summary>
		/// 备用
		/// </summary>
		public T ReturnType<T, T1>(T id, T1 age)
		{
			return id;
		}
    }
}
/// <summary>
/// System
/// </summary>
namespace System
{
	/// <summary>
	/// 返回类
	/// </summary>
	public class Return
    {
		/// <summary>
		/// 错误号
		/// </summary>
		public int Id = 0;
		
		/// <summary>
		/// 错误码
		/// </summary>
		public string Code = null;
		
		/// <summary>
		/// 错误信息
		/// </summary>
		public string Message = null;
		
		/// <summary>
		/// 数据
		/// </summary>
		public object _data = null;
			
		/// <summary>
		/// 跳转地址
		/// </summary>
		public string Url = null;
		
		/// <summary>
		/// 返回信息
		/// </summary>
		/// <returns></returns>
		public Return()
		{
		}
		
		/// <summary>
		/// 返回信息
		/// </summary>
		/// <param name="id">错误号</param>
		/// <returns></returns>
		public Return(int id)
		{
			Id = id;
		}
		
		/// <summary>
		/// 返回信息
		/// </summary>
		/// <param name="id">错误号</param>
		/// <param name="code">错误码</param>
		/// <returns></returns>
		public Return(int id, object code)
		{
			Id = id;
			Code = code + "";
		}
		
		/// <summary>
		/// 返回信息
		/// </summary>
		/// <param name="id">错误号</param>
		/// <param name="code">错误码</param>
		/// <param name="message">错误信息</param>
		/// <returns></returns>
		public Return(int id, object code, object message)
		{
			Id = id;
			Code = code + "";
			Message = message + "";
		}
		
		public Return(int id, object code, object message, object data)
		{
			Id = id;
			Code = code + "";
			Message = message + "";
			_data = data;
		}
		
		public Return(int id, object code, object message, object data, object url)
		{
			Id = id;
			Code = code + "";
			Message = message + "";
			_data = data;
			Url = url + "";
		}
		public object Data()
		{
			return _data;
		}
		public T Data<T>()
		{
			return _data == null ? default(T) : (T)_data;
		}
		
		public void TryAlertError(string type = "json")
		{
			if(Id>0)
			{
				if(type == "xml")
				{
					string xml = null;
					xml += "<id><![CDATA[" + Id + "]]></id>";
					xml += "<code><![CDATA[" + Code + "]]></code>";
					xml += "<message><![CDATA[" + Message + "]]></message>";
					if(Url != null){xml += "<url><![CDATA[" + Url + "]]></url>";}			
					xml = "<xml>" +  xml + "</xml>";
					System.Web.HttpContext.Current.Response.Clear();
					System.Web.HttpContext.Current.Response.Write(xml);
					System.Web.HttpContext.Current.Response.End();
				}
				else
				{
					string json = null;
					json += ",\"id\":" + Id + "";
					json += ",\"code\":\"" + Code + "\"";
					json += ",\"message\":\"" + Message + "\"";
					if(Url != null){json += ",\"url\":\"" + Url + "\"";}
					json = "{" + json.Substring(1) + "}";
					System.Web.HttpContext.Current.Response.Clear();
					System.Web.HttpContext.Current.Response.Write(json);
					System.Web.HttpContext.Current.Response.End();
					
				}
			}
		}
	}
}	