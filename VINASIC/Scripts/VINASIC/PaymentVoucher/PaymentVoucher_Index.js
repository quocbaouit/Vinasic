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
            PopupSearch: "popup_SearchPaymentVoucher",
            JtableOrderDetail: "jtableOrderDetail"
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
    function reloadListOrderDetail() {
        $('#' + global.Element.JtableOrderDetail).jtable('load', { 'keyword': "" });
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
    function initListOrderDetail() {
        var tableObj = {};
        //document.getElementById('show-dim').checked
        if (true) {
            tableObj = {
                CommodityId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Index: {
                    title: "STT",
                    width: "5%"
                },
                CommodityName: {
                    visibility: 'fixed',
                    title: "Tên Dịch Vụ",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.CommodityName + '</a>');
                        text.click(function () {
                            initComboBoxAllProduct(data.record.CommodityId);
                            global.Data.Idnew = data.record.CommodityId;
                            $("#dfilename").val(data.record.FileName);
                            $("#dnote").val(data.record.Description);
                            $("#dwidth").val(decimalAdjust('round', data.record.Width, -6));
                            $("#dheignt").val(decimalAdjust('round', data.record.Height, -6));
                            $("#dsquare").val(decimalAdjust('round', data.record.Square, -6));
                            $("#dsumsquare").val(decimalAdjust('round', data.record.SumSquare, -6));
                            $("#dquantity").val(data.record.Quantity);
                            $("#dprice").val(data.record.Price);
                            $("#dsubtotal").val(data.record.SubTotal);
                            global.Data.CurenIndex = data.record.Index;
                            global.Data.OrderDetailId = data.record.Id;
                        });
                        return text;
                    }
                },
                FileName: {
                    title: "Tên File",
                    width: "10%"
                },
                Description: {
                    title: "Ghi Chú",
                    width: "10%"
                },
                Width: {
                    title: "Chiều Ngang",
                    width: "5%"
                },
                Height: {
                    title: "Chiều Cao",
                    width: "5%"
                },
                Square: {
                    title: "Diện Tích",
                    width: "5%"
                },
                Quantity: {
                    title: "Số Lượng",
                    width: "5%"
                },
                SumSquare: {
                    title: "Tổng  Diện Tích",
                    width: "5%"
                },
                Price: {
                    title: "Đơn Giá",
                    width: "15%"
                },
                SubTotal: {
                    title: "Thành Tiền",
                    width: "5%"
                },
                Delete: {
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button  title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                removeItemInArray(global.Data.ModelOrderDetail, data.record.Index);
                                reloadListOrderDetail();
                                global.Data.OrderTotal = 0;
                                for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                                    global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                                }
                                $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                if (document.getElementById("dtax").checked == true) {
                                    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                                    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                }
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            };
        }
        else {
            tableObj = {
                CommodityId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Index: {
                    title: "STT",
                    width: "5%"
                },
                CommodityName: {
                    visibility: 'fixed',
                    title: "Tên Dịch Vụ",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.CommodityName + '</a>');
                        text.click(function () {
                            initComboBoxAllProduct(data.record.CommodityId);
                            global.Data.Idnew = data.record.CommodityId;
                            $("#dfilename").val(data.record.FileName);
                            $("#dnote").val(data.record.Description);
                            $("#dwidth").val(decimalAdjust('round', data.record.Width, -6));
                            $("#dheignt").val(decimalAdjust('round', data.record.Height, -6));
                            $("#dsquare").val(decimalAdjust('round', data.record.Square, -6));
                            $("#dsumsquare").val(decimalAdjust('round', data.record.SumSquare, -6));
                            $("#dquantity").val(data.record.Quantity);
                            $("#dprice").val(data.record.Price);
                            $("#dsubtotal").val(data.record.SubTotal);
                            global.Data.CurenIndex = data.record.Index;
                            global.Data.OrderDetailId = data.record.Id;
                        });
                        return text;
                    }
                },
                FileName: {
                    title: "Tên File",
                    width: "10%"
                },
                Description: {
                    title: "Ghi Chú",
                    width: "10%"
                },
                Quantity: {
                    title: "Số Lượng",
                    width: "5%"
                },
                Price: {
                    title: "Đơn Giá",
                    width: "15%"
                },
                SubTotal: {
                    title: "Thành Tiền",
                    width: "5%"
                },
                Delete: {
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button  title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                removeItemInArray(global.Data.ModelOrderDetail, data.record.Index);
                                reloadListOrderDetail();
                                global.Data.OrderTotal = 0;
                                for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                                    global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                                }
                                $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                if (document.getElementById("dtax").checked == true) {
                                    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                                    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                }
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            };
        }
        $('#' + global.Element.JtableOrderDetail).jtable({
            title: 'Danh Sách Chi Tiết Đơn Hàng',
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.Data.ModelOrderDetail
            },
            messages: {
                addNewRecord: 'Thêm Mới Đơn Hàng',
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột'
            },
            fields: tableObj,
        });
    }
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
                    title: "Số Tiền(VND)",
                    width: "25%",
                    display: function (data) {
                        return data.record.Money.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
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
        initListOrderDetail();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var paymentVoucher = new VINASIC.PaymentVoucher();
    paymentVoucher.Init();
});