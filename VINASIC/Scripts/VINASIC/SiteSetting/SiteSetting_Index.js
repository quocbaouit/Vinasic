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
VINASIC.namespace("SiteSetting");
VINASIC.SiteSetting = function () {
    var global = {
        UrlAction: {
            GetListSiteSetting: "/SiteSetting/GetSiteSettings",
            SaveSiteSetting: "/SiteSetting/SaveSiteSetting",
            DeleteSiteSetting: "/SiteSetting/DeleteSiteSetting"
        },
        Element: {
            JtableSiteSetting: "jtableSiteSetting",
            PopupSiteSetting: "popup_SiteSetting",
            PopupSearch: "popup_SearchSiteSetting"
        },
        Data: {
            ModelSiteSetting: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListSiteSetting() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableSiteSetting).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(siteSetting) {
        var siteSettingViewModel = {
            Id: 0,
            Code:"",
            Name: "",
            Value:"",
            Description: ""
        };
        if (siteSetting != null) {
            siteSettingViewModel = {
                Id: ko.observable(siteSetting.Id),   
                Code: ko.observable(siteSetting.Code),
                Name: ko.observable(siteSetting.Name),
                Value: ko.observable(siteSetting.Value),
                Description: ko.observable(siteSetting.Description)
            };
        }
        return siteSettingViewModel;
    }
    function bindData(siteSetting) {
        global.Data.ModelSiteSetting = initViewModel(siteSetting);
        ko.applyBindings(global.Data.ModelSiteSetting);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupSiteSetting() {
        $("#" + global.Element.PopupSiteSetting).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteSiteSetting,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListSiteSetting();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupSiteSetting, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListSiteSetting() {
        $("#" + global.Element.JtableSiteSetting).jtable({
            title: "Danh sách Cấu Hình",
            paging: true,
            pageSize: 10,
            pageSizeChangeSiteSetting: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListSiteSetting,
                createAction: global.Element.PopupSiteSetting,
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
                Code: {
                    title: "Code",
                    width: "10%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupSiteSetting();
                        });
                        return text;
                    }
                },
                Value: {
                    title: "Giá Trị",
                    width: "35%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%"
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
                //                realTimeHub.server.sendUpdateEvent("jtableSiteSetting");
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
    function saveSiteSetting() {
        $.ajax({
            url: global.UrlAction.SaveSiteSetting,
            type: 'post',
            data: ko.toJSON(global.Data.ModelSiteSetting),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListSiteSetting();
                        $("#" + global.Element.PopupSiteSetting).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, global.Element.PopupSiteSetting, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupSiteSetting() {
        $("#" + global.Element.PopupSiteSetting).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupSiteSetting + " button[save]").click(function () {
            saveSiteSetting();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableSiteSetting", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupSiteSetting + " button[cancel]").click(function () {
            $("#" + global.Element.PopupSiteSetting).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListSiteSetting = function () {
        reloadListSiteSetting();
    };
    this.initViewModel = function (siteSetting) {
        initViewModel(siteSetting);
    };
    this.bindData = function (siteSetting) {
        bindData(siteSetting);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListSiteSetting();
        reloadListSiteSetting();
        initPopupSiteSetting();       
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var siteSetting = new VINASIC.SiteSetting();
    siteSetting.Init();
});