<%@ Page Title="关于我们" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="WebManager.About" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" media="all" href="/style/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" media="all" href="/style/font-awesome/css/font-awesome.css" />
    <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <style>
        .btn-primary{
                width:300px;
                height:80px;
                line-height:50px;
                margin:2px;
                text-align:left;
            }
        .btn-primary a {
             color:white;
             font-size:20px;
        }
        .btn-primary a:link {
            text-decoration: none;
        }
        .btn-primary .fa {
            font-size:40px;color:#ccc;line-height:60px;text-align:left
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
  <div class="jumbotron">
       <div class="container">
           <h1>欢迎</h1>
           <p id="content" runat="server"></p>
       </div>
   </div>
</asp:Content>
