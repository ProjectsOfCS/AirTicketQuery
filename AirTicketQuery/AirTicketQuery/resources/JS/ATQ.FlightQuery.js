/// <reference path="jquery.min.js" />
/// <reference path="_MyFunction.js" />
/// <reference path="jquery.easyui.min.js" />
var grid = $("#grid");
$(function () {
    //init DataTable plugin
    grid.datagrid({
        url: ParseUrl("Ajax/Controller.asmx/Flight_Get"),
        toolbar: "#toolbar",
        pagination: true,
        pageSize: 20,
        pageList: [10, 20, 50, 100, 200],
        beforePageText: '',
        afterPageText: '/{pages}',
        displayMsg: '{from} - {to}({total})',
        columns: [[
                { field: 'chk', checkbox: true },
                { field: 'C_From', title: '出发城市', resizable: true, sortable: true, width: 65 },
                { field: 'C_To', title: '到达城市', resizable: true, sortable: true, width: 65 },
                {
                    field: 'C_Departure', title: '出发日期', resizable: true, sortable: true, width: 75,
                    formatter: function (value, row, index) {
                        if (typeof value !== "undefined" && value.length > 10) {
                            return value.substr(0, 10);
                        } else {
                            return value;
                        }
                    }
                },
                { field: 'C_DateSource', title: '数据来源网站', sortable: true, resizable: true, width: 85 },
                { field: 'C_Airline', title: '航空公司', sortable: true, resizable: true, width: 60 },
                { field: 'C_FlightNo', title: '航班编号', sortable: true, resizable: true, width: 65 },
                { field: 'C_DEPTIME', title: '起飞时间', sortable: true, resizable: true, width: 55 },
                { field: 'C_ARRTIME', title: '到达时间', sortable: true, resizable: true, width: 55 },
                { field: 'C_TotalTime', title: '航程时长', sortable: true, resizable: true, width: 100 },
                { field: 'C_FirstClass', title: '头等舱', sortable: true, align: 'right', resizable: true, width: 65 },
                { field: 'C_Business', title: '公务舱', sortable: true, align: 'right', resizable: true, width: 65 },
                { field: 'C_Economy', title: '经济舱', sortable: true, align: 'right', resizable: true, width: 65 },
                { field: 'C_Price', title: '价格', sortable: true, align: 'right', resizable: true, width: 65 },
                {
                    field: 'C_ADD_TIME', title: '搜集时间', sortable: true, resizable: true, width: 60,
                    formatter: function (value, row, index) {
                        if (typeof value !== "undefined"&&  value.length > 10) {
                            return value.substr(11,5);
                        } else {
                            return value;
                        }
                    }
                },
                { field: 'C_Remark', title: '其他信息', sortable: true, resizable: true, width: 500 },
        ]],
        onLoadError: function (jqXHR, textStatus, errorThrown) { ShowMsg(0, jqXHR.responseText); },
        onLoadSuccess: function (json) { ShowMsgJson(json); }
    });

    IntiComboCity($('#ddlFromCity'), true);
    IntiComboCity($('#ddlToCity'), true);
    $("#lnkSearch").click(CallSearch);

    function CallSearch() {
        var fromCity = $('#ddlFromCity').combobox('getValue');
        var toCity = $('#ddlToCity').combobox('getValue');
        var departDate = $('#txtC_Departure').datebox("getValue");
        var varQUNAR = $("#ckbQUNAR")[0].checked ? 1 : 0;
        var varCEAIR = $("#ckbCEAIR")[0].checked ? 1 : 0;
        var varCSAIR = $("#ckbCSAIR")[0].checked ? 1 : 0;
        var varCTRIP = $("#ckbCTRIP")[0].checked ? 1 : 0;
        var varWS = $("#ckbWS")[0].checked ? 1 : 0;

        grid.datagrid('load', { fromCity: fromCity, toCity: toCity, Departure: departDate, QUNAR: varQUNAR, CEAIR: varCEAIR, CSAIR: varCSAIR, CTRIP: varCTRIP, WS: varWS });
    }

    var currDate = new Date();
    currDate.setDate(currDate.getDate() + 10);
    $('#txtC_Departure').datebox('setValue', DateFormatter(currDate));
    $('#ddlFromCity').combobox('setValues', 'PEK');
    $('#ddlToCity').combobox('setValues', 'CAN');
});

function IntiComboCity(el, reqVal) {
    el.combobox({
        required: reqVal,
        panelHeight: 150,
        //editable: reqVal,
        loader: function (param, success, error) {
            $.ajax({
                type: 'POST',
                url: ParseUrl("Ajax/Controller.asmx/City_Get"),
                dataType: 'json',
                success: function (data) { success(data.rows); },
                error: function () { error.apply(this, arguments); }
            });
        },
        filter: function (q, row) {
            return row['C_NAME'].toLowerCase().indexOf(q.toLowerCase()) != -1;
        },
        valueField: 'C_CODE',
        textField: 'C_NAME'
    });
}
