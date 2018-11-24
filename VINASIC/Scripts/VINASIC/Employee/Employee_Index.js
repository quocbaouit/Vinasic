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
VINASIC.namespace("Employee");
VINASIC.Employee = function () {
    var global = {
        UrlAction: {
            GetListRole: '/Role/GetRolesForUser',
            GetListEmployee: "/Employee/GetEmployees",
            SaveEmployee: "/Employee/SaveEmployee",
            DeleteEmployee: "/Employee/DeleteEmployee",
            GetRoleIdByUserId: "/Employee/GetRoleIdByUserId",
            GetProductIdByUserId: "/Employee/GetProductIdByUserId",
            SaveUSerRole: "/Role/SaveUserRole",
            SaveUSerSalery: "/Employee/SaveUserSalery",
            SaveUSerProduct: "/Employee/SaveUserProduct",
            GetProductForUser: "/Employee/GetProductForUser"
        },
        Element: {
            JtableEmployee: "jtableEmployee",
            JtableSalary: "jtableSalary",
            PopupEmployee: "popup_Employee",
            PopupUserRole: "popup_UserRole",
            PopupSalary: "popup_salary",
            PopupSalaryEdit: "popup_salary_edit",
            PopupSearch: "popup_SearchEmployee",
            JtableUserRole: 'jtableUserRole',
            JtableProductUser: "jtableProductUser",
            PopupTiming: "popupEventForm",
            PopupMainTiming: "popup_MainTiming"
        },
        Data: {
            PcustomerName: '',
            PcustomePhone: '',
            PcustomerAddress:'',
            ModelEmployee: {},
            ModelConfig: {},
            UserId: 0,
            listSalary: [],
            listSelectRole: [],
            UserRoleModel: { UserId: 0, ListRole: [] },
            ListSelectProductModel: { UserId: 0, ListSelectProduct: [] },
            ClientId: "",
            ListSelectProduct: [],
            EmpId: 0
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function renderTable(Table,name,address,mobile) {
        var tableString = "<table id=\"renderTable\" border=\"1\" style=\"width:100%\" cellspacing=\"0\" cellpadding=\"0\">";
        var root = document.getElementById('Block4');
        document.getElementById("Block4").innerHTML = "";
        tableString += "<tr>";
        tableString += "<th style=\"padding-left: 5px;text-align: left;\">" + "Nội Dung" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Số Lượng" + "</th>";
        tableString += "<th style=\"padding-right: 5px;text-align: right;\">" + "Đơn Vị" + "</th>";
        tableString += "</tr>";
        for (row = 0; row < Table.length; row += 1) {
            var strAmount = Table[row].Amount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            tableString += "<tr>";
            tableString += "<td style=\"padding-left: 5px;\">" + Table[row].Content + "</td>";
            tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + strAmount + "</td>";
            tableString += "<td style=\"padding-right: 5px;text-align: right;\">" + Table[row].Unit + "</td>";
            tableString += "</tr>";
        }

        if (Table.length < 9) {
            var remain = 9 - Table.length;
            for (i = 0; i < remain; i += 1) {
                tableString += "<tr>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "<td>" + "&nbsp;" + "</td>";
                tableString += "</tr>";
            }
        }
        //tableString += "<tr>";
        //tableString += "<td colspan=\"6\">Tổng Tiền</td>";
        //tableString += "<td style=\"padding-right: 5px;;text-align: right;\"><span id=\"vtotal1\">55577854</span></td>";
        //tableString += "</tr>";
        //tableString += "<tr>";
        //tableString += "<td colspan=\"7\">Bằng Chữ:<span id=\"strtotal1\">Test </span></td>";
        //tableString += "</tr>";
        tableString += "</table>";
        root.innerHTML = tableString;
    }
    function printPanel() {
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
    function reloadListEmployee() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableEmployee).jtable("load", { 'keyword': keySearch });
    }
    function reloadListUserRole() {
        var keySearch = "";
        innitListSelect(global.Data.UserId);
        $("#" + global.Element.JtableUserRole).jtable("load", { 'keyword': keySearch, 'UserId': global.Data.UserId });
    }
    function reloadListSalary() {
        debugger;
        var keySearch = "";
        $("#" + global.Element.JtableSalary).jtable("load", { 'keyword': keySearch });
    }
    function reloadListProduct() {
        var keySearch = "abc";
        innitListSelectProduct(global.Data.UserId);
        $("#" + global.Element.JtableProductUser).jtable("load", { 'keyword': keySearch, 'UserId': global.Data.UserId });
    }
    this.reloadListUserRole = function () {
        reloadListUserRole();
    }
    function removeItemInArray(arr, id) {
        if (typeof (arr) != "undefined") {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] === id) {
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
    /*function init model using knockout Js*/
    function initViewModel(employee) {
        var employeeViewModel = {
            Id: 0,
            Name: "",
            Address: "",
            Mobile: "",
            Email: "",
            PositionName: "",
            UserName: ""
        };
        if (employee != null) {
            employeeViewModel = {
                Id: ko.observable(employee.Id),
                Name: ko.observable(employee.Name),
                UserName: ko.observable(employee.UserName),
                Address: ko.observable(employee.Address),
                Mobile: ko.observable(employee.Mobile),
                Email: ko.observable(employee.Email),
                PositionName: ko.observable(employee.PositionName)
            };
        }
        return employeeViewModel;
    }
    function bindData(employee) {
        global.Data.ModelEmployee = initViewModel(employee);
        ko.applyBindings(global.Data.ModelEmployee);
    }
    /*end function*/

    /*function show Popup*/
    function showPopupEmployee() {
        $("#" + global.Element.PopupEmployee).modal("show");
    }

    function showPopupUserRole() {
        $("#" + global.Element.PopupUserRole).modal("show");
    }

    function showPopupSalary() {
        $("#" + global.Element.PopupSalary).modal("show");
    }
    function showPopupSalaryEdit() {
        $("#" + global.Element.PopupSalaryEdit).modal("show");
    }
    function showPopupTiming() {
        $("#" + global.Element.PopupTiming).modal("show");

    }
    function showpopupMainTiming() {
        $("#" + global.Element.PopupMainTiming).modal("show");
    }

    /*End*/

    /*function Delete */
    function deleteRow(id) {
        $.ajax({
            url: global.UrlAction.DeleteEmployee,
            type: 'POST',
            data: JSON.stringify({ 'id': id }),
            contentType: 'application/json charset=utf-8',
            success: function (data) {
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result === "OK") {
                        reloadListEmployee();
                        toastr.success('Xóa Thành Công');
                    }
                }, false, global.Element.PopupEmployee, true, true, function () {

                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra.");
                });
            }
        });
    }
    /*End Delete */
    function initComboBoxPosition() {
        var url = "/Employee/GetEmployeePosition";
        $.getJSON(url, function (datas) {
            $('#position').empty();
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    $('#position').append('<option value="' + datas[i].Value + '">' + datas[i].Text + '</option>');
                }
            }
            else {
                $('#position').append('<option value="0">Không Có Dữ Liệu </option>');
            }
        });
    }
    function innitListSelectProduct(userId) {
        $.ajax({
            url: "/Employee/GetProductIdByUserId?userId=" + userId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        while (global.Data.ListSelectProduct.length) {
                            global.Data.ListSelectProduct.pop();
                        }
                        for (var i = 0; i < result.Records.length; i++) {
                            global.Data.ListSelectProduct.push(result.Records[i]);
                        };
                    }
                }, false, null, true, true, function () {

                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function ResetPass(empId) {
        $.ajax({
            url: "/Employee/ResetPass?empId=" + empId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListEmployee();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function updateLock(userId, isLock) {
        $.ajax({
            url: "/Employee/UpdateLock?userId=" + userId + "&isLock=" + isLock,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListEmployee();
                        toastr.success("Thành Công");
                    }
                }, false, global.Element.PopupOrder, true, true, function () {

                    toastr.error(result.Message);
                });
            }
        });
    }
    function initListSalary() {
        debugger;
        $('#' + global.Element.JtableSalary).jtable({
            title: 'Bảng Lương',
            paging: true,
            pageSize: 25,
            pageSizeChangeSalary: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.Data.listSalary,
                createAction: global.Element.PopupSalaryEdit,
            },
            messages: {
                addNewRecord: 'Thêm Mới',
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
                Index: {
                    title: "STT",
                    width: "5%"
                },
                Content: {
                    visibility: 'fixed',
                    title: "Nội Dung",
                    width: "10%",

                    display: function (data) {
                        var text = $('<a href="javascript:void(0)" class="clickable"  data-target="#popup_Order" title="Chỉnh sửa thông tin.">' + data.record.Content + '</a>');
                        text.click(function () {
                            $("#salary-id").val(data.record.Id);
                            $("#salary-index").val(data.record.Index);
                            $("#salary-content").val(data.record.Content);
                            $("#salary-amount").val(data.record.Amount);
                            $("#salary-unit").val(data.record.Unit);
                            showPopupSalaryEdit();
                            //global.Data.CurenIndex = data.record.Index;
                        });
                        return text;
                    }
                },
                Amount: {
                    title: "Giá Trị",
                    width: "10%",
                    display: function (data) {
                        return data.record.Amount.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    }
                },
                Unit: {
                    title: "Đơn Vị",
                    width: "10%"
                },
                Delete: {
                    title: 'Xóa',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button  title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                removeItemInArray1(global.Data.listSalary, data.record.Id);
                                reloadListSalary();
                                //var total = 0;
                                //for (var k = 0; k < global.Data.listSalary.length; k++) {
                                //    total += parseFloat(global.Data.listSalary[k].Amount.replace(/[^0-9-.]/g, ''));
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
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }
    /*function Init List Using Jtable */
    function initListEmployee() {
        $("#" + global.Element.JtableEmployee).jtable({
            title: "Danh Sách Nhân Viên",
            paging: true,
            pageSize: 50,
            pageSizeChangeEmployee: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListEmployee,
                createAction: global.Element.PopupEmployee,
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
                strIsLock: {
                    title: "Khóa TK",
                    width: "3%",
                    display: function (data) {
                        var text = "";
                        if (data.record.IsLock == false) { text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\" khóa tài khoản.\"><span class=\"fa fa-check-square-o fa-lg\" aria-hidden=\"true\"></span></a>"); }
                        else {
                            text = $("<a href=\"javascript:void(0)\" class=\"clickable\" title=\"mở khóa tài khoản.\"><span class=\"fa fa-times-circle fa-lg\" aria-hidden=\"true\"></span></a>");
                        }
                        text.click(function () {
                            updateLock(data.record.Id, data.record.IsLock);
                        });
                        return text;
                    }
                },
                Name: {
                    visibility: "fixed",
                    title: "Tên Nhân Viên",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.Name + "</a>");
                        text.click(function () {
                            $('#position').val(data.record.PositionId);
                            bindData(data.record);
                            showPopupEmployee();
                        });
                        return text;
                    }
                },
                Address: {
                    title: "Địa Chỉ",
                    width: "20%"
                },
                UserName: {
                    title: "Tên Đăng Nhập",
                    width: "5%"
                },
                Mobile: {
                    title: "Số Điện Thoại",
                    width: "10%"
                },
                stringRoleName: {
                    visibility: "fixed",
                    title: "Nhóm Quyền",
                    width: "20%",
                    display: function (data) {
                        var text = $("<a href=\"javascript:void(0)\"  class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.stringRoleName + "</a>");
                        text.click(function () {
                            global.Data.UserId = data.record.Id;
                            showPopupUserRole();
                            reloadListUserRole();
                        });
                        return text;
                    }
                },
                stringSalary: {
                    visibility: "fixed",
                    title: "Bảng Lương",
                    width: "10%",
                    display: function (data) {
                        var text = $("<a href=\"javascript:void(0)\"  class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + "Xem Bảng Lương" + "</a>");
                        text.click(function () {
                            debugger;
                            global.Data.UserId = data.record.Id;
                            while (global.Data.listSalary.length) {
                                global.Data.listSalary.pop();
                            }
                            global.Data.listSalary.push.apply(global.Data.listSalary, data.record.SalaryObj);
                            global.Data.PcustomerName = data.record.Name
                            global.Data.PcustomePhone = data.record.Mobile
                            global.Data.PcustomerAddress = data.record.Address;
                           // renderTable(data.record.SalaryObj);
                       
                            //var c = docso(global.Data.listSalary[global.Data.listSalary.length - 1].Amount);
                            //$("#strtotal1").append(c);
                            global.Data.listSalary.sort(function (a, b) {
                                return a.Index == b.Index ? 0 : +(a.Index > b.Index) || -1;
                            });
                            reloadListSalary();
                            showPopupSalary();

                        });
                        return text;
                    }
                },
                PositionName: {
                    title: "Chức Vụ",
                    width: "10%"
                },
                stringTiming: {
                    visibility: "fixed",
                    title: "Chấm Công",
                    width: "5%",
                    display: function (data) {
                        var text = $("<a href=\"javascript:void(0)\"  class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + "Chấm Công" + "</a>");
                        text.click(function () {

                            showpopupMainTiming();
                            var delay = 500;

                            setTimeout(function () {
                                global.Data.EmpId = data.record.Id;
                                document.getElementById('calendar1').innerHTML = '';
                                document.getElementById("calendar1").removeAttribute("class");
                                initCalendar();
                            }, delay);

                        });
                        return text;
                    }
                },
                //EditPermission: {
                //    title: 'In An',
                //    width: "3%",
                //    sorting: false,
                //    display: function (data) {
                //        var text = '';
                //        text = $('<a href="javascript:void(0)" title="chỉnh sửa"><span>Quyền In Ấn</a>');
                //            text.click(function () {
                //                global.Data.UserId = data.record.Id;
                //                $('.nav-tabs a:last').tab('show');
                //                document.getElementById("roleTemp").innerHTML = "In ấn cho nhân viên: " + data.record.Name;
                //                reloadListProduct();
                //            });

                //        return text;
                //    }
                //},
                ResetPass: {
                    visibility: "fixed",
                    title: "Reset Pass",
                    width: "5%",
                    display: function (data) {

                        var stringLabel = "Reset Pass";
                        if (data.record.PassWord === "e10adc3949ba59abbe56e057f20f883e") {
                            stringLabel = "Pass Default";
                        }
                        var text = $("<a href=\"javascript:void(0)\"  class=\"clickable\" title=\"Reset pass.\">" + stringLabel + "</a>");
                        text.click(function () {
                            ResetPass(data.record.Id);
                        });
                        return text;
                    }
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
                                realTimeHub.server.sendUpdateEvent("jtableEmployee");
                                $.connection.hub.start();
                            }, function () { }, "Đồng ý", "Hủy bỏ", "Thông báo");
                        });
                        return text;
                    }
                }
            }
        });
    }

    function InitListUserRole() {
        $('#' + global.Element.JtableUserRole).jtable({
            title: 'Chọn Nhóm Quyền Cho Nhân Viên',
            paging: false,
            pageSize: 100,
            pageSizeChangeRole: false,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetListRole
            },
            messages: {
                selectShow: 'Ẩn hiện cột',
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Select: {
                    title: 'Chọn',
                    width: '2%',
                    list: true,
                    display: function (data) {
                        var text = "";
                        if (data.record.Selected == true) {
                            text = $("<input id=check" + data.record.Id + " type=\"checkbox\" checked>");
                        }
                        else {
                            text = $("<input id=check" + data.record.Id + " type=\"checkbox\">");
                        }
                        text.click(function () {
                            var id = "check" + data.record.Id;
                            var isCheck = document.getElementById(id).checked;
                            if (isCheck == true) {
                                global.Data.listSelectRole.push(data.record.Id);
                            }
                            else {
                                removeItemInArray(global.Data.listSelectRole, data.record.Id)
                            }

                        });
                        return text;
                    }
                },
                RoleName: {
                    visibility: 'fixed',
                    title: "Tên Nhóm Quyền",
                    width: "20%"
                },

                IsSystem: {
                    title: "System",
                    width: "3%",
                },
                Decription: {
                    title: "Mô Tả",
                    width: "20%"
                }
            },

            selectionChanged: function () {
                toastr.success("Seclect");
                var $selectedRows = $('#jtableUserRole').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                } else {
                    //No rows selected
                    // $('#checkSum').val(0);
                }
            }
        });
    }
    /*End init List */
    function jtableProductUser() {
        $("#" + global.Element.JtableProductUser).jtable({
            title: "",
            paging: false,
            pageSize: 1000,
            pageSizeChangeProduct: false,
            sorting: false,
            selectShow: true,
            actions: {
                listAction: global.UrlAction.GetProductForUser
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

                Select: {
                    title: 'Chọn',
                    width: '2%',
                    list: true,
                    display: function (data) {
                        var text = "";
                        if (data.record.Selected === true) {
                            text = $("<input id=checkProduct" + data.record.Id + " type=\"checkbox\" checked>");
                        }
                        else {
                            text = $("<input id=checkProduct" + data.record.Id + " type=\"checkbox\">");
                        }

                        text.click(function () {
                            var id = "checkProduct" + data.record.Id;
                            var isCheck = document.getElementById(id).checked;
                            if (isCheck === true) {
                                global.Data.ListSelectProduct.push(data.record.Id);
                            }
                            else {
                                removeItemInArray(global.Data.ListSelectProduct, data.record.Id);
                            }

                        });
                        return text;
                    }
                },
                Name: {
                    title: "Tên Dich Vu",
                    width: "20%"
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%"
                }

            }
        });
    }
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
    function initCalendar() {
        var sourceSummaryView = { url: '/Timing/GetDiarySummary/' };

        $('#calendar1').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month'
            },
            defaultView: 'month',
            editable: true,
            allDaySlot: false,
            selectable: true,
            slotMinutes: 15,
            events: {
                url: '/Timing/GetDiarySummary',
                type: 'POST',
                data: {
                    empId: global.Data.EmpId,
                },
                error: function () {
                    toastr.warning('Da coloi xay ra');
                },
                color: 'yellow',   // a non-ajax option
                textColor: 'black' // a non-ajax option
            },
            monthNames: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu', 'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
            monthNamesShort: ['Một', 'Hai', 'Ba', 'Bốn', 'Năm', 'Sáu', 'Bảy', 'Tám', 'Chín', 'Mười', 'Mười Một', 'Mười Hai'],
            dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
            dayNamesShort: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],

            dayClick: function (date, allDay, jsEvent, view) {
                $('#eventTitle').val("");
                var events = $('#calendar1').fullCalendar('clientEvents');
                for (var i = 0; i < events.length; i++) {
                    if (moment(events[i].start).isSame(date, 'day')) {
                        $('#eventTitle').val(events[i].title);
                    }
                }
                $('#eventDate').val($.fullCalendar.formatDate(date, 'dd/MM/yyyy'));
                $('#eventTime').val($.fullCalendar.formatDate(date, 'HH:mm'));
                showPopupTiming();
                var delay = 500;
                setTimeout(function () {
                    $('#eventTitle').focus();
                }, delay);
            },
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                revertFunc();
            }
        });
    }
    /*function Save */
    function saveEmployee() {
        global.Data.ModelEmployee.PositionId = $('#position').val();
        $.ajax({
            url: global.UrlAction.SaveEmployee,
            type: 'post',
            data: ko.toJSON(global.Data.ModelEmployee),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        bindData(null);
                        reloadListEmployee();
                        $("#popup_Employee").modal("hide");
                        toastr.success("Thành Công");
                        var realTimeHub = $.connection.realTimeJTableDemoHub;
                        realTimeHub.server.sendUpdateEvent("jtableEmployee", global.Data.ClientId, "Cập nhật Nhân Viên");
                        $.connection.hub.start();
                    }
                }, false, global.Element.PopupEmployee, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function saveUserProduct() {
        global.Data.ListSelectProductModel.UserId = global.Data.UserId;
        global.Data.ListSelectProductModel.ListSelectProduct = global.Data.ListSelectProduct;
        $.ajax({
            url: global.UrlAction.SaveUSerProduct,
            type: 'post',
            data: JSON.stringify(global.Data.ListSelectProductModel),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListEmployee();
                        $("#" + global.Element.PopupUserRole).modal("hide");
                        toastr.success('Cập Nhật Thành Công');
                        $('.nav-tabs a:first').tab('show');
                    }
                }, false, global.Element.PopupUserRole, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function SaveSalary() {
        $.ajax({
            url: global.UrlAction.SaveUSerSalery + "?employId=" + global.Data.UserId,
            type: 'post',
            data: JSON.stringify(global.Data.listSalary),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListEmployee();
                        $("#" + global.Element.PopupSalary).modal("hide");
                        toastr.success('Cập Nhật Thành Công');
                    }
                }, false, global.Element.PopupSalary, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function SaveSalaryEdit() {
        debugger;
        var salary = { Id: $("#salary-id").val(), Index: $("#salary-index").val(), Content: $("#salary-content").val(), Amount: $("#salary-amount").val().replace(/[^0-9-.]/g, ''), Unit: $("#salary-unit").val() };
        if ($("#salary-id").val() != '') {
            for (var i = 0; i < global.Data.listSalary.length; i++) {
                if (global.Data.listSalary[i].Id === salary.Id) {
                    global.Data.listSalary.splice(i, 1, salary);
                    break;
                }
            };
        }
        else {
            salary.Id = guid();
            global.Data.listSalary.push(salary);

        }
        global.Data.listSalary.sort(function (a, b) {
            return a.Index == b.Index ? 0 : +(a.Index > b.Index) || -1;
        });
        $("#" + global.Element.PopupSalaryEdit).modal("hide");
        reloadListSalary();
    }
    function SaveUserRole() {
        global.Data.UserRoleModel.UserId = global.Data.UserId;
        global.Data.UserRoleModel.ListRole = global.Data.listSelectRole;
        $.ajax({
            url: global.UrlAction.SaveUSerRole,
            type: 'post',
            data: JSON.stringify(global.Data.UserRoleModel),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        reloadListEmployee();
                        $("#" + global.Element.PopupUserRole).modal("hide");
                        toastr.success('Cập Nhật Quyền Cho Nhân Viên Thành Công');
                    }
                }, false, global.Element.PopupUserRole, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }

    function innitListSelect(UserId) {
        $.ajax({
            url: "/Employee/GetRoleIdByUserId?userId=" + UserId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        while (global.Data.listSelectRole.length) {
                            global.Data.listSelectRole.pop();
                        }
                        for (var i = 0; i < result.Records.length; i++) {
                            global.Data.listSelectRole.push(result.Records[i])
                        };
                    }
                }, false, null, true, true, function () {

                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    /* Region Register and init bootrap Popup*/
    function initPopupEmployee() {
        $("#" + global.Element.PopupEmployee).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupEmployee + " button[save]").click(function () {
            saveEmployee();
            var realTimeHub = $.connection.realTimeJTableDemoHub;
            realTimeHub.server.sendUpdateEvent("jtableEmployee", global.Data.ClientId, "Cập nhật loại dịch vụ");
            $.connection.hub.start();
        });
        $("#" + global.Element.PopupEmployee + " button[cancel]").click(function () {
            $("#" + global.Element.PopupEmployee).modal("hide");
        });
    }

    function initPopupUserRole() {
        $("#" + global.Element.PopupUserRole).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupUserRole + " button[save]").click(function () {
            //saveUserRole();
            //var realTimeHub = $.connection.realTimeJTableDemoHub;
            //realTimeHub.server.sendUpdateEvent("jtableEmployee", global.Data.ClientId, "Cập nhật loại dịch vụ");
            //$.connection.hub.start();
        });
        $("#" + global.Element.PopupUserRole + " button[cancel]").click(function () {
            $("#" + global.Element.PopupUserRole).modal("hide");
        });
    }
    /*End bootrap*/
    function ClearPopupFormValues() {
        $('#eventID').val("");
        $('#eventTitle').val("");
        $('#eventDateTime').val("");
        $('#eventDuration').val("");
    }

    function UpdateEvent(EventID, EventStart, EventEnd) {

        var dataRow = {
            'ID': EventID,
            'NewEventStart': EventStart,
            'NewEventEnd': EventEnd
        }

        $.ajax({
            type: 'POST',
            url: "/Timing/UpdateEvent",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(dataRow)
        });
    }

    function initPopupTiming() {
        $("#" + global.Element.PopupTiming).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupTiming + " button[save]").click(function () {
        });
        $("#" + global.Element.PopupTiming + " button[cancel]").click(function () {
            $("#" + global.Element.PopupTiming).modal("hide");
        });
    }
    function initPopupMainTiming() {
        $("#" + global.Element.PopupMainTiming).modal({
            keyboard: false,
            show: false
        });
        $("#" + global.Element.PopupMainTiming + " button[save]").click(function () {
        });
        $("#" + global.Element.PopupMainTiming + " button[cancel]").click(function () {
            $("#" + global.Element.PopupMainTiming).modal("hide");
        });
    }
    /* Region Register and init*/
    this.reloadListEmployee = function () {
        reloadListEmployee();
    };
    this.reloadListProduct = function () {
        reloadListProduct();
    };
    this.initViewModel = function (employee) {
        initViewModel(employee);
    };
    this.bindData = function (employee) {
        bindData(employee);
    };
    var registerEvent = function () {
        $("[cancel]").click(function () {
            bindData(null);
        });

        $('#CreateStockIn').click(function (event) {
            return false;
        }); 
        $("#saveUserRole").click(function () {
            SaveUserRole();
        });
        $("#printSalary").click(function () {
            renderTable(global.Data.listSalary);
            var time = new Date().toLocaleDateString('en-GB');
            $("#no1").html("");
            $("#vcustomer1").html("");
            $("#vphone1").html("");
            $("#vaddress1").html("");
            $("#vproduct").html("");
            $("#vtotal").html("");
            $("#strtotal").html("");
            $("#vdate3").html("");
            $("#vdate1").html("");
            //////////////////////////////////////////////////////
            //$("#no1").append(global.Data.OrderId);
            $("#vcustomer1").append(global.Data.PcustomerName);
            $("#vphone1").append(global.Data.PcustomePhone);
            $("#vaddress1").append(global.Data.PcustomerAddress);

            //var payment = $("#prealpay").val();
            //var tempValue = payment.replace(/[^0-9-.]/g, '');
            //var b = parseFloat(tempValue);
            //var c = docso(b);
            //$("#vtotal").append(payment);
            //$("#strtotal").append(c);
            $("#vdate3").append(time);
            //$("#vdate1").append(time);
            printPanel();
        });

        $('#btnPopupCancel').click(function () {
            ClearPopupFormValues();
            $('#popupEventForm').hide();
        });

        $('#btnPopupSave').click(function () {

            $("#" + global.Element.PopupTiming).modal("hide");

            var dataRow = {
                'Title': $('#eventTitle').val(),
                'NewEventDate': $('#eventDate').val(),
                'NewEventTime': $('#eventTime').val(),
                'NewEventDuration': $('#eventDuration').val(),
                'EmpId': global.Data.EmpId
            }

            ClearPopupFormValues();

            $.ajax({
                type: 'POST',
                url: "/Timing/SaveEvent",
                data: dataRow,
                success: function (response) {
                    if (response == 'True') {

                        $('#calendar1').fullCalendar('refetchEvents');
                        toastr.success('Chấm công thành công');
                    }
                    else {
                        toastr.warning('Chấm công thất bại');
                    }
                }
            });
        });
        $('#fixedbutton').click(function () {
            saveUserProduct();
        });
        $('#fixedbutton1').click(function () {
            $('.nav-tabs a:first').tab('show');
        });
        $("#saveSalary").click(function () {
            SaveSalary();
        });
        $("#save-salary-edit").click(function () {
            SaveSalaryEdit();
        });
        $("#salary-amount").keyup(function () {
            var tempValue = $(this).val().replace(/[^0-9-.]/g, '');
            $("#salary-amount").val(tempValue.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
    };
    this.Init = function () {
        registerEvent();

        initComboBoxPosition();
        initListEmployee();
        reloadListEmployee();

        initListSalary();
        reloadListSalary();
        initPopupEmployee();
        initPopupMainTiming();
        initPopupUserRole();
        InitListUserRole();
        jtableProductUser();
        reloadListProduct();
        // initCalendar();
        bindData(null);
    };
};
/*End Region*/
$(document).ready(function () {
    var employee = new VINASIC.Employee();
    employee.Init();
});