<%@ Page Language="C#" %>
<%
string contents = Request.QueryString["text"] ?? Request.QueryString["txt"];
contents = contents ?? Request.QueryString["value"] ?? Request.QueryString["url"];
contents = contents ?? Request.QueryString["qr"] ?? Request.QueryString["qrcode"];
contents = contents ?? Request.QueryString.ToString();
if(contents == null || contents.Length == 0){contents = "null";}

int width = System.Convert.ToInt32("0" + (Request.QueryString["width"] ?? Request.QueryString["size"] ?? null));
if(width < 50){ width = 250;}

string head = Request.QueryString["head"] ?? Request.QueryString["title"] ?? "";
string body = Request.QueryString["body"] ?? "";
string foot = Request.QueryString["foot"] ?? "";

int height = width;
if((head + "" + foot).Length > 0){ height += 44;}

string charset = Request.QueryString["charset"] ?? null;
if(charset == null || charset.Length == 0){charset = "UTF-8";}

string level = Request.QueryString["level"] ?? null;
if(level == null || level.Length == 0){level = "Q";}
level = level.ToUpper();
ZXing.QrCode.Internal.ErrorCorrectionLevel errorCorrectionLevel = (level == "L" ? ZXing.QrCode.Internal.ErrorCorrectionLevel.L : level == "M" ? ZXing.QrCode.Internal.ErrorCorrectionLevel.M : level == "Q" ? ZXing.QrCode.Internal.ErrorCorrectionLevel.Q : ZXing.QrCode.Internal.ErrorCorrectionLevel.H);

int margin = System.Convert.ToInt32("0" + Request.QueryString["margin"]);
if(margin < 0){ margin = 0;}


ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;
barcodeWriter.Options = new ZXing.QrCode.QrCodeEncodingOptions
{
	DisableECI = false,
	CharacterSet = charset,
	Width = width,
	Height = height,
	ErrorCorrection = errorCorrectionLevel,
	Margin = margin,
};
System.Drawing.Bitmap bitmap = barcodeWriter.Write(contents);

if(height>width)
{
	System.Drawing.StringFormat stringFormat = new System.Drawing.StringFormat();
	stringFormat.Alignment = System.Drawing.StringAlignment.Far;
	stringFormat.Alignment = System.Drawing.StringAlignment.Center;
	
	System.Drawing.Rectangle headRectangle = new System.Drawing.Rectangle(0, 5, width, height);
	System.Drawing.Rectangle bodyRectangle1 = new System.Drawing.Rectangle(0, System.Convert.ToInt32(height/2-24), width, height);
 	System.Drawing.Rectangle footRectangle1 = new System.Drawing.Rectangle(0, height-24, width, height);
  
	System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
	System.Drawing.Font font = new System.Drawing.Font("Yahei", 11, System.Drawing.FontStyle.Bold);
	System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml("#FF0000"));
	graphics.DrawString(head, font, brush, headRectangle, stringFormat);
	graphics.DrawString(body, font, brush, bodyRectangle1, stringFormat);
	graphics.DrawString(foot, font, brush, footRectangle1, stringFormat);
}

System.Web.HttpContext.Current.Response.Clear();
System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
System.Web.HttpContext.Current.Response.Cache.SetNoStore();
System.Web.HttpContext.Current.Response.ClearContent();
System.Web.HttpContext.Current.Response.ContentType = "image/Png";
System.Web.HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
bitmap.Dispose();
System.Web.HttpContext.Current.Response.End();
%>
