<%@ Page Title="RECORD DETAIL" Language="C#" MasterPageFile="~/Modules/Common/Main.Master" AutoEventWireup="true" CodeBehind="RECORD.aspx.cs" Inherits="AirTicketQuery.Modules.Main.RECORD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainCPH" runat="server">
    <h3>RECORD DETAIL</h3>
    <div id="pnlResult" class="easyui-panel" style="width: 100%;" data-options="noheader:true,border:false">
        <%--DataTable--%>
        <table id="grid" width="98%"></table>
        <%--toolbar--%>
        <div id="toolbar" style="display: none; padding: 5px">
            <a id="lnkNew" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-plus blueIcon">&nbsp;</i>New</a>
            <a id="lnkClone" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-copy blueIcon">&nbsp;</i>Clone</a>
            <a id="lnkEdit" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-pencil greenIcon">&nbsp;</i>Edit</a>
            <a id="lnkDelete" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-minus redIcon">&nbsp;</i>Delete</a>
            Category:<input class="easyui-combobox" id="ddlC_CATEGORY_SEARCH" name="C_CATEGORY" style="width: 200px" />
            <%--<a id="lnkSearch" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-search blueIcon">&nbsp;</i>search</a>--%>
            <input id="searchBox" style="width: 300px" />
            <input type="checkbox" id="ckbDetail" name="ShowDetail" value="0" />ShowDetail
        </div>
        <div id="shMenu" style="display: none">
            <div data-options="name:'C_ITD_REF'">ITD REF</div>
            <div data-options="name:'C_CARDNO'">CARDNO</div>
        </div>
    </div>

    <div id="pnlMain" class="easyui-panel" style="width: 850px; display: none;" data-options="title:'New',closed:true">
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
                <td>
                    <label for='ddlC_AC_NAME_ID'>AC NAME ID</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_AC_NAME_ID" name="C_AC_NAME_ID" style="width: 300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_CCY_ID'>CURRENCY</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_CCY_ID" name="C_CCY_ID" style="width: 200px" />
                </td>
                <td>
                    <label for='id_C_ACTUAL_PAY'>ACTUAL PAY</label>
                </td>
                <td>
                    <input id='id_C_ACTUAL_PAY' name='C_ACTUAL_PAY' class='NumberClass2' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_CARDNO'>CARDNO</label>
                </td>
                <td colspan="3">
                   <%-- <input class="easyui-combobox" id="ddlC_CARDNO" name="C_CARDNO" style="width: 300px" />--%>
                     <input id='id_C_CARDNO' name='C_CARDNO' class='RequireBox' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_START_DATE'>START DATE</label>
                </td>
                <td>
                    <input id='id_C_START_DATE' name='C_START_DATE' class='DatetimeOpt' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_END_DATE'>END DATE</label>
                </td>
                <td>
                    <input id='id_C_END_DATE' name='C_END_DATE' class='DatetimeOpt' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_REMARK'>REMARK</label>
                </td>
                <td colspan="3">
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

    <script type="text/javascript" src="../../resources/JS/SHMA.RECORD.js"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphJS" runat="server"></asp:Content>
