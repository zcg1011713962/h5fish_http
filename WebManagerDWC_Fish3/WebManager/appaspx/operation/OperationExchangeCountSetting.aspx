<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationExchangeCountSetting.aspx.cs" Inherits="WebManager.appaspx.operation.OperationExchangeCountSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/module/sea.js"></script>
    <script type="text/javascript">
        seajs.use("../../Scripts/operation/OperationExchangeSetting.js?ver=1");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divTemplate" style="display:none">
        <div class="row" >
            <div class="form-inline">
                <label class="col-sm-2" for="{0}">{1}:</label>
                <input id="{0}" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="{0}" value="修改" />
                <span id="info_{0}"></span>
            </div>
        </div>
    </div>

    <h2 style="padding:20px;text-align:center">兑换数量设置</h2>
    <div id="container" class="container" style="padding:20px;width:800px">

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="1">30元话费:</label>
                <input id="1" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="1" value="修改" />
                <span id="info_1"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="2">50元话费:</label>
                <input id="2" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="2" value="修改" />
                <span id="info_2"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="3">100元京东卡:</label>
                <input id="3" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="3" value="修改" />
                <span id="info_3"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="4">青铜鱼雷x3:</label>
                <input id="4" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="4" value="修改" />
                <span id="info_4"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="5">金币x300000:</label>
                <input id="5" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="5" value="修改" />
                <span id="info_5"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="6">钻石x150:</label>
                <input id="6" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="6" value="修改" />
                <span id="info_6"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="7">白银鱼雷x3:</label>
                <input id="7" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="7" value="修改" />
                <span id="info_7"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="8">黄金鱼雷x2:</label>
                <input id="8" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="8" value="修改" />
                <span id="info_8"></span>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="form-inline">
                <label class="col-sm-2" for="9">钻石鱼雷:</label>
                <input id="9" type="text" class="form-control" placeholder="" />
                <input type="button" class="btn btn-primary" data="9" value="修改" />
                <span id="info_9"></span>
            </div>
        </div>
    </div>
</asp:Content>
