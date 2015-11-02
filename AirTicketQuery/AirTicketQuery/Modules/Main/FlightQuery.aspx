<%@ Page Title="Ticket Query" Language="C#" MasterPageFile="~/Modules/Common/Main.Master"
    AutoEventWireup="true" CodeBehind="FlightQuery.aspx.cs" Inherits="AirTicketQuery.Modules.Main.FlightQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainCPH" runat="server">
    <h3>Ticket Query</h3>
    <div id="pnlResult" class="easyui-panel" style="width: 100%;" data-options="noheader:true,border:false">
        <%--DataTable--%>
        <table id="grid" width="98%"></table>
        <%--toolbar--%>
        <div id="toolbar" style="display: none; padding: 5px">
            From:<input class="easyui-combobox" id="ddlFromCity" name="C_FROM" style="width: 100px" />
            To:<input class="easyui-combobox" id="ddlToCity" name="C_TO" style="width: 100px" />
            Departure:<input id="txtC_Departure" type="text" class="easyui-datebox" required="required"
                style="width: 100px" data-options="formatter:DateFormatter,parser:DateParser">
            <a id="lnkSearch" href="javascript:void(0)" class="easyui-linkbutton"><i class="fa fa-search blueIcon">
                &nbsp;</i>search</a>
            <br />
            DataSource:
            <input type="checkbox" id="ckbQUNAR" name="QUNAR" value="0" />QUNAR&nbsp;
            <input type="checkbox" id="ckbCEAIR" name="CEAIR" value="0" />CEAIR&nbsp;
            <input type="checkbox" id="ckbCSAIR" name="CSAIR" value="0" />CSAIR&nbsp;
            <input type="checkbox" id="ckbCTRIP" name="CTRIP" value="0" />CTRIP&nbsp;
            <input type="checkbox" id="ckbWS" name="webxml" value="0" />webxml&nbsp;
        </div>
    </div>

    <script type="text/javascript" src="../../resources/JS/ATQ.FlightQuery.js"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphJS" runat="server">
</asp:Content>
