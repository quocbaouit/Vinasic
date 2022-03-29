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
            SaveOrder: "/PaymentVoucher/SaveOrder",
            DeletePaymentVoucher: "/PaymentVoucher/DeletePaymentVoucher"
        },
        Element: {
            JtablePaymentVoucher: "jtablePaymentVoucher",
            JtablePaymentVoucher1: "jtablePaymentVoucher1",
            JtablePaymentVoucher12: "jtablePaymentVoucher12",
            PopupPaymentVoucher: "popup_PaymentVoucher",
            PopupSearch: "popup_SearchPaymentVoucher",
            JtableOrderDetail: "jtableOrderDetail"
        },
        Data: {
            ModelPaymentVoucher: {},
            ModelOrderDetail: [],
            CurenIndex: 0,
            Index: 1,
            Idnew: 0,
            OrderId: 0,
            OrderTotal: 0,
            CustomerId: 0,
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
        var keySearch = $("#keyword").val();
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        $("#" + global.Element.JtablePaymentVoucher).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate,'type':0 });
    }
    function reloadListPaymentVoucher1() {
        var keySearch = $("#keyword1").val();
        var fromDate = $("#datefrom1").val();
        var toDate = $("#dateto1").val();
        $("#" + global.Element.JtablePaymentVoucher1).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate, 'type': 1 });
    }
    function reloadListPaymentVoucher12() {
        var keySearch = $("#keyword12").val();
        var fromDate = $("#datefrom12").val();
        var toDate = $("#dateto12").val();
        $("#" + global.Element.JtablePaymentVoucher12).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate, 'type': 2 });
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
            PaymentDate: "",
            ReceiptName: "",
            ReceiptAddress: ""
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
                        reloadListPaymentVoucher1();
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
                            //initComboBoxAllProduct(data.record.CommodityId);
                            global.Data.Idnew = data.record.Index;
                            $("#dproduct").val(data.record.CommodityName);
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
                //FileName: {
                //    title: "Tên File",
                //    width: "10%"
                //},
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
                                //if (document.getElementById("dtax").checked == true) {
                                //    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                                //    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                //}
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
                            global.Data.Idnew = data.record.CommodityName;
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
                                //if (document.getElementById("dtax").checked == true) {
                                //    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                                //    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                //}
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
        $('#' + global.Element.JtablePaymentVoucher).jtable({
            title: 'Danh Sách Phiếu Chi Thông Thường',
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            //selectShow: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            selectOnRowClick: false,
            rowInserted: function (event, data) {
            },
            recordsLoaded: function (event, data) {
                //var SumA = data.serverResponse.Data[0].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var SumB = data.serverResponse.Data[1].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                //document.getElementById("sum11").innerHTML = SumA;
                document.getElementById("sum12").innerHTML = SumB;
            },
            actions: {
                listAction: global.UrlAction.GetListPaymentVoucher
            },
            datas: {
                jtableId: global.Element.JtablePaymentVoucher
            },
            messages: {
                selectShow: 'Ẩn hiện cột'
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                OrderDetail: {
                    title: ' Id',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (orderDetailData) {
                        var $img = $('<a detailKey style="color: red;" id="newdetail" href="javascript:void(0)">' + orderDetailData.record.Id + '</a>');
          
                        return $img;
                    }
                },
                ReceiptName: {
                    visibility: 'fixed',
                    title: "Tên Người Nhận",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.ReceiptName + '</a>');
                        text.click(function () {


                            global.Data.CustomerId = data.record.CustomerId;
                          
                            global.Data.OrderId = data.record.Id;
                            while (global.Data.ModelOrderDetail.length) {
                                global.Data.ModelOrderDetail.pop();
                            }
                            if (data.record.T_PaymentVoucherDetail.length > 0) {
                                global.Data.ModelOrderDetail.push.apply(global.Data.ModelOrderDetail, data.record.T_PaymentVoucherDetail);
                                global.Data.Index = global.Data.ModelOrderDetail[global.Data.ModelOrderDetail.length - 1].Index + 1;
                            }

                            if (global.Data.ModelOrderDetail.length > 0) {
                                //$("#viewDetail").css("display", "block");
                                $("#dsubtotal1").css("display", "none");
                                $("#dsubtotal2").css("display", "none");
                                $("#cemployee").val(1);
                                $("#cname1").val(data.record.ReceiptName);
                                $("#content1").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            else {
                                //$("#viewDetail").css("display", "none");
                                $("#dsubtotal1").css("display", "block");
                                $("#dsubtotal2").css("display", "block");
                                $("#cemployee").val(0);
                                $("#cname").val(data.record.ReceiptName);
                                $("#cphone").val('');
                                $("#cmail").val('');
                                $("#caddress").val('');
                                $("#content").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            reloadListOrderDetail();
                            resetDetail();
                            global.Data.OrderTotal = 0;

                            for (var j = 0; j < global.Data.ModelOrderDetail.length; j++) {
                                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[j].SubTotal);
                            }
                            for (var h = 0; h < global.Data.ModelOrderDetail.length; h++) {
                                global.Data.ModelOrderDetail[h].Price = global.Data.ModelOrderDetail[h].Price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                global.Data.ModelOrderDetail[h].SubTotal = global.Data.ModelOrderDetail[h].SubTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            }
                            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //$("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //if (data.record.HasTax) {
                            //    var totalIncludeTax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                            //    $("#dtotaltax").val(totalIncludeTax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //}

                            //popAllElementInArray(global.Data.ModelOrderDetail);
                            if (global.Data.ModelOrderDetail.length > 0) {
                                $('.nav-tabs a:last').tab('show');
                             
                            } else {
                                $('.nav-tabs a[href=#' + 'edit-profile' + ']').tab('show');
                            }        

                        });
                        return text;
                    }
                },
                StrCreatedDate: {
                    title: 'Ngày Tạo',
                    width: "8%"
                },
                Content: {
                    title: "Nội Dung",
                    width: "25%"
                },
                Money: {
                    title: "Số Tiền(VND)",
                    width: "25%",
                    display: function (data) {
                        return data.record.Money.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Print: {
                    visibility: 'fixed',
                    title: "In Phiếu",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="#" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "In Phiếu" + '</a>');
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
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                deleteRow(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                },

            },
            selectionChanged: function () {
                var $selectedRows = $('#jtableOrder').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    var sum = 0;
                    var haspay = 0;
                    var hasexit = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        sum = sum + record.SubTotal;
                        haspay = haspay + record.HasPay;
                        var current = record.SubTotal - record.HasPay;
                        hasexit = hasexit + current;
                    });
                    var strSum = sum.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhaspay = haspay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhasexist = hasexit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    toastr.options = {
                        "closeButton": true,
                        "positionClass": "toast-bottom-full-width",
                        "preventDuplicates": false,
                        "timeOut": "5000"
                    }
                    toastr.error("Tổng Tiền: " + strSum + " ------ Đã Thanh Toán: " + strhaspay + " ------  Còn Lại: " + strhasexist);
                } else {
                    //No rows selected
                    // $('#checkSum').val(0);
                }
            }
        });
    }
    function initListPaymentVoucher1() {
        $('#' + global.Element.JtablePaymentVoucher1).jtable({
            title: 'Danh Sách Phiếu Chi Nhập Hàng',
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            //selectShow: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            selectOnRowClick: false,
            recordsLoaded: function (event, data) {
                var SumA = data.serverResponse.Data[0].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var SumB = data.serverResponse.Data[1].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                document.getElementById("sum01").innerHTML = SumA;
                document.getElementById("sum02").innerHTML = SumB;
            },
            actions: {
                listAction: global.UrlAction.GetListPaymentVoucher
            },
            datas: {
                jtableId: global.Element.JtablePaymentVoucher
            },
            messages: {
                selectShow: 'Ẩn hiện cột'
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                OrderDetail: {
                    title: ' Id',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (orderDetailData) {
                        var $img = $('<a detailKey style="color: red;" id="newdetail" href="javascript:void(0)">' + orderDetailData.record.Id + '</a>');
                        $img.click(function () {
                            debugger;
                            $('#OrderId').val(orderDetailData.record.Id);

                            $('#jtablePaymentVoucher').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: 'Chi Tiết Của Phiếu Chi:' + orderDetailData.record.ReceiptName,
                                    actions: {
                                        listAction: '/PaymentVoucher/ListOrderDetail?OrderId=' + orderDetailData.record.Id,
                                    },
                                    messages: {
                                        addNewRecord: 'Thêm Chi Tiết Đơn Hàng'
                                    },
                                    fields: {
                                        OrderId: {
                                            type: 'hidden',
                                            defaultValue: orderDetailData.record.Id
                                        },
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        CommodityName: {
                                            title: "Tên Mặt Hàng",
                                            width: "10%"
                                        },
                                        Description: {
                                            title: "Ghi Chú",
                                            width: "10%"
                                        },
                                        Width: {
                                            title: "CNgang",
                                            width: "5%"
                                        },
                                        Height: {
                                            title: "CCao",
                                            width: "5%"
                                        },
                                        Square: {
                                            title: "DTích",
                                            width: "5%"
                                        },
                                        Quantity: {
                                            title: 'SLượng',
                                            width: '5%'
                                        },
                                        SumSquare: {
                                            title: 'Tổng DT',
                                            width: '10%'
                                        },
                                        strPrice: {
                                            title: 'ĐGiá',
                                            width: '5%'
                                        },
                                        strSubTotal: {
                                            title: 'Thành Tiền',
                                            width: '10%'
                                        },

                                    },
                                }, function (data) { //opened handler
                                    debugger;
                                    if (data) { }
                                    data.childTable.jtable('load');
                                });
                        });
                        return $img;
                    }
                },
                ReceiptName: {
                    visibility: 'fixed',
                    title: "Tên Người Nhận",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.ReceiptName + '</a>');
                        text.click(function () {


                            global.Data.CustomerId = data.record.CustomerId;

                            global.Data.OrderId = data.record.Id;
                            while (global.Data.ModelOrderDetail.length) {
                                global.Data.ModelOrderDetail.pop();
                            }
                            if (data.record.T_PaymentVoucherDetail.length > 0) {
                                global.Data.ModelOrderDetail.push.apply(global.Data.ModelOrderDetail, data.record.T_PaymentVoucherDetail);
                                global.Data.Index = global.Data.ModelOrderDetail[global.Data.ModelOrderDetail.length - 1].Index + 1;
                            }

                            if (global.Data.ModelOrderDetail.length > 0) {
                                //$("#viewDetail").css("display", "block");
                                $("#dsubtotal1").css("display", "none");
                                $("#dsubtotal2").css("display", "none");
                                $("#cemployee").val(1);
                                $("#cname1").val(data.record.ReceiptName);
                                $("#content1").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            else {
                                //$("#viewDetail").css("display", "none");
                                $("#dsubtotal1").css("display", "block");
                                $("#dsubtotal2").css("display", "block");
                                $("#cemployee").val(0);
                                $("#cname").val(data.record.ReceiptName);
                                $("#cphone").val('');
                                $("#cmail").val('');
                                $("#caddress").val('');
                                $("#content").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            reloadListOrderDetail();
                            resetDetail();
                            global.Data.OrderTotal = 0;

                            for (var j = 0; j < global.Data.ModelOrderDetail.length; j++) {
                                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[j].SubTotal);
                            }
                            for (var h = 0; h < global.Data.ModelOrderDetail.length; h++) {
                                global.Data.ModelOrderDetail[h].Price = global.Data.ModelOrderDetail[h].Price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                global.Data.ModelOrderDetail[h].SubTotal = global.Data.ModelOrderDetail[h].SubTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            }
                            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            $("#dhaspay").val(data.record.HasPay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //$("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //if (data.record.HasTax) {
                            //    var totalIncludeTax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                            //    $("#dtotaltax").val(totalIncludeTax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //}

                            //popAllElementInArray(global.Data.ModelOrderDetail);
                            if (global.Data.ModelOrderDetail.length > 0) {
                                $('.nav-tabs a:last').tab('show');

                            } else {
                                $('.nav-tabs a[href=#' + 'edit-profile' + ']').tab('show');
                            }

                        });
                        return text;
                    }
                },
                StrCreatedDate: {
                    title: 'Ngày Tạo',
                    width: "8%"
                },
                Content: {
                    title: "Nội Dung",
                    width: "15%"
                },
                Money: {
                    title: "Số Tiền(VND)",
                    width: "15%",
                    display: function (data) {
                        return data.record.Money.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                HasPay: {
                    title: "Đã trả(VND)",
                    width: "15%",
                    display: function (data) {
                        return data.record.HasPay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Remain: {
                    title: "Còn Nợ",
                    width: "15%",
                    display: function (data) {
                        return (data.record.Money - data.record.HasPay).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Print: {
                    visibility: 'fixed',
                    title: "In Phiếu",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="#" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "In Phiếu" + '</a>');
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
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                deleteRow(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                },

            },
            selectionChanged: function () {
                var $selectedRows = $('#jtableOrder').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    var sum = 0;
                    var haspay = 0;
                    var hasexit = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        sum = sum + record.SubTotal;
                        haspay = haspay + record.HasPay;
                        var current = record.SubTotal - record.HasPay;
                        hasexit = hasexit + current;
                    });
                    var strSum = sum.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhaspay = haspay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhasexist = hasexit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    toastr.options = {
                        "closeButton": true,
                        "positionClass": "toast-bottom-full-width",
                        "preventDuplicates": false,
                        "timeOut": "5000"
                    }
                    toastr.error("Tổng Tiền: " + strSum + " ------ Đã Thanh Toán: " + strhaspay + " ------  Còn Lại: " + strhasexist);
                } else {
                    //No rows selected
                    // $('#checkSum').val(0);
                }
            }
        });
    }
    function initListPaymentVoucher12() {
        $('#' + global.Element.JtablePaymentVoucher12).jtable({
            title: 'Danh Sách Phiếu Chi Nhập Hàng',
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            //selectShow: true,
            selecting: true, //Enable selecting
            multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            selectOnRowClick: false,
            recordsLoaded: function (event, data) {
                var SumA = data.serverResponse.Data[0].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var SumB = data.serverResponse.Data[1].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                document.getElementById("sum21").innerHTML = SumA;
                document.getElementById("sum22").innerHTML = SumB;
            },
            actions: {
                listAction: global.UrlAction.GetListPaymentVoucher
            },
            datas: {
                jtableId: global.Element.JtablePaymentVoucher12
            },
            messages: {
                selectShow: 'Ẩn hiện cột'
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                OrderDetail: {
                    title: ' Id',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (orderDetailData) {
                        var $img = $('<a detailKey style="color: red;" id="newdetail" href="javascript:void(0)">' + orderDetailData.record.Id + '</a>');
                        $img.click(function () {
                            debugger;
                            $('#OrderId').val(orderDetailData.record.Id);

                            $('#jtablePaymentVoucher').jtable('openChildTable',
                                $img.closest('tr'),
                                {
                                    title: 'Chi Tiết Của Phiếu Chi:' + orderDetailData.record.ReceiptName,
                                    actions: {
                                        listAction: '/PaymentVoucher/ListOrderDetail?OrderId=' + orderDetailData.record.Id,
                                    },
                                    messages: {
                                        addNewRecord: 'Thêm Chi Tiết Đơn Hàng'
                                    },
                                    fields: {
                                        OrderId: {
                                            type: 'hidden',
                                            defaultValue: orderDetailData.record.Id
                                        },
                                        Id: {
                                            key: true,
                                            create: false,
                                            edit: false,
                                            list: false
                                        },
                                        CommodityName: {
                                            title: "Tên Mặt Hàng",
                                            width: "10%"
                                        },
                                        Description: {
                                            title: "Ghi Chú",
                                            width: "10%"
                                        },
                                        Width: {
                                            title: "CNgang",
                                            width: "5%"
                                        },
                                        Height: {
                                            title: "CCao",
                                            width: "5%"
                                        },
                                        Square: {
                                            title: "DTích",
                                            width: "5%"
                                        },
                                        Quantity: {
                                            title: 'SLượng',
                                            width: '5%'
                                        },
                                        SumSquare: {
                                            title: 'Tổng DT',
                                            width: '10%'
                                        },
                                        strPrice: {
                                            title: 'ĐGiá',
                                            width: '5%'
                                        },
                                        strSubTotal: {
                                            title: 'Thành Tiền',
                                            width: '10%'
                                        },

                                    },
                                }, function (data) { //opened handler
                                    debugger;
                                    if (data) { }
                                    data.childTable.jtable('load');
                                });
                        });
                        return $img;
                    }
                },
                ReceiptName: {
                    visibility: 'fixed',
                    title: "Tên Người Nhận",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.ReceiptName + '</a>');
                        text.click(function () {


                            global.Data.CustomerId = data.record.CustomerId;

                            global.Data.OrderId = data.record.Id;
                            while (global.Data.ModelOrderDetail.length) {
                                global.Data.ModelOrderDetail.pop();
                            }
                            if (data.record.T_PaymentVoucherDetail.length > 0) {
                                global.Data.ModelOrderDetail.push.apply(global.Data.ModelOrderDetail, data.record.T_PaymentVoucherDetail);
                                global.Data.Index = global.Data.ModelOrderDetail[global.Data.ModelOrderDetail.length - 1].Index + 1;
                            }

                            if (global.Data.ModelOrderDetail.length > 0) {
                                //$("#viewDetail").css("display", "block");
                                $("#dsubtotal1").css("display", "none");
                                $("#dsubtotal2").css("display", "none");
                                $("#cemployee").val(1);
                                $("#cname1").val(data.record.ReceiptName);
                                $("#content1").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            else {
                                //$("#viewDetail").css("display", "none");
                                $("#dsubtotal1").css("display", "block");
                                $("#dsubtotal2").css("display", "block");
                                $("#cemployee").val(0);
                                $("#cname").val(data.record.ReceiptName);
                                $("#cphone").val('');
                                $("#cmail").val('');
                                $("#caddress").val('');
                                $("#content").val(data.record.Content);
                                $("#dsubtotal1").val(data.record.Money);
                            }
                            reloadListOrderDetail();
                            resetDetail();
                            global.Data.OrderTotal = 0;

                            for (var j = 0; j < global.Data.ModelOrderDetail.length; j++) {
                                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[j].SubTotal);
                            }
                            for (var h = 0; h < global.Data.ModelOrderDetail.length; h++) {
                                global.Data.ModelOrderDetail[h].Price = global.Data.ModelOrderDetail[h].Price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                                global.Data.ModelOrderDetail[h].SubTotal = global.Data.ModelOrderDetail[h].SubTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            }
                            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            $("#dhaspay").val(data.record.HasPay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //$("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //if (data.record.HasTax) {
                            //    var totalIncludeTax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                            //    $("#dtotaltax").val(totalIncludeTax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //}

                            //popAllElementInArray(global.Data.ModelOrderDetail);
                            if (global.Data.ModelOrderDetail.length > 0) {
                                $('.nav-tabs a:last').tab('show');

                            } else {
                                $('.nav-tabs a[href=#' + 'edit-profile' + ']').tab('show');
                            }

                        });
                        return text;
                    }
                },
                StrCreatedDate: {
                    title: 'Ngày Tạo',
                    width: "8%"
                },
                Content: {
                    title: "Nội Dung",
                    width: "15%"
                },
                Money: {
                    title: "Số Tiền(VND)",
                    width: "15%",
                    display: function (data) {
                        return data.record.Money.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                HasPay: {
                    title: "Đã trả(VND)",
                    width: "15%",
                    display: function (data) {
                        return data.record.HasPay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Remain: {
                    title: "Còn Nợ",
                    width: "15%",
                    display: function (data) {
                        return (data.record.Money - data.record.HasPay).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Print: {
                    visibility: 'fixed',
                    title: "In Phiếu",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="#" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "In Phiếu" + '</a>');
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
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                deleteRow(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                },

            },
            selectionChanged: function () {
                var $selectedRows = $('#jtableOrder').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    var sum = 0;
                    var haspay = 0;
                    var hasexit = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        sum = sum + record.SubTotal;
                        haspay = haspay + record.HasPay;
                        var current = record.SubTotal - record.HasPay;
                        hasexit = hasexit + current;
                    });
                    var strSum = sum.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhaspay = haspay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strhasexist = hasexit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    toastr.options = {
                        "closeButton": true,
                        "positionClass": "toast-bottom-full-width",
                        "preventDuplicates": false,
                        "timeOut": "5000"
                    }
                    toastr.error("Tổng Tiền: " + strSum + " ------ Đã Thanh Toán: " + strhaspay + " ------  Còn Lại: " + strhasexist);
                } else {
                    //No rows selected
                    // $('#checkSum').val(0);
                }
            }
        });
    }
    /*End init List */

    /*function Check Validate */
    function checkValidate() {
        if ($("#dproduct").val() === "" && $("#cemployee").val() == 1) {
            toastr.warning("Vui Lòng Chọn Dịch Vụ");
            $("#Name").focus();
            return false;
        }
        if (($("#dprice").val() === "" || $("#dsubtotal").val() === "" || $("#dquantity").val() === "") && $("#cemployee").val() == 1) {
            toastr.warning("Kiểm Tra số lượng đơn giá và thành tiền");
            return false;
        } else {
            return true;
        }
    }
    /*End Check Validate */

    /*function Save */
    //function savePaymentVoucher() {
    //    $.ajax({
    //        url: global.UrlAction.SavePaymentVoucher,
    //        type: 'post',
    //        data: ko.toJSON(global.Data.ModelPaymentVoucher),
    //        contentType: 'application/json',
    //        success: function (result) {
    //            $('#loading').hide();
    //            GlobalCommon.CallbackProcess(result, function () {
    //                if (result.Result === "OK") {
    //                    bindData(null);
    //                    reloadListPaymentVoucher();
    //                    $("#" + global.Element.PopupPaymentVoucher).modal("hide");
    //                    toastr.success('Thanh Cong');
    //                }
    //            }, false, global.Element.PopupPaymentVoucher, true, true, function () {
    //                var msg = GlobalCommon.GetErrorMessage(result);
    //                GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
    //            });
    //        }
    //    });
    //}
    function resetAll() {
        //$("#viewDetail").css("display", "none");
        $("#cemployee").val(0);
        $("#dsumsquare").val("");
        $("#cname").val("");
        $("#cphone").val("");
        $("#cmail").val("");
        $("#caddress").val("");
        $("#ctaxcode").val("");
        $("#dproductType").val('');
        $("#dproduct").val("");
        $("#dfilename").val("");
        $("#dnote").val("");
        $("#dwidth").val("");
        $("#dheignt").val("");
        $("#dsquare").val("");
        $("#dquantity").val("");
        $("#dprice").val("");
        $("#dsubtotal").val("");
    }
    function saveOrder() {
        if ($("#cname").val() === "") {
            toastr.warning("Vui Lòng Nhập Tên Người Nhận");
            $("#Name").focus();
            return false;
        }
        if ($("#content").val() === "") {
            toastr.warning("Vui Lòng Nhập Nội Dung");
            $("#content").focus();
            return false;
        }
        if ($("#dsubtotal1").val() === "" && $("#cemployee").val() == 0) {
            toastr.warning("Vui Lòng Nhập Tổng Tiền");
            $("#dsubtotal1").focus();
            return false;
        }
        if ($("#dtotal").val() === "" && $("#cemployee").val() == 1) {
            toastr.warning("Vui Lòng Nhập Tổng Tiền");
            $("#dtotal").focus();
            return false;
        }
        d = document.getElementById("cemployee").value;
        if (d == 0) {
            while (global.Data.ModelOrderDetail.length) {
                global.Data.ModelOrderDetail.pop();
            }
            global.Data.OrderTotal = 0;
        }
        var employeeId = 0;
        var customerName = $("#cname").val();
        var customerPhone = $("#cphone").val();
        var customerMail = '';
        var customerAddress = $("#caddress").val();
        var customerTaxCode = '';
        var dateDelivery = '22/11/2018';
        var content = $("#content").val();
        var totalInclude = $("#dsubtotal1").val().replace(/[^0-9-.]/g, '');
        if (totalInclude == '' || totalInclude == undefined) {
            totalInclude = 0;
        }
        $.ajax({
            url: global.UrlAction.SaveOrder + "?orderId=" + global.Data.OrderId + "&employeeId=" + employeeId + "&customerId=" + 1 + "&customerName=" + customerName + "&customerPhone=" + customerPhone + "&customerMail=" + customerMail + "&customerAddress=" + customerAddress + "&customerTaxCode=" + customerTaxCode + "&orderTotal=" + global.Data.OrderTotal + "&content=" + content + "&totalInclude=" + totalInclude + "&haspay=" + global.Data.OrderTotal,
            type: 'post',
            data: JSON.stringify({ 'listDetail': global.Data.ModelOrderDetail }),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        resetAll();
                        toastr.success("Thành Công");
                        reloadListPaymentVoucher();
                        reloadListPaymentVoucher1();
                        reloadListPaymentVoucher12();
                        while (global.Data.ModelOrderDetail.length) {
                            global.Data.ModelOrderDetail.pop();
                        }
                        reloadListOrderDetail();
                        $('.nav-tabs a:first').tab('show');
                    }
                }, false, global.Element.PopupOrder, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }

    function saveOrder1() {
        debugger;
        if ($("#cname1").val() === "") {
            toastr.warning("Vui Lòng Nhập Tên Người Nhận");
            $("#Name1").focus();
            return false;
        }
        if ($("#content1").val() === "") {
            toastr.warning("Vui Lòng Nhập Nội Dung");
            $("#content1").focus();
            return false;
        }
        if ($("#dtotal").val() === "" && $("#cemployee").val() == 1) {
            toastr.warning("Vui Lòng Nhập Tổng Tiền");
            $("#dtotal").focus();
            return false;
        }
        var employeeId = 0;
        var customerName = $("#cname1").val();
        var customerPhone = '';
        var customerMail = '';
        var customerAddress = '';
        var customerTaxCode = '';
        var haspay = $("#dhaspay").val().replace(/[^0-9-.]/g, '');
        if (haspay=='') {
            haspay = 0;
        }
        var dateDelivery = '22/11/2018';
        var content = $("#content1").val();
        var totalInclude = 0;
        $.ajax({
            url: global.UrlAction.SaveOrder + "?orderId=" + global.Data.OrderId + "&employeeId=" + employeeId + "&customerId=" + 1 + "&customerName=" + customerName + "&customerPhone=" + customerPhone + "&customerMail=" + customerMail + "&customerAddress=" + customerAddress + "&customerTaxCode=" + customerTaxCode + "&orderTotal=" + global.Data.OrderTotal + "&content=" + content + "&totalInclude=" + totalInclude + "&haspay=" + haspay,
            type: 'post',
            data: JSON.stringify({ 'listDetail': global.Data.ModelOrderDetail }),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        resetAll();
                        toastr.success("Thành Công");
                        reloadListPaymentVoucher();
                        reloadListPaymentVoucher1();
                        reloadListPaymentVoucher12();
                        while (global.Data.ModelOrderDetail.length) {
                            global.Data.ModelOrderDetail.pop();
                        }
                        reloadListOrderDetail();
                        $('.nav-tabs a[href=#' + 'recent-activity1' + ']').tab('show');
                        //$('.nav-tabs a:first').tab('show');
                    }
                }, false, global.Element.PopupOrder, true, true, function () {
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
            saveOrder();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePaymentVoucher", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupPaymentVoucher + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPaymentVoucher).modal("hide");
        });
    }
    function resetDetail() {
        $("#dsumsquare").val("");
        $("#dfilename").val("");
        $("#dnote").val("");
        $("#dwidth").val("");
        $("#dheignt").val("");
        $("#dsquare").val("");
        $("#dquantity").val("");
        // $("#dprice").val("");
        $("#dsubtotal").val("");
    }
    /*function Check Validate */
    function decimalAdjust(type, value, exp) {
        // If the exp is undefined or zero...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // If the value is not a number or the exp is not an integer...
        if (isNaN(value) || !(exp % 1 === 0)) {
            return NaN;
        }
        // Shift
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }
    function removeItemInArray(arr, id) {
        if (typeof (arr) != "undefined") {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].Index === id) {
                    arr.splice(i, 1);
                    break;
                }
            };
        }
    }

    function checkNumber(values) {
        var isNaN = Number.isNaN(Number(values));
        return isNaN;
    }
    function calculatorSquare(width, height) {
        return width * height;
    }
    function calculatorSumSquare(width, height, quantity) {
        return width * height * quantity;
    }
    function calculatorSubTotal(square, quantity, price) {
        if (!checkNumber(square) && square !== 0) {
            return decimalAdjust('round', square * quantity, -6) * price;
        } else {
            return quantity * price;
        }

    }
    /*End Check Validate */
    function calculatorPrice() {
        var width = $("#dwidth").val();
        var height = $("#dheignt").val();
        var quantity = $("#dquantity").val();
        var sqare = calculatorSquare(width, height);
        if (!checkNumber(sqare) && sqare !== 0) {
            var roundSquare = decimalAdjust('round', sqare, -6);
            $("#dsquare").val(roundSquare);
        } else {
            $("#dsquare").val("");
        }
        var sumsquare = calculatorSumSquare(width, height, quantity);
        if (!checkNumber(sumsquare) && sqare !== 0) {
            var roundSumSquare = decimalAdjust('round', sumsquare, -6);
            $("#dsumsquare").val(roundSumSquare);
        } else {
            $("#dsumsquare").val("");
        }
        var price = $("#dprice").val();
        if (!checkNumber(quantity) && quantity !== "" && price !== "") {
            var total = calculatorSubTotal(sqare, quantity, price.replace(/[^0-9-.]/g, ''));
            if (!checkNumber(total)) {
                var roundtotal = decimalAdjust('round', total, 0);
                $("#dsubtotal").val(roundtotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                $("#dtotaltax").val(roundtotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //if (document.getElementById("dtax").checked == true) {
                //    var totaltax = roundtotal * 0.1 + roundtotal;
                //    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //}
            }
            else {
                $("#dsubtotal").val("");
                $("#dtotaltax").val("");
            }
        }
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPaymentVoucher = function () {
        reloadListPaymentVoucher();
    };
    this.reloadListPaymentVoucher1 = function () {
        reloadListPaymentVoucher1();
    };
    this.reloadListPaymentVoucher12 = function () {
        reloadListPaymentVoucher12();
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
        $("#search").click(function () {
            reloadListPaymentVoucher();
        });
        $("#search1").click(function () {
            reloadListPaymentVoucher1();
        });
        $("#search12").click(function () {
            reloadListPaymentVoucher12();
        });
        $('#cemployee').change(function () {
            d = document.getElementById("cemployee").value;
            if (d == 0) {
                //$("#viewDetail").css("display", "none");
                $("#dsubtotal1").css("display", "block");
                $("#dsubtotal2").css("display", "block");

            }
            else {
                //$("#viewDetail").css("display", "block");
                $("#dsubtotal1").css("display", "none");
                $("#dsubtotal2").css("display", "none");
            }
        })
        $("#CreateOrder").click(function () {
            //document.getElementById("date").defaultValue = new Date().toISOString().substring(0, 10);
            $("#cname").attr("disabled", false);
            $("#cphone").attr("disabled", false);
            $("#dsubtotal1").css("display", "block");
            $("#dsubtotal2").css("display", "block");
            //$("#cemployee").value(0);
            //document.getElementById("dtax").checked = false;
            resetAll();
            //var ClientId = document.getElementById("ClientId").innerHTML;
            global.Data.CurenIndex = 0;
            //$("#cemployee").val(ClientId).change();

            global.Data.Index = 1;
            global.Data.OrderId = 0;
            while (global.Data.ModelOrderDetail.length) {
                global.Data.ModelOrderDetail.pop();
            }
            reloadListOrderDetail();
        });
        $("#dsubtotal").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dsubtotal").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#dsubtotal1").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dsubtotal1").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#dprice").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dprice").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#prealpay").keyup(function () {
            debugger;
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            if (parseFloat($(this).val().replace(/[^0-9-.]/g, '')) > parseFloat($('#ppayment').val().replace(/[^0-9-.]/g, ''))) {
                tempValue = $('#ppayment').val().replace(/[^0-9-.]/g, '');
            }
            $("#prealpay").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#ppay").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#ppay").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            var temp1 = $("#ppay").val().replace(/[^0-9-.]/g, '');
            var temp2 = $("#prealpay").val().replace(/[^0-9-.]/g, '');
            var a = parseFloat(temp1);
            var b = parseFloat(temp2);
            var c = a - b;
            if (isNaN(c)) {
                $("#prest").val(temp1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            } else {
                $("#prest").val(c.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        });
        $("#dhaspay").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dhaspay").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });

        $('.caculator').keyup(function () {
            calculatorPrice();
        });
        $('#cphone').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                var cusphone = $('#cphone').val();
                $.ajax({
                    url: "/Order/GetCustomerByPhone?phoneNumber=" + cusphone,
                    type: 'post',
                    contentType: 'application/json',
                    success: function (result) {
                        GlobalCommon.CallbackProcess(result, function () {
                            if (result.Records != null) {
                                var listCustomer = result.Records;
                                global.Data.CustomerId = listCustomer.Id;
                                $('#cname').val(listCustomer.Name);
                                $('#cphone').val(listCustomer.Mobile);
                                $('#cmail').val(listCustomer.Email);
                                $('#caddress').val(listCustomer.Address);
                                $('#ctaxcode').val(listCustomer.TaxCode);
                            } else {
                                $('#cname').val("");
                                $('#cphone').val("");
                                $('#cmail').val("");
                                $('#caddress').val("");
                                $('#ctaxcode').val("");

                            }

                        }, false, global.Element.PopupOrder, true, true, function () {
                            var msg = GlobalCommon.GetErrorMessage(result);
                            GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                        });
                    }
                });
            }
        });
        $(".detail").keydown(function (e) {

            if (e.which === 13) { //Enter
                global.Data.OrderTotal = 0;
                if (checkValidate()) {
                    e.preventDefault();
                    //removeItemAndInsertInArray(arr, id, obj);
                    var objectIndex = global.Data.Index;
                    if (global.Data.CurenIndex !== 0) {
                        objectIndex = global.Data.CurenIndex;
                        var object = { Id: global.Data.OrderDetailId, Index: objectIndex, CommodityId: objectIndex, CommodityName: $("#dproduct").val(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        for (var i = 0; i < global.Data.ModelOrderDetail.length; i++) {
                            if (global.Data.ModelOrderDetail[i].Index === global.Data.CurenIndex) {
                                global.Data.ModelOrderDetail.splice(i, 1, object);
                                break;
                            }
                        };
                        for (var j = 0; j < global.Data.ModelOrderDetail.length; j++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[j].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //global.Data.OrderTotal = global.Data.OrderTotal.replace(/[^0-9-.]/g, '');
                        $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //if (document.getElementById("dtax").checked == true) {
                        //    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                        //    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //}

                    } else {
                        var object1 = { Id: 0, Index: objectIndex, CommodityId: objectIndex, CommodityName: $("#dproduct").val(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        global.Data.ModelOrderDetail.push(object1);
                        global.Data.Index = global.Data.Index + 1;
                        for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //if (document.getElementById("dtax").checked == true) {
                        //    var totaltax = global.Data.OrderTotal * 0.1 + global.Data.OrderTotal;
                        //    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //}
                        //global.Data.OrderTotal = global.Data.OrderTotal.replace(/[^0-9-.]/g, '');
                    }

                    global.Data.CurenIndex = 0;
                    reloadListOrderDetail();
                    resetDetail();
                }
            }
        });
        $("#saveOrder").click(function () {
            saveOrder();
        });
        $("#saveOrder1").click(function () {
            saveOrder1();
        });

    };
    this.Init = function () {
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 30 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("datefrom1").defaultValue = new Date(new Date() - 24 * 30 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("datefrom12").defaultValue = new Date(new Date() - 24 * 30 * 60 * 60 * 1000).toISOString().substring(0, 10);
        var dateTo = new Date();
        dateTo.setDate(dateTo.getDate() + 1);
        document.getElementById("dateto").defaultValue = dateTo.toISOString().substring(0, 10);
        document.getElementById("dateto1").defaultValue = dateTo.toISOString().substring(0, 10);
        document.getElementById("dateto12").defaultValue = dateTo.toISOString().substring(0, 10);
        registerEvent();
        initListPaymentVoucher();
        reloadListPaymentVoucher();
        initListPaymentVoucher1();
        initListPaymentVoucher12();
        reloadListPaymentVoucher1();
        reloadListPaymentVoucher12();
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
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        toastr.warning("Vui lòng chỉ nhập số.");
        return false;
    }
}