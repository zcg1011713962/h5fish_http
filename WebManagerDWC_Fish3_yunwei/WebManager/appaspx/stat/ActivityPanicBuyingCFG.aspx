<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ActivityPanicBuyingCFG.aspx.cs" Inherits="WebManager.appaspx.stat.ActivityPanicBuyingCFG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/module/sea.js" type="text/javascript"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/stat/ActivityPanicBuyingCfg.js?ver=3");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">限时抢购活动操作</h2>
        <div class="container" style="margin-top:10px;">
            <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered" style="width:80%"></asp:Table>
         </div>
        具体值&ensp;：&ensp;<asp:TextBox ID="txtMaxCount" runat="server" CssClass="cTextBox" style="height:32px;"></asp:TextBox>&ensp;&ensp;&ensp;
                <input type="button" id="btnModify" value="修改最大次数" style="width:120px;"  class="cButton btn btn-primary"/>
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        <div style="padding-top:10px;"></div>
    </div>
</asp:Content>
