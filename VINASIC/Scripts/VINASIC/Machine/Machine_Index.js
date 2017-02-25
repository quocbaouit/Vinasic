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
VINASIC.namespace("Machine");
VINASIC.Machine = function () {
    var global = {
        UrlAction: {
            GetListMachine: "/Machine/GetMachines",
            SaveMachine: "/Machine/SaveMachine",
            DeleteMachine: "/Machine/DeleteMachine"
        },
        Element: {
            JtableMachine: "jtableMachine",
            PopupMachine: "popup_Machine",
            PopupSearch: "popup_SearchMachine"
        },
        Data: {
            ModelMachine: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListMachine() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableMachine).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(machine) {
        var machineViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Tempo: 0,
            Description: ""
        };
        if (machine != null) {
            machineViewModel = {
                Id: ko.observable(machine.Id),
                Code: ko.observable(machine.Code),
                Name: ko.observable(machine.Name),
                Tempo: ko.observable(machine.Tempo),
                Description: ko.observable(machine.Description)
            };
        }
        return machineViewModel;
    }
    function bindData(machine) {
        global.Data.ModelMachine = initViewModel(machine);
        ko.applyBindings(global.Data.ModelMachine);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupMachine() {
        $("#" + global.Element.PopupMachine).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteMachine,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListMachine();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupMachine, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListMachine() {
        $("#" + global.Element.JtableMachine).jtable({
            title: "Danh sách Máy In",
            paging: true,
            pageSize: 10,
            pageSizeChangeMachine: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListMachine,
                createAction: global.Element.PopupMachine,
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
                    title: "Mã Máy",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Máy",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupMachine();
                        });
                        return text;
                    }
                },
                Tempo: {
                    title: "Tốc Độ In(m/giờ)",
                    width: "20%"
                },
                Description: {
                    title: "Mô Tả",
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
                                global.Data.ClientId = document.getElementById("ClientName").innerHTML;
                                var realTimeHub = $.connection.realTimeJTableDemoHub;
                                realTimeHub.server.sendUpdateEvent("jtableMachine", global.Data.ClientId, "Cập nhật máy in");
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
    function saveMachine() {
        $.ajax({
            url: global.UrlAction.SaveMachine,
            type: 'post',
            data: ko.toJSON(global.Data.ModelMachine),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListMachine();
                        $("#" + global.Element.PopupMachine).modal("hide");
                        toastr.success('Thanh Cong');
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableMachine", global.Data.ClientId, "Cập nhật Máy In");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupMachine, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupMachine() {
        $("#" + global.Element.PopupMachine).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupMachine + " button[save]").click(function () {
            saveMachine();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableMachine", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupMachine + " button[cancel]").click(function () {
            $("#" + global.Element.PopupMachine).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListMachine = function () {
        reloadListMachine();
    };
    this.initViewModel = function (machine) {
        initViewModel(machine);
    };
    this.bindData = function (machine) {
        bindData(machine);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        initListMachine();
        reloadListMachine();
        initPopupMachine();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var machine = new VINASIC.Machine();
    machine.Init();
});