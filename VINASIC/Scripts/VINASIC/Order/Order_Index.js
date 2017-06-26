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
VINASIC.namespace("Order");
VINASIC.Order = function () {
    var global = {
        UrlAction: {
            GetListOrder: "/Order/GetOrders",
            GetListViewDetail: "/Order/GetListViewDetail",
            SaveOrder: "/Order/SaveOrder",
            UpdateHaspay: "/Order/UpdateHaspay",
            DeleteOrder: "/Order/DeleteOrder"
        },
        Element: {
            JtableOrder: "jtableOrder",
            JtableOrderDetail: "jtableOrderDetail",
            jtableViewDetail: "jtableViewDetail",
            PopupOrder: "popup_Order",
            PopupSearch: "popup_SearchOrder",
            PopupDesignProcess: "popup_DesignProcess",
            PopupHasPay: "popup_HasPay",
            PopupPaymentProcess: "popup_PaymentProcess",
            PopupPrintProcess: "popup_PrintProcess"
        },
        Data: {
            ModelOrder: {},
            ModelOrderDetail: [],
            ModelConfig: {},
            ListCustomerName: [],
            ProductTypeId: 0,
            CustomerId: 0,
            OrderId: 0,
            Idnew: 0,
            Index: 1,
            CurenIndex: 0,
            UpdateOrderId: 0,
            DesignStaus: 0,
            PrintStaus: 0,
            DetailId: 0,
            OrderTotal: 0,
            Realpay: 0,
            PcustomerName: "",
            PcustomePhone: "",
            PcustomerAddress: "",
            Pdate: "",
            Pproduct: "",
            NumberDetail: 0
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function renderTable(Table) {
        var tableString = "<table id=\"renderTable\" border=\"1\" style=\"width:100%\" cellspacing=\"0\" cellpadding=\"0\">";
        var root = document.getElementById('Block4');
        document.getElementById("Block4").innerHTML = "";
        tableString += "<tr>";
        tableString += "<th style=\"padding-left: 5px;text-align: left;\">" + "Dịch Vụ" + "</th>";
        tableString += "<th style=\"padding-left: 5px;text-align: left;\">" + "Ghi Chú" + "</th>";
        tableString += "<th>" + "CDài" + "</th>";
        tableString += "<th>" + "CRộng" + "</th>";
        tableString += "<th>" + "SLượng" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Đơn Giá" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Thành Tiền" + "</th>";
        tableString += "</tr>";
        for (row = 0; row < Table.length; row += 1) {
            var strPrice = Table[row].Price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            var strSubTotal = Table[row].SubTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            tableString += "<tr>";
            if (Table[row].Description == null) {
                Table[row].Description = '';
            }
            if (Table[row].Height == null) {
                Table[row].Height = '';
            }
            if (Table[row].Width == null) {
                Table[row].Width = '';
            }
            tableString += "<td style=\"padding-left: 5px;\">" + Table[row].CommodityName + "</td>";
            tableString += "<td style=\"padding-left: 5px;\">" + Table[row].Description + "</td>";
            tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Height + "</td>";
            tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Width + "</td>";
            tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Quantity + "</td>";
            tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + strPrice + "</td>";
            tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + strSubTotal + "</td>";
            tableString += "</tr>";
        }

        if (Table.length < 9) {
            var remain = 9 - Table.length;
            for (i = 0; i < remain; i += 1) {
                tableString += "<tr>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "</tr>";
            }
        }
        tableString += "<tr>";
        tableString += "<td colspan=\"6\">Tổng Tiền</td>";
        tableString += "<td style=\"padding-right: 5px;;text-align: right;\"><span id=\"vtotal1\">55577854</span></td>";
        tableString += "</tr>";
        tableString += "<tr>";
        tableString += "<td colspan=\"7\">Bằng Chữ:<span id=\"strtotal1\">Test </span></td>";
        tableString += "</tr>";
        tableString += "</table>";
        root.innerHTML = tableString;
    }
    function printPanel() {
        var panel = document.getElementById("paymentvoucher");
        var printWindow = window.open('', '', 'height=' + screen.height, 'width=' + screen.width);
        printWindow.document.write(panel.innerHTML);
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    function printPanel1() {
        var panel = document.getElementById("paymentvoucher1");
        var printWindow = window.open('', '', 'height=' + screen.height, 'width=' + screen.width);
        printWindow.document.write(panel.innerHTML);
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    function reloadListOrder() {
        debugger;
        var keySearch = $("#keyword").val();
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        var delivery = $("#DeliveryType").val();
        var paymentStatus = $("#PaymentStatus").val();
        $("#" + global.Element.JtableOrder).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate, 'delivery': delivery, 'paymentStatus': paymentStatus });
    }
    function reloadListOrder(isdelivery, ispayment) {
        debugger;
        var keySearch = $("#keyword").val();
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        var delivery = $("#DeliveryType").val();
        if (isdelivery != -1)
        {
            delivery = isdelivery;
        }
        
        var paymentStatus = $("#PaymentStatus").val();
        if (ispayment != -1) {
            paymentStatus = ispayment;
        }
        $("#" + global.Element.JtableOrder).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate, 'delivery': delivery, 'paymentStatus': paymentStatus });
    }
    function reloadViewDetail() {
        var keySearch = $("#vkeyword").val();
        var fromDate = $("#vdatefrom").val();
        var toDate = $("#vdateto").val();
        var employee = $("#cemployee2").val();
        if (employee === 0) {
            employee = global.Data.SaleId;
        }
        $("#" + global.Element.jtableViewDetail).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate });
    }
    function reloadListOrderDetail() {
        $('#' + global.Element.JtableOrderDetail).jtable('load', { 'keyword': "" });
    }
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
    /*function init model using knockout Js*/
    function initViewModel(order) {
        var orderViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (order != null) {
            orderViewModel = {
                Id: ko.observable(order.Id),
                Code: ko.observable(order.Code),
                Name: ko.observable(order.Name),
                Description: ko.observable(order.Description)
            };
        }
        return orderViewModel;
    }
    function bindData(order) {
        global.Data.ModelOrder = initViewModel(order);
        ko.applyBindings(global.Data.ModelOrder);
    }
    /*end function*/
    /*function show Popup*/
    function showPopupDesignProcess() {
        $("#" + global.Element.PopupDesignProcess).modal("show");
    }
    function showPopupPrintProcess() {
        $("#" + global.Element.PopupPrintProcess).modal("show");
    }
    function showPopupPaymentProcess() {
        $("#" + global.Element.PopupPaymentProcess).modal("show");
    }
    function showPopupHaspay() {
        $("#" + global.Element.PopupHasPay).modal("show");
    }
    /*End*/
    function updateDesignUser(id, empId, description) {
        $.ajax({
            url: "/Order/UpdateDesignUser?detailId=" + id + "&employeeId=" + empId + "&description=" + description,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        //reloadListDesign();
                        reloadViewDetail();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updatePrintUser(id, empId, description) {
        $.ajax({
            url: "/Order/UpdatePrintUser?detailId=" + id + "&employeeId=" + empId + "&description=" + description,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        //reloadListDesign();
                        reloadViewDetail();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updatePayment(orderId, payment, paymentType) {
        if (paymentType == 3)
        {
            payment = 0;
        }
        $.ajax({
            url: "/Order/UpdatePayment?orderId=" + orderId + "&payment=" + payment + "&paymentType=" + paymentType,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateHasPay(orderId, payment, paymentType) {
        if (payment == "") payment = 0;
        var a = payment;
        $.ajax({
            url: "/Order/UpdateHaspayCustom?orderId=" + orderId + "&haspay=" + payment + "&paymentType=" + paymentType,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateDelivery(orderId, delivery) {
        $.ajax({
            url: "/Order/UpdateDelivery?orderId=" + orderId + "&delivery=" + delivery,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListOrder();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateApproval(orderId, approval) {
        $.ajax({
            url: "/Order/UpdateApproval?orderId=" + orderId + "&approval=" + approval,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListOrder();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteOrder,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListOrder();
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function initComboBoxDesign() {
        var url = "/Order/GetCustomerByOrganization?shortName=PTK";
        $.getJSON(url, function (datas) {
            $('#dDesignName').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#dDesignName').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#dDesignName').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    //function exportReport() {
    //    var keySearch = $("#keyword").val();
    //    var fromDate = $("#datefrom").val();
    //    var toDate = $("#dateto").val();
    //    var url = "/Order/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate;
    //    $.getJSON(url, function (datas) {
    //    });
    //}
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
    function checkValidate() {
        if ($("#dproduct").val() === "0") {
            toastr.warning("Vui Lòng Chọn Dịch Vụ");
            $("#Name").focus();
            return false;
        }
        else if ($("#dprice").val() === "" || $("#dsubtotal").val() === "" || $("#dquantity").val() === "") {
            toastr.warning("Kiểm Tra số lượng đơn giá và thành tiền");
            return false;
        } else {
            return true;
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
    function initComboBoxBusiness() {
        var url = "/Order/GetCustomerByOrganization?shortName=PKD";
        $.getJSON(url, function (datas) {
            $('#cemployee').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#cemployee').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#cemployee').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });

    }
    function initComboBoxBusiness1() {
        var url = "/Order/GetCustomerByOrganization?shortName=PKD";
        $.getJSON(url, function (datas) {
            $('#cemployee1').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#cemployee1').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
                return datas[0].Value;
            }
            else {
                $('#cemployee1').append('<option value="0">Không Có Dữ Liệu </option>');
                return 0;
            }
        });
    }
    function initComboBoxBusiness2() {
        var url = "/Order/GetCustomerByOrganization?shortName=PKD";
        $.getJSON(url, function (datas) {
            $('#cemployee2').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#cemployee2').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#cemployee2').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function calculatorProduct(arrayProduct) {
        var tempArray = [];
        tempArray.push(arrayProduct[0]);
        for (var i = 1; i < arrayProduct.length; i++) {
            var flat = true;
            for (var j = 0; j < tempArray.length; j++) {
                if (arrayProduct[i].CommodityId === tempArray[j].CommodityId) {
                    flat = false;
                    break;
                }
            }
            if (flat === true) {
                tempArray.push(arrayProduct[i]);
            }
        }
        for (var k = 0; k < tempArray.length; k++) {
            if (k !== tempArray.length - 1) {
                global.Data.Pproduct = global.Data.Pproduct + tempArray[k].CommodityName + ",";
            } else {
                global.Data.Pproduct = global.Data.Pproduct + tempArray[k].CommodityName;
            }
        }
    }
    /*function Init List Using Jtable */
    function initListOrder() {
        $('#' + global.Element.JtableOrder).jtable({
            title: 'Danh Sách Đơn Hàng',
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
                if (data.record.IsDelivery != 2) {
                    data.row.css("background", "#F5ECCE");
                }
                if (data.record.IsDelivery == 2) {
                    data.row.css("background", "#dacfcf");
                }
                if (data.record.PaymentMethol != 0) {
                    data.row.css("background", "#f5cece");
                }
               
            },
            toolbar: {
                items: [{
                    tooltip: 'Click here to export this table to excel',
                    text: 'Export to Excel',
                    click: function () {
                        var keySearch = $("#keyword").val();
                        var fromDate = $("#datefrom").val();
                        var toDate = $("#dateto").val();
                        var employee = $("#cemployee1").val();
                        var delivery = $("#DeliveryType").val();
                        var paymentStatus = $("#PaymentStatus").val();
                        var url = "/Order/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate + "&employee=" + employee + "&keySearch=" + keySearch + "&delivery=" + delivery + "&paymentStatus=" + paymentStatus;
                        window.location = url;
                    }
                }]
            },
            actions: {
                listAction: global.UrlAction.GetListOrder
            },
            datas: {
                jtableId: global.Element.JtableOrder
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
                    title: ' CTĐH',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (orderDetailData) {
                        var $img = $('<a style="color: red;" id="newdetail" href="javascript:void(0)">' + orderDetailData.record.Id + '</a>');
                        $img.click(function () {
                            $('#OrderId').val(orderDetailData.record.Id);

                            $('#jtableOrder').jtable('openChildTable',
                                    $img.closest('tr'),
                                    {
                                        title: 'Chi Tiết Của Đơn hàng:' + orderDetailData.record.Name,
                                        actions: {
                                            listAction: '/Order/ListOrderDetail?OrderId=' + orderDetailData.record.Id,
                                            createAction: global.Element.Popup_OrderDetail
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
                                            DesignProcess: {
                                                visibility: 'fixed',
                                                title: "Thiết Kế",
                                                width: "10%",
                                                display: function (data) {
                                                    var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.DesignUserName + ":" + data.record.strDesignStatus + '</a>');
                                                    text.click(function () {
                                                        $("#dDesignName").val(data.record.DesignUser);
                                                        $("#gDescription").val(data.record.DesignDescription);
                                                        $("#gStatus").val(data.record.PrintStatus);
                                                        global.Data.DetailId = data.record.Id;
                                                        showPopupDesignProcess();
                                                        data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                                                        var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                                                    });
                                                    return text;
                                                }
                                            },
                                            "": {
                                                visibility: 'fixed',
                                                title: "",
                                                width: "1%",
                                                display: function (data) {
                                                    var text = $('<a style="color: red" href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "!" + '</a>');
                                                    text.click(function () {
                                                    });
                                                    return text;
                                                }
                                            },
                                            PrintProcess: {
                                                visibility: 'fixed',
                                                title: "In Ấn",
                                                width: "10%",

                                                display: function (data) {
                                                    var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.PrintUserName + ":" + data.record.strPrinStatus + '</a>');
                                                    text.click(function () {
                                                        $("#gPrintName").val(data.record.PrintUser);
                                                        $("#dDescription").val(data.record.PrintDescription);
                                                        $("#dStatus").val(data.record.DesignStatus);
                                                        global.Data.DetailId = data.record.Id;
                                                        showPopupPrintProcess();
                                                        data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                                                        var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                                                    });
                                                    return text;
                                                }
                                            },
                                            " ": {
                                                visibility: 'fixed',
                                                title: "",
                                                width: "1%",
                                                display: function (data) {
                                                    var text = $('<a style="color: red" href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "!" + '</a>');
                                                    text.click(function () {
                                                    });
                                                    return text;
                                                }
                                            },
                                        }
                                    }, function (data) { //opened handler
                                        data.childTable.jtable('load');
                                    });
                        });
                        return $img;
                    }
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên ĐH",
                    width: "12%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.Name + '</a>');
                        text.click(function () {
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                            $('#date').val(data.record.StrDeliveryDate);
                            // var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");

                            global.Data.CustomerId = data.record.CustomerId;
                            $("#cname").attr("disabled", true);
                            $("#cphone").attr("disabled", true);

                            $("#cemployee").val(data.record.CreatedForUser);

                            $("#cname").val(data.record.Name);
                            $("#cphone").val(data.record.CustomerPhone);
                            $("#cmail").val(data.record.CustomerEmail);
                            $("#caddress").val(data.record.CustomerAddress);
                            $("#ctaxcode").val(data.record.CustomerTaxCode);
                            global.Data.OrderId = data.record.Id;
                            while (global.Data.ModelOrderDetail.length) {
                                global.Data.ModelOrderDetail.pop();
                            }

                            global.Data.ModelOrderDetail.push.apply(global.Data.ModelOrderDetail, data.record.T_OrderDetail);
                            global.Data.Index = global.Data.ModelOrderDetail[global.Data.ModelOrderDetail.length - 1].Index + 1;
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
                            //popAllElementInArray(global.Data.ModelOrderDetail);
                            $('.nav-tabs a:last').tab('show');

                        });
                        return text;
                    }
                },
                StrCreatedDate: {
                    title: 'Ngày Tạo',
                    width: "8%"
                },
                strSubTotal: {
                    title: "Tổng Tiền",
                    width: "7%"
                },
                strHaspay: {
                    title: "Đã Thu",
                    width: "7%",
                    display: function (data) {
                        var text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="Cập nhật số tiền đã thu.">' + data.record.strHaspay + '</a>');
                        text.click(function () {
                            global.Data.OrderId = data.record.Id;
                            showPopupHaspay();
                        });
                        return text;
                    }
                },
             
                StrHas: {
                    title: "Còn Lại",
                    width: "7%",
                    display: function (data) {
                        var text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + (data.record.SubTotal - data.record.HasPay).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</a>');
                        return text;
                    }
                },
                //StrPaymentType: {
                //    visibility: "fixed",
                //    title: "Loại Thanh Toán",
                //    width: "7%",
                //    display: function (data) {
                //        var text = "";
                //        if (data.record.PaymentMethol == 3)
                //        { text = $("<a href=\"javascript:void(0)\" style=\"color:#89798d;\"   class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + "Công Nợ" + "</a>"); }
                //        else if (data.record.PaymentMethol == 0) {
                //            { text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\"><span class=\"fa fa-info-circle fa-lg\" aria-hidden=\"true\"></span></a>"); }
                //        }
                //        else  {
                //            text = $("<a href=\"javascript:void(0)\" style=\"color:#89798d;\"   class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrPaymentType + "</a>");
                //        }
                //        text.click(function () {
                //            // updateStatus(data.record.Id, data.record.DesignStatus);
                //        });
                //        return text;
                //    }
                //},
                PaymentProcess: {
                    visibility: 'fixed',
                    title: "Thanh Toán",
                    width: "7%",
                    display: function (data) {
                        var text = "";
                        if (data.record.PaymentMethol == 3)
                        { text = $("<a href=\"javascript:void(0)\"   class=\"clickable\" title=\"Cập nhật thanh toán.\">" + "Công Nợ" + "</a>"); }
                        else if (data.record.PaymentMethol == 0) {
                            { text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật thanh toán.\"><span class=\"fa fa-money fa-lg\" aria-hidden=\"true\"></span></a>"); }
                        }
                        else {
                            text = $("<a href=\"javascript:void(0)\"  class=\"clickable\" title=\"Cập nhật thanh toán.\">" + data.record.StrPaymentType + "</a>");
                        }
                        text.click(function () {
                            global.Data.NumberDetail = data.record.T_OrderDetail.length;
                            $("#type2").prop("checked", true);
                            $("#ppay").val("");
                            $("#prest").val("");
                            $("#ptotal").val(data.record.strSubTotal);
                            $("#phaspay").val(data.record.strHaspay);
                            var a1 = data.record.SubTotal - data.record.HasPay;
                            var b = a1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            $("#ppayment").val(b);
                            $("#prealpay").val(b);
                            global.Data.OrderId = data.record.Id;
                            global.Data.PcustomerName = data.record.Name;
                            global.Data.PcustomePhone = data.record.CustomerPhone;
                            global.Data.PcustomerAddress = data.record.CustomerAddress;
                            global.Data.Pproduct = "";
                            calculatorProduct(data.record.T_OrderDetail);
                            //rendertable                                                    
                            renderTable(data.record.T_OrderDetail);
                            showPopupPaymentProcess();
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                            var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                        });
                        return text;
                    }
                },
                strIsApproval: {
                    title: "Duyệt",
                    width: "3%",
                    display: function (data) {
                        var text = "";
                        if (data.record.IsApproval == true)
                        { text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật duyệt đơn hàng.\"><span class=\"fa fa-check-square-o fa-lg\" aria-hidden=\"true\"></span></a>"); }
                        else {
                            text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật duyệt đơn hàng.\"><span class=\"fa fa-times-circle fa-lg\" aria-hidden=\"true\"></span></a>");
                        }
                        text.click(function () {
                            updateApproval(data.record.Id, data.record.IsApproval);
                        });
                        return text;
                    }
                },

                StrHasDelivery: {
                    visibility: "fixed",
                    title: "GHàng",
                    width: "3%",
                    display: function (data) {
                        var text = "";
                        if (data.record.IsDelivery==2)
                        { text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật giao hàng.\"><span class=\"fa fa-check-square-o fa-lg\" aria-hidden=\"true\"></span></a>"); }
                        else {
                            text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật giao hàng.\"><span class=\"fa fa-times-circle fa-lg\" aria-hidden=\"true\"></span></a>");
                        }
                        text.click(function () {
                            updateDelivery(data.record.Id, data.record.IsDelivery);
                        });
                        return text;
                    }
                },
                CreateUserName: {
                    title: "NV Kinh Doanh",
                    width: "10%"
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
                }
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
    function initListViewDetail() {
        $("#" + global.Element.jtableViewDetail).jtable({
            title: "Danh Sách Chi Tiết Đơn Hàng",
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            selectShow: true,
            toolbar: {
                items: [{
                    tooltip: "Click here to export this table to excel",
                    text: "Export to Excel",
                    click: function () {
                        var keySearch = $("#vkeyword").val();
                        var fromDate = $("#vdatefrom").val();
                        var toDate = $("#vdateto").val();
                        var employee = $("#cemployee2").val();
                        var url = "/Order/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate + "&employee=" + employee + "&keySearch=" + keySearch;
                        window.location = url;
                    }
                }]
            },
            actions: {
                listAction: global.UrlAction.GetListViewDetail
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
                CreatedDate: {
                    title: 'Ngày Tạo',
                    width: "5%",
                    type: 'date',
                    displayFormat: 'dd-mm-yy'
                },

                CustomerName: {
                    title: "Tên Khách Hàng",
                    width: "7%"
                },
                CommodityName: {
                    title: "Tên Dịch Vụ",
                    width: "7%"
                },
                FileName: {
                    title: "Tên File",
                    width: "7%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "7%"
                },

                Width: {
                    title: "Chiều Ngang",
                    width: "3%"
                },
                Height: {
                    title: "Chiều Cao",
                    width: "3%"
                },
                Square: {
                    title: 'Diện Tích',
                    width: "3%"
                },

                Quantity: {
                    title: "Số Lượng",
                    width: "3%"
                },
                strPrice: {
                    title: "Đơn Giá",
                    width: "3%"
                },
                strSubTotal: {
                    title: "Thành Tiền",
                    width: "3%"
                },
                DesignProcess: {
                    visibility: 'fixed',
                    title: "Thiết Kế",
                    width: "10%",
                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.DesignUserName + ":" + data.record.strDesignStatus + '</a>');
                        text.click(function () {

                            $("#dDesignName").val(data.record.DesignUser);
                            $("#gDescription").val(data.record.DesignDescription);
                            $("#gStatus").val(data.record.PrintStatus);
                            global.Data.DetailId = data.record.Id;
                            showPopupDesignProcess();
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                            var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                        });
                        return text;
                    }
                },
                "": {
                    visibility: 'fixed',
                    title: "",
                    width: "1%",
                    display: function (data) {
                        var text = $('<a style="color: red" href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "!" + '</a>');
                        text.click(function () {
                        });
                        return text;
                    }
                },
                PrintProcess: {
                    visibility: 'fixed',
                    title: "In Ấn",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.PrintUserName + ":" + data.record.strPrinStatus + '</a>');
                        text.click(function () {
                            $("#gPrintName").val(data.record.PrintUser);
                            $("#dDescription").val(data.record.PrintDescription);
                            $("#dStatus").val(data.record.DesignStatus);
                            global.Data.DetailId = data.record.Id;
                            showPopupPrintProcess();
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                            var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                        });
                        return text;
                    }
                },
                " ": {
                    visibility: 'fixed',
                    title: "",
                    width: "1%",
                    display: function (data) {
                        var text = $('<a style="color: red" href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + "!" + '</a>');
                        text.click(function () {
                        });
                        return text;
                    }
                }
            }
        });
    }
    function initComboBoxAllProduct(productId) {
        var url = "/Order/GetListProduct?productType=0";
        $.getJSON(url, function (datas) {
            $("#dproduct").empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $("#dproduct").append('<option value="' + datas[i].Value + '">' + datas[i].Text + "</option>");
                }
                $("#dproduct").val(productId);
            }
            else {
                $('#dproduct').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initListOrderDetail() {
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
            fields: {
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
                    title: "Chiều dài",
                    width: "5%"
                },
                Height: {
                    title: "Chiều Rộng",
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
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;

                    }
                }
            }
        });
    }
    /*End init List */
    function resetAll() {
        $("#cemployee").val(0);
        $("#dsumsquare").val("");
        $("#cname").val("");
        $("#cphone").val("");
        $("#cmail").val("");
        $("#caddress").val("");
        $("#ctaxcode").val("");
        $("#dproductType").val(0);
        $("#dproduct").val(0);
        $("#dfilename").val("");
        $("#dnote").val("");
        $("#dwidth").val("");
        $("#dheignt").val("");
        $("#dsquare").val("");
        $("#dquantity").val("");
        $("#dprice").val("");
        $("#dsubtotal").val("");
    }
    /*function Save */
    function saveOrder() {
        if ($("#cemployee").val() !== "0") {
            if ($("#cname").val() !== "") {
                if (global.Data.ModelOrderDetail.length !== 0) {
                    //global.Data.OrderTotal = 0;
                    //for (var j = 0; j < global.Data.ModelOrderDetail.length; j++) {
                    //    global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[j].SubTotal);
                    //}
                    var employeeId = $("#cemployee").val();
                    var customerName = $("#cname").val();
                    var customerPhone = $("#cphone").val();
                    var customerMail = $("#cmail").val();
                    var customerAddress = $("#caddress").val();
                    var customerTaxCode = $("#ctaxcode").val();
                    var dateDelivery = $("#date").val();
                    $.ajax({
                        url: global.UrlAction.SaveOrder + "?orderId=" + global.Data.OrderId + "&employeeId=" + employeeId + "&customerId=" + global.Data.CustomerId + "&customerName=" + customerName + "&customerPhone=" + customerPhone + "&customerMail=" + customerMail + "&customerAddress=" + customerAddress + "&customerTaxCode=" + customerTaxCode + "&dateDelivery=" + dateDelivery + "&orderTotal=" + global.Data.OrderTotal,
                        type: 'post',
                        data: JSON.stringify({ 'listDetail': global.Data.ModelOrderDetail }),
                        contentType: 'application/json',
                        success: function (result) {
                            $('#loading').hide();
                            GlobalCommon.CallbackProcess(result, function () {
                                if (result.Result === "OK") {
                                    resetAll();
                                    toastr.success("Tạo mới Đơn hàng thành công");
                                    reloadListOrder();
                                    reloadViewDetail();
                                    $('.nav-tabs a:first').tab('show');
                                }
                            }, false, global.Element.PopupOrder, true, true, function () {
                                var msg = GlobalCommon.GetErrorMessage(result);
                                GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                            });
                        }
                    });
                } else {
                    toastr.warning("Không có chi tiết đơn hàng nào");
                }

            } else {
                toastr.warning("Vui lòng nhập tên khách hàng");
            }
        } else {
            toastr.warning("Chọn nhân viên kinh doanh");
        }

    }
    /*End Save */
    /*init Combobox*/
    function initComboBox() {
        var url = "/Order/GetListProductType";
        $.getJSON(url, function (datas) {
            $('#dproductType').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#dproductType').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#dproductType').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initComboBoxPrint() {
        var url = "/Order/GetCustomerByOrganization?shortName=BPI";
        $.getJSON(url, function (datas) {
            $('#gPrintName').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#gPrintName').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#gPrintName').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initComboBoxProduct(id) {
        var url = "/Order/GetListProduct?productType=" + id;
        $.getJSON(url, function (datas) {
            $('#dproduct').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#dproduct').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#dproduct').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    /* End*/
    /* Region Register and init bootrap Popup*/
    function initPopupOrder() {
        $("#" + global.Element.PopupOrder).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupOrder + "button[save]").click(function () {
            saveOrder();
        });
        $("#" + global.Element.PopupOrder + "button[cancel]").click(function () {
            $("#" + global.Element.PopupOrder).modal("hide");
        });
    }
    function initPopupDesignProcess() {
        $("#" + global.Element.PopupDesignProcess).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupDesignProcess + " button[save]").click(function () {
            var designId = $("#dDesignName").val();

            var description = $("#dDescription").val();
            updateDesignUser(global.Data.DetailId, designId, description);
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableDesign", global.Data.ClientId, "Cập nhật");
            $.connection.hub.start();
            $("#" + global.Element.PopupDesignProcess).modal("hide");
            reloadListOrder();
        });
        $("#" + global.Element.PopupDesignProcess + " button[cancel]").click(function () {
            $("#" + global.Element.PopupDesignProcess).modal("hide");
        });
    }
    function initPopupPaymentProcess() {
        $("#" + global.Element.PopupPaymentProcess).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPaymentProcess + " button[save]").click(function () {
            var orderId = global.Data.OrderId;
            var payment = $("#prealpay").val();
            var paymentType = $("#PaymentType").val();
            updatePayment(orderId, payment, paymentType);
            //var designId = $("#dDesignName").val();

            //var description = $("#dDescription").val();
            //updateDesignUser(global.Data.DetailId, designId, description);
            //global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            //var realTimeHub = $.connection.realTimeJTableDemoHub;
            //realTimeHub.server.sendUpdateEvent("jtableDesign", global.Data.ClientId, "Cập nhật");
            //$.connection.hub.start();
            $("#" + global.Element.PopupPaymentProcess).modal("hide");
            reloadListOrder();
        });
        $("#" + global.Element.PopupPaymentProcess + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPaymentProcess).modal("hide");
        });
        $("#" + global.Element.PopupPaymentProcess + " a[print]").click(function () {
            var time = new Date().toLocaleDateString('en-GB');
            $("#no").html("");
            $("#vcustomer").html("");
            $("#vphone").html("");
            $("#vaddress").html("");
            $("#vproduct").html("");
            $("#vtotal").html("");
            $("#strtotal").html("");
            $("#vdate2").html("");
            $("#vdate1").html("");
            //////////////////////////////////////////////////////
            $("#no").append(global.Data.OrderId);
            $("#vcustomer").append(global.Data.PcustomerName);
            $("#vphone").append(global.Data.PcustomePhone);
            $("#vaddress").append(global.Data.PcustomerAddress);
            $("#vproduct").append(global.Data.Pproduct);

            var payment = $("#prealpay").val();
            var tempValue = payment.replace(/[^0-9-.]/g, '');
            var b = parseFloat(tempValue);
            var c = docso(b);
            $("#vtotal").append(payment);
            $("#strtotal").append(c);
            $("#vdate2").append(time);
            $("#vdate1").append(time);
            // printPanel();

            //custome Print reset
            $("#no1").html("");
            $("#vcustomer1").html("");
            $("#vphone1").html("");
            $("#vaddress1").html("");
            $("#vdate3").html("");
            $("#vtotal1").html("");
            $("#strtotal1").html("");


            //custome Print set value
            $("#no1").append(global.Data.OrderId);
            $("#vcustomer1").append(global.Data.PcustomerName);
            $("#vphone1").append(global.Data.PcustomePhone);
            $("#vaddress1").append(global.Data.PcustomerAddress);
            $("#vtotal1").append(payment);
            $("#strtotal1").append(c);
            $("#vdate3").append(time);
            var check = document.getElementById('type2').checked;

            if (check) {
                if (global.Data.NumberDetail > 9) {
                    toastr.warning('Chi tiết đơn hàng này quá nhiều vui lòng chọn kiểu in khác');
                }
                else {
                    printPanel1();
                }

            }
            else {
                printPanel();
            }

        });
    }
    function initPopupPrintProcess() {
        $("#" + global.Element.PopupPrintProcess).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPrintProcess + " button[save]").click(function () {
            var printId = $("#gPrintName").val();
            var description = $("#gDescription").val();
            updatePrintUser(global.Data.DetailId, printId, description);
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePrint", global.Data.ClientId, "Cập nhật");
            $.connection.hub.start();
            $("#" + global.Element.PopupPrintProcess).modal("hide");
            reloadListOrder();
        });
        $("#" + global.Element.PopupPrintProcess + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPrintProcess).modal("hide");
        });
    }
    function initPopupSearch() {
        $("#" + global.Element.PopupSearch).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupSearch + 'button[save]').click(function () {
            reloadListOrder();
        });
        $("#" + global.Element.PopupSearch + ' button[cancel]').click(function () {
            $("#" + global.Element.PopupSearch).modal("hide");
        });
    }
    function initPopupHasPay() {
        $("#" + global.Element.PopupHasPay).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupHasPay + ' button[save]').click(function () {
            var orderId = global.Data.OrderId;
            var payment = $("#haspay").val();
            var paymentType = $("#PaymentType1").val();
            updateHasPay(orderId, payment, paymentType);
            reloadListOrder();
            $("#" + global.Element.PopupHasPay).modal("hide");
        });
        $("#" + global.Element.PopupHasPay + ' button[cancel]').click(function () {
            $("#" + global.Element.PopupHasPay).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListOrder = function () {
        reloadListOrder();
    };
    this.reloadViewDetail = function () {
        reloadViewDetail();
    };
    this.initViewModel = function (order) {
        initViewModel(order);
    };
    this.bindData = function (order) {
        bindData(order);
    };
    var registerEvent = function () {
        $("#dproductType").change(function () {
            var id = $(this).val();
            global.Data.ProductTypeId = id;
            initComboBoxProduct(id);
        });
        $("#saveOrder").click(function () {
            saveOrder();
        });
        $("#dproduct").change(function () {
            $.ajax({
                url: "/Order/GetPriceForCustomerAndProduct?customerId=" + global.Data.CustomerId + "&productId=" +$(this).val(),
                type: 'post',
                contentType: 'application/json',
                success: function (result) {
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Records != 0) {
                            var temp = result.Records.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            $('#dprice').val(temp);
                        } else {
                            $('#dprice').val('');
                        }

                    }, false, global.Element.PopupOrder, true, true, function () {
                    });
                }
            });
            //$("#dfilename").val("");
            //$("#dnote").val("");
            //$("#dwidth").val("");
            //$("#dheignt").val("");
            //$("#dsquare").val("");
            //$("#dquantity").val("");
            //$("#dprice").val("");
            //$("#dsubtotal").val("");
            //if (productTypeId !== "1") {
            //    $(".forPrint").css({ "display": "none" });
            //} else {
            //    $(".forPrint").css({ "display": "inline" });
            //}
        });
        //$("[save]").click(function () {
        //    saveOrder();
        //    var realTimeHub = $.connection.realTimeJTableDemoHub;
        //    realTimeHub.server.sendUpdateEvent("jtableOrder");
        //    $.connection.hub.start();
        //});
        $("[cancel]").click(function () {
            bindData(null);
        });
        $("[search]").click(function () {
            reloadListOrder();
        });
        $("#resetDetail").click(function () {
            resetDetail();
            global.Data.CurenIndex = 0;
        });
        $("#resetOrder").click(function () {
            if (global.Data.OrderId !== 0) {
                $("#cemployee").val(0);
                $("#cmail").val("");
                $("#caddress").val("");
                $("#ctaxcode").val("");
                $("#date").val("");
                $("#dproductType").val(0);
                $("#dproduct").val(0);
                $("#dfilename").val("");
                $("#dnote").val("");
                $("#dwidth").val("");
                $("#dheignt").val("");
                $("#dsumsquare").val("");
                $("#dsquare").val("");
                $("#dquantity").val("");
                $("#dprice").val("");
                $("#dsubtotal").val("");

            } else {
                global.Data.OrderId = 0;
                resetAll();
            }
            global.Data.CurenIndex = 0;
        });
        $("#ProcessOrder").click(function () {
            $("#cname").attr("disabled", false);
            $("#cphone").attr("disabled", false);
        });
        $("#search").click(function () {
            reloadListOrder(-1,-1);
        });
        $("#noDelivery").click(function () {
            reloadListOrder(1,-1);
        });
        $("#Deliveried").click(function () {
            reloadListOrder(2,-1);
        });
        $("#paid").click(function () {
            reloadListOrder(-1, 1);
        });
        $("#vsearch").click(function () {
            reloadViewDetail();
        });
        $("#CreateOrder").click(function () {
            document.getElementById("date").defaultValue = new Date().toISOString().substring(0, 10);
            $("#cname").attr("disabled", false);
            $("#cphone").attr("disabled", false);
            resetAll();
            var ClientId = document.getElementById("ClientId").innerHTML;
            global.Data.CurenIndex = 0;
            $("#cemployee").val(ClientId).change();
            
            global.Data.Index = 1;
            global.Data.OrderId = 0;
            while (global.Data.ModelOrderDetail.length) {
                global.Data.ModelOrderDetail.pop();
            }
            reloadListOrderDetail();
        });
        $("#canelOrder").click(function () {
            resetAll();
            global.Data.CurenIndex = 0;
            global.Data.Index = 1;
            while (global.Data.ModelOrderDetail.length) {
                global.Data.ModelOrderDetail.pop();
            }
            reloadListOrderDetail();
            $("#cname").attr("disabled", false);
            $("#cphone").attr("disabled", false);
            $('.nav-tabs a:first').tab('show');
        });
        $("#cname").keydown(function () {
            global.Data.CustomerId = 0;
            $('#dprice').val('');
            $("#cphone").val("");
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
        });
        $("#cphone").keydown(function () {
            global.Data.CustomerId = 0;
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
        });
        $("#dsubtotal").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dsubtotal").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#dprice").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#dprice").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
        $("#prealpay").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
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
        $('.caculator').keyup(function () {
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
                }
                else {
                    $("#dsubtotal").val("");
                }
            }

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
                        var object = { Index: objectIndex, CommodityId: $("#dproduct").val(), CommodityName: $("#dproduct option:selected").text(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
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

                    } else {
                        var object1 = { Index: objectIndex, CommodityId: $("#dproduct").val(), CommodityName: $("#dproduct option:selected").text(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        global.Data.ModelOrderDetail.push(object1);
                        global.Data.Index = global.Data.Index + 1;
                        for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //global.Data.OrderTotal = global.Data.OrderTotal.replace(/[^0-9-.]/g, '');
                    }

                    global.Data.CurenIndex = 0;
                    reloadListOrderDetail();
                    resetDetail();
                }
            }
        });
        $('#datefrom').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadListOrder();
            }
        });
        $('#dateto').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadListOrder();
            }
        });
        $('#cemployee1').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadListOrder();
            }
        });
        $('#keyword').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadListOrder();
            }
        });
        $('#vdatefrom').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadViewDetail();
            }
        });
        $('#vdateto').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadViewDetail();
            }
        });
        $('#cemployee2').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadViewDetail();
            }
        });
        $('#vkeyword').keydown(function (e) {
            if (e.which === 13) { //Enter
                e.preventDefault();
                reloadViewDetail();
            }
        });
    };
    function mappingAutoComplete() {
        $(function () {
            $("#cname").autocomplete({
                source: global.Data.ListCustomerName,
                select: function (a, b) {
                    var cusName = b.item.value;
                    $.ajax({
                        url: "/Order/GetCustomerByName?customerName=" + cusName,
                        type: 'post',
                        contentType: 'application/json',
                        success: function (result) {
                            GlobalCommon.CallbackProcess(result, function () {
                                if (1 < 2) {
                                    var listCustomer = result.Records;
                                    $('#cname').val(listCustomer.Name);
                                    $('#cphone').val(listCustomer.Mobile);
                                    $('#cmail').val(listCustomer.Email);
                                    $('#caddress').val(listCustomer.Address);
                                    $('#ctaxcode').val(listCustomer.TaxCode);
                                    global.Data.CustomerId = listCustomer.Id;
                                }

                            }, false, global.Element.PopupOrder, true, true, function () {
                                var msg = GlobalCommon.GetErrorMessage(result);
                                GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                            });
                        }
                    });
                }
            });
        });
    }
    this.Init = function () {
        registerEvent();
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
        document.getElementById("vdatefrom").defaultValue = new Date(new Date() - 24 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("vdateto").defaultValue = new Date().toISOString().substring(0, 10);
        initComboBoxBusiness();
        initComboBoxBusiness1();
        initComboBoxBusiness2();
        initComboBoxDesign();
        initComboBoxPrint();
        initListOrder();
        initListViewDetail();
        reloadListOrder();
        initComboBox();

        initListOrderDetail();
        reloadViewDetail();
        reloadListOrderDetail();
        initPopupOrder();
        initPopupDesignProcess();
        initPopupPrintProcess();
        initPopupPaymentProcess();
        initPopupHasPay();
        initPopupSearch();

        bindData(null);



        $.ajax({
            url: "/Order/GetAllCustomer",
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                GlobalCommon.CallbackProcess(result, function () {
                    if (1 < 2) {
                        global.Data.ListCustomerName = result.Records;
                        mappingAutoComplete();
                    }

                }, false, global.Element.PopupOrder, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    };
};
/*End Region*/
$(document).ready(function () {
    var order = new VINASIC.Order();
    order.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    {
        toastr.warning("Vui lòng chỉ nhập số.");
        return false;
    }
    
}