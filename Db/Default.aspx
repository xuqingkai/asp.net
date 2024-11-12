<%@Page Language="C#" Debug="true" Inherits="System.Web.UI.Page"%>
<%
	string databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
	System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(databaseConnectionString);
	if (connection.State != System.Data.ConnectionState.Open) { connection.Open(); }
	
	string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
	double timestamp = System.Math.Floor((System.DateTime.Now - System.Convert.ToDateTime("1970-01-01 00:00:00")).TotalSeconds);
	
	string sql = "SELECT * FROM [xqk_ip_log] WHERE 1=1 ORDER BY ID DESC;";
	System.Data.OleDb.OleDbCommand command = new System.Data.OleDb.OleDbCommand(sql, connection);
	System.Data.DataTable dataTable = new System.Data.DataTable();
	new System.Data.OleDb.OleDbDataAdapter(command).Fill(dataTable);
	
	foreach(System.Data.DataRow dr in dataTable.Rows)
	{
		Response.Write(dr["id"] + "【" + dr["ip"] + "】" + dr["create_datetime"] + "<hr />");
	}
	
	connection.Close();
	connection.Dispose();
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
	<form class="uk-container" method="post" action="./init.php">
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
            <div class="uk-width-expand"><input class="uk-input" type="text" name="hostname" value=""></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">端口</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="hostport" value="3306"></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">数据库</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="database" value="" placeholder="SQLite库文件路径""></div>
        </div>
        <div class="uk-grid-collapse uk-flex-middle" uk-grid>
            <div class="uk-width-1-4 uk-width-1-6@s uk-padding-small uk-text-right">用户名</div>
            <div class="uk-width-expand"><input class="uk-input" type="text" name="username" value="root"></div>
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
            $.post(window.location.href, $(this).serialize(),function(data){
                $('#result').text(data);

            });

        });
    </script>
</body>
</html>
