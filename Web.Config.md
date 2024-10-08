```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
	<connectionStrings>
        <add name="Database" providerName="System.Data.OleDb" connectionString="Provider=SqlOleDb;Data Source=127.0.0.1;Initial Catalog=dbname;User ID=sa;Password=123456" /> 
        <add name="Database" providerName="System.Data.SqlClient" connectionString="Data Source=127.0.0.1;Initial Catalog=dbname;User ID=sa;Password=12345" /> 
        <add name="Database" providerName="System.Data.SqlClient" connectionString="server=127.0.0.1;database=dbname;uid=sa;pwd=12345" /> 
		<add name="Com.Xuqingkai.Data" providerName="SqlServer" connectionString="SqlServer://sa:123456@127.0.0.1/dbname" />
		<add name="Com.Xuqingkai.Data" providerName="SqlServer" connectionString="Data Source=127.0.0.1;Initial Catalog=dbname;User ID=sa;Password=123456" />
		<add name="Com.Xuqingkai.Data" providerName="SqlServer" connectionString="server=127.0.0.1;database=dbname;uid=sa;pwd=123456" />
		<add name="Com.Xuqingkai.Data" providerName="SqlOleDb" connectionString="SqlOleDb://sa:123456@127.0.0.1/dbname" />
		<add name="Com.Xuqingkai.Data" providerName="SqlOleDb" connectionString="Provider=SqlOleDb;Data Source=127.0.0.1;Initial Catalog=dbname;User ID=sa;Password=123456" />
	</connectionStrings>
    
	<!-- appSettings ASP.NET 2.0- -->
	<appSettings>
		<add key="Com.Xuqingkai.UrlReWrite" value="/UrlReWrite.txt" />
	</appSettings>
    
    <!-- appSettings ASP.NET 2.0+ -->
    <applicationSettings>
        <Com.Xuqingkai.UrlReWrite.Properties.Settings>
            <setting name="File" serializeAs="String">
                <value>/UrlReWrite.txt</value>
            </setting>
        </Com.Xuqingkai.UrlReWrite.Properties.Settings>
    </applicationSettings>
    
	<system.net> 
        <settings>
            <!-- 防止HttpWebRequest访问Https链接异常 -->
            <httpWebRequest useUnsafeHeaderParsing="true" />
        </settings> 
    </system.net>
    
	<system.web>
        <!-- 设置SESSION过期时间 -->
		<sessionState mode="InProc" timeout="600"/>
		<!-- 设置默认编译语言，是否启用调试模式 -->
		<compilation defaultLanguage="c#" debug="true" targetFramework="4.0" />
		<!-- 设置编码，否则签名和非签名UTF8文件混合使用时中文会乱码 -->
		<globalization requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="utf-8" />
		<!-- 关闭HTTP请求验证 -->
		<pages validateRequest="false" controlRenderingCompatibilityVersion="3.5" clientIDMode="AutoID" />
		<!-- framework4.0下,允许请求html代码　requestPathInvalidCharacters为空表示允许所有，否则请指定并用逗号分割　上传文件大小限制　　超时限制 -->
		<httpRuntime requestPathInvalidCharacters="" requestValidationMode="2.0" maxRequestLength="1048576" executionTimeout="3600" />
		<!-- 是否显示详细错误信息，如果Mode=On，则指定错误时转向URI，IIS，双击错误页，右侧编辑功能设置，显示详细信息 -->
		<customErrors mode="Off" defaultRedirect="/Error/" redirectMode="ResponseRewrite" />
		<!-- 指定WebServices可接受的请求方式 -->
        <webServices>
			<protocols>
				<add name="HttpGet" />
				<add name="HttpPost" />
			</protocols>
		</webServices>
	</system.web>
	<system.webServer>
		<!-- 禁止访问目录 -->
		<directoryBrowse enabled="false" />
		<modules runAllManagedModulesForAllRequests="true">
			<add name="UrlRewrite" type="Com.Xuqingkai.UrlReWrite" preCondition="managedHandler" />
		</modules>
		<!-- 
		<httpErrors errorMode="Custom" defaultResponseMode="File">
			<remove statusCode="409" subStatusCode="-1" />
			<error statusCode="409" subStatusCode="-1" path="/Error/" />
		</httpErrors>
		 -->
		<!-- 指示不用再检测system.web里相同的配置，IIS7.0+ -->
		<validation validateIntegratedModeConfiguration="false" />
		<!-- 默认文档
		<defaultDocument enabled="false">
			<files>
				<add value="Index.aspx" />
			</files>
		</defaultDocument>
		-->
	</system.webServer>
</configuration>
```