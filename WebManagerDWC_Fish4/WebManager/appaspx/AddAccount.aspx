<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AddAccount.aspx.cs" Inherits="WebManager.appaspx.AddAccount" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        td.left{width:100px;}
        table.tableForm td{padding-top:5px; padding-bottom:5px;}
    </style>
    <script type="text/javascript" src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../Scripts/AddAccount.js");
	</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="cSafeWidth" style="text-align:center">
        <h2 style="padding:20px;">添加账号</h2>
        <div>
            <table style="margin:0 auto" class="tableForm">
                <tr>
                    <td>账号：</td>
                    <td><asp:TextBox ID="m_accountName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>密码：</td>
                    <td><asp:TextBox ID="m_key1" runat="server" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>确认密码：</td>
                    <td><asp:TextBox ID="m_key2" runat="server" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>账号类型：</td>
                    <td>
                         <asp:DropDownList ID="m_type" runat="server" style="height:30px;width:155px;"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&ensp;</td>
                    <td>
                        <asp:Button ID="Button5" runat="server" Height="30px" Text="提交" Width="155px" 
                            onclick="onAddAccount" />
                    </td>
                </tr>
            </table>
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>

        <div>
            <h2>当前账号列表</h2>
            <br />
            <asp:Table ID="m_curAccount" runat="server" class="cTable">
            </asp:Table>
        </div>
    </div>
</asp:Content>
