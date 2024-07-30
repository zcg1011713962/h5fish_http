<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cdkeyNew.aspx.cs" Inherits="WebManager.appaspx.cdkeyNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../Scripts/cdkeyNew.js");
    </script>
    <style>
        td:nth-of-type(odd) {
            min-width:95px
        }
        td:nth-of-type(even) {
            min-width:120px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color:#ccc;margin:20px;">
        <h2 style="padding:20px;text-align:center">可重复使用礼包码CDKEY生成器</h2>
        <table style="margin:0 auto">
            <tr style="text-align:right">
                <td>失效时间：</td>
                <td style="min-width:245px;"><asp:TextBox ID="m_deadTime" runat="server" placeHolder="格式：2018/08/08，大于当前时间" CssClass="form-control" style=""></asp:TextBox></td>
                <td>对应礼包：</td>
                <td><asp:DropDownList ID="m_gift" runat="server" CssClass="form-control" style="min-width:130px;"></asp:DropDownList></td>
                <td><asp:TextBox ID="m_customItem" runat="server" CssClass="form-control" style="min-width:200px;"></asp:TextBox></td>
                <td>礼包码输入：</td>
                <td><asp:TextBox ID="m_cdkeyCode" runat="server" placeHolder="7或9位字符，如BYDR888" CssClass="form-control" style="min-width:190px;"></asp:TextBox></td>
                <td>最大可用次数：</td>
                <td><asp:TextBox ID="m_maxUseCount" runat="server" CssClass="form-control"></asp:TextBox></td>
                <td style="min-width:60px;">备注：</td>
                <td><asp:TextBox ID="m_comment" runat="server" CssClass="form-control"></asp:TextBox></td>
            </tr>
        </table><br /><br />
        <div  style="text-align:center">
            <span style="font-weight:bolder">注：生成的CDKey默认每个账号只可兑换一次</span><br /><br />
            <asp:Button ID="Button1" runat="server" Text="点击生成" onclick="onSubmit" CssClass="btn btn-primary"/>
            <asp:Button ID="Button2" runat="server" Text="刷新服务器" onclick="onRefresh" CssClass="btn btn-primary"/>
            <span id="m_res" runat="server" style="color:red"></span>
           
            
        </div><br />
    </div>
    <hr />
    <%-- ///////////////////////////////////////////////////////////////////////////////////////////////////// --%>
    <h2 style="margin:20px 0 10px 0;text-align:center">历史礼包码</h2> 
    <div class="container" style="text-align:center">        
        <span id="m_opRes" runat="server" style="color:red;text-align:center"></span>
        <br />
        <br />
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered"></asp:Table>
        </div>
    <br />
</asp:Content>
