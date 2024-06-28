namespace Pay
{
	/// <summary>
	/// 第三方支付
	/// </summary>
	public partial class XPay : System.Web.UI.Page
	{
		/// <summary>
        /// 商户配置
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, string> Config = new System.Collections.Generic.Dictionary<string, string>();

        /// <summary>
        /// OleDb
        /// </summary>
        private string _oleDbConnectionString = null;

		/// <summary>
        /// SQL执行错误日志
        /// </summary>
		public string SqlExecuteLog = null;
		
		/// <summary>
        /// 二维码生成地址
        /// </summary>
		public static string QRUrl = "http://mobile.qq.com/qrcode/?url="; 
		
		/// <summary>
        /// 支付表单
        /// </summary>
		public static string PayForm(string action, string form, string method = "post", string target = null)
		{
			if(target != null && target.Length > 0)
			{
				target = " target=\"" + target + "\"";
			}
			string html = "<form" + target + " id=\"payform\" method=\"" + method + "\" action=\"" + action + "\">\r\n";
			html += form + "\r\n";
			html += "<input type=\"submit\" value=\"我已支付，点击下发\" />\r\n";
			html += "</form>\r\n";
			html += "<script type=\"text/javascript\">\r\n";
			html += "document.getElementById('payform').submit();\r\n";
			html += "</script>\r\n";
			return html;
		}

        /// <summary>
        /// SqlOleDb
        /// </summary>
        public XPay ()
        {
        }

        /// <summary>
        /// SqlOleDb
        /// </summary>
        public XPay(string database)
        {
            System.Uri uri = new System.Uri(database);
			_oleDbConnectionString += "Provider=SqlOleDb;";
			_oleDbConnectionString += "Data Source=" + uri.Authority + uri.LocalPath.Substring(0, uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/")).Replace("/","\\") + ";";
			_oleDbConnectionString += "Initial Catalog=" + uri.LocalPath.TrimEnd("/".ToCharArray()).Substring(uri.LocalPath.TrimEnd("/".ToCharArray()).LastIndexOf("/") + 1) + ";";
			_oleDbConnectionString += "User ID=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(0, uri.UserInfo.IndexOf(":"))) + ";";
			_oleDbConnectionString += "Password=" + System.Web.HttpUtility.UrlDecode(uri.UserInfo.Substring(uri.UserInfo.IndexOf(":") + 1));
        }
		public XPay ConfigAdd(string key, string val)
        {
			if (!Config.ContainsKey(key))
			{
				Config.Add(key, val);
			}
			else
			{
				Config[key] = val;
			}
			return this;
		}
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            int rows = 0;
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_oleDbConnectionString);
            if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
            System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
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
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            object result = null;
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_oleDbConnectionString);
            if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
            System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
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
        /// DataAdapter
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        private System.Data.DataTable DataAdapter(string sql)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(_oleDbConnectionString);
			if (oleDbConnection.State != System.Data.ConnectionState.Open) { oleDbConnection.Open(); }
            System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(sql, oleDbConnection);
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
        /// DataRow
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        public System.Data.DataRow DataRow(string sql)
        {
            System.Data.DataTable dataTable = DataAdapter(sql);
            if (dataTable.Rows.Count == 0) { return dataTable.NewRow(); }
            return dataTable.Rows[0];
        }

        /// <summary>
        /// DataRows
        /// </summary>
        /// <param name="sql">SQL</param>
        protected System.Data.DataRowCollection DataRows(string sql)
        {
            return DataAdapter(sql).Rows;
        }
		
		/// <summary>
        /// 判断浏览器类型
        /// </summary>
        /// <param name="browers">数据</param>
		public static bool UserAgentContains(string browers)
		{
			string userAgent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] + "";
			if(browers == "weixin" && userAgent.Contains(" MicroMessenger/"))
			{
				return true;
			}
			else if(browers == "alipay" && userAgent.Contains(" AlipayClient/"))
			{
				return true;
			}
			else if(browers == "qq" && userAgent.Contains(" QQ/"))
			{
				return true; 
			}
			else if(browers == "mobile" && (userAgent.Contains("Android") || userAgent.Contains("iPhone") || userAgent.Contains("ios") || userAgent.Contains("iPod")))
			{
				return true;
			}
			else if(userAgent.Contains(browers))
			{
				return true;
			}
			return false;
		}
        	
        /// <summary>
		/// 调试保存
		/// </summary>
		/// <param name="content">内容</param>
		/// <param name="name">生成文件名</param>
		/// <returns></returns>
		public static string Debug(object content, object filename)
		{
			string log = null;
            log += "<debug>";
			log += "<datetime>" + System.DateTime.Now.ToString() + "</datetime>\r\n";
			log += "<referer>" + System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] + "<referer>\r\n";
			log += "<path>" + System.Web.HttpContext.Current.Request.Url.ToString() + "<path>\r\n";
			log += "<content>" + content + "</content>\r\n";
			log += "</debug>\r\n\r\n";

			filename = System.Web.HttpContext.Current.Server.MapPath("/") + filename.ToString();
			System.IO.File.AppendAllText(filename.ToString(), log, System.Text.Encoding.UTF8);
			return null;
		}
		
		/// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="dict">数据</param>
        /// <returns></returns>
        public static string MD5(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data));
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (byte b in bytes) { stringBuilder.Append(b.ToString("x2")); }
            return stringBuilder.ToString();
        }
		
		/// <summary>
		/// 当前URL，带协议和域名
		/// </summary>
		/// <param name="path">可取空，空字符串，斜杠，问号，路径</param>
		public static string Url(string path = null)
		{
			string url = System.Web.HttpContext.Current.Request.Url.ToString();
			if (path == "?")
			{
				url = url.Substring(0, url.IndexOf("/", 10)) + System.Web.HttpContext.Current.Request.FilePath;
			}
			else if (path == "./" || path == "")
			{
				string filePath = System.Web.HttpContext.Current.Request.FilePath;
				url = url.Substring(0, url.IndexOf("/", 10)) + filePath.Substring(0, filePath.LastIndexOf("/") + 1);
			}
			else if (path == "/")
			{
				url = url.Substring(0, url.IndexOf("/", 10));
			}
			else if (path != null && path.StartsWith("/"))
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
		/// GET请求
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="headers">头</param>
		/// <returns></returns>
		public static string HttpGet(string url, System.Collections.Generic.Dictionary<string, string> headers, string charset = "UTF-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			if(headers != null)
			{
				if(!headers.ContainsKey("Content-Type") && headers.ContainsKey("ContentType"))
				{
					headers.Add("Content-Type", headers["ContentType"]);
					headers.Remove("ContentType");
				}
				if(!headers.ContainsKey("User-Agent") && headers.ContainsKey("UserAgent"))
				{
					headers.Add("User-Agent", headers["UserAgent"]);
					headers.Remove("UserAgent");
				}
				foreach(string key in headers.Keys)
				{
					webClient.Headers.Add(key,headers[key]);
				}
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
		/// <returns></returns>
		public static string HttpPost()
		{
			byte[] bytes = new byte[System.Web.HttpContext.Current.Request.InputStream.Length];
			System.Web.HttpContext.Current.Request.InputStream.Read(bytes, 0, bytes.Length);
			string result = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
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
		public static string HttpPost(string url, string data, System.Collections.Generic.Dictionary<string, string> headers = null, string charset="utf-8", string contentType="utf-8")
		{
			string result = null;
			System.Net.WebClient webClient = new System.Net.WebClient();
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
			//webClient.Encoding = System.Text.Encoding.UTF8;
			if(headers != null)
			{
				if(!headers.ContainsKey("Content-Type") && headers.ContainsKey("ContentType"))
				{
					headers.Add("Content-Type", headers["ContentType"]);
					headers.Remove("ContentType");
				}
				if(!headers.ContainsKey("User-Agent") && headers.ContainsKey("UserAgent"))
				{
					headers.Add("User-Agent", headers["UserAgent"]);
					headers.Remove("UserAgent");
				}
				foreach(string key in headers.Keys)
				{
					webClient.Headers.Add(key,headers[key]);
				}
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
		/// Dict转请求字符串
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="asciiSort"></param>
		public static string DictToRequest(System.Collections.Generic.Dictionary<string, string> dict)
		{
			string requestString = null;
			foreach(string key in dict.Keys)
			{
				requestString += "&" + key + "=" + System.Web.HttpUtility.UrlEncode(dict[key]) + "";
			}
			if(requestString != null && requestString.Length > 0){requestString = requestString.Substring(1);}
			return requestString;
		}
		
		/// <summary>
		/// Dict转签名字符串
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="outKeys"></param>
		/// <param name="sort"></param>
		public static string DictToSign(System.Collections.Generic.Dictionary<string, string> dict, string keys, int asciiSort)
		{
			string signString = null;
			if(asciiSort > 0)
			{
				if(keys != null && keys.Length > 0)
				{
					foreach(string key in keys.Split(",".ToCharArray()))
					{
						signString += "&" + key + "=" + dict[key] + "";
					}
				}
			}
			else
			{
				keys = ("," + keys + ",").ToLower();
				string[] keyArray = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
				if(asciiSort != 0)
				{
					System.Array.Sort(keyArray,string.CompareOrdinal);
				}
				
				foreach(string key in keyArray)
				{
					if(!keys.Contains("," + key.ToLower() + ","))
					{
						signString += "&" + key + "=" + dict[key] + "";
					}
				}
			}
			if(signString != null && signString.Length > 0){signString = signString.Substring(1);}
			return signString;
		}
		
		/// <summary>
		/// Dict转Xml
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="asciiSort"></param>
		public static string DictToXml(System.Collections.Generic.Dictionary<string, string> dict, int asciiSort = 0)
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort > 0)
			{
				System.Array.Sort(keys,string.CompareOrdinal);
			}
			
			string xml = null;
			foreach(string key in keys)
			{
				xml += "<" + key + "><![CDATA[" + dict[key] + "]]></" + key + ">";
			}
			xml = "xml" + xml + "</xml>";
			return xml;
		}
		
		/// <summary>
		/// Dict转Xml
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="asciiSort"></param>
		public static string DictToJson(System.Collections.Generic.Dictionary<string, string> dict, int asciiSort = 0)
		{
			string[] keys = new System.Collections.Generic.List<string>(dict.Keys).ToArray();
			if(asciiSort > 0)
			{
				System.Array.Sort(keys,string.CompareOrdinal);
			}
			
			string json = null;
			foreach(string key in keys)
			{
				json += ",\"" + key + "\":\"" + dict[key] + "\"";
			}
			if(json != null && json.Length > 0){json = json.Substring(1);}
			json = "{" + json + "}";
			return json;
		}
		
		/// <summary>
		/// JSON字符串转为DICT对象
		/// </summary>
		/// <param name="strJSON">JSON字符串</param>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, dynamic> JsonToDict(string strJSON)
		{
			System.Collections.Generic.Dictionary<string, dynamic> dict = null;
			try
			{
				System.Web.Script.Serialization.JavaScriptSerializer javascriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
				dict = javascriptSerializer.Deserialize<dynamic>(strJSON);
			}
			catch { }
			return dict;
		}
		
		/// <summary>
		/// 请求转数组
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, string> RequestToDict(System.Collections.Specialized.NameValueCollection request = null)
		{
			if(request == null)
			{
				request = System.Web.HttpContext.Current.Request.QueryString;
			}
			System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
			foreach(string key in request.AllKeys)
			{
				dict.Add(key, request[key]);
			}
			return dict;
		}
		
		/// <summary>
		/// 过滤请求字符串
		/// </summary>
		/// <param name="queryString">请求字符串</param>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, string> RequestToDict(string queryString)
		{
			System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Specialized.NameValueCollection request = System.Web.HttpUtility.ParseQueryString(queryString);
			foreach(string key in request.AllKeys)
			{
				dict.Add(key, request[key]);
			}
			return dict;
		}
		
		/// <summary>
		/// Xml转Dict
		/// </summary>
		/// <param name="strXml"></param>
		/// <returns></returns>
		public static System.Collections.Generic.Dictionary<string, string> XmlToDict(string strXml)
		{
			System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(strXml);
			foreach(System.Xml.XmlNode xmlNode in xmlDocument["xml"])
			{
				dict.Add(xmlNode.Name, xmlNode.InnerText);
			}
			return dict;
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
				help += "<br />System.Convert.ToBase64String(publicRSA.Encrypt(System.Text.Encoding.GetEncoding(charset).GetBytes(content), false));";
				help += "<br />System.Text.Encoding.GetEncoding(charset).GetString(privateRSA.Decrypt(System.Convert.FromBase64String(content), false));";
				help += "<br />System.Convert.ToBase64String(privateRSA.SignData(System.Text.Encoding.GetEncoding(charset).GetBytes(content), signType));";
				help += "<br />publicRSA.VerifyData(System.Text.Encoding.GetEncoding(charset).GetBytes(content), signType, System.Convert.FromBase64String(signData));";
			}

			string filePath = (cert + "").ToLower(); string content = null;
			if (filePath.EndsWith(".xml"))
			{
				if (!filePath.Contains(":")) 
				{
					filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); 
				}
				content = System.IO.File.ReadAllText(filePath, System.Text.Encoding.ASCII);
				System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
				rsa.FromXmlString(content);
				return rsa;
			}
			else if (filePath.EndsWith(".pfx") || filePath.EndsWith(".p12"))
			{
				if (filePath.Contains(":") == false) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = null;
				if (password == null)
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				}
				else
				{
					x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath, password.ToString(), System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
				}
				if (dwKeySize >= 0)
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
				if (filePath.Contains(":") == false) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				System.Security.Cryptography.X509Certificates.X509Certificate2 rsa = new System.Security.Cryptography.X509Certificates.X509Certificate2(filePath);
				return (System.Security.Cryptography.RSACryptoServiceProvider)rsa.PublicKey.Key;
			}
			else if (filePath.EndsWith(".key") || filePath.EndsWith(".pem") || filePath.EndsWith(".txt"))
			{
				if (filePath.Contains(":") == false) { filePath = System.Web.HttpContext.Current.Server.MapPath(filePath); }
				content = System.IO.File.ReadAllText(filePath);
				content = content.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r", "");
				content = content.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\n", "");
			}
			else
			{
				content = cert + "";
			}

			if (content.Length > 0)
			{
				byte[] keyBytes = System.Convert.FromBase64String(content);
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
						System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(dwKeySize, cspParameters);
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
		/// 公钥加密
		/// </summary>
		/// <param name="publicRSA">RSA对象</param>
		/// <param name="content">待加密数据</param>
		/// <returns></returns>
		public static string RSAEncrypt(System.Security.Cryptography.RSACryptoServiceProvider publicRSA, string content)
		{
			byte[] byteText = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
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
		public static string RSADecrypt(System.Security.Cryptography.RSACryptoServiceProvider privateRSA, string content)
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
			return System.Text.Encoding.GetEncoding("UTF-8").GetString(target.ToArray());
		}

		/// <summary>
		/// 私钥签名
		/// </summary>
		/// <param name="privateRSA">RSA对象</param>
		/// <param name="content">待签数据</param>
		/// <returns></returns>
		public static string RSASign(System.Security.Cryptography.RSACryptoServiceProvider privateRSA, string content)
		{
			byte[] byteText = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
			byte[] byteSign = privateRSA.SignData(byteText, privateRSA.KeySize == 2048 ? "SHA256" : "SHA1");
			return System.Convert.ToBase64String(byteSign);
		}

		/// <summary>
		/// 公钥验签
		/// </summary>
		/// <param name="rsa">RSA对象</param>
		/// <param name="content">待签数据</param>
		/// <param name="signature">签名结果</param>
		/// <returns></returns>
		public static bool RSAVerify(System.Security.Cryptography.RSACryptoServiceProvider publicRSA, string content, string signature)
		{
			byte[] byteText = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
			byte[] byteSign = System.Convert.FromBase64String(signature);
			return publicRSA.VerifyData(byteText, publicRSA.KeySize == 2048 ? "SHA256" : "SHA1", byteSign);
		}
    }

}
