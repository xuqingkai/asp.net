<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%=VerifyImage(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"])%>
<script runat="server">
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

