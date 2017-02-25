if (typeof VINASIC == "undefined" || !VINASIC) {
    var VINASIC = {};
}
VINASIC.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ("" + a[i]).split(".");
        o = VINASIC;
        for (j = (d[0] === "VINASIC") ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};
VINASIC.namespace("StandardSale");
VINASIC.StandardSale = function () {
    var global = {
        UrlAction: {
            GetListStandardSale: "/StandardSale/GetStandardSales",
            SaveStandardSale: "/StandardSale/SaveStandardSale",
            DeleteStandardSale: "/StandardSale/DeleteStandardSale"
        },
        Element: {
            JtableStandardSale: "jtableStandardSale",
            PopupStandardSale: "popup_StandardSale",
            PopupSearch: "popup_SearchStandardSale"
        },
        Data: {
            ModelStandardSale: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListStandardSale() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableStandardSale).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(standardSale) {
        var standardSaleViewModel = {
            Id: 0,
            BaseSalary: "",
            Sales: "",
            Percent: "",
            Bonus: "",
            IncomeTotal: ""
        };
        if (standardSale != null) {
            standardSaleViewModel = {
                Id: ko.observable(standardSale.Id),
                BaseSalary: ko.observable(standardSale.BaseSalary),
                Sales: ko.observable(standardSale.Sales),
                Percent: ko.observable(standardSale.Percent),
                Bonus: ko.observable(standardSale.Bonus),
                IncomeTotal: ko.observable(standardSale.IncomeTotal)
            };
        }
        return standardSaleViewModel;
    }
    function bindData(standardSale) {
        global.Data.ModelStandardSale = initViewModel(standardSale);
        ko.applyBindings(global.Data.ModelStandardSale);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupStandardSale() {
        $("#" + global.Element.PopupStandardSale).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteStandardSale,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListStandardSale();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupStandardSale, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListStandardSale() {
        $("#" + global.Element.JtableStandardSale).jtable({
            title: "Doanh Số Chuẩn",
            paging: false,
            pageSize: 100,
            pageSizeChangeStandardSale: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListStandardSale,
                createAction: global.Element.PopupStandardSale,
                createObjDefault: initViewModel(null)
            },
            messages: {
                addNewRecord: "Thêm mới"
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                strBaseSalary: {
                    visibility: "fixed",
                    title: "Lương Cơ bản",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.strBaseSalary + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupStandardSale();
                        });
                        return text;
                    }
                },
                strSales: {
                    title: "Doanh Số",
                    width: "25%"
                },
                Percent: {
                    title: "%",
                    width: "10%"
                },
                strBonus: {
                    title: "Hoa Hồng",
                    width: "15%"
                },
                strIncomeTotal: {
                    title: "Tổng Thu Nhập",
                    width: "15%"
                },
                Delete: {
                    title: "Xóa",
                    width: "5%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog("Bạn có chắc chắn muốn xóa?", function () {
                                deleteRow(data.record.Id);
                                var realTimeHub = $.connection.realTimeJTableDemoHub;
                                realTimeHub.server.sendUpdateEvent("jtableStandardSale");
                                $.connection.hub.start();
                            }, function () { }, "Đồng ý", "Hủy bỏ", "Thông báo");
                        });
                        return text;
                    }
                }
            }
        });
    }
    /*End init List */

    /*function Check Validate */
    function checkValidate() {
        if ($('#Name').val().trim() === "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên Ngân Hàng.", function () { }, "Lỗi Nhập liệu");
            $("#Name").focus();
            return false;
        }
        return true;
    }
    /*End Check Validate */

    /*function Save */
    function saveStandardSale() {
        global.Data.ModelStandardSale.BaseSalary = $('#BaseSalary').val().replace(/[^0-9-.]/g, '');
        global.Data.ModelStandardSale.Sales = $('#Sales').val().replace(/[^0-9-.]/g, '');
        global.Data.ModelStandardSale.Percent = $('#Percent').val().replace(/[^0-9-.]/g, '');
        global.Data.ModelStandardSale.Bonus = $('#Bonus').val().replace(/[^0-9-.]/g, '');
        global.Data.ModelStandardSale.IncomeTotal = $('#IncomeTotal').val().replace(/[^0-9-.]/g, '');
        $.ajax({
            url: global.UrlAction.SaveStandardSale,
            type: 'post',
            data: ko.toJSON(global.Data.ModelStandardSale),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListStandardSale();
                        $("#" + global.Element.PopupStandardSale).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupStandardSale, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupStandardSale() {
        $("#" + global.Element.PopupStandardSale).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupStandardSale + " button[save]").click(function () {
            saveStandardSale();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableStandardSale", global.Data.ClientId, "Cập nhật");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupStandardSale + " button[cancel]").click(function () {
            $("#" + global.Element.PopupStandardSale).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListStandardSale = function () {
        reloadListStandardSale();
    };
    this.initViewModel = function (standardSale) {
        initViewModel(standardSale);
    };
    this.bindData = function (standardSale) {
        bindData(standardSale);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
        $("#BaseSalary").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#BaseSalary").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#Sales").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#Sales").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#Bonus").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#Bonus").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#IncomeTotal").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#IncomeTotal").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
    };
    this.Init = function () {
        registerEvent();
        initListStandardSale();
        reloadListStandardSale();
        initPopupStandardSale();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var standardSale = new VINASIC.StandardSale();
    standardSale.Init();
});