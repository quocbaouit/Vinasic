if (typeof GPRO == 'undefined' || !GPRO) {
    var GPRO = {};
}

GPRO.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = GPRO;
        for (j = (d[0] == 'GPRO') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}

GPRO.namespace('User');
GPRO.User = function () {
    var Global = {
        UrlAction: {
            GetListUser: '/User/GetUsers',
            SaveUserUrl: '/User/SaveUser',
            DeleteUser: '/User/DeleteUser',
            UnLockUser: '/User/UnLockTimeUser',
            ChangePassword: '/User/ChangePassword',
            ChangeUserState: '/User/ChangeUserState',
            GetListRole: '/User/GetRoles',
            UploadFile: '/UploadFile/Upload'
        },
        Element: {
            JtableUser: 'jtableUser',
            popupCreateUser: 'userModal',
            popupSearch: 'searchUserModal'
        },
        Data: {
            ModelUser: {},
            UserContextId: 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        InitListUser();
        ReloadListUser();
        BindData(null);
    }

    this.BindindUsercontextId = function(userContextId){
        Global.Data.UserContextId = userContextId;
    }
    

    this.reloadListUser = function () {
        ReloadListUser();
    }

    this.initViewModel = function (user) {
        InitViewModel(user);
    }

    this.bindData = function (user) {       
        BindData(user);
    }

    var RegisterEvent = function () {
        $('[btn="saveUser"]').click(function () {
            var uploadobj = document.getElementById('uploader');
            if (CheckValidate()) {              

 
                      SaveUser();
                    clearSelectBox();
                
            }           
        });

        $('[btn="addUser"]').click(function () {
            BindData(null);
           
        });

        $('[btn="updatePassword"]').click(function () {
            if (checkValidateUpdatePass()) {
                ChangePassword();
            }
        });

        $('[cancel]').click(function () {
            ResetData()
        });

        $('[close]').click(function () {
            ResetData();
        });

        // Register event after upload file done the value of [filelist] will be change => call function save your Data 
        $('[filelist]').change(function () {         
            SaveUser();
            clearSelectBox();
        });
    }

    function InitViewModel(user) {
        var userViewModel = {
            Id: 0,
            UserName: '',
            PassWord: '',
            LastName: '',
            FisrtName:'',
            Email: '', 
            ImagePath: '',
            IsForgotPassword: false,
            NoteForgotPassword: '',
            IsLock: false,
            UserRoles :null
        }
        if (user != null) {
            userViewModel = {
                        Id: ko.observable(user.Id),
                        UserName: ko.observable(user.UserName),
                        PassWord: ko.observable(user.PassWord),
                        LastName: ko.observable(user.LastName),
                        FisrtName: ko.observable(user.FisrtName),
                        Email: ko.observable(user.Email),
                        ImagePath: ko.observable(user.ImagePath),
                        IsForgotPassword:ko.observable(user.IsForgotPassword),
                        NoteForgotPassword: ko.observable(user.NoteForgotPassword),
                        IsLock: ko.observable(user.IsLock),
                        UserRoles:ko.observable(user.UserRoles)
            };
        }
        return userViewModel;
    }

    function BindData(user) {
        Global.Data.ModelUser = InitViewModel(user);
        ko.applyBindings(Global.Data.ModelUser);        
    }  

    function InitListUser() {
        $('#' + Global.Element.JtableUser).jtable({
            title: 'Danh Sách Tài Khoản',
            paging: true,
            pageSize: 10,
            pageSizeChangeUser: true,
            sorting: true,
            selectShow: true,
            actions: {
                listAction: Global.UrlAction.GetListUser,
                createAction: Global.Element.popupCreateUser,
                createObjDefault: InitViewModel(null),
                searchAction: Global.Element.popupSearch
            },
            messages: {
                addNewRecord: 'Thêm Tài Khoản',
                searchRecord: 'Tìm kiếm',
                selectShow: 'Ẩn hiện cột',
            },
            fields: {
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                UserName: {
                    visibility: 'fixed',
                    title: "Tên Đăng Nhập",
                    width: "20%",
                    display: function (data) {
                        var text = $('<a class="clickable" data-toggle="modal" data-target="#userModal" title="Chỉnh sửa thông tin Tài Khoản">' + data.record.UserName + '</a>');
                        text.click(function () {          
                            
                            // set value for avatar
                            $('#avatar').attr('src', data.record.ImagePath);
                           //check if isRequiredPassChange == true => show password row 
                            if (data.record.IsRequireChangePW) {
                                $('#passwordRow').show();
                                data.record.PassWord = '';
                                $('#required').val('1')
                            }
                            $('#rowUsername').hide();

                            // bidind data to select box
                            BindinDataToSelectBox(data.record.UserRoles);               

                            //do data vao modal
                            BindData(data.record);                            
                        });
                        return text;
                    }                    
                },
                UserRoles: {
                    title: 'Nhóm quyền TK',
                    display: function (data) {
                        var text = '';
                        var listUserRole = data.record.UserRoles;
                        
                        if (typeof (listUserRole) != 'undefined' && listUserRole != null)
                        {
                            $.each(listUserRole, function (index, item) {
                                text += '<span >' + item.RoleName + '</span> </br>';
                                
                            });
                        }
                        return text;
                    }
                },
                FisrtName: {
                    title: "Họ",
                    width: "20%",
                },
                LastName: {
                    title: "Tên",
                    width: "20%",
                },
                Email: {
                    title: "Email",
                    width: "20%",
                },
                ImagePath: {
                    visibility: 'hidden', // ẩn collumn
                    title: "Hình",
                    width: "3%",
                    display: function (data) {
                        var text = $('<img src="' + data.record.ImagePath + '" width="40"/>');
                        if (data.record.ImagePath != null) {
                            return text;
                        }                   
                    }
                },
                IsRequireChangePW: {
                    visibility: 'hidden',
                    title: 'YC đổi MK',
                    width: '1%',
                    display: function (data) {
                        var elementDisplay = "";
                        if (data.record.IsShow)
                        { elementDisplay = "<input  type='checkbox' checked='checked' disabled/>"; }
                        else {
                            elementDisplay = "<input  type='checkbox' disabled />";
                        }
                        return elementDisplay;
                    }
                },
                Delete: {
                    title: '',
                    width: "3%",
                    sorting: false,
                    display: function (data) {
                        var text = $('<button title="Xóa" class="jtable-command-button jtable-delete-command-button"><span>Xóa</span></button>');
                        text.click(function () {
                            GlobalCommon.ShowConfirmDialog('Bạn có chắc chắn muốn xóa?', function () {
                                Delete(data.record.Id);
                            }, function () { }, 'Đồng ý', 'Hủy bỏ', 'Thông báo');
                        });
                        return text;
                    }
                }
            }
        });
    }

    function ReloadListUser() {
        var keySearch = $('#txtSearch').val();
        $('#' + Global.Element.JtableUser).jtable('load', { 'keyword': keySearch });
    }

    function SaveUser() {
        Global.Data.ModelUser.UserCategoryId = $('#UserCategory').val();       
        if ($('#userRoles').val() != '' && $('#userRoles').val() != null) {
            Global.Data.ModelUser.NoteForgotPassword = $('#userRoles').val().toString();
            Global.Data.ModelUser.userRoles = $('#userRoles').val().toString();
        }
        $.ajax({
            url: Global.UrlAction.SaveUserUrl,
            type: 'post',
            data: ko.toJSON(  Global.Data.ModelUser),
            contentType: 'application/json',
            beforeSend: function () { $('#loading').show(); },
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result == "OK") {
                        ReloadListUser();
                        $('#userModal').modal('hide');
                        BindData(null);
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupArea, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(result);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                });
            }
        });
        
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.DeleteUser,
            type: 'POST',
            data: JSON.stringify({ 'id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListUser(); 
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupArea, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xóa.");
                });
            }
        });
    }

    function UnLockTime(Id) {
        $.ajax({
            url: Global.UrlAction.UnLockUser,
            type: 'POST',
            data: JSON.stringify({ 'id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListUser(); 
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupArea, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xóa.");
                });
            }
        });
    }

    function ChangePassword() {
        $.ajax({
            url: Global.UrlAction.ChangePassword,
            type: 'POST',
            data: JSON.stringify({ 'id': $('[txt="userId"]').val(), 'Password': $('[txt="txtNewPass"]').val() }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListUser();
                        $('#fogotPassModal').modal('hide');
                        BindData(null);
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupArea, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xóa.");
                });
            }
        });
    }

    function ChangeUserState(Id) {
        $.ajax({
            url: Global.UrlAction.ChangeUserState,
            type: 'POST',
            data: JSON.stringify({ 'id': Id }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('#loading').show(); },
            success: function (data) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(data, function () {
                    if (data.Result == "OK") {
                        ReloadListUser(); 
                    }
                    else
                        GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình sử lý.");
                }, false, Global.Element.PopupArea, true, true, function () {
                    var msg = GlobalCommon.GetErrorMessage(data);
                    GlobalCommon.ShowMessageDialog(msg, function () { }, "Đã có lỗi xảy ra trong quá trình xóa.");
                });
            }
        });
    }
    //
    function CheckValidate() {
      
        if ($('[txt="userName"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập tên Tài Khoản.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if (($('[txt="txtpass"]').val() == "" && $('[txt="userId"]').val() == "0") || ($('[txt="txtpass"]').val() == "" && $('[txt="userId"]').val() != "0" && $('#required').val() == "1")) {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Mật Khẩu.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if (($('[txt="txtpass"]').val() != $('[txt="txtcpass"]').val() && $('[txt="userId"]').val() == "0") || ($('[txt="txtpass"]').val() != $('[txt="txtcpass"]').val() && $('[txt="userId"]').val() != "0" && $('#required').val() == "1")) {
            GlobalCommon.ShowMessageDialog("Mật Khẩu không khớp.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('[txt="txtho"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Họ.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('[txt="txtTen"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Tên.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        //else if ($('[select="categoryId"]').val() == "") {
        //    GlobalCommon.ShowMessageDialog("Vui lòng chọn Menu Category.", function () { }, "Lỗi Nhập liệu");
        //    return false;
        //}
        return true;
    }

    function checkValidateUpdatePass() {
        if ($('[txt="txtNewPass"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập Mật Khẩu.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('[txt="txtConfirmNewPass"]').val() == "") {
            GlobalCommon.ShowMessageDialog("Vui lòng nhập xác nhận Mật Khẩu.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        else if ($('[txt="txtConfirmNewPass"]').val() != $('[txt="txtNewPass"]').val()) {
            GlobalCommon.ShowMessageDialog("Mật Khẩu không giống nhau.", function () { }, "Lỗi Nhập liệu");
            return false;
        }
        return true;
    }

    function BindinDataToSelectBox(listUserRole) {
        var vt = 0;
        $("#userRoles > option").each(function () {
            var optionValue = this.value;
            var optionText = this.text;
            var a = $(this);
            if (typeof (listUserRole) != 'undefined' && listUserRole != null) {
                $.each(listUserRole, function (index, item) {
                    if (item.Id == optionValue) {
                        a.attr("selected", "selected");
                        $('[class="chosen-choices"]').append('<li class="search-choice"><span>' + optionText + '</span><a class="search-choice-close" data-option-array-index="' + vt + '"></a></li>');
                    }
                });
            }
            vt++;
        });
        $('#userRoles').trigger('liszt:updated');  // refresh select box 
        $('#userRoles').trigger("chosen:updated");  // refresh harvest chosen
    }

    function clearSelectBox() {
        $('#userRoles').find('option:selected').removeAttr('selected');
        $('#userRoles').trigger('liszt:updated');
        $('#userRoles').trigger("chosen:updated");
        $('[txt="txtcpass"]').val('');
        $('#avatar').attr('src', '');
    }
    
    function ResetData() {
        BindData(null);
        clearSelectBox();  
        $('#avatar').attr('src', '/Content/MasterPage/Images/no-image.png');
        //search
        //$('#keyword').val('');
        //$('#S_Position').val(1);
        //$('#searchBy').val('')

        var uploadobj = document.getElementById('uploader');
        uploadobj.cancelall();
    }
    
}

$(document).ready(function () {
    var User = new GPRO.User();
    User.Init();

    // show column username when button add click
    $('.jtable-toolbar-item-add-record').click(function () {
        $('#rowUsername').show();
        $('#passwordRow').show();
    });
});
   
