<?php
$db=$_POST;

if($db){
    //exit(json_encode($db, JSON_UNESCAPED_UNICODE));
    //header('Content-Type: application/json');
    $dsn="mysql:host=".trim($db['hostname']).";port=".trim($db['hostport']).";dbname=".trim($db['database']).";charset=".trim($db['charset']);
    if($db['type']=='sqlite'){ $dsn="sqlite:".trim($db['database']); }
    //exit($dsn);
    $result='';
    try{
        $pdo = new \PDO($dsn, trim($db['username']), trim($db['password']));
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION); // 设置错误模式为抛出异常
        $query = $pdo->query($db['sql']);
        $result = $query->fetchALL(\PDO::FETCH_ASSOC);
        $result=json_encode($result, JSON_UNESCAPED_UNICODE);
    }catch(PDOException $e){
        $result=$e->getMessage();
    }
    exit($result);
}
?>
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
