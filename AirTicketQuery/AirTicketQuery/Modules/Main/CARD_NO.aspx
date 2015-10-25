<%@ Page Title="CARD NO LIST" Language="C#" MasterPageFile="~/Modules/Common/Main.Master" AutoEventWireup="true" CodeBehind="CARD_NO.aspx.cs" Inherits="AirTicketQuery.Modules.Main.CARD_NO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../resources/CSS/jquery.autocomplete.css" rel="stylesheet" />

    <script type="text/javascript" src="../../resources/JS/jquery.autocomplete.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainCPH" runat="server">
    <h3>CARD NO List</h3>
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
        </div>
        <div id="shMenu" style="display: none">
            <div data-options="name:'C_ID'">CARD NO</div>
            <div data-options="name:'C_EQUIPMENT'">EQUIPMENT</div>
            <div data-options="name:'C_SERIALNO'">SERIALNO</div>
            <div data-options="name:'C_DEPARTMENT'">DEPARTMENT</div>
            <div data-options="name:'C_VENDOR'">VENDOR</div>
            <div data-options="name:'C_TYPE'">TYPE</div>
            <div data-options="name:'C_BUY_MAINTENANCE'">BUY MAINTENANCE</div>
            <div data-options="name:'C_WARRANTY'">WARRANTY</div>
        </div>
    </div>

    <div id="pnlMain" class="easyui-panel" style="width: 700px; display: none;" data-options="title:'New',closed:true">
        <form id="frmMain" method="post" action="#">
        <input id="id_C_ID" name="C_ID" type="hidden" value="-1" />
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <label for='id_C_CATNO'>CATNO</label>
                </td>
                <td>
                    <input id='id_C_CATNO' name='C_CATNO' class='RequireBox' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_SERIALNO'>SERIALNO</label>
                </td>
                <td>
                    <input id='id_C_SERIALNO' name='C_SERIALNO' class='RequireBox' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='ddlC_EQUIPMENT'>EQUIPMENT</label>
                </td>
                <td>
                    <input class="easyui-combobox" id="ddlC_EQUIPMENT" name="C_EQUIPMENT" style="width: 250px" />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_ITD_REF'>ITD_REF</label>
                </td>
                <td>
                    <input id='id_C_ITD_REF' name='C_ITD_REF' class='RequireBox' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_DEPARTMENT'>DEPARTMENT</label>
                </td>
                <td>
                    <select id="id_C_DEPARTMENT" class="easyui-combobox RequireBox" name="C_DEPARTMENT" style="width: 100px;">
                        <option>BOP</option>
                        <option>CBD</option>
                        <option>CCD</option>
                        <option>FCB</option>
                        <option>FCK</option>
                        <option>HKS</option>
                        <option>HLU</option>
                        <option>HPS</option>
                        <option>HSV</option>
                        <option>IHB</option>
                        <option>KTV</option>
                        <option>MBR</option>
                        <option>REM</option>
                        <option>SEC</option>
                        <option>SHO</option>
                        <option>SKB</option>
                        <option>SME</option>
                        <option>SWH</option>
                        <option>TSB</option>
                        <option>TSE</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_VENDOR'>VENDOR</label>
                </td>
                <td colspan="3">
                    <input class="easyui-combobox" id="ddlC_VENDOR" name="C_VENDOR" style="width: 300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_DN_NO'>DN_NO</label>
                </td>
                <td>
                    <input id='id_C_DN_NO' name='C_DN_NO' class='RequireBox' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_WARRANTY'>WARRANTY</label>
                </td>
                <td>
                    <input id='id_C_WARRANTY' name='C_WARRANTY' class='RequireBox' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_START_DATE'>START_DATE</label>
                </td>
                <td>
                    <input id='id_C_START_DATE' name='C_START_DATE' class='DatetimeOpt' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_END_DATE'>END_DATE</label>
                </td>
                <td>
                    <input id='id_C_END_DATE' name='C_END_DATE' class='DatetimeOpt' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_TYPE'>TYPE</label>
                </td>
                <td>
                    <select id="id_C_TYPE" class="easyui-combobox RequireBox" name="C_TYPE" style="width: 100px;">
                        <option>HW</option>
                        <option>SW</option>
                    </select>
                </td>
                <td>
                    <label for='id_C_EQUIPMENT_COST'>EQUIPMENT_COST</label>
                </td>
                <td>
                    <input id='id_C_EQUIPMENT_COST' name='C_EQUIPMENT_COST' class='NumberClass2' style='width: 100px' />
                </td>
            </tr>
            <tr>
                <td>
                    <label for='id_C_DELIVERY_DATE'>DELIVERY_DATE</label>
                </td>
                <td>
                    <input id='id_C_DELIVERY_DATE' name='C_DELIVERY_DATE' class='DatetimeOpt' style='width: 100px' />
                </td>
                <td>
                    <label for='id_C_BUY_MAINTENANCE'>BUY_MAINTENANCE</label>
                </td>
                <td>
                    <select id="id_C_BUY_MAINTENANCE" class="easyui-combobox RequireBox" name="C_BUY_MAINTENANCE" style="width: 100px;">
                        <option>Y</option>
                        <option>N</option>
                        <option>OR</option>
                    </select>
                </td>
            </tr>
        </table>
        </form>
        <div id="dlg-buttons" style="padding-left: 100px; text-align: left;">
            <a href="#" id="btnOK" class="easyui-linkbutton"><i class="fa fa-check-circle greenIcon">&nbsp;</i>OK</a>
            <a href="#" id="btnCancel" class="easyui-linkbutton"><i class="fa fa-times-circle redIcon">&nbsp;</i>Cancel</a>
        </div>
    </div>

    <script type="text/javascript" src="../../resources/JS/SHMA.CARD_NO.js"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphJS" runat="server"></asp:Content>
