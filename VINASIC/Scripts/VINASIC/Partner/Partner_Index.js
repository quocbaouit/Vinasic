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
VINASIC.namespace("Partner");
VINASIC.Partner = function () {
    var global = {
        UrlAction: {
            GetListPartner: "/Partner/GetPartners",
            SavePartner: "/Partner/SavePartner",
            DeletePartner: "/Partner/DeletePartner"
        },
        Element: {
            JtablePartner: "jtablePartner",
            PopupPartner: "popup_Partner",
            PopupSearch: "popup_SearchPartner"
        },
        Data: {
            ModelPartner: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListPartner() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtablePartner).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(partner) {
        var partnerViewModel = {
            Id: 0,
            Name: "",
            Address: "",
            Mobile: "",
            Email: "",
            TaxCode: ""
        };
        if (partner != null) {
            partnerViewModel = {
                Id: ko.observable(partner.Id),
                Name: ko.observable(partner.Name),
                Address: ko.observable(partner.Address),
                Mobile: ko.observable(partner.Mobile),
                Email: ko.observable(partner.Email),
                TaxCode: ko.observable(partner.TaxCode)
            };
        }
        return partnerViewModel;
    }
    function bindData(partner) {
        global.Data.ModelPartner = initViewModel(partner);
        ko.applyBindings(global.Data.ModelPartner);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupPartner() {
        $("#" + global.Element.PopupPartner).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeletePartner,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListPartner();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupPartner, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListPartner() {
        $("#" + global.Element.JtablePartner).jtable({
            title: "Danh sách Nhà Cung Cấp",
            paging: true,
            pageSize: 10,
            pageSizeChangePartner: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListPartner,
                createAction: global.Element.PopupPartner,
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
                Name: {
                    visibility: "fixed",
                    title: "Tên Nhà Cung Cấp",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupPartner();
                        });
                        return text;
                    }
                },
                Address: {
                    title: "Địa Chỉ",
                    width: "25%"
                },
                Mobile: {
                    title: "Số Điện Thoại",
                    width: "25%"
                },
                Email: {
                    title: "Email",
                    width: "25%"
                },
                TaxCode: {
                    title: "Mã Số Thuế",
                    width: "25%"
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
                                realTimeHub.server.sendUpdateEvent("jtablePartner");
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
    function savePartner() {
        $.ajax({
            url: global.UrlAction.SavePartner,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPartner),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListPartner();
                        $("#" + global.Element.PopupPartner).modal("hide");
                        toastr.success('Thành Công');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtablePartner", global.Data.ClientId, "Cập nhật Nhà Cung Cấp");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupPartner, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupPartner() {
        $("#" + global.Element.PopupPartner).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPartner + " button[save]").click(function () {
            savePartner();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePartner", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupPartner + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPartner).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPartner = function () {
        reloadListPartner();
    };
    this.initViewModel = function (partner) {
        initViewModel(partner);
    };
    this.bindData = function (partner) {
        bindData(partner);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListPartner();
        reloadListPartner();
        initPopupPartner();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var partner = new VINASIC.Partner();
    partner.Init();
});