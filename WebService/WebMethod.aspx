<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<script runat="server">
[System.Web.Services.WebMethod]
public static string Test()   
{
    return "{\"name\":\"test\",\"age\":22}";
}
</script>

