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
VINASIC.namespace("DashBoard");
VINASIC.DashBoard = function () {
    var global = {
        UrlAction: {
            GetListDashBoard: "/DashBoard/GetDashBoards",
            SaveDashBoard: "/DashBoard/SaveDashBoard",
            DeleteDashBoard: "/DashBoard/DeleteDashBoard"
        },
        Element: {
            JtableDashBoard: "jtableDashBoard",
            PopupDashBoard: "popup_DashBoard",
            PopupSearch: "popup_SearchDashBoard"
        },
        Data: {
            ModelDashBoard: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListDashBoard() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableDashBoard).jtable("load", { 'keyword': keySearch });
    }
    /*function init model using knockout Js*/
    function initViewModel(dashBoard) {
        var dashBoardViewModel = {
            Id: 0,
            Code: "",
            Name: "",
            Description: ""
        };
        if (dashBoard != null) {
            dashBoardViewModel = {
                Id: ko.observable(dashBoard.Id),
                Code: ko.observable(dashBoard.Code),
                Name: ko.observable(dashBoard.Name),
                Description: ko.observable(dashBoard.Description)
            };
        }
        return dashBoardViewModel;
    }
    function bindData(dashBoard) {
        global.Data.ModelDashBoard = initViewModel(dashBoard);
        ko.applyBindings(global.Data.ModelDashBoard);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupDashBoard() {
        $("#" + global.Element.PopupDashBoard).modal("show");
    }
    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteDashBoard,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListDashBoard();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupDashBoard, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */

    /*function Init List Using Jtable */
    function initListDashBoard() {
        $("#" + global.Element.JtableDashBoard).jtable({
            title: "Danh sách Loại Dịch Vụ",
            paging: true,
            pageSize: 10,
            pageSizeChangeDashBoard: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListDashBoard,
                createAction: global.Element.PopupDashBoard,
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
                    title: "Mã Loại",
                    width: "5%"
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Loại",
                    width: "25%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            bindData(data.record);
                            showPopupDashBoard();
                        });
                        return text;
                    }
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
                                var realTimeHub = $.connection.realTimeJTableDemoHub;
                                realTimeHub.server.sendUpdateEvent("jtableDashBoard");
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
    function saveDashBoard() {
        $.ajax({
            url: global.UrlAction.SaveDashBoard,
            type: 'post',
            data: ko.toJSON(global.Data.ModelDashBoard),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListDashBoard();
                        $("#" + global.Element.PopupDashBoard).modal("hide");
                        toastr.success('Thanh Cong');
                    }
                }, false, global.Element.PopupDashBoard, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupDashBoard() {
        $("#" + global.Element.PopupDashBoard).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupDashBoard + " button[save]").click(function () {
            saveDashBoard();
            global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableDashBoard", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupDashBoard + " button[cancel]").click(function () {
            $("#" + global.Element.PopupDashBoard).modal("hide");
        });
    }
    /*End bootrap*/
    /* Region Register and init*/
    this.reloadListDashBoard = function () {
        reloadListDashBoard();
    };
    this.initViewModel = function (dashBoard) {
        initViewModel(dashBoard);
    };
    this.bindData = function (dashBoard) {
        bindData(dashBoard);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });
    };
    this.Init = function () {
        registerEvent();
        var lineChartData = {
            labels: ["", "", "", "", "", "", ""],
            datasets: [
                {
                    fillColor: "rgba(220,220,220,0.5)",
                    strokeColor: "rgba(220,220,220,1)",
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: "#fff",
                    data: [65, 59, 90, 81, 56, 55, 40]
                },
                {
                    fillColor: "rgba(151,187,205,0.5)",
                    strokeColor: "rgba(151,187,205,1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    data: [28, 48, 40, 19, 96, 27, 100]
                }
            ]

        };
        new Chart(document.getElementById("linebusiness").getContext("2d")).Line(lineChartData);
        new Chart(document.getElementById("lineproduct").getContext("2d")).Line(lineChartData);
        //initListDashBoard();
        //reloadListDashBoard();
        //initPopupDashBoard();
        //bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var dashBoard = new VINASIC.DashBoard();
    dashBoard.Init();
});