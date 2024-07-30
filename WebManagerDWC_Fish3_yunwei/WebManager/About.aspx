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
           <%--<div class="btn btn-primary btn-lg">
               <i class="fa fa-cny"></i>&ensp;&ensp;
               <a href="/appaspx/service/ServiceRechargeQueryNew.aspx" runat="server" id="m_totalIncome">今日收入</a>
           </div>

            <div class="btn btn-primary btn-lg">
               <i class="fa fa-cny"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdActivation.aspx" runat="server" id="m_arpu">今日ARPU</a>
           </div>

            <div class="btn btn-primary btn-lg">
               <i class="fa fa-cny"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdActivation.aspx" runat="server" id="m_arppu">今日ARPPU</a>
           </div>

             <div class="btn btn-primary btn-lg">
               <i class="fa fa-line-chart"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdActivation.aspx" runat="server" id="m_rechargeRate">今日付费率</a>
           </div>

             <div class="btn btn-primary btn-lg">
               <i class="fa fa-users"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdActivation.aspx" runat="server" id="m_register">新增注册</a>
           </div>

           <div class="btn btn-primary btn-lg">
               <i class="fa fa-users"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdActivation.aspx" runat="server" id="m_dau">今日DAU</a>
           </div>

            <div class="btn btn-primary btn-lg">
               <i class="fa fa-users"></i>&ensp;&ensp;
               <a href="/appaspx/td/TdOnlinePerHour.aspx" runat="server" id="m_online">实时在线</a>
           </div>--%>
       </div>
   </div>
</asp:Content>
