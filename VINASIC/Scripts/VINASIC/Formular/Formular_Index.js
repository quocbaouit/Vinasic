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
VINASIC.namespace("Formular");
VINASIC.Formular = function () {
    var global = {
        UrlAction: {
            GetListFormular: "/Formular/GetFormulars",
            SaveFormular: "/Formular/SaveFormular",
            DeleteFormular: "/Formular/DeleteFormular"
        },
        Element: {
            JtableFormular: "jtableFormular",
            PopupFormular: "popup_Formular",
            JtableFormularDetail: "jtableFormularDetail",
            PopupSearch: "popup_SearchFormular"
        },
        Data: {
            ModelFormular: {},
            ModelConfig: {},
            ModelFormularDetail: [],
            ListElementFormular: ["aaa", "bbbb", "cccc", "dddd", "eeee", "ffff"],
            CurenIndex: 0,
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListFormular() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableFormular).jtable("load", { 'keyword': keySearch });
    }
    function reloadListFormularDetail() {
        $('#' + global.Element.JtableFormularDetail).jtable('load', { 'keyword': "" });
    }
    /*function init model using knockout Js*/
    function initViewModel(formular) {
        var formularViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (formular != null) {
            formularViewModel = {
                Id: ko.observable(formular.Id),
                Code: ko.observable(formular.Code),
                Name: ko.observable(formular.Name),
                Description: ko.observable(formular.Description)
            };
        }
        return formularViewModel;
    }
    function bindData(formular) {
        global.Data.ModelFormular = initViewModel(formular);
        ko.applyBindings(global.Data.ModelFormular);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupFormular() {
        $("#" + global.Element.PopupFormular).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteFormular,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListFormular();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupFormular, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListFormular() {
        $("#" + global.Element.JtableFormular).jtable({
            title: "Danh sách Công Thức Lương",
            paging: true,
            pageSize: 10,
            pageSizeChangeFormular: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListFormular
               
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
                Name: {
                    visibility: "fixed",
                    title: "Mô Tả",
                    width: "40%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupFormular();
                        });
                        return text;
                    }
                },
                Formular: {
                    title: "Công Thức",
                    width: "50%"
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
                                realTimeHub.server.sendUpdateEvent("jtableFormular");
                                $.connection.hub.start();
                            }, function () { }, "Đồng ý", "Hủy bỏ", "Thông báo");
                        });
                        return text;
                    }
                }
            }
        });
    }

    function initListOrderDetail() {
        $('#' + global.Element.JtableFormularDetail).jtable({
            title: 'Công Thức',
            paging: true,
            pageSize: 25,
            pageSizeChangeOrder: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.Data.ModelFormularDetail
            },
            messages: {
                addNewRecord: 'Thêm Mới Đại Lượng',
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
                CommodityName: {
                    visibility: 'fixed',
                    title: "Đại Lượng",
                    width: "10%",
                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.CommodityName + '</a>');
                        text.click(function () {
                            //initComboBoxAllProduct(data.record.CommodityId);
                            //global.Data.Idnew = data.record.CommodityId;
                            //$("#dfilename").val(data.record.FileName);
                            //$("#dnote").val(data.record.Description);
                            //$("#dwidth").val(decimalAdjust('round', data.record.Width, -6));
                            //$("#dheignt").val(decimalAdjust('round', data.record.Height, -6));
                            //$("#dsquare").val(decimalAdjust('round', data.record.Square, -6));
                            //$("#dsumsquare").val(decimalAdjust('round', data.record.SumSquare, -6));
                            //$("#dquantity").val(data.record.Quantity);
                            //$("#dprice").val(data.record.Price);
                            //$("#dsubtotal").val(data.record.SubTotal);
                            //global.Data.CurenIndex = data.record.Index;
                        });
                        return text;
                    }
                },
                DefaultValue: {
                    title: "Giá Trị Mặc Định",
                    width: "10%"
                },
                Delete: {
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button  title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        //text.click(function () {
                        //    GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                        //        removeItemInArray(global.Data.ModelOrderDetail, data.record.Index);
                        //        reloadListOrderDetail();
                        //        global.Data.OrderTotal = 0;
                        //        for (var k = 0; k < global.Data.ModelOrderDetail.length; k++) {
                        //            global.Data.OrderTotal += parseFloat(global.Data.ModelOrderDetail[k].SubTotal.replace(/[^0-9-.]/g, ''));
                        //        }
                        //        $("#dtotal").val(global.Data.OrderTotal.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                        //    }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        //});
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
    function saveFormular() {
        $.ajax({
            url: global.UrlAction.SaveFormular,
            type: 'post',
            data: ko.toJSON(global.Data.ModelFormular),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListFormular();
                        $("#" + global.Element.PopupFormular).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupFormular, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupFormular() {
        $("#" + global.Element.PopupFormular).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupFormular + " button[save]").click(function () {
            saveFormular();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableFormular", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupFormular + " button[cancel]").click(function () {
            $("#" + global.Element.PopupFormular).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListFormular = function () {
        reloadListFormular();
    };
    this.initViewModel = function (formular) {
        initViewModel(formular);
    };
    this.bindData = function (formular) {
        bindData(formular);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    function mappingAutoComplete() {
        $(function () {
            $("#ElementFormular").autocomplete({
                source: global.Data.ListElementFormular,
                select: function (a, b) {
                    var name = b;
                    var full = a;
                    //var cusName = b.item.value;
                    //$.ajax({
                    //    url: "/Order/GetCustomerByName?customerName=" + cusName,
                    //    type: 'post',
                    //    contentType: 'application/json',
                    //    success: function (result) {
                    //        GlobalCommon.CallbackProcess(result, function () {
                    //            if (1 < 2) {
                    //                var listCustomer = result.Records;
                    //                $('#cname').val(listCustomer.Name);
                    //                $('#cphone').val(listCustomer.Mobile);
                    //                $('#cmail').val(listCustomer.Email);
                    //                $('#caddress').val(listCustomer.Address);
                    //                $('#ctaxcode').val(listCustomer.TaxCode);
                    //                global.Data.CustomerId = listCustomer.Id;
                    //            }

                    //        }, false, global.Element.PopupOrder, true, true, function () {
                    //            var msg = GlobalCommon.GetErrorMessage(result);
                    //            GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                    //        });
                    //    }
                    //});
                }
            });
        });
    }
    this.Init = function () {
        registerEvent();
        initListFormular();
        initListOrderDetail();
        reloadListFormular();
        initPopupFormular();
        reloadListFormularDetail();
        mappingAutoComplete();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var formular = new VINASIC.Formular();
    formular.Init();
});