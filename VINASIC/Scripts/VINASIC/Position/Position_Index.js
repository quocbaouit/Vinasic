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
VINASIC.namespace("Position");
VINASIC.Position = function () {
    var global = {
        UrlAction: {
            GetListPosition: "/Position/GetPositions",
            SavePosition: "/Position/SavePosition",
            DeletePosition: "/Position/DeletePosition"
        },
        Element: {
            JtablePosition: "jtablePosition",
            PopupPosition: "popup_Position",
            PopupSearch: "popup_SearchPosition"
        },
        Data: {
            ModelPosition: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListPosition() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtablePosition).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(position) {
        var positionViewModel = {
            Id: 0,
            OrganizationName: "",
            Name: ""
        };
        if (position != null) {
            positionViewModel = {
                Id: ko.observable(position.Id),
                Name: ko.observable(position.Name),
                OrganizationName: ko.observable(position.OrganizationName)
            };
        }
        return positionViewModel;
    }
    function bindData(position) {
        global.Data.ModelPosition = initViewModel(position);
        ko.applyBindings(global.Data.ModelPosition);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupPosition() {
        $("#" + global.Element.PopupPosition).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeletePosition,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListPosition();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupPosition, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function initComboBoxOrganization() {
        var url = "/Position/GetOrganization";
        $.getJSON(url, function (datas) {
            $('#organization').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#organization').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#organization').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    /*function Init List Using Jtable */
    function initListPosition() {
        $("#" + global.Element.JtablePosition).jtable({
            title: "Danh sách Loại Chức Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangePosition: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListPosition,
                createAction: global.Element.PopupPosition,
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
                    title: "Tên Loại",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            $('#organization').val(data.record.OrganizationId);
                            bindData(data.record);
                            showPopupPosition();
                        });
                        return text;
                    }
                },
                OrganizationName: {
                    title: "Phòng Ban",
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
                                realTimeHub.server.sendUpdateEvent("jtablePosition");
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
    function savePosition() {
        global.Data.ModelPosition.OrganizationId = $('#organization').val();
        $.ajax({
            url: global.UrlAction.SavePosition,
            type: 'post',
            data: ko.toJSON(global.Data.ModelPosition),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListPosition();
                        $("#" + global.Element.PopupPosition).modal("hide");
                        toastr.success('Thanh Cong');
                        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtablePosition", global.Data.ClientId, "Cập nhật Chức Vụ");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupPosition, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupPosition() {
        $("#" + global.Element.PopupPosition).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupPosition + " button[save]").click(function () {
            savePosition();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtablePosition", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupPosition + " button[cancel]").click(function () {
            $("#" + global.Element.PopupPosition).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListPosition = function () {
        reloadListPosition();
    };
    this.initViewModel = function (position) {
        initViewModel(position);
    };
    this.bindData = function (position) {
        bindData(position);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initComboBoxOrganization();
        initListPosition();
        reloadListPosition();
        initPopupPosition();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var position = new VINASIC.Position();
    position.Init();
});