<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StatBuyuLog.aspx.cs" Inherits="WebManager.appaspx.stat.StatBuyuLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="../../Scripts/operation/OperationPlayerOp.js?ver=1" type="text/javascript"></script>
    <style type="text/css">
        .cOp1 table{border-collapse:collapse}
        .cOp1 td{padding:10px;width:200px;border:1px solid black;text-align:center;
                 font-size:16px;
                 background:#ccf;
                 color:#000;
                 font-weight:bold;
        }
        .cOp1 input[type=text]{padding:1px;width:90%;height:20px;}
        .cOp1 input[type=button]{padding:10px;width:80px;height:40px;line-height:10px;
        }
        .cOp1 input.SingleOp{width:160px;}
        #logFish li{list-style:none;font-size:14px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth">
        <h2 style="text-align:center;padding:20px;">捕鱼LOG</h2>
        <div class="cOp1 cDiv1"">
            <table style="margin:0 auto">
                <tr>
                    <td>操作内容</td>
                    <td>玩家ID</td>
                    <td>数值</td>
                    <td></td>
                </tr>
                <tr>
                    <td>捕鱼LOG开关</td>
                    <td><input type="text" id="playerLog"/></td>
                    <td></td>
                    <td>
                        <input type="button" id="Button1" value="打开捕鱼log" 
                            class="SingleOp" player="playerLog" opType="logFish" playerProp="open"/>
                        <input type="button" id="Button2" value="关闭捕鱼log" 
                            class="SingleOp" player="playerLog" opType="logFish" playerProp="close"/>
                    </td>
                </tr>
            </table>
        </div>
        <div id="logFish" class="cOp1">
            <div class="container-fluid" style="text-align:left;margin-left:6.5%;">
                <div class="row">
                    <div class="col-lg-6">
                        <h3 style="padding:5px;">当前打开捕鱼Log的玩家ID列表</h3>
                        <input id="logFishRefresh" type="button" value="刷新列表"/>
                        <ul></ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
