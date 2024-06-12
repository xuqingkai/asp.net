namespace Com.Xuqingkai
{
	/// <summary>
	/// SMTP
	/// </summary>
	public class Smtp
	{
		/// <summary>
		/// SMTP对象
		/// </summary>
		private System.Net.Mail.SmtpClient _smtp = new System.Net.Mail.SmtpClient();
		
		/// <summary>
		/// SMTP服务器
		/// </summary>
		private string _server = null;
		
		/// <summary>
		/// 发件人
		/// </summary>
		private string _email = null;

		/// <summary>
		/// 邮箱密码
		/// </summary>
		private string _password = null;

		/// <summary>
		/// Smtp
		/// </summary>
		public Smtp(string server, string email, string password)
		{
			_email = email;

			if(server.Contains(":"))
			{
				_smtp.Host = server.Substring(0, server.IndexOf(":"));
				_smtp.EnableSsl = true;
				_smtp.Port = System.Convert.ToInt32(server.Substring(server.IndexOf(":") + 1));	
			}
			else
			{
				_smtp.Host = _server;
			}
			//创建验证凭据，现在大部分SMTP邮箱都需要这个了
			_smtp.UseDefaultCredentials = false;
			_smtp.Credentials = new System.Net.NetworkCredential(email, password);
			//指定电子邮件发送方式：代发
			_smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			
		}

		/// <summary>
		/// 发送
		/// </summary>
		/// <param name="to">收件人地址，多个以竖线分割</param>
		/// <param name="subject">标题</param>
		/// <param name="body">内容</param>
		/// <param name="attachment">附件路径，多个以竖线分割</param>
		/// <returns></returns>
		public string Send(string to = null, string subject = null, string body = null, string attachment = null)
		{

			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
			//优先级
			mail.Priority = System.Net.Mail.MailPriority.High;
			//发件人地址，一般就是email的值，切勿更改
			mail.From = new System.Net.Mail.MailAddress(_email);

			//标题
			if (subject == null || subject.Length == 0) { subject = "空标题：" + System.DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒"); }
			mail.Subject = subject;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;

			//内容
			if (body == null || body.Length == 0) { body = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
			mail.Body = body;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			mail.IsBodyHtml = true;

			//附件
			if (attachment != null && attachment.Length > 0)
			{
				foreach (string attach in attachment.Split(",".ToCharArray()))
				{
					string path = attach;
               if (!path.Contains(":")) { path = System.Web.HttpContext.Current.Server.MapPath(path); }
					mail.Attachments.Add(new System.Net.Mail.Attachment(path, System.Net.Mime.MediaTypeNames.Application.Octet));
				}
			}

			//回复地址
			mail.ReplyToList.Add(_email);

			//收件人地址
			foreach (string email in to.Split(",".ToCharArray())) 
			{
				if(email.Length > 0)
				{
					mail.To.Add(email); 
				}
			}

			//尝试发送
			string result = null;
			try 
			{
				_smtp.Send(mail);
			}
			catch (System.Net.Mail.SmtpException ex) 
			{
				result = ex.Message; 
			}
			return result;
		}
	}
}