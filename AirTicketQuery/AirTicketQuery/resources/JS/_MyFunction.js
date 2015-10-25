$.extend($.fn.validatebox.defaults.rules, {
    minLen: {
        validator: function (value, param) { return value.length >= param[0]; },
        message: 'Please enter at least {0} characters.'
    },
    maxLen: {
        validator: function (value, param) { return value.length <= param[0]; },
        message: 'Only allow {0} characters.'
    },
    maxLenNum: {
        validator: function (value, param) { return value.replace(/,/g, '').length <= param[0]; },
        message: 'Only allow {0} digits.'
    },
    equLen: {
        validator: function (value, param) { return value.length <= param[0] && value.length >= param[0]; },
        message: '{0} characters limit.'
    },
    digits: {
        validator: function (value, param) { return /^\d+$/.test(value); },
        message: "Please enter only digits."
    },
});

//Function - Date Mask (jquery.mask.min.js)
function InitDateMask(el) {
    el.mask('0000/m0/d0', {
        translation: {
            'm': { pattern: /[0-1]/ },
            'd': { pattern: /[0-3]/ }
        },
        placeholder: "YYYY/MM/DD"
    });
}

//Function - Money Mask (jquery mask plugin)
function InitMoneyMask(el) { el.mask('#,##0.00', { reverse: true }); }

//Function - Money Mask (jquery mask plugin)
function InitNumberMask(el) { el.mask('9'); }
//Function disable form Validation
function DisableFormValidation(frm) { frm.form('disableValidation'); }

//Format Number (jquery.number.min.js)
function Formatnumber(value) { return $.number(value, 2); }

//Format Percent (jquery.number.min.js)
function FormatPercent(value) { return value + '%'; }

