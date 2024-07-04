<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<title>System.Environment</title>
</head>
<body>
<h1>System.Environment</h1>
<%
    Response.Write("<p>CommandLine: " + System.Environment.CommandLine + "</p>");

    Response.Write("<p>GetCommandLineArgs: " + String.Join(", ", System.Environment.GetCommandLineArgs()) + "</p>");

    Response.Write("<p>CurrentDirectory: " + System.Environment.CurrentDirectory + "</p>");

    Response.Write("<p>ExitCode: " + System.Environment.ExitCode + "</p>");

    Response.Write("<p>HasShutdownStarted: " + System.Environment.HasShutdownStarted + "</p>");

    Response.Write("<p>MachineName: " + System.Environment.MachineName + "</p>");

    Response.Write("<p>NewLine: " + System.Environment.NewLine + "</p>");

    Response.Write("<p>OSVersion: " + System.Environment.OSVersion.ToString() + "</p>");

    Response.Write("<p>StackTrace: <br />" + System.Environment.StackTrace.Replace("\r\n", "<br />\r\n") + "</p>");

    Response.Write("<p>SystemDirectory: " + System.Environment.SystemDirectory + "</p>");

    Response.Write("<p>TickCount: " + System.Environment.TickCount + "</p>");

    Response.Write("<p>UserDomainName: " + System.Environment.UserDomainName + "</p>");

    Response.Write("<p>UserInteractive: " + System.Environment.UserInteractive + "</p>");

    Response.Write("<p>UserName: " + System.Environment.UserName + "</p>");

    Response.Write("<p>Version: " + System.Environment.Version.ToString() + "</p>");

    Response.Write("<p>WorkingSet: " + System.Environment.WorkingSet + "</p>");

     Response.Write("<p>No example for Exit(exitCode) because doing so would terminate this example.</p>");

    string query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
    Response.Write("<p>ExpandEnvironmentVariables: " + System.Environment.ExpandEnvironmentVariables(query) + "</p>");

    Response.Write("<p>System.Environment.GetEnvironmentVariable(\"TEMP\"): " + System.Environment.GetEnvironmentVariable("TEMP") + "</p>");

    Response.Write("<p>GetFolderPath: " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "</p>");

    Response.Write("<p>GetLogicalDrives: " + String.Join(", ", System.Environment.GetLogicalDrives()) + "</p>");

    Response.Write("<p>--------------------------------------------------------------------</p>");
    Response.Write("<p>GetEnvironmentVariables: ↓</p>");
    System.Collections.IDictionary environmentVariables = System.Environment.GetEnvironmentVariables();
    foreach (System.Collections.DictionaryEntry de in environmentVariables)
    {
        Response.Write("<p>System.Environment.GetEnvironmentVariable[\"" + de.Key + "\"] = " + de.Value + "</p>");
    }

%>

</body>
</html>
