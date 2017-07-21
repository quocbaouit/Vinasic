if (typeof VINASIC == 'undefined' || !VINASIC) {
    var VINASIC = {};
}

VINASIC.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = VINASIC;
        for (j = (d[0] == 'VINASIC') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}

VINASIC.namespace('Role');
VINASIC.Role = function () {
    var Global = {
        UrlAction: {
            GetListRole: '/Role/GetRoles',
            CreateRole: '/Role/CreateRole',
            DeleteRole: '/Role/Delete',
            GetListPermission: "/Permission/GetPermissionsForRole",
            SaveRolePermission: '/Role/SaveRolePermission',
            SaveUSerProduct: "/Employee/SaveUserProduct",
            GetProductForUser: "/Employee/GetProductForUser"
        },
        Element: {
            JtableRole: 'jtableRole',
            JtablePermission: "jtablePermission",
            PopupRole: "popup_Role",
            JtableProductUser: "jtableProductUser",
        },
        Data: {
            ModelRole: {},
            RoleId: 0,
            RoleNameTemp:"",
            listSelectPermision: [],
            RolePermissionModel: { RoleId: 0, ListPermission: [] },
            ListSelectProductModel: { UserId: 0, ListSelectProduct: [] },
            ClientId: "",
            ListSelectProduct: []
        }
    }
    this.GetGlobal = function () {
        return Global;
    }
    function reloadListPermission() {
        var keySearch = "abc";
        innitListSelect(Global.Data.RoleId);
        $("#" + Global.Element.JtablePermission).jtable("load", { 'keyword': keySearch, 'roleId': Global.Data.RoleId });
    }
    function reloadListProduct() {
        var keySearch = "abc";
        innitListSelectProduct(Global.Data.UserId);
        $("#" + Global.Element.JtableProductUser).jtable("load", { 'keyword': keySearch, 'UserId': Global.Data.UserId });
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
    this.Init = function () {
        RegisterEvent();
        InitListRole();
        jtableProductUser();
        ReloadListRole();
        initListPermission();
        initPopupRole();
        BindData(null);
    }
    this.reloadListRole = function () {
        ReloadListRole();
    }
    this.reloadListPermission = function () {
        reloadListPermission();
    }
    this.initViewModel = function (role) {
        InitViewModel(role);
    }
    this.bindData = function (role) {
        BindData(role);
    }
    /*function show Popup*/
    function showPopupRole() {
        $("#" + Global.Element.PopupRole).modal("show");
    }
    /* Region Register and init bootrap Popup*/
    function initPopupRole() {
        $("#" + Global.Element.PopupRole).modal({
            keyboard: false,
            show: false
        });
        $("#" + Global.Element.PopupRole + " button[saveRole]").click(function () {
            saveRole();
            //Global.Data.ClientId = document.getElementById("ClientName").innerHTML;
            //var realTimeHub = $.connection.realTimeJTableDemoHub;
            //realTimeHub.server.sendUpdateEvent("jtableRole", Global.Data.ClientId, "Cập nhật loại dịch vụ");
            //$.connection.hub.start();
        });
        $("#" + Global.Element.PopupRole + " button[cancel]").click(function () {
            $("#" + Global.Element.PopupRole).modal("hide");
        });
    }
    /*End bootrap*/
    var RegisterEvent = function () {
        
        $('[btn="save"]').click(function () {
            SaveRolePermission();
        }); 
        $('#fixedbutton').click(function () {
            SaveRolePermission();
        });
        $('#fixedbutton1').click(function () {
            $('.nav-tabs a:first').tab('show');
        });
        $('#fixedbuttonprint').click(function () {
            saveUserProduct();
        });
        $('#fixedbuttonprint1').click(function () {
            $('.nav-tabs a:first').tab('show');
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
                        while (Global.Data.ListSelectProduct.length) {
                            Global.Data.ListSelectProduct.pop();
                        }
                        for (var i = 0; i < result.Records.length; i++) {
                            Global.Data.ListSelectProduct.push(result.Records[i]);
                        };
                    }
                }, false, null, true, true, function () {

                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function initListPermission() {
        $("#" + Global.Element.JtablePermission).jtable({
            title: "",
            paging: false,
            pageSize: 1000,
            pageSizeChangePermission: false,
            sorting: false,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListPermission
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
                FeatureName: {
                    visibility: "fixed",
                    title: "Chức Năng",
                    width: "20",
                    display: function (data) {
                        var text = "";
                        if (data.record.PermissionName === null) {

                            text = $("<br/><hr/><a style=\"color: red\"  class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.FeatureName + "</a>");
                        }
                        else {
                            text = $("");
                        }
                        //var text = $("<br/><hr/><a style=\"color: red\" href=\"#\" class=\"clickable\" title=\"Chỉnh sửa thông tin.\">" + data.record.FeatureName + "</a>");
                        text.click(function () {
                            //bindData(data.record);
                            //showPopupPermission();
                        });
                        return text;
                    }
                },
                Select: {
                    title: 'Chọn',
                    width: '2%',
                    list: true,
                    display: function (data) {
                        var text = "";
                        if (data.record.PermissionName != null) {
                            if (data.record.Selected == true) {
                                text = $("<input id=check" + data.record.Id + " type=\"checkbox\" checked>");
                            }
                            else {
                                text = $("<input id=check" + data.record.Id + " type=\"checkbox\">");
                            }
                        }
                        else {
                            text = $("<hr/>");
                        }
                        text.click(function () {
                            var id = "check" + data.record.Id;
                            var isCheck = document.getElementById(id).checked;
                            if (isCheck == true) {
                                Global.Data.listSelectPermision.push(data.record.Id);
                            }
                            else {
                                removeItemInArray(Global.Data.listSelectPermision, data.record.Id)
                            }

                        });
                        return text;
                    }
                },
                PermissionName: {
                    title: "Tên Quyền",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.PermissionName + "</p>";
                        }
                        return elementDisplay;
                    }
                },
                Description: {
                    title: "Mô Tả",
                    width: "20%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.Description + "</p>";
                        }
                        return elementDisplay;
                    }
                },
                Url: {
                    title: "Đường Dẫn",
                    width: "25%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.PermissionName === null)
                        { elementDisplay = "<hr/>"; }
                        else {
                            elementDisplay = "<p>" + data.record.Url + "</p>";
                        }
                        return elementDisplay;
                    }
                }

            }
        });
    }
    function InitViewModel(role) {
        var roleViewModel = {
            Id: 0,
            RoleName: '',
            Decription: '',
            IsSystem: false
        }
        if (role != null) {
            roleViewModel = {
                Id: ko.observable(role.Id),
                RoleName: ko.observable(role.RoleName),
                Decription: ko.observable(role.Decription),
                IsSystem: ko.observable(role.IsSystem)
            };
        }
        return roleViewModel;
    }
    function BindData(role) {
        Global.Data.ModelRole = InitViewModel(role);
        ko.applyBindings(Global.Data.ModelRole);
    }
   
    function InitListRole() {
        $('#' + Global.Element.JtableRole).jtable({
            title: 'Quản Lý Nhóm Quyền',
            paging: true,
            pageSize: 1000,
            pageSizeChangeRole: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListRole,
                createAction: Global.Element.PopupRole,
                createObjDefault: InitViewModel(null),
            },
            messages: {
                selectShow: 'Ẩn hiện cột',
                addNewRecord: 'Thêm Nhóm Quyền',
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                RoleName: {
                    visibility: 'fixed',
                    title: "Tên Nhóm Quyền",
                    width: "20%",
                    display: function (data) {
                        var text = $('<a style="color:red" href="javascript:void(0) class="SystemClass" title="Đây là quyền Hệ Thống.\nBạn không được thao tác trên Quyền Hệ Thống">' + data.record.RoleName + '</a>');
                        if (!data.record.IsSystem) {
                            text = $('<a href="javascript:void(0) class="clickable" data-toggle="modal" data-target="#myModal" title="Chỉnh sửa thông tin Quyền Tài Khoản">' + data.record.RoleName + '</a>');
                            text.click(function () {
                                BindData(data.record);
                                showPopupRole();
                            });
                        }
                        return text;
                    }
                },
                
                IsSystem: {
                    title: "Quyền Hệ Thống",
                    width: "3%",
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.IsSystem)
                        { elementDisplay = "<input  type='checkbox' checked='checked' disabled/>"; }
                        else {
                            elementDisplay = "<input  type='checkbox' disabled />";
                        }
                        return elementDisplay;
                    }
                },
                Decription: {
                    title: "Mô Tả",
                    width: "20%",
                },
                EditPermission: {
                    title: 'Quản Lý quyền',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = '';
                        if (!data.record.IsSystem) {
                            text = $('<button title="chỉnh sửa" class="jtable-command-button jtable-edit-command-button"><span>Phân Quyền</span></button>');
                            text.click(function () {
                                Global.Data.RoleId = data.record.Id;
                                $('.nav-tabs a[href="#edit-profile"]').tab('show');
                                //$('.nav-tabs a:last').tab('show');
                                document.getElementById("roleTemp").innerHTML = "Nhóm Quyền " + data.record.RoleName;
                                reloadListPermission();
                            });
                        }
                        return text;
                    }
                },
                EditPrintingPermission: {
                    title: 'In Ấn',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = '';
                        text = $('<button title="quyền in ấn" class="jtable-command-button jtable-edit-command-button"><span>Phân Quyền</span></button>');
                        text.click(function () {
                            Global.Data.UserId = data.record.Id;
                            document.getElementById("roleTemp123").innerHTML = "In ấn cho nhóm quyền: " + data.record.RoleName;
                            $('.nav-tabs a:last').tab('show');                          
                            reloadListProduct();                          
                        });

                        return text;
                    }
                },
                Delete: {
                    title: 'Xóa Nhóm Quyền',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = '';
                        if (!data.record.IsSystem) {
                            text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                            text.click(function () {
                                    GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                        Delete(data.record.Id);
                                    }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                            });
                        }                 
                        return text;
                    }
                }
            }
        });
    }
    function ReloadListRole() {
        var keySearch = $('#txtSearch').val();
        $('#' + Global.Element.JtableRole).jtable('load', { 'keyword': keySearch });
    }
    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteRole,
            type: 'POST',
            data: JSON.stringify({ 'id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListRole(); 
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupModule, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xóa.");
                });
            }
        });
    }

    /*function Save */
    function saveRole() {
        $.ajax({
            url: Global.UrlAction.CreateRole,
            type: 'post',
            data: ko.toJSON(Global.Data.ModelRole),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        BindData(null);
                        ReloadListRole();
                        $("#" + Global.Element.PopupRole).modal("hide");
                        toastr.success('Thành Công');
                    }
                }, false, Global.Element.PopupRole, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function saveUserProduct() {
        Global.Data.ListSelectProductModel.UserId = Global.Data.UserId;
        Global.Data.ListSelectProductModel.ListSelectProduct = Global.Data.ListSelectProduct;
        $.ajax({
            url: Global.UrlAction.SaveUSerProduct,
            type: 'post',
            data: JSON.stringify(Global.Data.ListSelectProductModel),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        ReloadListRole();
                        //$("#" + Global.Element.PopupUserRole).modal("hide");
                        toastr.success('Cập Nhật Thành Công');
                        $('.nav-tabs a:first').tab('show');
                    }
                }, false, Global.Element.PopupUserRole, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    /*End Save */
    function CheckValidate() {
        if ($('[txt="RoleName"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập tên Role.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('[select="categoryId"]').val() == "0") {
            GlobalCommon.ShowMessageDialog("Vui lòng chọn Role Category.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }
    function SaveRolePermission() {
        Global.Data.RolePermissionModel.RoleId = Global.Data.RoleId;
        Global.Data.RolePermissionModel.ListPermission = Global.Data.listSelectPermision;
        $.ajax({
            url: Global.UrlAction.SaveRolePermission,
            type: 'post',
            data: JSON.stringify(Global.Data.RolePermissionModel),
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                      
                        toastr.success('Thay Đổi Quyền Thành Công');
                        $('.nav-tabs a:first').tab('show');
                        reloadListPermission();
                    }
                }, false, Global.Element.PopupRole, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
    function jtableProductUser() {
        $("#" + Global.Element.JtableProductUser).jtable({
            title: "",
            paging: false,
            pageSize: 1000,
            pageSizeChangeProduct: false,
            sorting: false,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetProductForUser
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
                                Global.Data.ListSelectProduct.push(data.record.Id);
                            }
                            else {
                                removeItemInArray(Global.Data.ListSelectProduct, data.record.Id);
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
    function innitListSelect(roleId) {
        $.ajax({
            url: "/Permission/GetPermissionIdsByRole?roleId=" + roleId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        while (Global.Data.listSelectPermision.length) {
                            Global.Data.listSelectPermision.pop();
                        }
                        for (var i = 0; i < result.Records.length; i++) {
                            Global.Data.listSelectPermision.push(result.Records[i])
                        };
                    }
                }, false, null, true, true, function () {

                    toastr.error("Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
    }
}
$(document).ready(function () {
    var Role = new VINASIC.Role();
    Role.Init();
})

