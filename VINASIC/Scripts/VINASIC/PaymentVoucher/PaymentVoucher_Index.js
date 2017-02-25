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
VINASIC.namespace("PaymentVoucher");
VINASIC.PaymentVoucher = function () {
    var global = {
        UrlAction: {
            GetListPaymentVoucher: "/PaymentVoucher/GetPaymentVouchers",
            SavePaymentVoucher: "/PaymentVoucher/SavePaymentVoucher",
            DeletePaymentVoucher: "/PaymentVoucher/DeletePaymentVoucher"
        },
        Element: {
            JtablePaymentVoucher: "jtablePaymentVoucher",
            PopupPaymentVoucher: "popup_PaymentVoucher",
            PopupSearch: "popup_SearchPaymentVoucher"
        },
        Data: {
            ModelPaymentVoucher: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function printPanel() {
        var panel = document.getElementById("pnlContents");
        var printWindow = window.open('', '', 'height=' + screen.height, 'width=' + screen.width);
        printWindow.document.write(panel.innerHTML);
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    function reloadListPaymentVoucher() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtablePaymentVoucher).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(paymentVoucher) {
        var paymentVoucherViewModel = {
            Id: 0,
            Content: "",
            Note: "",
            Money: 0,
            PaymentDate:"",
            ReceiptName:"",
            ReceiptAddress:""
        };
        if (paymentVoucher != null) {
            paymentVoucherViewModel = {
                Id: ko.observable(paymentVoucher.Id),
                Content: ko.observable(paymentVoucher.Content),
                Note: ko.observable(paymentVoucher.Note),
                Money: ko.observable(paymentVoucher.Money),
                PaymentDate: ko.observable(paymentVoucher.PaymentDate),
                ReceiptName: ko.observable(paymentVoucher.ReceiptName),
                ReceiptAddress: ko.observable(paymentVoucher.ReceiptAddress)
            };
        }
        return paymentVoucherViewModel;
    }
    function bindData(paymentVoucher) {
        global.Data.ModelPaymentVoucher = initViewModel(paymentVoucher);
        ko.applyBindings(global.Data.ModelPaymentVoucher);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupPaymentVoucher() {
        $("#" + global.Element.PopupPaymentVoucher).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeletePaymentVoucher,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListPaymentVoucher();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupPaymentVoucher, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListPaymentVoucher() {
        $("#" + global.Element.JtablePaymentVoucher).jtable({
            title: "Danh sách Phiếu Chi",
            paging: true,
            pageSize: 10,
            pageSizeChangePaymentVoucher: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListPaymentVoucher,
                createAction: global.Element.PopupPaymentVoucher,
                createObjDefault: initViewModel(null),
                searchAction: global.Element.PopupSearch
            },
            messages: {
                addNewRecord: "Thêm mới",
                searchRecord: "Tìm kiếm",
                selectShow: "Ẩn hiện cột"
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Content: {
                    visibility: "fixed",
                    title: "Tên Loại",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Content + "</a>");
                        text.click(function () {
                            data.record.PaymentDate = FormatDateJsonToString(data.record.PaymentDate, "yyyy-mm-dd");
                            $('#date').val(data.record.PaymentDate);
                            bindData(data.record);
                            showPopupPaymentVoucher();
                        });
                        return text;
                    }
                },
                Money: {
                    title: "Số Tiền",
                    width: "25%"
                },
                Note: {
                    title: "Mô Tả",
                    width: "25%"
                },
                PaymentDate: {
                    title: 'Ngày Chi',
                    width: "10%",
                    type: 'date',
                    displayFormat: 'dd-mm-yy'
                },
                ReceiptName: {
                    title: "Người Nhận",
                    width: "20%"
                },
                ReceiptAddress: {
                    title: "Địa Chỉ Người Nhận",
                    width: "25%"
                },
                Print: {
                    visibility: 'fixed',
                    title: "In Phiếu",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="#" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">'  + "In Phiếu" + '</a>');
                        text.click(function () {
                            var paydate = FormatDateJsonToString(data.record.PaymentDate, "dd-mm-yyyy");
                            $("#pid").html("");
                            $("#pname").html("");
                            $("#paddress").html("");
                            $("#pcontent").html("");
                            $("#pmoney").html("");
                            $("#ppaymentdate2").html("");
                            $("#ppaymentdate1").html("");
                            //////////////////////////////////////////////////////
                            $("#pid").append(data.record.Id);
                            $("#pname").append(data.record.ReceiptName);
                            $("#paddress").append(data.record.ReceiptAddress);
                            $("#pcontent").append(data.record.Content);
                            $("#pmoney").append(data.record.Money);
                            $("#ppaymentdate2").append(paydate);
                            $("#ppaymentdate1").append(paydate);
                            printPanel();
                           // window.location.href = "/PaymentVoucher/Index";
                        });
                        return text;
                    }
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
                                realTimeHub.server.sendUpdateEvent("jtablePaymentVoucher");
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
    function savePaymentVoucher() {
        $.ajax({
            url: global.UrlAction.SavePaymentVoucher,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPaymentVoucher),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListPaymentVoucher();
                        $("#" + global.Element.PopupPaymentVoucher).modal("hide");
                        toastr.success('Thanh Cong');
                    }
                }, false, global.Element.PopupPaymentVoucher, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupPaymentVoucher() {
        $("#" + global.Element.PopupPaymentVoucher).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPaymentVoucher + " button[save]").click(function () {
            savePaymentVoucher();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePaymentVoucher", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupPaymentVoucher + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPaymentVoucher).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPaymentVoucher = function () {
        reloadListPaymentVoucher();
    };
    this.initViewModel = function (paymentVoucher) {
        initViewModel(paymentVoucher);
    };
    this.bindData = function (paymentVoucher) {
        bindData(paymentVoucher);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        document.getElementById("datefrom").defaultValue = new Date().toISOString().substring(0, 10);
        document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
        registerEvent();
        initListPaymentVoucher();
        reloadListPaymentVoucher();
        initPopupPaymentVoucher();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var paymentVoucher = new VINASIC.PaymentVoucher();
    paymentVoucher.Init();
});