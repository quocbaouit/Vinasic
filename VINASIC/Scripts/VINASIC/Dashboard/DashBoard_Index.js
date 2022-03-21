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
            ClientId: "",
            OrderChart: {},
            PaymentChart: {},
            SumChart: {}
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

    function initDashBoard() {   
        var pieOptions = {
            segmentShowStroke: false,
            animateScale: true
        }
        global.Data.OrderChart = new Chart(document.getElementById("countries").getContext("2d"), {
            type: 'pie',
            data: {
                datasets: [{
                    data: [0, 0, 0],
                    backgroundColor: [

                        "#46BFBD",
                        "#FDB45C",
                        "#F7464A",
                    ],
                }],
                labels: [

                    'Đã Thu Tiền Mặt',
                    'Đã thu Chuyển Khoản',
                    'Còn Lại',
                ]
            },
            options: pieOptions
        });
        global.Data.PaymentChart = new Chart(document.getElementById("countries1").getContext("2d"), {
            type: 'pie',
            data: {
                datasets: [{
                    data: [0, 0],
                    backgroundColor: [

                        "#46BFBD",
                        "#F7464A"
                    ],
                }],
                labels: [

                    'Đã Thanh Toán',
                    'Còn Lại'
                ]
            },
            options: pieOptions
        });
        global.Data.SumChart = new Chart(document.getElementById("income"), {
            type: 'bar',
            data: {
                labels: ["ĐƠN HÀNG", "NHẬP HÀNG"],
                datasets: [
                    {
                        label: 'Tổng Tiền',
                        data: [0, 0],
                        backgroundColor: ['rgba(255, 99, 132, 0.2)',
                            'rgba(255, 99, 132, 0.2)'
                        ],

                        borderWidth: 1
                    },
                    {
                        label: 'Đã Thanh Toán',
                        data: [0, 0],
                        backgroundColor: ['rgba(54, 162, 235, 0.2)',
                            'rgba(54, 162, 235, 0.2)'
                        ],

                        borderWidth: 1
                    }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        }); 
        getDashBoardData();
    }
    /*End init */


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
    function getDashBoardData() {
        var fromDate = $("#datefrom").val();
        var toDate = $("#dateto").val();
        var url = "/Dashboard/GetData?from=" + fromDate + "&to=" + toDate + "";
        $.getJSON(url, function (datas) {
            debugger;
            global.Data.OrderChart.data.datasets[0].data[0] = datas.ModelDashBoardOrder.Value1;
            global.Data.OrderChart.data.datasets[0].data[1] = datas.ModelDashBoardOrder.Value2;
            global.Data.OrderChart.data.datasets[0].data[2] = datas.ModelDashBoardOrder.Value3;
            global.Data.OrderChart.update();
            var stra1 = datas.ModelDashBoardOrder.Value1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            $('#a1').html(stra1);
            var stra2 = datas.ModelDashBoardOrder.Value2.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            $('#a2').html(stra2);
            var stra3 = datas.ModelDashBoardOrder.Value3.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            $('#a3').html(stra3);

            global.Data.PaymentChart.data.datasets[0].data[0] = datas.ModelDashBoardPayment.Value1;
            global.Data.PaymentChart.data.datasets[0].data[1] = datas.ModelDashBoardPayment.Value2;
            global.Data.PaymentChart.update();
            var strb1 = datas.ModelDashBoardPayment.Value1.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            $('#b1').html(strb1);
            var strb2 = datas.ModelDashBoardPayment.Value2.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            $('#b2').html(strb2);

            global.Data.SumChart.data.datasets[0].data[0] = datas.ModelDashBoardSum.Value1;
            global.Data.SumChart.data.datasets[0].data[1] = datas.ModelDashBoardSum.Value2;

            global.Data.SumChart.data.datasets[1].data[0] = datas.ModelDashBoardSum.Value3;
            global.Data.SumChart.data.datasets[1].data[1] = datas.ModelDashBoardSum.Value4;
            global.Data.SumChart.update();
        });
    }
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
        $("[search]").click(function () {
            getDashBoardData();           
        });
    };
    this.Init = function () {
        registerEvent();
        document.getElementById("datefrom").defaultValue = new Date(new Date() - 24 * 90 * 60 * 60 * 1000).toISOString().substring(0, 10);
        var dateTo = new Date();
        dateTo.setDate(dateTo.getDate() + 1);
        document.getElementById("dateto").defaultValue = dateTo.toISOString().substring(0, 10);    
        initDashBoard();
        reloadListDashBoard();
        initPopupDashBoard();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var dashBoard = new VINASIC.DashBoard();
    dashBoard.Init();
});