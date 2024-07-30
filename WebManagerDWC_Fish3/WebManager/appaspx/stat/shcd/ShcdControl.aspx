<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ShcdControl.aspx.cs" Inherits="WebManager.appaspx.stat.shcd.ShcdControl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/module/sea.js" type="text/javascript"></script>
    <script type="text/javascript">
        seajs.use("../../../Scripts/stat/ShcdControl.js?ver=4");
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="text-align:center;padding:20px;">黑红梅方参数调整</h2>
        <asp:Table ID="m_expRateTable" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <br />
        具体值：<asp:TextBox ID="txtExpRate" runat="server" CssClass="cTextBox"></asp:TextBox>
        <%--<select id="level">
            <option value="0">自动控制</option>
            <option value="1">天堂</option>
            <option value="2">普通</option>
            <option value="3">困难</option>
            <option value="4">超难</option>
            <option value="5">最难</option>
        </select>--%>
        <br /><br />
        <asp:Button ID="Button2" runat="server" Text="修改作弊最小阀值" onclick="onModifyExpRate" CssClass="btn btn-primary"/>
        <input type="button" value="修改作弊最大阀值" id="btnModifyMaxExpRate" class ="btn btn-warning" />
        <%--<input type="button" value="修改难度" id="btnModifyLevel" class="cButton"/>--%>
        <asp:Button ID="Button1" runat="server" Text="重置" onclick="onReset" CssClass="btn btn-success" />
       <%-- <input type="button" value="修改大小王个数" id="btnModifyJokerCount" />--%>
        <input type="button" value="设置作弊局数" id="btnCheat" class ="btn btn-danger" />
        <div style="padding-top:10px;">
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>

    </div>
</asp:Content>
