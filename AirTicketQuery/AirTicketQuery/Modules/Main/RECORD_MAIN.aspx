<%@ Page Title="RECORD SUMMARY" Language="C#" MasterPageFile="~/Modules/Common/Main.Master" AutoEventWireup="true" CodeBehind="RECORD_MAIN.aspx.cs" Inherits="AirTicketQuery.Modules.Main.RECORD_MAIN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../resources/CSS/jquery.autocomplete.css" rel="stylesheet" />

    <script type="text/javascript" src="../../resources/JS/jquery.autocomplete.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainCPH" runat="server">
    <h3>RECORD SUMMARY</h3>
    <div id="pnlResult" class="easyui-panel" style="width: 100%;" data-options="noheader:true,border:false">
        <%--DataTable--%>
        <table id="grid" width="98%"></table>
        <%--toolbar--%>
        <div id="toolbar" style="display: none; padding: 5px">
            <a id="lnkNew" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-plus blueIcon">&nbsp;</i>New</a>
            <a id="lnkClone" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-copy blueIcon">&nbsp;</i>Clone</a>
            <a id="lnkEdit" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-pencil greenIcon">&nbsp;</i>Edit</a>
            <a id="lnkDelete" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-minus redIcon">&nbsp;</i>Delete</a>
            <input id="searchBox" style="width: 300px" />
            Category:<input class="easyui-combobox" id="ddlC_CATEGORY_SEARCH" name="C_CATEGORY" style="width: 200px" />
            <a id="lnkSearch" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-search blueIcon">&nbsp;</i>search</a>
            <input type="checkbox" id="ckbDetail" name="ShowDetail" value="0" />ShowDetail        
        </div>
        <div id="shMenu" style="display: none">
            <div data-options="name:'C_ITD_REF'">ITD REF</div>
            <div data-options="name:'C_CONTACT_PERSON'">CONTACT PERSON</div>
        </div>
    </div>

    <div id="pnlMain" class="easyui-panel" style="width: 700px; display: none;" data-options="title:'New',closed:true">
        <form id="frmMain" method="post" action="#">
        <input id="id_C_ID" name="C_ID" type="hidden" value="-1" />
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <label for='id_C_ITD_REF'>ITD REF</label>
                </td>
                <td>
                    <input id='id_C_ITD_REF' name='C_ITD_REF' class='RequireBox' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_CATEGORY'>CATEGORY</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_CATEGORY" name="C_CATEGORY" style="width: 300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_SUPPLIER'>SUPPLIER</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_SUPPLIER" name="C_SUPPLIER" style="width: 300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_AGREEMENTNO'>AGREEMENTNO</label>
                </td>
                <td>
                    <input id='id_C_AGREEMENTNO' name='C_AGREEMENTNO' style='width: 300px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_MAINTENANCE_SERVICE_CONTENT'>MAINTENANCE SERVICE CONTENT</label>
                </td>
                <td>
                    <input id='id_C_MAINTENANCE_SERVICE_CONTENT' name='C_MAINTENANCE_SERVICE_CONTENT' style='width: 300px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_MAINTENANCE_COST_CCY'>CURRENCY</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_MAINTENANCE_COST_CCY" name="C_MAINTENANCE_COST_CCY" style="width: 300px" />
                </td>
            </tr>

            <tr>
                <td>
                    <label for='id_C_MAINTENANCE_COST'>MAINTENANCE COST</label>
                </td>
                <td>
                    <input id='id_C_MAINTENANCE_COST' name='C_MAINTENANCE_COST' class='NumberClass2' style='width: 300px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_HYPERLINK'>HYPERLINK</label>
                </td>
                <td>
                    <input id='id_C_HYPERLINK' name='C_HYPERLINK' style='width: 300px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_REMARK'>REMARK</label>
                </td>
                <td>
                    <input id='id_C_REMARK' name='C_REMARK' style='width: 300px' />
                </td>
            </tr>
        </table>
        </form>
        <div id="dlg-buttons" style="padding-left: 100px; text-align: left;">
            <a href="#" id="btnOK" class="easyui-linkbutton"><i class="fa fa-check-circle greenIcon">&nbsp;</i>OK</a>
            <a href="#" id="btnCancel" class="easyui-linkbutton"><i class="fa fa-times-circle redIcon">&nbsp;</i>Cancel</a>
        </div>
    </div>

    <script type="text/javascript" src="../../resources/JS/SHMA.RECORD_MAIN.js"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphJS" runat="server"></asp:Content>
