<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebManager.appaspx.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="/style/bootstrap.min.css" />
    <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .container{width:400px;margin:240px auto;background:#FFFF66;
                   height:300px;padding:5px;
                   border:outset #fff 5px;
        }
        .container h2{text-align:center;margin-bottom:20px;}
        .container p{text-align:center;font-size:16px;padding-top:20px;color:red;}
        body{
           background-image: linear-gradient(to bottom, #353535 0%, #111111 60%, #111111 70%, #222222 100%);
        }
    </style>
</head>
<body>
    <div class="container">
        <form class="form-horizontal" role="form" runat="Server">
            <h2>捕鱼H5管理后台登录</h2>
            <div class="form-group">
            <label for="account" class="col-sm-2 control-label">账号:</label>
            <div class="col-sm-10">
              <input type="account" class="form-control" id="account" placeholder="账号" runat="Server"/>
            </div>
          </div>
            <div class="form-group">
            <label for="password" class="col-sm-2 control-label">密码:</label>
            <div class="col-sm-10">
              <input type="password" class="form-control" id="password" placeholder="密码" runat="Server"/>
            </div>
          </div>
            <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <asp:Button ID="Button1" runat="server" Text="登录" CssClass="btn btn-default form-control"
                                onclick="LoginButton_Click"  ClientIDMode="Static"/>
            </div>
          </div>
         <p runat="server" ID="ErrorInfo" style="color:red"></p>
        </form>
    </div>
</body>
</html>
