﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ServiceExcelDownLoad.aspx.cs" Inherits="WebManager.appaspx.service.ExcelDownLoad" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container">
       <h2 style="text-align:center;padding:20px;">EXCEL下载</h2>
       <p>Excel表格数据在服务器保存2小时后将删除，请及时下载备份。</p>
       <p>采用目标另存为可以下载到完整文档</p>
       <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
       </asp:Table>
    </div>
</asp:Content>
