﻿﻿<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%
System.Collections.Specialized.NameValueCollection db = System.Web.HttpContext.Current.Request.Form;
if(db.Count>0){
	Response.ContentType = "application/json";
	string databaseConnectionString = "Provider=SQLOLEDB;Data Source=" + db["hostname"] + ";Initial Catalog=" + db["database"] + ";User ID=" + db["username"] + ";Password=" + db["password"] + ";";
	if(!string.IsNullOrEmpty(db["hostport"])){ databaseConnectionString += "Port=" + db["hostport"] + ";";;}
	System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
	if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); }

	string sql=System.Text.Encoding.GetEncoding("utf-8").GetString(System.Convert.FromBase64String(db["sql"]+""));
	//Response.Clear(); Response.Write(sql); Response.End();

	System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(sql, connection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.OleDb.OleDbDataAdapter(command).Fill(dataTable);
	
	string json = "";
	foreach(System.Data.DataRow dr in dataTable.Rows)
	{
		string data = "";
		foreach (System.Data.DataColumn dataColumn in dr .Table.Columns)
		{
			data += ",\"" + dataColumn.ColumnName + "\":\"" + dr [dataColumn] + "\"";
		}
		json += ",{" + data.Substring(1) + "}";	
	}
	if(!string.IsNullOrEmpty(json))
	{
		json = json.Substring(1);
	}
	json = "[" + json + "]";
	connection.Close();
	connection.Dispose();
	
	Response.Clear();
	Response.Write(json);
	Response.End();
}
%>
<!DOCTYPE html>
<html lang="zh-cmn-Hans">
<head>
	<meta charset="utf-8">
	<title>数据库初始化</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/uikit/dist/css/uikit.min.css" />
	<script src="https://cdn.jsdelivr.net/npm/uikit/dist/js/uikit.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/uikit/dist/js/uikit-icons.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/jquery@3/dist/jquery.min.js"></script>
	<style type="text/css">
	    .delete,.delete:hover{text-decoration: line-through;}
	</style>
</head>
<body>
	<form class="uk-container" method="post" action="<%=Request.Url.PathAndQuery%>">
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">类型</div>
            <div class="uk-width-expand">
                <select class="uk-select" name="type">
                    <option value="">请选择</option>
                    <option value="sqlserver">SQLServer</option>
                </select>
            </div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">主机</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="hostname" value="127.0.0.1"></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">端口</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="hostport" value=""></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">数据库</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="database" value=""></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">用户名</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="username" value="sa"></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">密码</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="password" value=""></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">编码</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="charset" value="utf8"></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">SQL语句</div>
            <div class="uk-width-expand">
                <textarea class="uk-textarea" rows="5" name="sql"></textarea>
            </div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">&nbsp;</div>
            <div class="uk-width-expand"><button class="uk-button uk-button-default">提交</button></div>
        </div>
        <div id="result"></div>
    </form>
    <script>
        $('select').change(function(){
            $.get('https://xuqingkai.github.io/code/sql/'+$(this).val()+'.sql',function(data){
                $('textarea').val(data);
            });
        });
        $('form').submit(function(e){
            e.preventDefault();
			let sql = '';
			(new TextEncoder()).encode($('textarea[name=sql]').val()).forEach((byte) => { sql += String.fromCharCode(byte); });

			var form={
				type:$('select[name=type]').val(),
				hostname:$('input[name=hostname]').val(),
				hostport:$('input[name=hostport]').val(),
				database:$('input[name=database]').val(),
				username:$('input[name=username]').val(),
				password:$('input[name=password]').val(),
				charset:$('input[name=charset]').val(),
				sql:btoa(sql),
			};
            $.post(window.location.href, form,function(data){
                $('#result').text(JSON.stringify(data));
            });

        });
    </script>
</body>
</html>