<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceExchange.aspx.cs" Inherits="WebManager.appaspx.service.ServiceExchange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="text-align:center;width:90%">
        <h2 style="text-align:center;padding:20px;">兑换管理</h2>
        兑换类型：&nbsp;&nbsp;<asp:DropDownList ID="m_type" runat="server" style="width:180px;height:35px"></asp:DropDownList>&ensp;&ensp;
        兑换状态：&nbsp;&nbsp;<asp:DropDownList ID="m_filter" runat="server" style="width:180px;height:35px"></asp:DropDownList>&ensp;&ensp;
        玩家ID：&nbsp;&nbsp;<asp:TextBox ID="m_param" runat="server" style="width:180px;height:35px"></asp:TextBox>&ensp;&ensp;<br/>
        <br />
        &nbsp;&nbsp;&nbsp;渠道：&nbsp;&nbsp;<asp:DropDownList ID="m_channel" runat="server" style="width:180px;height:35px"></asp:DropDownList>&ensp;&ensp;
        绑定手机：&nbsp;&nbsp;<asp:TextBox ID="m_phone" runat="server" style="width:180px;height:35px"></asp:TextBox>&ensp;&ensp;
        &nbsp;&ensp;&ensp;&ensp;&ensp;&ensp;<asp:Button ID="Button3" runat="server" onclick="onSearch" Text="查询" CssClass="btn btn-primary" style="width:140px" />
        &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;
        <br /><br />
        <asp:Table ID="GiftTable" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <br />
        <asp:Button ID="m_btnActive" runat="server" onclick="onActivateGift" Text="激活"  CssClass="btn btn-primary btn-lg" />
        <span id="m_page" style="text-align:center;display:block" runat="server"></span>
        <br />
        <span id="m_foot" style="font-size:x-large;text-align:center;display:block" runat="server"></span>
    </div>
</asp:Content>
