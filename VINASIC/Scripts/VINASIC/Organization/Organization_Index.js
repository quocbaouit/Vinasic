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
VINASIC.namespace("Organization");
VINASIC.Organization = function () {
    var global = {
        UrlAction: {
            GetListOrganization: "/Organization/GetOrganizations",
            SaveOrganization: "/Organization/SaveOrganization",
            DeleteOrganization: "/Organization/DeleteOrganization"
        },
        Element: {
            JtableOrganization: "jtableOrganization",
            PopupOrganization: "popup_Organization",
            PopupSearch: "popup_SearchOrganization"
        },
        Data: {
            ModelOrganization: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListOrganization() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableOrganization).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(organization) {
        var organizationViewModel = {
            Id: 0,
            ShortName: "",
            Name: "",
            Description: ""
        };
        if (organization != null) {
            organizationViewModel = {
                Id: ko.observable(organization.Id),
                ShortName: ko.observable(organization.ShortName),
                Name: ko.observable(organization.Name),
                Description: ko.observable(organization.Description)
            };
        }
        return organizationViewModel;
    }
    function bindData(organization) {
        global.Data.ModelOrganization = initViewModel(organization);
        ko.applyBindings(global.Data.ModelOrganization);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupOrganization() {
        $("#" + global.Element.PopupOrganization).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteOrganization,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListOrganization();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupOrganization, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListOrganization() {
        $("#" + global.Element.JtableOrganization).jtable({
            title: "Danh sách Phòng Ban",
            paging: true,
            pageSize: 10,
            pageSizeChangeOrganization: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListOrganization,
                createAction: global.Element.PopupOrganization,
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
                ShortName: {
                    title: "Mã",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Phòng Ban",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupOrganization();
                        });
                        return text;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "25%"
                }
                //Delete: {
                //    title: "Xóa",
                //    width: "5%",
                //    sorting: false,
                //    display: function (data) {
                //        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                //        text.click(function () {
                //            GlobalCommon.ShowConfirmDialog("Bạn có chắc chắn muốn xóa?", function () {
                //                deleteRow(data.record.Id);
                //                var realTimeHub = $.connection.realTimeJTableDemoHub;
                //                realTimeHub.server.sendUpdateEvent("jtableOrganization");
                //                $.connection.hub.start();
                //            }, function () { }, "Đồng ý", "Hủy bỏ", "Thông báo");
                //        });
                //        return text;
                //    }
                //}
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
    function saveOrganization() {
        $.ajax({
            url: global.UrlAction.SaveOrganization,
            type: 'post',
            data: ko.toJSON(global.Data.ModelOrganization),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListOrganization();
                        $("#" + global.Element.PopupOrganization).modal("hide");
                        toastr.success('Thanh Cong');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableOrganization", global.Data.ClientId, "Cập nhật Phòng Ban");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupOrganization, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupOrganization() {
        $("#" + global.Element.PopupOrganization).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupOrganization + " button[save]").click(function () {
            saveOrganization();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableOrganization", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupOrganization + " button[cancel]").click(function () {
            $("#" + global.Element.PopupOrganization).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListOrganization = function () {
        reloadListOrganization();
    };
    this.initViewModel = function (organization) {
        initViewModel(organization);
    };
    this.bindData = function (organization) {
        bindData(organization);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListOrganization();
        reloadListOrganization();
        initPopupOrganization();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var organization = new VINASIC.Organization();
    organization.Init();
});