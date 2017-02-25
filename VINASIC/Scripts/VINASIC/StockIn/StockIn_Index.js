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
VINASIC.namespace("StockIn");
VINASIC.StockIn = function () {
    var global = {
        UrlAction: {
            GetListStockIn: "/StockIn/GetStockIns",
            SaveStockIn: "/StockIn/SaveStockIn",
            DeleteStockIn: "/StockIn/DeleteStockIn"
        },
        Element: {
            JtableStockIn: "jtableStockIn",
            JtableStockInDetail: "jtableStockInDetail",
            PopupStockIn: "popup_StockIn",
            PopupSearch: "popup_SearchStockIn",
            PopupDesignProcess: "popup_DesignProcess",
            PopupPrintProcess: "popup_PrintProcess"
        },
        Data: {
            ModelStockIn: {},
            ModelStockInDetail: [],
            ModelConfig: {},
            ListCustomerName: [],
            ProductTypeId: 0,
            CustomerId: 0,
            StockInId: 0,
            Idnew: 0,
            Index: 1,
            CurenIndex: 0,
            UpdateStockInId: 0,
            OrderTotal:0
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListStockIn() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableStockIn).jtable("load", { 'keyword': keySearch });
    }
    function reloadListStockInDetail() {
        $('#' + global.Element.JtableStockInDetail).jtable('load', { 'keyword': "" });
    }
    function destroyListStockInDetail() {
        $('#' + global.Element.JtableStockInDetail).jtable('destroy', { 'keyword': "" });
    }
    function removeItemInArray(arr, id) {
        if (typeof (arr) != "undefined") {
            for (i = 0; i < arr.length; i++) {
                if (arr[i].Index === id) {
                    arr.splice(i, 1);
                    break;
                }
            };
        }
    }

    function popAllElementInArray(array) {
        while (array.length) {
            array.pop();
        }
    }
    function removeItemAndInsertInArray(arr, id, obj) {
        if (typeof (arr) != "undefined") {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].Index === id) {
                    arr.splice(i, 1, obj);
                    break;
                }
            };
        }
    }
    /*function init model using knockout Js*/
    function initViewModel(stockIn) {
        var stockInViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (stockIn != null) {
            stockInViewModel = {
                Id: ko.observable(stockIn.Id),
                Code: ko.observable(stockIn.Code),
                Name: ko.observable(stockIn.Name),
                Description: ko.observable(stockIn.Description)
            };
        }
        return stockInViewModel;
    }
    function bindData(stockIn) {
        global.Data.ModelStockIn = initViewModel(stockIn);
        ko.applyBindings(global.Data.ModelStockIn);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupStockIn() {
        $("#" + global.Element.PopupStockIn).modal("show");
    }
    function showPopupDesignProcess() {
        $("#" + global.Element.PopupDesignProcess).modal("show");
    }
    function showPopupPrintProcess() {
        $("#" + global.Element.PopupPrintProcess).modal("show");
    }

    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteStockIn,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListStockIn();
                    }
                }, false, global.Element.PopupStockIn, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function resetDetail() {
        $("#dproduct").val(0);
        $("#dfilename").val("");
        $("#dnote").val("");
        $("#dquantity").val("");
        $("#dprice").val("");
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
    function calculatorSubTotal(square, quantity, price) {
        if (!checkNumber(square) && square !== 0) {
            return square * quantity * price;
        } else {
            return quantity * price;
        }

    }

    /*End Check Validate */
    /*function Init List Using Jtable */
    function initListStockIn() {
        $('#' + global.Element.JtableStockIn).jtable({
            title: 'Danh Sách Nhập Hàng',
            paging: true,
            pageSize: 10,
            pageSizeChangeStockIn: true,
            sorting: true,
            selectShow: true,
            toolbar: {
                items: [{
                    tooltip: 'Click here to export this table to excel',
                    text: 'Export to Excel',
                    click: function () {
                        var keySearch = $("#keyword").val();
                        var fromDate = $("#datefrom").val();
                        var toDate = $("#dateto").val();
                        var url = "/StockIn/ExportReport?fromDate=" + fromDate + "&toDate=" + toDate + "&keySearch=" + keySearch;
                        window.location = url;
                    }
                }]
            },
            actions: {
                listAction: global.UrlAction.GetListStockIn,
                searchAction: global.Element.PopupSearch
            },
            datas: {
                jtableId: global.Element.JtableStockIn
            },
            messages: {
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột'
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                StockInDetail: {
                    title: ' CTĐH',
                    width: '2%',
                    sorting: false,
                    edit: false,
                    display: function (stockInDetailData) {
                        var $img = $('<a style="color: red;" id="newdetail" href="javascript:void(0)"><img style="Width:16px;height:16px" src="/img/edit.png" title="Chi Tiết Đơn Hàng" />New</a>');
                        $img.click(function () {
                            $('#StockInId').val(stockInDetailData.record.Id);

                            $('#jtableStockIn').jtable('openChildTable',
                                    $img.closest('tr'),
                                    {
                                        title: 'Chi Tiết Của Đơn hàng:' + stockInDetailData.record.Name,
                                        actions: {
                                            listAction: '/StockIn/ListStockInDetail?StockInId=' + stockInDetailData.record.Id,
                                            createAction: global.Element.Popup_StockInDetail
                                        },
                                        messages: {
                                            addNewRecord: 'Thêm Chi Tiết Đơn Hàng'
                                        },
                                        fields: {
                                            StockInId: {
                                                type: 'hidden',
                                                defaultValue: stockInDetailData.record.Id
                                            },
                                            Id: {
                                                key: true,
                                                create: false,
                                                edit: false,
                                                list: false
                                            },
                                            MateriaName: {
                                                title: "Tên Mặt Hàng",
                                                width: "10%"
                                            },
                                            Description: {
                                                title: "Ghi Chú",
                                                width: "10%"
                                            },
                                            Quantity: {
                                                title: 'Số Lượng',
                                                width: '5%'
                                            },
                                            Price: {
                                                title: 'Đơn Giá',
                                                width: '5%'
                                            },
                                            SubTotal: {
                                                title: 'Thành Tiền',
                                                width: '5%'
                                            },
                                            Edit: {
                                                title: 'Sửa',
                                                width: "3%",
                                                sorting: false,
                                                display: function (data) {
                                                    var text = $('<button title="Xóa" class="jtable-command-button jtable-edit-command-button"><span>Sửa</span></button>');
                                                    text.click(function () {
                                                        $("#id").val(data.record.Id);
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
                                                            //DeleteStockInDetail(7);
                                                            //DeleteStockInDetail(data.record.Id);
                                                        }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                                                    });
                                                    return text;

                                                }
                                            }
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
                        var text = $('<a href="#" class="clickable"  data-target="#popup_StockIn" title="Chỉnh sửa thông tin.">' + data.record.Name + '</a>');
                        text.click(function () {
                            data.record.StrDeliveryDate = FormatDateJsonToString(data.record.StockInDate, "yyyy-mm-dd");
                            // var a = FormatDateJsonToString(data.record.DeliveryDate, "yyyy-mm-dd'T'HH:MM:ss");

                            global.Data.CustomerId = data.record.PartnerId;
                            global.Data.StockInId = data.record.Id;
                            $("#spartner").val(data.record.PartnerId);
                            $("#spartner").attr("disabled", true);
                            $("#cphone").attr("disabled", true);
                            $('#date').val(data.record.StrDeliveryDate);
                            $("#sdescription").val(data.record.Description);

                           
                            $("#cphone").val(data.record.CustomerPhone);
                            $("#cmail").val(data.record.CustomerEmail);
                            $("#caddress").val(data.record.CustomerAddress);
                            $("#ctaxcode").val(data.record.CustomerTaxCode);
                        
                            while (global.Data.ModelStockInDetail.length) {
                                global.Data.ModelStockInDetail.pop();
                            }
                            global.Data.ModelStockInDetail.push.apply(global.Data.ModelStockInDetail, data.record.T_StockInDetail);
                            global.Data.Index = global.Data.ModelStockInDetail[global.Data.ModelStockInDetail.length - 1].Index + 1;
                            reloadListStockInDetail();
                            resetDetail();
                            global.Data.OrderTotal = 0;
                            for (var j = 0; j < global.Data.ModelStockInDetail.length; j++) {
                                global.Data.OrderTotal += parseFloat(global.Data.ModelStockInDetail[j].SubTotal);
                            }
                            $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                            //popAllElementInArray(global.Data.ModelStockInDetail);
                            $('.nav-tabs a:last').tab('show');

                        });
                        return text;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "15%"
                },
                SubTotal: {
                    title: "Tổng Tiền",
                    width: "7%"
                },
                StockInDate: {
                    title: 'Ngày Nhập',
                    width: "7%",
                    type: 'date',
                    displayFormat: 'dd-mm-yy'
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
            }
        });
    }

    function initComboBoxAllProduct(productId) {
        var url = "/StockIn/GetListProduct?productType=0";
        $.getJSON(url, function (datas) {
            $('#dproduct').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#dproduct').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
                $("#dproduct").val(productId);
            }
            else {
                $('#dproduct').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }

    function initListStockInDetail() {
        $('#' + global.Element.JtableStockInDetail).jtable({
            title: 'Danh Sách Chi Tiết Nhập Hàng',
            paging: true,
            pageSize: 15,
            pageSizeChangeStockIn: true,
            sorting: true,
            selectShow: true,         
            actions: {
                listAction: global.Data.ModelStockInDetail
            },
            messages: {
                addNewRecord: 'Thêm Mới nhập Hàng',
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột'
            },
            fields: {
                MateriaId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Index: {
                    title: "STT",
                    width: "5%"
                },
                MateriaName: {
                    visibility: 'fixed',
                    title: "Tên Dịch Vụ",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_StockIn" title="Chỉnh sửa thông tin.">' + data.record.MateriaName + '</a>');
                        text.click(function () {
                            initComboBoxAllProduct(data.record.MaterialId);
                            global.Data.Idnew = data.record.MarterialId;
                            $("#dnote").val(data.record.Description);                   
                            $("#dquantity").val(data.record.Quantity);
                            $("#dprice").val(data.record.Price);
                            $("#dsubtotal").val(data.record.SubTotal);
                            global.Data.CurenIndex = data.record.Index;
                        });
                        return text;
                    }
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
                                removeItemInArray(global.Data.ModelStockInDetail, data.record.Index);
                                reloadListStockInDetail();
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
        $("#cname").val("");
        $("#cphone").val("");
        $("#cmail").val("");
        $("#caddress").val("");
        $("#ctaxcode").val("");
        $("#dproductType").val(0);
        $("#dproduct").val(0);
        $("#dfilename").val("");
        $("#dquantity").val("");
        $("#dprice").val("");
        $("#dsubtotal").val("");
    }

    /*function Save */
    function saveStockIn() {
        if ($("#spartner").val() !== "0") {
            if (global.Data.ModelStockInDetail.length !== 0) {
                global.Data.OrderTotal = 0;
                for (var j = 0; j < global.Data.ModelStockInDetail.length; j++) {
                    global.Data.OrderTotal += parseFloat(global.Data.ModelStockInDetail[j].SubTotal);
                }
                var description = $("#sdescription").val();
                var customerName = $("#spartner option:selected").text();
                var dateDelivery = $("#date").val();
                $.ajax({
                    url: global.UrlAction.SaveStockIn + "?stockInId=" + global.Data.StockInId + "&description=" + description + "&customerId=" + global.Data.CustomerId + "&customerName=" + customerName + "&dateDelivery=" + dateDelivery + "&orderTotal=" + global.Data.OrderTotal,
                    type: 'post',
                    data: JSON.stringify({ 'listDetail': global.Data.ModelStockInDetail }),
                    contentType: 'application/json',
                    success: function (result) {
                        $('#loading').hide();
                        GlobalCommon.CallbackProcess(result, function () {
                            if (result.Result === "OK") {
                                resetAll();
                                toastr.success("Tạo mới Đơn hàng thành công");
                                reloadListStockIn();
                                $('.nav-tabs a:first').tab('show');
                            }
                        }, false, global.Element.PopupStockIn, true, true, function () {
                            var msg = GlobalCommon.GetErrorMessage(result);
                            GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                        });
                    }
                });
            } else {
                toastr.warning("Không có chi tiết đơn hàng nào");
            }

        } else {
            toastr.warning("Vui lòng chọn nhà cung cấp ");
        }

    }
    /*End Save */

    /*init Combobox*/
    function initComboBox() {
        var url = "/StockIn/GetListPartner";
        $.getJSON(url, function (datas) {
            $('#spartner').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#spartner').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#spartner').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initComboBox1() {
        var url = "/StockIn/GetListPartner";
        $.getJSON(url, function (datas) {
            $('#spartner1').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#spartner1').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#spartner1').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function initComboBoxProductType() {
        var url = "/StockIn/GetListProductType";
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
    function initComboBoxProduct(id) {
        var url = "/StockIn/GetListProduct?productType=" + id;
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
    function initPopupStockIn() {
        $("#" + global.Element.PopupStockIn).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupStockIn + "button[save]").click(function () {
            saveStockIn();
        });
        $("#" + global.Element.PopupStockIn + "button[cancel]").click(function () {
            $("#" + global.Element.PopupStockIn).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListStockIn = function () {
        reloadListStockIn();
    };
    this.initViewModel = function (stockIn) {
        initViewModel(stockIn);
    };
    this.bindData = function (stockIn) {
        bindData(stockIn);
    };
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
    var registerEvent = function () {
        $("#dproductType").change(function () {
            var id = $(this).val();
            global.Data.ProductTypeId = id;
            initComboBoxProduct(id);
        });
        $("#spartner").change(function () {
            var id = $(this).val();
            global.Data.CustomerId = id;
            $.ajax({
                url: "/StockIn/GetPartnerById?partId=" + id,
                type: 'post',
                contentType: 'application/json',
                success: function (result) {
                    GlobalCommon.CallbackProcess(result, function () {
                        if (result != null) {
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

        });
        $("#saveStockIn").click(function () {
            saveStockIn();
        });

        $("#dproduct").change(function () {
        });
        //$("[save]").click(function () {
        //    saveStockIn();
        //    var realTimeHub = $.connection.realTimeJTableDemoHub;
        //    realTimeHub.server.sendUpdateEvent("jtableStockIn");
        //    $.connection.hub.start();
        //});
        $("[cancel]").click(function () {
            bindData(null);
        });
        $("#resetDetail").click(function () {
            resetDetail();
            global.Data.CurenIndex = 0;
        });
        $("#resetStockIn").click(function () {
            if (global.Data.StockInId !== 0) {
              
                resetDetail();
                $("#sdescription").val("");

            } else {
                global.Data.StockInId = 0;
                resetAll();
            }
            global.Data.CurenIndex = 0;
        });

        $("#ProcessStockIn").click(function () {
            global.Data.StockInId = 0;
            $("#spartner").attr("disabled", false);
            $("#spartner").val(0);
            $("#sdescription").val("");
            $("#cphone").val("");
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
        });
        $("#CreateStockIn").click(function () {
            document.getElementById("date").defaultValue = new Date().toISOString().substring(0, 10);
            global.Data.StockInId = 0;
            $("#spartner").attr("disabled", false);
            $("#spartner").val(0);
            $("#sdescription").val("");
            $("#cphone").val("");
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
            resetAll();
            global.Data.CurenIndex = 0;
            global.Data.Index = 1;
            global.Data.StockInId = 0;
            while (global.Data.ModelStockInDetail.length) {
                global.Data.ModelStockInDetail.pop();
            }
            reloadListStockInDetail();
        });
        $("#canelStockIn").click(function () {
            resetAll();
            global.Data.CurenIndex = 0;
            global.Data.Index = 1;
            global.Data.StockInId = 0;
            while (global.Data.ModelStockInDetail.length) {
                global.Data.ModelStockInDetail.pop();
            }
            reloadListStockInDetail();
            $("#spartner").attr("disabled", false);
            $("#spartner").val(0);
            $("#sdescription").val("");
            $("#cphone").val("");
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
            $('.nav-tabs a:first').tab('show');
        });
        $("#cname").keydown(function (e) {
            global.Data.CustomerId = 0;
            $("#cphone").val("");
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
        });
        $("#cphone").keydown(function (e) {
            global.Data.CustomerId = 0;
            $("#cmail").val("");
            $("#caddress").val("");
            $("#ctaxcode").val("");
        });
        $('.caculator').keyup(function () {
            var width = $("#dwidth").val();
            var height = $("#dheignt").val();
            var sqare = calculatorSquare(width, height);
            if (!checkNumber(sqare) && sqare !== 0) {
                var roundSquare = decimalAdjust('round', sqare, -2);
                $("#dsquare").val(roundSquare);
            } else {
                $("#dsquare").val("");
            }
            var quantity = $("#dquantity").val();
            var price = $("#dprice").val();
            if (!checkNumber(quantity) && !checkNumber(price) && quantity !== "" && price !== "") {
                var total = calculatorSubTotal(sqare, quantity, price);
                if (!checkNumber(total)) {
                    var roundtotal = decimalAdjust('round', total, -2);
                    $("#dsubtotal").val(roundtotal);
                }
                else {
                    $("#dsubtotal").val("");
                }
            }

        });
        $(".detail").keydown(function (e) {

            if (e.which === 13) { //Enter
                if (checkValidate()) {
                    e.preventDefault();
                    //removeItemAndInsertInArray(arr, id, obj);
                    var objectIndex = global.Data.Index;
                    if (global.Data.CurenIndex !== 0) {
                        objectIndex = global.Data.CurenIndex;
                        var object = { Index: objectIndex, MaterialId: $("#dproduct").val(), MateriaName: $("#dproduct option:selected").text(), Description: $("#dnote").val(), Quantity: $("#dquantity").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        for (var i = 0; i < global.Data.ModelStockInDetail.length; i++) {
                            if (global.Data.ModelStockInDetail[i].Index === global.Data.CurenIndex) {
                                global.Data.ModelStockInDetail.splice(i, 1, object);
                                break;
                            }
                        };
                        global.Data.OrderTotal = 0;
                        for (var j = 0; j < global.Data.ModelStockInDetail.length; j++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelStockInDetail[j].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    } else {
                        var object1 = { Index: objectIndex, MaterialId: $("#dproduct").val(), MateriaName: $("#dproduct option:selected").text(), Description: $("#dnote").val(), Quantity: $("#dquantity").val(), Price: $("#dprice").val(), SubTotal: $("#dsubtotal").val() }
                        global.Data.ModelStockInDetail.push(object1);
                        global.Data.Index = global.Data.Index + 1;
                        global.Data.OrderTotal = 0;
                        for (var k = 0; k < global.Data.ModelStockInDetail.length; k++) {
                            global.Data.OrderTotal += parseFloat(global.Data.ModelStockInDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                        }
                        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }
                    global.Data.CurenIndex = 0;
                    reloadListStockInDetail();
                    resetDetail();
                }
            }
        });
    };
    this.Init = function () {
        registerEvent();
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24*30 * 60 * 60 * 1000).toISOString().substring(0, 10);
        document.getElementById("dateto").defaultValue = new Date().toISOString().substring(0, 10);
        initComboBox();
        initComboBox1();
        initComboBoxProductType();
        initListStockIn();
        initListStockInDetail();
        reloadListStockIn();
        reloadListStockInDetail();
        initPopupStockIn();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var stockIn = new VINASIC.StockIn();
    stockIn.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
    return true;
}