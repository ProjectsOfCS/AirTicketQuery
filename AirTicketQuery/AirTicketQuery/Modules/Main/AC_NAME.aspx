<%@ Page Title="AC NAME" Language="C#" MasterPageFile="~/Modules/Common/Main.Master" AutoEventWireup="true" CodeBehind="AC_NAME.aspx.cs" Inherits="AirTicketQuery.Modules.Main.AC_NAME" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainCPH" runat="server">
    <h3>AC NAME List</h3>
    <%--DataTable--%>
    <table id="grid" width="90%"></table>
    <%--toolbar--%>
    <div id="toolbar" style="display: none; padding: 5px">
        <div id="dlgAdd">
            <form id="frmAdd" method="post" action="#">
            <input id="txtC_ID" name="C_ID" type="hidden" value="-1" />
            <table>
                <tr>
                    <td>Name</td>
                    <td>
                        <input id="txtC_NAME" name="C_NAME" type="text" style="width: 300px" />
                    </td>
                    <td>ShortName</td>
                    <td>
                        <input id="txtC_SHORTNAME" name="C_SHORTNAME" type="text" style="width: 120px" />
                        <a href="#" id="btnNew" class="easyui-linkbutton"><i class="fa fa-check-circle greenIcon">&nbsp;</i>OK</a>
                    </td>
                </tr>
            </table>
            </form>
        </div>
        <a id="lnkEdit" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-pencil greenIcon">&nbsp;</i>Edit</a>
        <a id="lnkDelete" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-minus redIcon">&nbsp;</i>Delete</a>
        <a id="lnkRefresh" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-refresh blueIcon">&nbsp;</i>Refresh</a>
        <input id="searchBox" style="width: 300px" />
    </div>
    <div id="shMenu" style="display: none">
        <div data-options="name:'C_NAME'">Name</div>
        <div data-options="name:'C_SHORTNAME'">ShortName</div>
    </div>

    <div id="dlgEdit" style="display: none; padding-top: 10px;">
        <form id="frmEdit" method="post" action="#">
        <input id="txtC_ID_EDIT" name="C_ID" type="hidden" value="-1" />
        <table>
            <tr>
                <td>Name</td>
                <td>
                    <input id="txtC_NAME_EDIT" name="C_NAME" type="text" style="width: 300px" />
                </td>
            </tr>
            <tr>
                <td>ShortName</td>
                <td>
                    <input id="txtC_SHORTNAME_EDIT" name="C_SHORTNAME" type="text" style="width: 120px" />
                </td>
            </tr>
        </table>
        </form>
    </div>
    <div id="dlg-buttons" style="display: none">
        <a href="#" id="btnOK" class="easyui-linkbutton"><i class="fa fa-check-circle greenIcon">&nbsp;</i>OK</a>
        <a href="#" id="btnCancel" class="easyui-linkbutton"><i class="fa fa-times-circle redIcon">&nbsp;</i>Cancel</a>
    </div>

    <script type="text/javascript" src="../../resources/JS/SHMA.AC_NAME.js"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphJS" runat="server"></asp:Content>
