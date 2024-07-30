<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Right.aspx.cs" Inherits="WebManager.appaspx.Right" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../Scripts/RightEdit.js?ver=2");
	</script>
    <style type="text/css">
        #gmTypeList{width:200px;height:40px;font-size:18px;}
        .EditRInfo{font-size:18px;}
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2 style="text-align:center;padding:20px;">编辑权限</h2>
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="808px" style="height:auto;display:none;" >
        <asp:Table ID="m_right" runat="server">
        </asp:Table>
        <asp:Button ID="Button1" runat="server" onclick="onCommitRight" Text="提交" style="width:98px;height:40px" />
        <span id="m_res" style="font-size:medium;color:red" runat="server"></span>
    </asp:Panel>

    <div id="editRight" style="display:block;padding-top:10px;text-align:center">
        <div class="container-fluid">
            <div class="row">
                <label class="EditRInfo">在这里选择要编辑的GM账号类型：</label>
                <select id="gmTypeList"></select>
                <input type="button" id="btnEditSelAll" class="btn btn-success" style="margin-left:30px;" value="全选" />
                <input type="button" id="btnEditCancelSelAll" class="btn btn-success" value="取消全选" />
                <input type="button" id="btnSubmitOnceKey" class="btn btn-success" value="一键提交"/>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_op"></div>
                <div class="col-lg-3 col-md-4col-sm-6" id="div_fish"></div>
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_td"></div>
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_data"></div>
            </div>
            <div class="row">
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_svr"></div>
            </div>
            <div class="row">
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_player"></div>
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_fishing"></div>
                <div class="col-lg-3 col-md-4 col-sm-6" id="div_operationNew"></div>
            </div>
        </div>
    </div>
</asp:Content>
