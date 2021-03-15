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
            DeleteOrder: "/Order/DeleteOrder",
            GetListDesign: "/Employee/GetListDetailForBusiness",
        },
        Element: {
            JtableOrder: "jtableOrder",
            JtableOrderDetail: "jtableOrderDetail",
            jtableViewDetail: "jtableViewDetail",
            JtableDesign: "jtableSubOrder",
            PopupOrder: "popup_Order",
            PopupSearch: "popup_SearchOrder",
            PopupDesignProcess: "popup_DesignProcess",
            PopupHasPay: "popup_HasPay",
            PopupCost: "popup_Cost",
            popupCustom: "popup_Custom",
            popupCustom1: "popup_Custom1",
            popupCustom2: "popup_Custom2",
            popupCustom3: "popup_Custom3",
            PopupPaymentProcess: "popup_PaymentProcess",
            PopupPrintProcess: "popup_PrintProcess",
            PopupNotification: "popup_notification",
            JtableCost: "jtableCost",
            PopupCostEdit: "popup_cost_edit"
        },
        Data: {
            listproduct: [],
            listproductName: [],
            ModelOrder: {},
            ModelOrderDetail: [],
            ModelConfig: {},
            ListCustomerName: [],
            ListEmployeePrint: [],
            ListEmployeeDesign: [],
            ListEmployeeAddon: [],
            listCost: [],
            ProductTypeId: 0,
            CustomerId: 0,
            OrderId: 0,
            OrderDetailId: 0,
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
            PBusinessName: "",
            PcustomerAddress: "",
            Pdate: "",
            Pproduct: "",
            NumberDetail: 0,
            IdOrderStatus: 0,
            IdDetailStatus: 0,
            DetailStatus: 0,
            IdForView: 0,
            ProductPrice: [],
            ProductPrice1: [],
            ForView: 0,
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function renderTable(Table, subTotal, haspay, type) {
        var tableString = "<table id=\"renderTable\" border=\"1\" style=\"width:100%\" cellspacing=\"0\" cellpadding=\"0\">";
        var root = document.getElementById('Block4');
        document.getElementById("Block4").innerHTML = "";
        tableString += "<tr>";
        tableString += "<th style=\"padding-left: 5px;text-align: left;\">" + "Dịch Vụ" + "</th>";
        tableString += "<th style=\"padding-left: 5px;text-align: left;\">" + "Tên File" + "</th>";
        if (document.getElementById('show-dim').checked) {
            tableString += "<th>" + "CNgang" + "</th>";
            tableString += "<th>" + "CCao" + "</th>";
        }

        tableString += "<th>" + "SLượng" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Đơn Giá" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Thành Tiền" + "</th>";
        tableString += "</tr>";
        for (row = 0; row < Table.length; row += 1) {
            var strPrice = Table[row].Price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            var strSubTotal = Table[row].SubTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            tableString += "<tr>";
            if (Table[row].FileName == null) {
                Table[row].FileName = '';
            }
            if (Table[row].Height == null || Table[row].Height == 0) {
                Table[row].Height = '';
            }
            if (Table[row].Width == null || Table[row].Width == 0) {
                Table[row].Width = '';
            }
            tableString += "<td style=\"padding-left: 5px;\">" + Table[row].CommodityName + "</td>";
            tableString += "<td style=\"padding-left: 5px;\">" + Table[row].FileName + "</td>";
            if (document.getElementById('show-dim').checked) {
                tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Width + "</td>";
                tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Height + "</td>";
            }

            tableString += "<td style=\"padding-center: 5px;text-align: center;\">" + Table[row].Quantity + "</td>";
            if (type == 0) {
                tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + strPrice + "</td>";
                tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + strSubTotal + "</td>";
            } else {
                tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + '' + "</td>";
                tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + '' + "</td>";

            }

            tableString += "</tr>";
        }

        if (Table.length < 9) {
            var remain = 9 - Table.length;
            for (i = 0; i < remain; i += 1) {
                tableString += "<tr>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";

                if (document.getElementById('show-dim').checked) {
                    tableString += "<td>" + "&nbsp;" + "</td>";
                    tableString += "<td>" + "&nbsp;" + "</td>";
                }
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "</tr>";
            }
        }
        var strThanhToan = "Tổng Tiền";
        if (haspay > 0) {
            strThanhToan = "Còn Lại";
            var strSubTotal1 = subTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            var strHaspay1 = haspay.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            tableString += "<tr>";
            var colspan = 4;
            if (document.getElementById('show-dim').checked)
                colspan = 6;
            tableString += "<td colspan=\"" + colspan + "\">Tổng Tiền:</td>";
            tableString += "<td style=\"padding-right: 5px;;text-align: right;\"><span id=\"vtotal2\">" + strSubTotal1 + "</span></td>";
            tableString += "</tr>";

            tableString += "<tr>";
            var colspan = 4;
            if (document.getElementById('show-dim').checked)
                colspan = 6;
            tableString += "<td colspan=\"" + colspan + "\">Đã Thanh Toán(Đặt Cọc):</td>";
            tableString += "<td style=\"padding-right: 5px;;text-align: right;\"><span id=\"vtotal3\">" + strHaspay1 + "</span></td>";
            tableString += "</tr>";

        }
        tableString += "<tr>";
        var colspan = 4;
        if (document.getElementById('show-dim').checked)
            colspan = 6;
        tableString += "<td colspan=\"" + colspan + "\">" + strThanhToan + ":</td>";
        tableString += "<td style=\"padding-right: 5px;;text-align: right;\"><span id=\"vtotal1\">55577854</span></td>";
        tableString += "</tr>";
        tableString += "<tr>";
        tableString += "<td colspan=\"" + (colspan + 1) + "\">Bằng Chữ:<span id=\"strtotal1\">Test </span></td>";
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
            //updateOrderStatus(global.Data.IdOrderStatus, 2);
        }, 500);
        return false;
    }
    function reloadListOrder() {
        
        var keySearch = $("#keyword").val();
        //var fromDate = $("#datefrom").val();
        //var toDate = $("#dateto").val();
        moment.utc($("#datefrom").val()).toJSON().slice(0, 10);
        moment.utc($("#dateto").val()).toJSON().slice(0, 10);
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        var delivery = $("#DeliveryType").val();
        var paymentStatus = $("#PaymentStatus").val();
        $("#" + global.Element.JtableOrder).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate, 'orderStatus': 1, fromApproval:1 });

    }
    function reloadListCost() {
        var keySearch = "";
        $("#" + global.Element.JtableCost).jtable("load", { 'keyword': keySearch });
    }
    function reloadListDesign() {
        var keySearch = $("#subkeyword").val();
        var fromDate = $("#subdatefrom").val();
        var toDate = $("#subdateto").val();
        var employee = $("#subcemployee1").val();
        $("#" + global.Element.JtableDesign).jtable("load", { 'keyword': keySearch, 'fromDate': fromDate, 'toDate': toDate, 'employee': employee });
    }
    function reloadListOrder(orderStatus) {
        
        if (orderStatus == undefined) {
            orderStatus = -1;
        }
        var keySearch = $("#keyword").val();
        //var fromDate = $("#datefrom").val();
        //var toDate = $("#dateto").val();
        moment.utc($("#datefrom").val()).toJSON().slice(0, 10);
        moment.utc($("#dateto").val()).toJSON().slice(0, 10);
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        var delivery = $("#DeliveryType").val();
        $("#" + global.Element.JtableOrder).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate, 'orderStatus': orderStatus, fromApproval: 1 });
    }
    function reloadListOrderSpecial(orderStatus) {
        var keySearch = $("#keyword").val();
        //var fromDate = $("#datefrom").val();
        //var toDate = $("#dateto").val();
        moment.utc($("#datefrom").val()).toJSON().slice(0, 10);
        moment.utc($("#dateto").val()).toJSON().slice(0, 10);
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var employee = $("#cemployee1").val();
        var delivery = $("#DeliveryType").val();
        $("#" + global.Element.JtableOrder).jtable("load", { 'keyword': keySearch, 'employee': employee, 'fromDate': fromDate, 'toDate': toDate, 'orderStatus': orderStatus });
        setTimeout(function () { $('#jtableOrder').jtable('getRowByKey', global.Data.OrderId).find("a#newdetail").trigger('click'); }, 1000);

    }
    function reloadViewDetail() {
        var keySearch = '';
        $("#" + global.Element.jtableViewDetail).jtable("load", { 'keyword': keySearch, 'orderId': global.Data.IdForView });
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
    function removeItemInArray1(arr, id) {
        if (typeof (arr) != "undefined") {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].Id === id) {
                    arr.splice(i, 1);
                    break;
                }
            };
        }
    }
    function search(nameKey, myArray) {
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].Text === nameKey) {
                return myArray[i];
            }
        }
    }
    function searchById(key, myArray) {
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].Value === key) {
                return myArray[i];
            }
        }
    }
    function searchByCode(key, myArray) {
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].Code === key) {
                return myArray[i];
            }
        }
    }
    function getProductPrice(productCode, quantity) {
        var listProductTemp = [];
        var result = {};
        var data = global.Data.ProductPrice;
        if ($('#customerType').val() == 0) {
            data = global.Data.ProductPrice1;
        }
        for (var i = 0; i < data.length; i++) {
            if (data[i].Code === productCode) {
                listProductTemp = data[i].Products;
            }
        }
        if (listProductTemp.length > 0) {
            for (var i = 0; i < listProductTemp.length; i++) {
                if (listProductTemp[i].Min != "0" && listProductTemp[i].Max != "0") {
                    if (parseFloat(listProductTemp[i].Min) <= quantity && quantity <= listProductTemp[i].Max) {

                        result = { price: parseFloat(listProductTemp[i].Price), isFixed: listProductTemp[i].isFixed }
                        return result;
                    }
                }
                else if (listProductTemp[i].Min == "0" && listProductTemp[i].Max != "0") {
                    if (quantity <= parseFloat(listProductTemp[i].Max)) {
                        result = { price: parseFloat(listProductTemp[i].Price), isFixed: listProductTemp[i].isFixed }
                        return result;
                    }
                }
                else if (listProductTemp[i].Max == "0") {
                    result = { price: parseFloat(listProductTemp[i].Price), isFixed: listProductTemp[i].isFixed }
                    return result;
                }
            }

        } else {
            return 0;
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

        //$('#DesignDescription').val('');
        //$('#DesignDescription').val(content);
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
    function showPopupCost() {
        $("#" + global.Element.PopupCost).modal("show");
    }
    function showPopupCustom() {
        $("#" + global.Element.popupCustom).modal("show");
    }
    function showPopupCustom1() {
        $("#" + global.Element.popupCustom1).modal("show");
    }
    function showPopupCustom2() {
        $("#" + global.Element.popupCustom2).modal("show");
    }
    function showPopupCustom3() {
        $("#" + global.Element.popupCustom3).modal("show");
    }
    function showPopupNotification() {
        $("#" + global.Element.PopupNotification).modal("show");
    }
    function showPopupCost() {
        $("#" + global.Element.PopupCost).modal("show");
    }
    function showPopupCostEdit() {
        $("#" + global.Element.PopupCostEdit).modal("show");
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
    function updatePayment(orderId, payment, paymentType, transferDescription) {
        if (paymentType == 3) {
            payment = 0;
        }
        $.ajax({
            url: "/Order/UpdatePayment?orderId=" + orderId + "&payment=" + payment + "&paymentType=" + paymentType + "&transferDescription=" + transferDescription,
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
    function updateCost(orderId, cost) {
        if (cost == "") cost = 0;
        $.ajax({
            url: "/Order/UpdateCost?orderId=" + orderId + "&cost=" + cost,
            type: 'post',
            data: JSON.stringify(global.Data.listCost),
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
    function SaveCostEdit() {
        var cost = { Id: $("#cost-id").val(), Content: $("#cost-content").val(), Amount: $("#cost-amount").val().replace(/[^0-9-.]/g, '') };
        if ($("#cost-content").val() == '') {
            toastr.error("Vui lòng nhập tên chi phí");
            return false;
        }
        if ($("#cost-amount").val() == '') {
            toastr.error("Vui lòng nhập số tiền");
            return false;
        }
        if ($("#cost-id").val() != '') {
            for (var i = 0; i < global.Data.listCost.length; i++) {
                if (global.Data.listCost[i].Id === cost.Id) {
                    global.Data.listCost.splice(i, 1, cost);
                    break;
                }
            };
        }
        else {
            cost.Id = guid();
            global.Data.listCost.push(cost);

        }
        //global.Data.listCost.sort(function (a, b) {
        //    return a.Index == b.Index ? 0 : +(a.Index > b.Index) || -1;
        //});
        var totalCost = 0;
        for (var i = 0; i < global.Data.listCost.length; i++) {
            totalCost = totalCost + parseFloat(global.Data.listCost[i].Amount);
        };
        $('#cost').val(totalCost.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

        $("#" + global.Element.PopupCostEdit).modal("hide");
        reloadListCost();
    }
    function updateHasPay(orderId, payment, transferDescription) {
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

    function updateOrderStatus(orderId, status) {
        $.ajax({
            url: "/Order/UpdateOrderStatus?orderId=" + orderId + "&status=" + status,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListOrder();
                        toastr.success("Cập nhật trạng thái đơn hàng thành công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }

    function updateOrderApproval(orderId, status) {
        $.ajax({
            url: "/Order/UpdateOrderApproval?orderId=" + orderId + "&status=" + status,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListOrder();
                        toastr.success("thành công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateOrderStatus2(orderId, status, sendSMS, sendMail) {
        $.ajax({
            url: "/Order/UpdateOrderStatus?orderId=" + orderId + "&status=" + status + "&sendSMS=" + sendSMS + "&sendEmail=" + sendMail,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListOrder();
                        toastr.success("Cập nhật trạng thái đơn hàng thành công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function updateDetailStatus(detailId, status, employeeUpdateId, content) {

        $.ajax({
            url: "/Order/UpdateDetailStatus?detailId=" + detailId + "&status=" + status + "&employeeId=" + employeeUpdateId + "&content=" + content,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        $('#jtableOrder').jtable('getRowByKey', global.Data.OrderId).find("a#newdetail").trigger('click');
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function GetJobDescriptionForEmployee(detailId, status, employeeUpdateId, content) {

        $.ajax({
            url: "/Order/GetJobDescriptionForEmployee?detailId=" + detailId + "&status=" + status + "&employeeId=" + employeeUpdateId + "&content=" + content,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {

                        $("#dDescription").val(result.Data);
                        showPopupDesignProcess();
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
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
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
    function calculatorSubTotal(square, quantity, price, isFixed) {
        if (quantity != 0) {
            if (isFixed == true) {
                return price;
            }
            else {
                if (!checkNumber(square) && square !== 0) {
                    return decimalAdjust('round', square * quantity, -6) * price;
                } else {
                    return quantity * price;
                }
            }
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
                    $('#subcemployee1').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
                return datas[0].Value;
            }
            else {
                $('#cemployee1').append('<option value="0">Không Có Dữ Liệu </option>');
                $('#subcemployee1').append('<option value="0">Không Có Dữ Liệu </option>');
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
    function initListDesign() {
        $("#" + global.Element.JtableDesign).jtable({
            title: "Danh sách file thiết kế/in ấn",
            paging: true,
            pageSize: 10,
            pageSizeChangeDesign: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListDesign,
                createAction: global.Element.PopupDesign
            },
            messages: {
                selectShow: "Ẩn hiện cột"
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
                    width: "15%",
                    display: function (data) {
                        var text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.CustomerName + "</a>");
                        text.click(function () {

                            bindData(data.record);
                            showPopupDesign();
                        });
                        return text;
                    }
                },
                CommodityName: {
                    title: "Dịch Vụ",
                    width: "15%"
                },
                FileName: {
                    title: "Tên File",
                    width: "15%"
                },
                Width: {
                    visibility: "fixed",
                    title: "Chiều dài",
                    width: "2%"
                },
                Height: {
                    visibility: "fixed",
                    title: "Chiều Rộng",
                    width: "2%"
                },
                Quantity: {
                    visibility: "fixed",
                    title: "số Lượng",
                    width: "2%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "15%"
                },
                StrdesignStatus: {
                    visibility: "fixed",
                    title: "Xử Lý",
                    width: "20%",
                    display: function (data) {
                        //var text = $("<a href=\"#\" class=\"clickable\" title=\"Cập nhật Trạng Thái.\">" + data.record.StrdesignStatus + "</a>");
                        //text.click(function () {
                        //    returnString = '<span data-id="'+data.record.OrderId+'" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                        //    updateStatus(data.record.Id, data.record.DetailStatus, returnString);
                        //});
                        var text = "";
                        var arrayNVIN = global.Data.ListEmployeePrint;
                        var arrayNVGC = global.Data.ListEmployeeAddon;
                        var textNVTK = '';
                        var textNVIN = '';
                        var textNVGC = '';
                        var strStatus = '';
                        strStatus = getOrderDetailStatus(data.record.DetailStatus);
                        if (data.record.DetailStatus == 3) {
                            if (data.record.PrintUser == null) { strStatus = 'Đã Thiết Kế Xong'; }
                            else { strStatus = 'Đã Chuyển Cho In Ấn:' + data.record.PrintView; }
                        }
                        if (data.record.DetailStatus == 5) { strStatus = 'Đã Chuyển Cho Gia Công:' + data.record.AddOnView; }
                        for (var i = 0; i < arrayNVIN.length; i++) {
                            textNVIN = textNVIN + '<li><a onclick="GetDetaildataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                        };
                        for (var i = 0; i < arrayNVGC.length; i++) {
                            textNVGC = textNVGC + '<li><a onclick="GetDetaildataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                        };
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown"><a tabindex="-1" href="#" class="detailstatus1" href="javascript:void(0)">Đang thiết kế</a></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus2" href="javascript:void(0)">Đã thiết kế xong</a></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li></ul></div>');
                        text.click(function () {
                            global.Data.returnString = '<span data-id="' + data.record.OrderId + '" class="viewUpdateDetail">' + data.record.OrderId + '</br>' + data.record.CustomerName + ':' + data.record.Width + '*' + data.record.Height + '-NVKD:' + data.record.EmployeeName;
                            global.Data.OrderId = data.record.OrderId;
                            global.Data.IdDetailStatus = data.record.Id;
                        });

                        return text;
                    }
                },
                EmployeeName: {
                    title: "Nhân Viên Kinh Doanh",
                    width: "20%"
                },
            }
        });
    }
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
            recordsLoaded: function (event, data) {

                var SumA = data.serverResponse.Data[0].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var SumB = data.serverResponse.Data[1].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var SumC = data.serverResponse.Data[2].Value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                document.getElementById("sum01").innerHTML = SumA;
                document.getElementById("sum02").innerHTML = SumB;
                document.getElementById("sum03").innerHTML = SumC;
            },
            rowInserted: function (event, data) {
                if (data.record.OrderStatus == 1) {
                    data.row.css("background", "#cef5da");
                }
                if (data.record.OrderStatus == 2) {
                    data.row.css("background", "#d2ec6a");
                }
                if (data.record.OrderStatus == 3) {
                    data.row.css("background", "#F5ECCE");
                }
                if (data.record.OrderStatus == 4) {
                    data.row.css("background", "#dacfcf");
                }
                if (data.record.OrderStatus == 5) {
                    data.row.css("background", "#f5cece");
                }

            },
            toolbar: {
                items: [{
                    tooltip: 'Click here to export this table to excel',
                    text: 'Xuất Excel Các Đơn Hàng Đã Chọn',
                    click: function () {
                        var keySearch = $("#keyword").val();
                        var fromDate = $("#datefrom").val();
                        var toDate = $("#dateto").val();
                        var employee = $("#cemployee1").val();
                        var delivery = $("#DeliveryType").val();
                        var paymentStatus = $("#PaymentStatus").val();
                        var listOrderid = [];
                        var $selectedRows = $('#jtableOrder').jtable('selectedRows');
                        if ($selectedRows.length == 0) {
                            toastr.error("Vui lòng chọn đơn hàng.");
                            return;
                        }
                        $selectedRows.each(function () {
                            var record = $(this).data('record');
                            listOrderid.push(record.Id);
                        });
                        var orderidsParam = JSON.stringify(listOrderid);
                        var url = "/Order/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate + "&employee=" + employee + "&keySearch=" + keySearch + "&delivery=" + delivery + "&paymentStatus=" + paymentStatus + "&type=" + 0 + "&orderIds=" + orderidsParam;
                        window.location = url;
                    }
                },
                {
                    tooltip: 'Click here to export this table to excel',
                    text: 'Export to Excel',
                    click: function () {
                        var keySearch = $("#keyword").val();
                        var fromDate = $("#datefrom").val();
                        var toDate = $("#dateto").val();
                        var employee = $("#cemployee1").val();
                        var delivery = $("#DeliveryType").val();
                        var paymentStatus = $("#PaymentStatus").val();
                        var url = "/Order/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate + "&employee=" + employee + "&keySearch=" + keySearch + "&delivery=" + delivery + "&paymentStatus=" + paymentStatus + "&type=" + 0;
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
                    title: ' Id',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (orderDetailData) {
                        var tableObj = {};
                        if (document.getElementById('show-dim').checked) {
                            tableObj = {
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
                                FileName: {
                                    title: "Tên File",
                                    width: "10%"
                                },
                                Description: {
                                    title: "Ghi Chú",
                                    width: "10%"
                                },
                                strDetailStatus: {
                                    visibility: 'fixed',
                                    title: "Trạng Thái",
                                    width: "10%",
                                    display: function (data) {
                                        var text = "";
                                        var arrayNVTK = global.Data.ListEmployeeDesign;
                                        var arrayNVIN = global.Data.ListEmployeePrint;
                                        var arrayNVGC = global.Data.ListEmployeeAddon;
                                        var textNVTK = '';
                                        var textNVIN = '';
                                        var textNVGC = '';

                                        var strStatus = getOrderDetailStatus(data.record.DetailStatus);
                                        for (var i = 0; i < arrayNVTK.length; i++) {
                                            textNVTK = textNVTK + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVTK[i].Id + ' class="detailstatus1" href="#">' + arrayNVTK[i].Name + '</a></li>';
                                        };
                                        for (var i = 0; i < arrayNVIN.length; i++) {
                                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                                        };
                                        for (var i = 0; i < arrayNVGC.length; i++) {
                                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                                        };
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho thiết kế</a><ul class="dropdown-menu">' + textNVTK + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus7" href="javascript:void(0)">Đã xong</a></li></ul></div>');
                                        text.click(function () {
                                            global.Data.OrderId = orderDetailData.record.Id;
                                            global.Data.IdDetailStatus = data.record.Id;
                                        });
                                        return text;
                                    }
                                },
                                UserProcess: {
                                    visibility: 'fixed',
                                    title: "Nhân Viên",
                                    width: "10%",
                                    display: function (data) {
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + data.record.UserProcess + '</a></span></button><ul class="dropdown-menu"><li><a class="user1" href="javascript:void(0)">Thiết Kế: ' + data.record.DesignView + '</a></li><li><a class="user2" href="javascript:void(0)">In: ' + data.record.PrintView + '</a></li><li><a class="user3" href="javascript:void(0)">Gia Công:' + data.record.AddOnView + '</a></li></ul></div>');
                                        text.click(function () {
                                        });
                                        return text;
                                    }
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

                            };
                        }
                        else {
                            tableObj = {
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
                                FileName: {
                                    title: "Tên File",
                                    width: "10%"
                                },
                                Description: {
                                    title: "Ghi Chú",
                                    width: "10%"
                                },
                                strDetailStatus: {
                                    visibility: 'fixed',
                                    title: "Trạng Thái",
                                    width: "10%",
                                    display: function (data) {
                                        var text = "";
                                        var arrayNVTK = global.Data.ListEmployeeDesign;
                                        var arrayNVIN = global.Data.ListEmployeePrint;
                                        var arrayNVGC = global.Data.ListEmployeeAddon;
                                        var textNVTK = '';
                                        var textNVIN = '';
                                        var textNVGC = '';

                                        var strStatus = getOrderDetailStatus(data.record.DetailStatus);
                                        for (var i = 0; i < arrayNVTK.length; i++) {
                                            textNVTK = textNVTK + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVTK[i].Id + ' class="detailstatus1" href="#">' + arrayNVTK[i].Name + '</a></li>';
                                        };
                                        for (var i = 0; i < arrayNVIN.length; i++) {
                                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                                        };
                                        for (var i = 0; i < arrayNVGC.length; i++) {
                                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                                        };
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho thiết kế</a><ul class="dropdown-menu">' + textNVTK + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus7" href="javascript:void(0)">Đã xong</a></li></ul></div>');
                                        text.click(function () {
                                            global.Data.OrderId = orderDetailData.record.Id;
                                            global.Data.IdDetailStatus = data.record.Id;
                                        });
                                        return text;
                                    }
                                },
                                UserProcess: {
                                    visibility: 'fixed',
                                    title: "Nhân Viên",
                                    width: "10%",
                                    display: function (data) {
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + data.record.UserProcess + '</a></span></button><ul class="dropdown-menu"><li><a class="user1" href="javascript:void(0)">Thiết Kế: ' + data.record.DesignView + '</a></li><li><a class="user2" href="javascript:void(0)">In: ' + data.record.PrintView + '</a></li><li><a class="user3" href="javascript:void(0)">Gia Công:' + data.record.AddOnView + '</a></li></ul></div>');
                                        text.click(function () {
                                        });
                                        return text;
                                    }
                                },
                                Quantity: {
                                    title: 'SLượng',
                                    width: '5%'
                                },
                                strPrice: {
                                    title: 'ĐGiá',
                                    width: '5%'
                                },
                                strSubTotal: {
                                    title: 'Thành Tiền',
                                    width: '10%'
                                },
                                strDetailStatus: {
                                    visibility: 'fixed',
                                    title: "Trạng Thái",
                                    width: "10%",
                                    display: function (data) {
                                        var text = "";
                                        var arrayNVTK = global.Data.ListEmployeeDesign;
                                        var arrayNVIN = global.Data.ListEmployeePrint;
                                        var arrayNVGC = global.Data.ListEmployeeAddon;
                                        var textNVTK = '';
                                        var textNVIN = '';
                                        var textNVGC = '';

                                        var strStatus = getOrderDetailStatus(data.record.DetailStatus);
                                        for (var i = 0; i < arrayNVTK.length; i++) {
                                            textNVTK = textNVTK + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVTK[i].Id + ' class="detailstatus1" href="#">' + arrayNVTK[i].Name + '</a></li>';
                                        };
                                        for (var i = 0; i < arrayNVIN.length; i++) {
                                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                                        };
                                        for (var i = 0; i < arrayNVGC.length; i++) {
                                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                                        };
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho thiết kế</a><ul class="dropdown-menu">' + textNVTK + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus7" href="javascript:void(0)">Đã xong</a></li></ul></div>');
                                        text.click(function () {
                                            global.Data.OrderId = orderDetailData.record.Id;
                                            global.Data.IdDetailStatus = data.record.Id;
                                        });
                                        return text;
                                    }
                                },
                                UserProcess: {
                                    visibility: 'fixed',
                                    title: "Nhân Viên",
                                    width: "10%",
                                    display: function (data) {
                                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + data.record.UserProcess + '</a></span></button><ul class="dropdown-menu"><li><a class="user1" href="javascript:void(0)">Thiết Kế: ' + data.record.DesignView + '</a></li><li><a class="user2" href="javascript:void(0)">In: ' + data.record.PrintView + '</a></li><li><a class="user3" href="javascript:void(0)">Gia Công:' + data.record.AddOnView + '</a></li></ul></div>');
                                        text.click(function () {
                                        });
                                        return text;
                                    }
                                },
                            };
                        }
                        var $img = $('<a detailKey style="color: red;" id="newdetail" href="javascript:void(0)">' + orderDetailData.record.Id + '</a>');
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
                                    fields: tableObj,
                                }, function (data) { //opened handler
                                    data.childTable.jtable('load');
                                });
                        });
                        return $img;
                    }
                },
                EditDetail: {
                    title: 'Chi Tiết',
                    width: "5%",
                    sorting: false,
                    display: function (data) {
                        var text = '';
                        if (!data.record.IsSystem) {
                            text = $('<button title="chỉnh sửa" class="jtable-command-button jtable-edit-command-button"><span>Phân Quyền</span></button>');
                            text.click(function () {
                                global.Data.IdForView = data.record.Id;
                                $('.nav-tabs a[href="#ViewDetail"]').tab('show');
                                reloadViewDetail();
                            });
                        }
                        return text;
                    }
                },
                Name: {
                    visibility: 'fixed',
                    title: "Tên Khách Hàng",
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
                            $("#ddeposit").val(data.record.Deposit);
                            $("#ddeposit").val(data.record.Deposit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //document.getElementById("dtax").checked = data.record.HasTax;
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
                            $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            var totalIncludeTax = global.Data.OrderTotal - data.record.Deposit;
                            $("#dtotaltax").val(totalIncludeTax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));


                            //popAllElementInArray(global.Data.ModelOrderDetail);
                            $('.nav-tabs a:last').tab('show');

                        });
                        return text;
                    }
                },
                //strFileName: {
                //    title: "Tên File",
                //    width: "12%"
                //},
                StrCreatedDate: {
                    title: 'Ngày Tạo',
                    width: "5%"
                },
                StrDeliveryDate: {
                    title: 'Ngày Giao Hàng',
                    width: "5%",
                    display: function (data) {

                        var text = '';
                        if (new Date(data.record.DeliveryDate.match(/\d+/)[0] * 1).getTime() == new Date(moment(new Date()).format("YYYY/MM/DD")).getTime()) {
                            text = $('<a  href="javascript:void(0)" style="color:red;"  class="clickable"  data-target="#popup_Order" title="Ngày giao hàng.">' + data.record.StrDeliveryDate + '</a>');

                        } else {
                            text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="Ngày giao Hàng.">' + data.record.StrDeliveryDate + '</a>');
                        }
                        return text;
                    }

                },
                //HasTax: {
                //    title: "Có Thuế",
                //    width: "3%",
                //    display: function (data) {
                //        var elementDisplay = "";
                //        if (data.record.HasTax) { elementDisplay = "<input  type='checkbox' checked='checked' disabled/>"; }
                //        else {
                //            elementDisplay = "<input  type='checkbox' disabled />";
                //        }
                //        return elementDisplay;
                //    }
                //},
                strSubTotal: {
                    title: "Tổng Tiền",
                    width: "7%"
                },
                //strCost: {
                //    title: "Chi Phí",
                //    width: "7%",
                //    display: function (data) {
                //        var text = $('<a  href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Cập nhật số tiền đã thu.">' + data.record.strCost + '</a>');
                //        text.click(function () {
                //            global.Data.OrderId = data.record.Id;
                //            while (global.Data.listCost.length) {
                //                global.Data.listCost.pop();
                //            }
                //            global.Data.listCost.push.apply(global.Data.listCost, data.record.CostObj);
                //            //global.Data.listCost.sort(function (a, b) {
                //            //    return a.Index == b.Index ? 0 : +(a.Index > b.Index) || -1;
                //            //});
                //            var totalCost = 0;
                //            for (var i = 0; i < global.Data.listCost.length; i++) {
                //                totalCost = totalCost + parseFloat(global.Data.listCost[i].Amount);
                //            };
                //            $('#cost').val(totalCost.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //            reloadListCost();
                //            showPopupCost();
                //        });
                //        return text;
                //    }
                //},
                //strIncome: {
                //    title: "Tiền Lãi",
                //    width: "7%"
                //},
                strHaspay: {
                    title: "Đã Thu TM",
                    width: "7%",
                    display: function (data) {
                        var text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="Cập nhật số tiền đã thu.">' + data.record.strHaspay + '</a>');
                        text.click(function () {
                            global.Data.OrderId = data.record.Id;
                            //showPopupHaspay();
                        });
                        return text;
                    }
                },
                strHaspayTransfer: {
                    title: "Đã Thu CK",
                    width: "7%",
                    display: function (data) {
                        var text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="' + data.record.Description + '">' + data.record.strHaspayTransfer + '</a>');
                        text.click(function () {
                            global.Data.OrderId = data.record.Id;
                            //showPopupHaspay();
                        });
                        return text;
                    }
                },

                StrHas: {
                    title: "Còn Lại",
                    width: "7%",
                    display: function (data) {
                        var text = $('<a  href="javascript:void(0)" style="color:#89798d;"  class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + (data.record.SubTotal - (data.record.HasPay + data.record.HaspayTransfer)).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</a>');
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
                        if (data.record.PaymentMethol == 3) { text = $("<a href=\"javascript:void(0)\"   class=\"clickable\" title=\"Cập nhật thanh toán.\">" + "Công Nợ" + "</a>"); }
                        else {
                            text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật thanh toán.\"><span class=\"fa fa-money fa-lg\" aria-hidden=\"true\"></span></a>");
                        }
                        text.click(function () {
                            global.Data.ForView = 0;
                            global.Data.IdOrderStatus = data.record.Id;
                            global.Data.NumberDetail = data.record.T_OrderDetail.length;
                            $("#type2").prop("checked", true);
                            $("#ppay").val("");
                            $("#prest").val("");
                            $("#ptotal").val(data.record.strSubTotal);
                            $("#transferId").val(data.record.Description);
                            $("#phaspay").val((data.record.HasPay + data.record.HaspayTransfer).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            var a1 = data.record.SubTotal - (data.record.HasPay + data.record.HaspayTransfer);
                            var b = a1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            var deposit = data.record.Deposit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            $("#hasDeposit").val(deposit);
                            $("#ppayment").val(b);
                            $("#prealpay").val(b);
                            global.Data.OrderId = data.record.Id;
                            global.Data.PcustomerName = data.record.Name;
                            global.Data.PcustomePhone = data.record.CustomerPhone;
                            global.Data.PcustomerAddress = data.record.CustomerAddress;
                            global.Data.PBusinessName = data.record.CreateUserName;
                            global.Data.Pproduct = "";
                            calculatorProduct(data.record.T_OrderDetail);
                            //rendertable                                                    
                            renderTable(data.record.T_OrderDetail, data.record.SubTotal, data.record.HasPay + data.record.HaspayTransfer, 0);
                            if (data.record.HasTax) {
                                $('#hastaxnote').css("display", "inline");
                            } else {
                                $('#hastaxnote').css("display", "none")
                            }
                            showPopupPaymentProcess();
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                            var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                        });
                        return text;
                    }
                },
                //CustomPrint: {
                //    visibility: 'fixed',
                //    title: "Sản Xuất",
                //    width: "7%",
                //    display: function (data) {
                //        var text = "";
                //        if (data.record.PaymentMethol == 3) { text = $("<a href=\"javascript:void(0)\"   class=\"clickable\" title=\"Cập nhật thanh toán.\">" + "Công Nợ" + "</a>"); }
                //        else {
                //            text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"In Phiếu.\">Phiếu SX </a>");
                //        }
                //        text.click(function () {
                //            global.Data.ForView = 1;
                //            global.Data.IdOrderStatus = data.record.Id;
                //            global.Data.NumberDetail = data.record.T_OrderDetail.length;
                //            $("#type2").prop("checked", true);
                //            $("#ppay").val("");
                //            $("#prest").val("");
                //            $("#ptotal").val(data.record.strSubTotal);
                //            $("#transferId").val(data.record.Description);
                //            $("#phaspay").val((data.record.HasPay + data.record.HaspayTransfer).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //            var a1 = data.record.SubTotal - (data.record.HasPay + data.record.HaspayTransfer);
                //            var b = a1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                //            var deposit = data.record.Deposit.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                //            $("#hasDeposit").val(deposit);
                //            $("#ppayment").val(b);
                //            $("#prealpay").val(b);
                //            global.Data.OrderId = data.record.Id;
                //            global.Data.PcustomerName = data.record.Name;
                //            global.Data.PcustomePhone = data.record.CustomerPhone;
                //            global.Data.PcustomerAddress = data.record.CustomerAddress;
                //            global.Data.Pproduct = "";
                //            calculatorProduct(data.record.T_OrderDetail);
                //            //rendertable                                                    
                //            renderTable(data.record.T_OrderDetail, data.record.SubTotal, data.record.HasPay + data.record.HaspayTransfer, 1);
                //            if (data.record.HasTax) {
                //                $('#hastaxnote').css("display", "inline");
                //            } else {
                //                $('#hastaxnote').css("display", "none")
                //            }
                //            showPopupPaymentProcess();
                //            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd");
                //            var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");
                //        });
                //        return text;
                //    }
                //},
                strOrderStatus: {
                    title: "Trạng Thái Đơn Hàng",
                    width: "12%",
                    display: function (data) {
                        var text = "";
                        var strStatus = data.record.StatusName;
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật trạng thái đơn hàng.\">' + strStatus + '</a>' + resultStatusList + '</div>');
                        text.click(function (e) {
                            global.Data.IdOrderStatus = data.record.Id;
                        });
                        return text;
                    }
                },
                CreateUserName: {
                    title: "NV Kinh Doanh",
                    width: "10%"
                },
                ApprovalStatus: {
                    title: 'Duyệt Sửa/Xóa ',
                    width: "10%",
                    sorting: false,
                    display: function (data) {
                        var text = "";
                        var strStatus = data.record.ApprovalStatus;
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Cập nhật trạng thái đơn hàng.\">' + strStatus + '</a>' + "<ul class=\"dropdown-menu\"><li><a class=\"orderApproval\" data-id=\"3\" href=\"javascript:void(0)\">Duyệt</a></li><li><a class=\"orderApproval\" data-id=\"4\" href=\"javascript:void(0)\">Không Duyệt</a></li></ul>" + '</div>');
                        text.click(function (e) {
                            global.Data.IdOrderStatus = data.record.Id;
                        });
                        return text;

                        //var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        //text.click(function () {
                        //    GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                        //        deleteRow(data.record.Id);
                        //    }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        //});
                        //return text;

                    }
                },
                Delete: {
                    title: 'Xóa ',
                    width: "10%",
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
                strDetailStatus: {
                    visibility: 'fixed',
                    title: "Trạng Thái",
                    width: "10%",
                    display: function (data) {
                        var text = "";
                        var strStatus = getOrderDetailStatus(data.record.DetailStatus);
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu"><li><a class="detailstatus1" href="javascript:void(0)">Chuyển cho thiết kế</a></li><li><a class="detailstatus3" href="javascript:void(0)">Chuyển cho in ấn</a></li><li><a class="detailstatus5" href="javascript:void(0)">chuyển cho gia công</a></li><li><a class="detailstatus7" href="javascript:void(0)">Đã xong</a></li></ul></div>');

                        text.click(function () {
                            global.Data.IdDetailStatus = data.record.Id;
                        });
                        return text;
                    }
                },
                UserProcess: {
                    visibility: 'fixed',
                    title: "Nhân Viên",
                    width: "10%",
                    display: function (data) {
                        var text = "";
                        var arrayNVTK = global.Data.ListEmployeeDesign;
                        var arrayNVIN = global.Data.ListEmployeePrint;
                        var arrayNVGC = global.Data.ListEmployeeAddon;
                        var textNVTK = '';
                        var textNVIN = '';
                        var textNVGC = '';
                        var strStatus = getOrderDetailStatus(data.record.DetailStatus);
                        for (var i = 0; i < arrayNVTK.length; i++) {
                            textNVTK = textNVTK + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVTK[i].Id + ' class="detailstatus1" href="#">' + arrayNVTK[i].Name + '</a></li>';
                        };
                        for (var i = 0; i < arrayNVIN.length; i++) {
                            textNVIN = textNVIN + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVIN[i].Id + ' class="detailstatus3" href="#">' + arrayNVIN[i].Name + '</a></li>'
                        };
                        for (var i = 0; i < arrayNVGC.length; i++) {
                            textNVGC = textNVGC + '<li><a onclick="GetdataId(this)" data-id=' + arrayNVGC[i].Id + ' class="detailstatus5" href="#">' + arrayNVGC[i].Name + '</a></li>'
                        };
                        var text = $(' <div class="dropdown"><a class="dropdown-toggle" data-target="#" type="button" data-toggle="dropdown" href=\"javascript:void(0)\" class=\"clickable\" title=\"Chi tiết đơn hàng.\">' + strStatus + '</a></span></button><ul class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho thiết kế</a><ul class="dropdown-menu">' + textNVTK + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">Chuyển cho in ấn</a><ul class="dropdown-menu">' + textNVIN + '</ul></li><li class="dropdown-submenu"><a tabindex="-1" href="javascript:void(0)">chuyển cho gia công</a><ul class="dropdown-menu">' + textNVGC + '</ul></li><li class="dropdown"><a tabindex="-1" href="#" class=" detailstatus7" href="javascript:void(0)">Đã xong</a></li></ul></div>');
                        text.click(function () {
                            global.Data.OrderId = orderDetailData.record.Id;
                            global.Data.IdDetailStatus = data.record.Id;
                        });
                        return text;
                    }
                },
            }
        });
    }

    function initListCost() {

        $('#' + global.Element.JtableCost).jtable({
            title: 'Chi Phí',
            paging: true,
            pageSize: 25,
            pageSizeChangeCost: true,
            sorting: true,
            selectShow: false,
            actions: {
                listAction: global.Data.listCost,
                createAction: global.Element.PopupCostEdit,
            },
            messages: {
                addNewRecord: 'Thêm Mới'
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Content: {
                    visibility: 'fixed',
                    title: "Nội Dung",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Cost" title="Chỉnh sửa thông tin.">' + data.record.Content + '</a>');
                        text.click(function () {
                            $("#cost-id").val(data.record.Id);
                            $("#cost-content").val(data.record.Content);
                            $("#cost-amount").val(data.record.Amount);
                            showPopupCostEdit();
                        });
                        return text;
                    }
                },
                Amount: {
                    title: "Số Tiền",
                    width: "10%",
                    display: function (data) {
                        return data.record.Amount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Delete: {
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button  title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                removeItemInArray1(global.Data.listCost, data.record.Id);
                                var totalCost = 0;
                                for (var i = 0; i < global.Data.listCost.length; i++) {
                                    totalCost = totalCost + parseFloat(global.Data.listCost[i].Amount);
                                };
                                $('#cost').val(totalCost.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                reloadListCost();
                                //var total = 0;
                                //for (var k = 0; k < global.Data.listCost.length; k++) {
                                //    total += parseFloat(global.Data.listCost[k].Amount.replace(/[^0-9-.]/g, ''));
                                //}
                                //$("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
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
            global.Data.listproduct = datas;
            $("#dproduct").empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    global.Data.listproductName.push(datas[i].Text);
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
        var tableObj = {};
        if (document.getElementById('show-dim').checked) {
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
                                //if (document.getElementById("dtax").checked == true) {
                                var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
                                deposit = parseFloat(deposit);
                                var totaltax = global.Data.OrderTotal - ddeposit;
                                $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
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
                                //if (document.getElementById("dtax").checked == true) {
                                var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
                                ddeposit = parseFloat(ddeposit);
                                var totaltax = global.Data.OrderTotal - ddeposit;
                                $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
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
        $("#ddeposit").val(0);
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
                    var deposit = $("#ddeposit").val();
                    //var tax = document.getElementById("dtax").checked;
                    var tax = false;
                    var totalIncludeTax = $("#dtotal").val().replace(/[^0-9-.]/g, '');
                    totalIncludeTax = parseFloat(totalIncludeTax);
                    if (tax) {
                        totalIncludeTax = totalIncludeTax * 0.1 + totalIncludeTax;
                    }

                    $.ajax({
                        url: global.UrlAction.SaveOrder + "?orderId=" + global.Data.OrderId + "&employeeId=" + employeeId + "&customerId=" + global.Data.CustomerId + "&customerName=" + customerName + "&customerPhone=" + customerPhone + "&customerMail=" + customerMail + "&customerAddress=" + customerAddress + "&customerTaxCode=" + customerTaxCode + "&dateDelivery=" + dateDelivery + "&orderTotal=" + global.Data.OrderTotal + "&tax=" + tax + "&orderTotalTax=" + totalIncludeTax + "&deposit=" + deposit,
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

    function initProductPrice() {
        var url = "/Order/GetProductPrice?type=2";
        $.getJSON(url, function (datas) {
            
            global.Data.ProductPrice = JSON.parse(datas.Content);
        });
    }
    function initProductPrice1() {
        var url = "/Order/GetProductPrice?type=3";
        $.getJSON(url, function (datas) {
            global.Data.ProductPrice1 = JSON.parse(datas.Content);
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
    function initComboBoxProduct(id, prePresect) {
        var url = "/Order/GetListProduct?productType=" + id;
        $.getJSON(url, function (datas) {
            $('#dproduct').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#dproduct').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            if (prePresect != undefined) {
                $('#dproduct').val(prePresect);
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
            updateDetailStatus(global.Data.IdDetailStatus, global.Data.DetailStatus, employeeUpdateId, description);
            //updateDesignUser(global.Data.DetailId, designId, description);
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
            var transfer = $('#transferId').val();
            updatePayment(orderId, payment, paymentType, transfer);
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
            if (global.Data.ForView == 0) {
                $("#vcustomer").append(global.Data.PcustomerName);
                $("#vphone").append(global.Data.PcustomePhone);
                $("#vaddress").append(global.Data.PcustomerAddress);
            } else {
                $("#vcustomer").append('');
                $("#vphone").append('');
                $("#vaddress").append('');
            }

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
            $("#kinhdoanh").html("");
            $("#vtotal1").html("");
            $("#strtotal1").html("");


            //custome Print set value
            $("#no1").append(global.Data.OrderId);
            if (global.Data.ForView == 0) {
                $("#vcustomer1").append(global.Data.PcustomerName);
                $("#vphone1").append(global.Data.PcustomePhone);
                $("#vaddress1").append(global.Data.PcustomerAddress);
                $("#vtotal1").append(payment);
                $("#strtotal1").append(c);
            } else {
                $("#vcustomer1").append('');
                $("#vphone1").append('');
                $("#vaddress1").append('');
            }


            $("#vdate3").append(time);
            $("#kinhdoanh").append(global.Data.PBusinessName);
            var check = document.getElementById('type2').checked;

            if (check) {
                //if (global.Data.NumberDetail > 9) {
                //    toastr.warning('Chi tiết đơn hàng này quá nhiều vui lòng chọn kiểu in khác');
                //}
                //else {
                printPanel1();
                //}

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
    function calculatorPrice() {
        var isFixed = false;
        var width = $("#dwidth").val();
        var height = $("#dheignt").val();
        var quantity = $("#dquantity").val();
        //var searchResult = searchById($("#dproduct").val(), global.Data.listproduct);
        //if (searchResult != undefined) {
        //    var tempPrice = getProductPrice(searchResult.Code, parseFloat(quantity));
        //    if (tempPrice != undefined) {
        //        isFixed = tempPrice.isFixed;
        //        $("#dprice").val(tempPrice.price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        //    }
        //}

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
            var total = calculatorSubTotal(sqare, quantity, price.replace(/[^0-9-.]/g, ''), isFixed);
            if (!checkNumber(total)) {
                var roundtotal = decimalAdjust('round', total, 0);
                $("#dsubtotal").val(roundtotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                $("#dtotaltax").val(roundtotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //if (document.getElementById("dtax").checked == true) {
                var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
                ddeposit = parseFloat(ddeposit);
                var totaltax = roundtotal - ddeposit;
                $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                //}
            }
            else {
                $("#dsubtotal").val("");
                $("#dtotaltax").val("");
            }
        }
    }

    function calculatorPriceCustom(quantityDiv, code, priceDiv, totalDiv) {
        var isFixed = false;
        var quantity = $(quantityDiv).val();
        if (quantity == '') {
            quantity = 0;
        }
        if (code == "giacongkhac") {
            var temp1 = $(priceDiv).val().replace(/[^0-9-.]/g, '');
            var tempTotal = temp1 * parseFloat(quantity);
            $(totalDiv).val(tempTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }

        else {
            var tempPrice = getProductPrice(code, parseFloat(quantity));
            if (code == "cat") {
                tempPrice = {};
                if (parseFloat(quantity) > 0) {
                    tempPrice.price = 500;
                    tempPrice.isFixed = false;
                }
                if (parseFloat(quantity) > 1000) {
                    tempPrice.price = 300;
                    tempPrice.isFixed = false;
                }
            }
            if (code == "be") {
                tempPrice = {};
                if (parseFloat(quantity) > 0) {
                    tempPrice.price = 5000;
                    tempPrice.isFixed = false;
                }
                if (parseFloat(quantity) > 10) {
                    tempPrice.price = 4000;
                    tempPrice.isFixed = false;
                }
                if (parseFloat(quantity) > 20) {
                    tempPrice.price = 3000;
                    tempPrice.isFixed = false;
                }
                if (parseFloat(quantity) > 50) {
                    tempPrice.price = 2500;
                    tempPrice.isFixed = false;
                }
                if (parseFloat(quantity) > 100) {
                    tempPrice.price = 2000;
                    tempPrice.isFixed = false;
                }
            }
            if (tempPrice != undefined) {
                isFixed = tempPrice.isFixed;
                if (quantity != '0' && quantity != '') {
                    $(priceDiv).val(tempPrice.price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
            }
            var total = calculatorSubTotal(0, quantity, tempPrice.price, isFixed);
            if (quantity != 0 && quantity != '') {
                $(totalDiv).val(total.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        }

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
            updateHasPay(orderId, payment, paymentType, $('#transferId').val());
            reloadListOrder();
            $("#" + global.Element.PopupHasPay).modal("hide");
        });
        $("#" + global.Element.PopupHasPay + ' button[cancel]').click(function () {
            $("#" + global.Element.PopupHasPay).modal("hide");
        });
    }
    function initPopupCost() {
        $("#" + global.Element.PopupCost).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupCost + ' button[save]').click(function () {
            var orderId = global.Data.OrderId;
            var payment = $("#cost").val();
            updateCost(orderId, payment);
            reloadListOrder();
            $("#" + global.Element.PopupCost).modal("hide");
        });
        $("#" + global.Element.PopupCost + ' button[cancel]').click(function () {
            $("#" + global.Element.PopupCost).modal("hide");
        });
    }
    function addToDetail(quatityDiv, code, priceDiv, totalDiv) {
        var searchbycode = searchByCode(code, global.Data.listproduct)
        if ($(quatityDiv).val() != '' && $(quatityDiv).val() != 0 && $(priceDiv).val() != '' && $(priceDiv).val() != 0) {
            var object1 = { Id: 0, Index: global.Data.Index, CommodityId: searchbycode.Value, CommodityName: searchbycode.Text, FileName: '', Description: '', Width: '', Height: '', Square: '', Quantity: $(quatityDiv).val(), SumSquare: '', Price: $(totalDiv).val(), SubTotal: $(totalDiv).val() }
            global.Data.ModelOrderDetail.push(object1);
            global.Data.Index = global.Data.Index + 1;
        }
    }
    function initPopupCustom() {
        $("#" + global.Element.popupCustom).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.popupCustom + ' button[save]').click(function () {
            addToDetail('#quantity1_1', 'ingiay', '#price1_1', '#total1_1');
            addToDetail('#quantity1_2', $("#customSelect1").val(), '#price1_2', '#total1_2');
            addToDetail('#quantity1_3', 'canmang', '#price1_3', '#total1_3');
            addToDetail('#quantity1_4', 'dongkim', '#price1_4', '#total1_4');
            addToDetail('#quantity1_5', 'dongloxo', '#price1_5', '#total1_5');
            addToDetail('#quantity1_6', 'dongkeogay', '#price1_6', '#total1_6');
            addToDetail('#quantity1_7', 'cat', '#price1_7', '#total1_7');
            addToDetail('#quantity1_8', 'giacongkhac', '#price1_8', '#total1_8');

            for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
            }
            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
            ddeposit = parseFloat(ddeposit);
            var totaltax = global.Data.OrderTotal - ddeposit;
            $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

            global.Data.CurenIndex = 0;
            reloadListOrderDetail();
            resetDetail();

            $("#" + global.Element.popupCustom).modal("hide");
        });
        $("#" + global.Element.popupCustom + ' button[cancel]').click(function () {
            $("#" + global.Element.popupCustom).modal("hide");
        });
    }
    function initPopupCustom1() {
        $("#" + global.Element.popupCustom1).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.popupCustom1 + ' button[save]').click(function () {

            addToDetail('#quantity2_1', 'indecalgiay', '#price2_1', '#total2_1');
            addToDetail('#quantity2_2', 'decalgiay', '#price2_2', '#total2_2');
            addToDetail('#quantity2_3', 'canmang', '#price2_3', '#total2_3');
            addToDetail('#quantity2_4', 'be', '#price2_4', '#total2_4');
            addToDetail('#quantity2_5', 'giacongkhac', '#price2_5', '#total2_5');

            for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
            }
            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
            ddeposit = parseFloat(ddeposit);
            var totaltax = global.Data.OrderTotal - ddeposit;
            $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

            global.Data.CurenIndex = 0;
            reloadListOrderDetail();
            resetDetail();
            $("#" + global.Element.popupCustom1).modal("hide");
        });
        $("#" + global.Element.popupCustom1 + ' button[cancel]').click(function () {
            $("#" + global.Element.popupCustom1).modal("hide");
        });
    }
    function initPopupCustom2() {
        $("#" + global.Element.popupCustom2).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.popupCustom2 + ' button[save]').click(function () {
            addToDetail('#quantity3_1', 'indecalxi', '#price3_1', '#total3_1');
            addToDetail('#quantity3_2', $("#customSelect3").val(), '#price3_2', '#total3_2');
            addToDetail('#quantity3_3', 'canmang', '#price3_3', '#total3_3');
            addToDetail('#quantity3_4', 'be', '#price3_4', '#total3_4');
            addToDetail('#quantity3_5', 'giacongkhac', '#price3_5', '#total3_5');

            for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
            }
            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
            ddeposit = parseFloat(ddeposit);
            var totaltax = global.Data.OrderTotal - ddeposit;
            $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

            global.Data.CurenIndex = 0;
            reloadListOrderDetail();
            resetDetail();
            $("#" + global.Element.popupCustom2).modal("hide");
        });
        $("#" + global.Element.popupCustom2 + ' button[cancel]').click(function () {
            $("#" + global.Element.popupCustom2).modal("hide");
        });
    }
    function initPopupCustom3() {
        $("#" + global.Element.popupCustom3).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.popupCustom3 + ' button[save]').click(function () {
            $("#" + global.Element.popupCustom3).modal("hide");
        });
        $("#" + global.Element.popupCustom3 + ' button[cancel]').click(function () {
            $("#" + global.Element.popupCustom3).modal("hide");
        });
    }
    function initPopupNotification() {
        $("#" + global.Element.PopupNotification).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupNotification + ' button[save]').click(function () {

            var orderId = global.Data.OrderId;
            var sendSMS = document.getElementById("sendSMS").checked;
            var sendEmail = document.getElementById("sendEmail").checked;
            updateOrderStatus2(global.Data.IdOrderStatus, 2, sendSMS, sendEmail);
            reloadListOrder();
            $("#" + global.Element.PopupNotification).modal("hide");
        });
        $("#" + global.Element.PopupNotification + ' button[cancel]').click(function () {
            $("#" + global.Element.PopupNotification).modal("hide");
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
        $('#PaymentType').change(function () {
            if ($(this).val() === '2') {
                $('#transferIdShell').css('display', 'inline');
                // Do something for option "b"
            } else {
                $('#transferIdShell').css('display', 'none');
            }
        });
        $("#saveOrder").click(function () {
            saveOrder();
        });
        $("#dproduct").change(function () {
            //var resultObject = searchById($(this).val(), global.Data.listproduct);
            //if (resultObject.Code == 'ingiay') {
            //    showPopupCustom();
            //}
            //if (resultObject.Code == 'decalgiay') {
            //    showPopupCustom1();
            //}
            //if (resultObject.Code == 'decalxi') {
            //    showPopupCustom2();
            //}
            //if (resultObject.Code == 'catalogue') {
            //    showPopupCustom3();
            //} 
            $.ajax({
                url: "/Order/GetPriceForCustomerAndProduct?customerId=" + global.Data.CustomerId + "&productId=" + $(this).val(),
                type: 'post',
                contentType: 'application/json',
                success: function (result) {
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result.Records != 0) {
                            var temp = result.Records.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                            $('#dprice').val(temp);
                            calculatorPrice();
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
            reloadListOrder();
        });
        $("#subsearch").click(function () {
            reloadListDesign();
        });
        $("#inprogess").click(function () {
            reloadListOrder(1);
        });
        $("body").delegate(".searchStatus", "click", function (event) {
            var statusId = $(this).attr("data-id");
            event.preventDefault();
            reloadListOrder(statusId);
        });
        $("#vsearch").click(function () {
            reloadViewDetail();
        });
        $("#print1").click(function () {
            showPopupCustom();
        });
        $("#print2").click(function () {
            showPopupCustom1();
        });
        $("#print3").click(function () {
            showPopupCustom2();
        });
        $("#print4").click(function () {
            showPopupCustom3();
        });
        $("#CreateOrder").click(function () {
            document.getElementById("date").defaultValue = new Date().toISOString().substring(0, 10);
            $("#cname").attr("disabled", false);
            $("#cphone").attr("disabled", false);
            //document.getElementById("dtax").checked = false;
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
        $("body").delegate(".orderstatus", "click", function (event) {
            var statusId = $(this).attr("data-id");
            event.preventDefault();
            updateOrderStatus(global.Data.IdOrderStatus, statusId);
        });
        $("body").delegate(".orderApproval", "click", function (event) {
            var statusId = $(this).attr("data-id");
            event.preventDefault();
            updateOrderApproval(global.Data.IdOrderStatus, statusId);
        });

        $("body").delegate(".detailstatus1", "click", function (event) {
            event.preventDefault();
            //showPopupDesignProcess();
            global.Data.DetailStatus = 1;
            GetJobDescriptionForEmployee(global.Data.IdDetailStatus, 1, employeeUpdateId);
        });
        $("body").delegate(".detailstatus3", "click", function (event) {
            event.preventDefault();
            //showPopupDesignProcess();
            global.Data.DetailStatus = 3;
            GetJobDescriptionForEmployee(global.Data.IdDetailStatus, 3, employeeUpdateId);
        });
        $("body").delegate(".detailstatus5", "click", function (event) {
            event.preventDefault();
            global.Data.DetailStatus = 5;
            var description = $("#dDescription").val();
            updateDetailStatus(global.Data.IdDetailStatus, global.Data.DetailStatus, employeeUpdateId, description);
        });
        $("body").delegate(".detailstatus7", "click", function (event) {
            event.preventDefault();
            global.Data.DetailStatus = 7;
            var description = $("#dDescription").val();
            updateDetailStatus(global.Data.IdDetailStatus, global.Data.DetailStatus, employeeUpdateId, description);
        });

        $("body").delegate(".viewUpdateDetail", "click", function (event) {
            var orderId = $(this).data("id");
            $('html, body').animate({ scrollTop: $('tr[data-record-key="' + orderId + '"]').offset().top - 100 }, 'slow');
            $('#jtableOrder').jtable('getRowByKey', orderId).find("a#newdetail").trigger('click');
            event.preventDefault();
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

        $('.caculator').keyup(function () {
            calculatorPrice();
        });
        $('.customInput').keyup(function () {
            //quantityDiv, code, priceDiv, totalDiv
            calculatorPriceCustom('#quantity1_1', 'ingiay', '#price1_1', '#total1_1');
            calculatorPriceCustom('#quantity1_2', $("#customSelect1").val(), '#price1_2', '#total1_2');
            calculatorPriceCustom('#quantity1_3', 'canmang', '#price1_3', '#total1_3');
            calculatorPriceCustom('#quantity1_4', 'dongkim', '#price1_4', '#total1_4');
            calculatorPriceCustom('#quantity1_5', 'dongloxo', '#price1_5', '#total1_5');
            calculatorPriceCustom('#quantity1_6', 'dongkeogay', '#price1_6', '#total1_6');
            calculatorPriceCustom('#quantity1_7', 'cat', '#price1_7', '#total1_7');
            calculatorPriceCustom('#quantity1_8', 'giacongkhac', '#price1_8', '#total1_8');

            calculatorPriceCustom('#quantity2_1', 'indecalgiay', '#price2_1', '#total2_1');
            calculatorPriceCustom('#quantity2_2', 'decalgiay', '#price2_2', '#total2_2');
            calculatorPriceCustom('#quantity2_3', 'canmang', '#price2_3', '#total2_3');
            calculatorPriceCustom('#quantity2_4', 'be', '#price2_4', '#total2_4');
            calculatorPriceCustom('#quantity2_5', 'giacongkhac', '#price2_5', '#total2_5');

            calculatorPriceCustom('#quantity3_1', 'indecalxi', '#price3_1', '#total3_1');
            calculatorPriceCustom('#quantity3_2', $("#customSelect3").val(), '#price3_2', '#total3_2');
            calculatorPriceCustom('#quantity3_3', 'canmang', '#price3_3', '#total3_3');
            calculatorPriceCustom('#quantity3_4', 'be', '#price3_4', '#total3_4');
            calculatorPriceCustom('#quantity3_5', 'giacongkhac', '#price3_5', '#total3_5');
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
                        var object = { Id: global.Data.OrderDetailId, Index: objectIndex, CommodityId: $("#dproduct").val(), CommodityName: $("#dproduct option:selected").text(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
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
                        var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
                        ddeposit = parseFloat(ddeposit);
                        var totaltax = global.Data.OrderTotal - ddeposit;
                        $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //}

                    } else {
                        var object1 = { Id: 0, Index: objectIndex, CommodityId: $("#dproduct").val(), CommodityName: $("#dproduct option:selected").text(), FileName: $("#dfilename").val(), Description: $("#dnote").val(), Width: $("#dwidth").val(), Height: $("#dheignt").val(), Square: $("#dsquare").val(), Quantity: $("#dquantity").val(), SumSquare: $("#dsumsquare").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        global.Data.ModelOrderDetail.push(object1);
                        global.Data.Index = global.Data.Index + 1;
                        for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        $("#dtotaltax").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //if (document.getElementById("dtax").checked == true) {
                        var ddeposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');
                        ddeposit = parseFloat(ddeposit);
                        var totaltax = global.Data.OrderTotal - ddeposit;
                        $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //}
                        //global.Data.OrderTotal = global.Data.OrderTotal.replace(/[^0-9-.]/g, '');
                    }

                    global.Data.CurenIndex = 0;
                    reloadListOrderDetail();
                    resetDetail();
                }
            }
        });

        $("#ddeposit").keydown(function (e) {
            
            var subtotal = $("#dtotal").val().replace(/[^0-9-.]/g, '');
            var deposit = $("#ddeposit").val().replace(/[^0-9-.]/g, '');

            if (subtotal == "") {
                $("#dtotaltax").val("");
            } else {
                subtotal = parseFloat(subtotal);
                deposit = parseFloat(deposit);
                if (deposit > subtotal) {
                    //return false;
                }
                else {
                    var totaltax = subtotal - deposit;
                    $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }


            }

        });
        //$("#dtax").change(function () {
        //    var subtotal = $("#dtotal").val().replace(/[^0-9-.]/g, '');
        //    if (subtotal == "") {
        //        $("#dtotaltax").val("");
        //    } else {
        //        subtotal = parseFloat(subtotal);
        //        if (this.checked) {
        //            var totaltax = subtotal * 0.1 + subtotal;
        //            $("#dtotaltax").val(totaltax.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        //        } else {
        //            $("#dtotaltax").val(subtotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        //        }
        //    }

        //});
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
        $("#save-cost-edit").click(function () {
            SaveCostEdit();
        });

        $("#cost-amount").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#cost-amount").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
    };
    function mappingAutoComplete() {
        $(function () {
            $("#cname").autocomplete({
                source: global.Data.ListCustomerName,
                //focus: function (a, b) {
                //    
                //    var cusName = b.item.value;
                //    $.ajax({
                //        url: "/Order/GetCustomerByName?customerName=" + cusName,
                //        type: 'post',
                //        contentType: 'application/json',
                //        success: function (result) {
                //            GlobalCommon.CallbackProcess(result, function () {
                //                if (1 < 2) {
                //                    var listCustomer = result.Records;
                //                    $('#cname').val(listCustomer.Name);
                //                    $('#cphone').val(listCustomer.Mobile);
                //                    $('#cmail').val(listCustomer.Email);
                //                    $('#caddress').val(listCustomer.Address);
                //                    $('#ctaxcode').val(listCustomer.TaxCode);
                //                    global.Data.CustomerId = listCustomer.Id;
                //                }

                //            }, false, global.Element.PopupOrder, true, true, function () {
                //                var msg = GlobalCommon.GetErrorMessage(result);
                //                GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                //            });
                //        }
                //    });
                //},
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
            $("#keyword").autocomplete({
                source: global.Data.ListCustomerName,
                select: function (a, b) {
                    var cusName = b.item.value;
                    $.ajax({
                        url: "/Order/GetCustomerByName?customerName=" + cusName,
                        type: 'post',
                        contentType: 'application/json',
                        success: function (result) {
                            GlobalCommon.CallbackProcess(result, function () {
                                var listCustomer = result.Records;
                                $('#keyword').val(listCustomer.Name);
                                reloadListOrder();
                            }, false, global.Element.PopupOrder, true, true, function () {
                                var msg = GlobalCommon.GetErrorMessage(result);
                                GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                            });
                        }
                    });
                }
            });
            $("#dproductSuggest").autocomplete({
                source: global.Data.listproductName,
                select: function (a, b) {

                    var cusName = b.item.value;
                    var resultObject = search(cusName, global.Data.listproduct);
                    //if (resultObject.Code =='ingiay') {
                    //    showPopupCustom();
                    //} 
                    //if (resultObject.Code == 'decalgiay') {
                    //    showPopupCustom1();
                    //} 
                    //if (resultObject.Code == 'decalxi') {
                    //    showPopupCustom2();
                    //} 
                    //if (resultObject.Code == 'catalogue') {
                    //    showPopupCustom3();
                    //} 
                    if (resultObject != undefined) {
                        $('#dproductType').val(resultObject.Type);
                        initComboBoxProduct(0, resultObject.Value);
                    }
                }
            });
        });
    }
    this.Init = function () {
        registerEvent();
        initComboBoxAllProduct(0);
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 1 * 60 * 60 * 1000).toISOString().substring(0, 10);
        var dateTo = new Date();
        dateTo.setDate(dateTo.getDate() + 1);
        document.getElementById("dateto").defaultValue = dateTo.toISOString().substring(0, 10);
        document.getElementById("subdatefrom").defaultValue = new Date(new Date() - 24 * 1 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("subdateto").defaultValue = dateTo.toISOString().substring(0, 10);
        initComboBoxBusiness();
        initComboBoxBusiness1();
        initComboBoxBusiness2();
        initComboBoxDesign();
        initComboBoxPrint();
        initListOrder();
        initListViewDetail();
        reloadListOrder(-1);
        initComboBox();
        initListDesign();
        reloadListDesign();
        initListOrderDetail();
        reloadViewDetail();
        reloadListOrderDetail();
        initPopupOrder();
        initPopupDesignProcess();
        initPopupPrintProcess();
        initPopupPaymentProcess();
        initPopupHasPay();
        initPopupCost();
        initPopupCustom();
        initPopupCustom1();
        initPopupCustom2();
        initPopupCustom3();
        initPopupNotification();
        initPopupSearch();
        initListCost();
        reloadListCost();
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
        $.ajax({
            url: "/Employee/GetSimpleEmployee",
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == 'OK') {
                        global.Data.ListEmployeeDesign = result.Records.designUser;
                        global.Data.ListEmployeePrint = result.Records.printingUser;
                        global.Data.ListEmployeeAddon = result.Records.addOnUser;
                    }

                }, false, global.Element.PopupOrder, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
        initProductPrice();
        initProductPrice1();
    };
};
/*End Region*/
$(document).ready(function () {
    var employeeUpdateId = 0;
    var order = new VINASIC.Order();
    order.Init();
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
function GetdataId(obj) {
    employeeUpdateId = $(obj).data("id");
}
function GetDetaildataId(obj) {
    employeeUpdateId = $(obj).data("id");
}