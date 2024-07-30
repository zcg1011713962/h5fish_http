<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="OperationNotify.aspx.cs" Inherits="WebManager.appaspx.operation.OperationNotify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .cSafeWidth{
            text-align:center;
        }
        .cSafeWidth td{text-align:left;padding:6px;}
    </style>
    <script src="../../Scripts/module/sea.js"></script>
    <script src="/Scripts/ckeditor/ckeditor.js"></script>
    <script src="../../Scripts/tool/jquery.base64.js"></script>
    <script type="text/javascript">
        $(function () {
            CKEDITOR.replace('m_content', {
                height: 800,
                removePlugins: 'elementspath',
            });

            //var content = CKEDITOR.instances.MainContent_m_content.getData();
            //console.log(content);
            //alert(content);
            seajs.use("../../Scripts/operation/OperationNotify.js?ver=2");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container form-horizontal">
        <h2 style="text-align:center;padding-bottom:20px;">运营公告</h2>
       <%-- <table>
            <tr>
                <td>id:</td>
                <td><asp:TextBox ID="m_noticeId" runat="server" style="width:360px" ></asp:TextBox>
                    id为空时，表示增加公告，不为空表示修改公告
                </td>
            </tr>
            <tr>
                <td>标题：</td>
                <td><asp:TextBox ID="m_title" runat="server" style="width:360px" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>内容：</td>
                <td><asp:TextBox ID="m_content" runat="server"
                TextMode="MultiLine" Wrap="False" style="height:800px;width:1200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>显示天数：</td>
                <td><asp:TextBox ID="m_day" runat="server" style="width:340px" ></asp:TextBox></td>
            </tr>
            <tr>
                <td>排序字段：</td>
                <td><asp:TextBox ID="m_order" runat="server" style="width:340px" ></asp:TextBox>数字越小，越排前面，默认值为0</td>
            </tr>
            <tr>
                <td> 备注(可填写本次操作原因)：</td>
                <td> <asp:TextBox ID="m_comment" runat="server" style="width:340px" ></asp:TextBox></td>
            </tr>
            <tr>
                <td> <asp:Button ID="Button1" runat="server" onclick="onPublishNotice" 
                Text="发布公告" style="width:134px;height:28px" /></td>
                <td><span id="m_res" style="font-size:medium;color:red" runat="server"></span></td>
            </tr>
        </table> --%>  
        
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">id:</label>
            <div class="col-sm-10">
                <input type="text" id="m_noticeId"  class="form-control" style="width:50%;display:inline-block" />
                <label>&ensp;id为空时，表示增加公告，不为空表示修改公告</label>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">标题：</label>
            <div class="col-sm-10">
                <input type="text" id="m_title"  class="form-control" style="width:50%;display:inline-block"/>
                <label>&ensp;标题最大长度为6</label>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">内容：</label>
            <div class="col-sm-10">
                <textarea  id="m_content"  class="form-control"  style="height:800px;" ></textarea>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">开始日期：</label>
            <div class="col-sm-10">
                <input type="text" id="m_timeStart"  class="form-control" style="width:40%;display:inline-block"/>
                <label>&ensp;输入格式为：2017/08/08 08:08 或 2017/08/08（即为2017/08/08 00:00）</label>
            </div>
        </div>  
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">结束日期：</label>
            <div class="col-sm-10">
                <input type="text" id="m_timeEnd" class="form-control" style="width:40%;display:inline-block"/>
                <label>&ensp;输入格式为：2017/08/08 08:08 或 2017/08/08（即为2017/08/09 00:00） </label>
            </div>
        </div>  
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">排序字段:</label>
            <div class="col-sm-10">
                <input type="text" id="m_order" class="form-control" style="width:50%;display:inline-block" />
                <label>&ensp;数字越小，越排前面，默认值为0</label>
            </div>
        </div> 
        <div class="form-group">
            <label for="account" class="col-sm-2 control-label">备注(可填写本次操作原因):</label>
            <div class="col-sm-10">
                <input type="text" id="m_comment" class="form-control" />
            </div>
        </div>
         <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                 <input type="button" id="btn_publish" value="发布公告" class="btn btn-primary form-control" />
            </div>
        </div>
        <div class="col-sm-offset-2 col-sm-10">
            <span id="m_res" style="font-size:medium;color:red"></span>
        </div>         
    </div>

    <div class="container-fluid">
        <asp:Table ID="m_result" runat="server" CssClass="table table-hover table-bordered">
        </asp:Table>
        <asp:Button ID="Button3" runat="server" onclick="onCancelNotice" Text="撤消"  CssClass="btn btn-primary form-control" />
    </div>
</asp:Content>