//Function - Init Combox
function InitCombo(el, width, urlStr, par, req) {
    el.combobox({
        required: req,
        panelHeight: 'auto',
        panelWidth: width,
        editable: false,
        loader: function (param, success, error) {
            $.ajax({
                type: 'POST',
                url: urlStr,
                data: { par: par },
                dataType: 'json',
                editable: false,
                success: function (data) {
                    success(data.data);
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        },
        valueField: 'CODE',
        textField: 'NAME'
    });
}

//Function - InitDateBox
function InitDateBox(el, req) {
    el.datebox({
        required: req
    });
}

//Function - Init validatebox
function InitValidatebox(el) {
    el.validatebox({ required: true });
}

function InitValidateboxWidth(el, setWidth) {
    el.validatebox({
        required: true,
        width: setWidth
    });
}

function DateFormatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '/' + (m < 10 ? ('0' + m) : m) + '/' + (d < 10 ? ('0' + d) : d);
}

function DateParser(s) {
    if (!s) return new Date();
    var ss = (s.split('/'));
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}

//Function - Init textbox
function InitTextbox(el, req, width) {
    el.textbox({
        required: req,
        width: width
    });
}

//Init Number Box
function InitNumberbox(el, p, setWidth) {
    el.numberbox({
        precision: p,
        groupSeparator: ',',
        required: true,
        width: setWidth
    });
}

function InitNumBox(el, req, groupSeparator, precision, setWidth) {
    el.numberbox({
        required: req,
        groupSeparator: groupSeparator,
        precision: precision,
        width: setWidth
    });
}

//Function - Init Dialog Form
function InitDialog(el, top, width, height, btnDiv4Dialog) {
    el.dialog({
        top: top,
        width: width,
        height: height,
        modal: true,
        closed: true,
        maximizable: true,
        buttons: btnDiv4Dialog
    });
}

//Do Submit
function DoSubmit(frm, urlStr, params, DoValid, Dialog2Close, callReloadGrid) {
    frm.form('submit', {
        url: urlStr,
        queryParams: params,
        onSubmit: function () {
            var val = true;
            if (DoValid)
                val = frm.form('enableValidation').form('validate');

            return val;
        },
        success: function (data) {
            var json = $.parseJSON(data);
            var result = false;
            if (Dialog2Close !== null)
                result = ShowDlgJson(json);
            else
                result = ShowMsgJson(json);

            if (result) {
                if (Dialog2Close !== null)
                    CloseDialog(Dialog2Close);

                frm.form('reset');
                DisableFormValidation(frm);
                if (callReloadGrid !== null)
                    callReloadGrid();
            }
        }
    });
}

//Function - Show message
function ShowDlgJson(json) {
    if (typeof json.msg !== "undefined" && json.msg !== null && json.msg !== "")
        $.messager.alert('Info', json.msg, 'info');
    else if (typeof json.err !== "undefined" && json.err !== null && json.err !== "")
        $.messager.alert('Error', json.err, 'error');
    else if (typeof json.war !== "undefined" && json.war !== null && json.war !== "")
        $.messager.alert('Warning', json.err, 'warning');
    else
        return true;
    return false;
}

function ShowMsgJson(json) {
    var msgContent = "";
    var msgType = -1;
    if (typeof json.msg !== "undefined" && json.msg !== null && json.msg !== "") { msgType = 1; msgContent = json.msg; }
    else if (typeof json.war !== "undefined" && json.war !== null && json.war !== "") { msgType = 2; msgContent = json.war; }
    else if (typeof json.err !== "undefined" && json.err !== null && json.err !== "") { msgType = 0; msgContent = json.err; }

    if (msgType == -1 || msgContent == "") { return true; }
    else { return ShowMsg(msgType, msgContent); }
}

function ShowMsg(msgType, strMsg) {
    try {
        if (typeof msgType == "undefined" || msgType == null)
            msgType = 0;
        msgContent = strMsg;

        if (msgContent !== "") {
            msgContent = "<br />" + msgContent;
            if (msgType == 0) { $.modaldialog.error(msgContent, { timeout: 60 }); }
            else if (msgType == 1) { $.modaldialog.success(msgContent, { timeout: 2 }); }
            else if (msgType == 2) { $.modaldialog.warning(msgContent, { timeout: 4 }); }
            return false;
        } else { return true; }
    }
    catch (e) { alert(e); return false; }
}

//Function - GetSelectRow
function GetSelectedRow(grid) {
    var rowCnt, row;

    rowCnt = grid.datagrid('getRows').length;
    if (rowCnt == 0)
        return null;
    else {
        if (grid.datagrid('getSelected') == null)
            grid.datagrid('selectRow', 0);
        row = grid.datagrid('getSelected');
    }

    return row;
}

//Function - CloseDialog
function CloseDialog(dg) {
    dg.dialog('close');
}

//Function - ShowDialog
function ShowDialog(dg, title) {
    dg.dialog('setTitle', title);
    dg.show();
    dg.dialog('open');
    //dg.attr('tabIndex', '-1').bind('keydown', function (e) {
    //    if (e.keyCode == 27){dg.dialog('close');}
    //});
}

//Get Selected Data For edting
function LoadEditData(dlgEdit, frm, urlStr, parms) {
    $.ajax({
        type: 'POST',
        data: parms,
        url: urlStr
    }).done(function (json) {
        if (ShowMsgJson(json)) {
            frm.form('load', json.data[0]);
            ShowDialog(dlgEdit, 'Edit');
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        ShowMsg(0, jqXHR.responseText);
    });
}

function EnterToSubmit(conObj, submitfn) {
    /// <param name="conObj" type="container">container object</param>
    /// <param name="submitfn" type="function">sumbit funcation name</param>
    //var inputs = conObj.find("input[type=text]").not(':button,:hidden');
    var inputs = conObj.find("input[type=text]");
    inputs.each(function () {
        $(this).keydown(function (e) {
            var kc = e.which ? e.which : e.keyCode;
            if (kc == 40 || kc == 38 || kc == 13) {
                if (kc == 13) {
                    if (submitfn !== null)
                        submitfn();
                    return true;
                }

                var maxIndex = inputs.length - 1;
                var currIndex = inputs.index(this);

                if (currIndex == maxIndex && kc == 40) {
                    if (submitfn !== null)
                        submitfn();
                    return true;
                }

                var iterator = 1;
                if (kc == 38) iterator = -1;
                var nextIndex = currIndex + iterator;
                while (true) {
                    var nextInput = $(inputs[nextIndex]);
                    if (nextInput !== null) {
                        var ishide = false;
                        if (nextInput.attr("style") !== undefined && nextInput.attr("style").toLowerCase().indexOf("display: none") > -1)
                            ishide = true;
                        if (ishide) {
                            nextIndex = nextIndex + iterator;
                        } else { break; }
                    }
                    else { break; }
                }

                if (nextIndex > -1) {
                    $(inputs[nextIndex]).focus();
                }

                return false;
            }
            else { return true; }
        });
    });
}
function EnterToTab(conObj, submitfn) {
    /// <param name="conObj" type="container">container object</param>
    /// <param name="submitfn" type="function">sumbit funcation name</param>
    //var inputs = conObj.find("input[type=text]").not(':button,:hidden');
    var inputs = conObj.find("input[type=text]");
    inputs.each(function () {
        $(this).keydown(function (e) {
            var kc = e.which ? e.which : e.keyCode;
            if (kc == 13) {
                var maxIndex = inputs.length - 1;
                var currIndex = inputs.index(this);

                if (currIndex == maxIndex && kc == 13) {
                    if (submitfn !== null)
                        submitfn();
                    return false;
                }

                var iterator = 1;                
                var nextIndex = currIndex + iterator;
                while (true) {
                    var nextInput = $(inputs[nextIndex]);
                    if (nextInput !== null) {
                        var ishide = false;
                        if (nextInput.attr("style") !== undefined && nextInput.attr("style").toLowerCase().indexOf("display: none") > -1)
                            ishide = true;
                        if (ishide) {
                            nextIndex = nextIndex + iterator;
                        } else { break; }
                    }
                    else { break; }
                }

                if (nextIndex > -1) {
                    $(inputs[nextIndex]).focus();
                }

                return false;
            }
            else { return true; }
        });
    });
}

function EnterArrowToTab(conObj, submitfn) {
    /// <param name="conObj" type="container">container object</param>
    /// <param name="submitfn" type="function">sumbit funcation name</param>
    //var inputs = conObj.find("input[type=text]").not(':button,:hidden');
    var inputs = conObj.find("input[type=text]");
    inputs.each(function () {
        $(this).keydown(function (e) {
            var kc = e.which ? e.which : e.keyCode;
            if (kc == 40 || kc == 38 || kc == 13) {
                var maxIndex = inputs.length - 1;
                var currIndex = inputs.index(this);

                if (currIndex == maxIndex && (kc == 40 || kc == 13)) {
                    if (submitfn !== null)
                        submitfn();
                    return true;
                }

                var iterator = 1;
                if (kc == 38) iterator = -1;
                var nextIndex = currIndex + iterator;
                while (true) {
                    var nextInput = $(inputs[nextIndex]);
                    if (nextInput !== null) {
                        var ishide = false;
                        if (nextInput.attr("style") !== undefined && nextInput.attr("style").toLowerCase().indexOf("display: none") > -1)
                            ishide = true;
                        if (ishide) {
                            nextIndex = nextIndex + iterator;
                        } else { break; }
                    }
                    else { break; }
                }

                if (nextIndex > -1) {
                    $(inputs[nextIndex]).focus();
                }

                return false;
            }
            else { return true; }
        });
    });
}

function EleExists(name) { return $("#ctl00_" + name).length > 0; }
function CtlVal(name) { if (EleExists(name)) { return $.trim($("#ctl00_" + name).val()); } else if (EleExists("ctl00_" + name)) { return $.trim($("#ctl00_ctl00_" + name).val()); } else return ""; }

function OpenNewWindowsMax(url) {
    var number = (new Date()).getSeconds();
    var availHeight = screen.availHeight - 70;
    var availWidth = screen.availWidth - 20;
    var arguments = 'height=' + availHeight + ',width=' + availWidth + ',status=yes,scrollbars=yes,toolbar=no,menubar=no,resizable=yes,location=no,top = 5, left = 10';
    window.open(url, number, arguments);
}

function OpenNewWinByAjax(title) {
    var ajaxUrl = ParseUrl("Ajax/GetEncryptURLParam.ashx");
    $.get(ajaxUrl, { "action": title }, function (data) {
        if ($.trim(data) !== "") { window.open(data); }
    });
}

function OpenNewWinMaxByAjax(title) {
    var ajaxUrl = ParseUrl("Ajax/GetEncryptURLParam.ashx");
    $.get(ajaxUrl, { "action": title }, function (data) {
        if ($.trim(data) !== "") { OpenNewWindowsMax(data); }
    });
}

function ParseUrl(strUrl) {
    var result = CtlVal("hfBaseUrl"); if (result == "") { return "/Modules/" + strUrl; }
    return result + strUrl;
}

function txt_change() { $(this).css('color', 'blue'); }

function GoToUrl(url) { window.location = url; }

function Log4JS(msg) { try { console.log(msg); } catch (e) { } }

String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};