﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CowsCardType.aspx.cs" Inherits="WebManager.appaspx.stat.cows.CowsCardType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cSafeWidth" style="text-align:center">
        <div style="border:1px solid black;padding:5px;">
            <h2 style="padding:20px;">牛牛牌型设置</h2>
            <p>
                &nbsp;&nbsp;&nbsp;&nbsp;
                庄家牌型：<asp:DropDownList ID="m_banker" runat="server" class="cDropDownList"
                    ></asp:DropDownList>
            </p>
        
            <div>
                闲家牌型-东：<asp:DropDownList ID="m_other1" runat="server" class="cDropDownList"></asp:DropDownList>
                <br />  <br />
                闲家牌型-南：<asp:DropDownList ID="m_other2" runat="server" class="cDropDownList"></asp:DropDownList>
                <br />  <br />
                闲家牌型-西：<asp:DropDownList ID="m_other3" runat="server" class="cDropDownList"></asp:DropDownList>
                <br />  <br />
                闲家牌型-北：<asp:DropDownList ID="m_other4" runat="server" class="cDropDownList"></asp:DropDownList>
            </div>
            <br />
            <asp:Button ID="Button2" runat="server" Text="增加" onclick="onAddCardType" CssClass="cButton"/>
            <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
        </div>
        <br />
        <div style="border:1px solid black;margin-top:10px;padding:5px;">
            <h2>牌型列表</h2>
            <asp:Table ID="m_allCards" runat="server" CssClass="cTable">
            </asp:Table>
            <asp:Button ID="Button1" runat="server" Text="删除" onclick="onDelCard" CssClass="cButton"/>
        </div>
    </div>
</asp:Content>
