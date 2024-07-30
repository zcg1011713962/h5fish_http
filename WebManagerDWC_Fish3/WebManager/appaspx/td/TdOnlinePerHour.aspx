<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="TdOnlinePerHour.aspx.cs" Inherits="WebManager.appaspx.td.TdOnlinePerHour" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
	<link rel="stylesheet" type="text/css" media="all" href="../../Scripts/datepicker/daterangepicker.css" />
    <script type="text/javascript" src="../../Scripts/datepicker/moment.min.js"></script>
    <script type="text/javascript" src="../../Scripts/datepicker/daterangepicker.js"></script>

    <script src="../../Scripts/stat/highcharts.js" type="text/javascript"></script>
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(function () {
	        $('#m_time').daterangepicker();
	    });

	    seajs.use("../../Scripts/td/TdOnlinePerHour.js?ver=4");
	</script>
    <style type="text/css">
        .cSafeWidth li{list-style:none;float:left;width:80px;height:30px;line-height:30px;font-size:16px;text-align:center;
                       background:#aaa;margin-left:10px;color:#000;border-radius:5px;border:1px solid #000;
        }
        .cSafeWidth li:hover{background:orange}
        .cSafeWidth .Active{background:orange}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="width:1000px;text-align:center;margin:0 auto">
        <h2 style="padding:20px;">实时在线&ensp;&ensp;(当前在线&ensp;<asp:Label ID="m_count" runat="server" Enabled="false" style="width:300px;height:30px;font-size:20px;color:red;" ></asp:Label>)</h2> 

        <table style="margin:0 auto">
            <tr>
                <td style="min-width:40px;">时间：</td>
                <td><input id="m_time" style="width:300px;height:30px" /></td>
                <td>&ensp;</td>
                <td colspan="2">
                    <%-- <input type="button" id="onQuery" value="查询" style="width:60px;height:30px" /> --%>
                </td>
            </tr>
        </table>
        <br />
        <h2 style="display:inline"></h2>
        <br />
        <ul id="optionGame">
            <li class="Active" gameId="0">总体</li>
            <li class="" gameId="1">捕鱼</li>
        </ul>
        <div class="clear"></div>
    </div>

    <table id="m_result" class="cTable"></table>
    
    <div id="divTemplate" style="display:none;">
        <div id="{0}" style="max-width:1200px;min-height:400px; margin:10px auto;border:1px solid #000;border-radius:10px;padding:10px;"></div>
    </div>
    
    <div id="divContent" style="padding:10px;"></div>
</asp:Content>
