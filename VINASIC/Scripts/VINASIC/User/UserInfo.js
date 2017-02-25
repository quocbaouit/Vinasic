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
VINASIC.namespace("UserProfile");
VINASIC.UserProfile = function () {
    var global = {
        UrlAction: {
            GetListUserProfile: "/Employee/GetJobForUserProfile",
            SaveUserGeneral: "/UserProfile/SaveUserGeneral",
            SaveUserUserName: "/UserProfile/SaveUserName",
            SaveUserPassWord: "/UserProfile/SaveUserPassword",
            DeleteUserProfile: "/Employee/DeleteUserProfile"
        },
        Element: {
            JtableUserProfile: "jtableUserProfile",
            PopupUserProfile: "popup_UserProfile"
        },
        Data: {
            ModelUserProfile: {},
            ModelConfig: {},
            ClientId: ""
        }
    };
    this.GetGlobal = function () {
        return global;
    };
    function reloadListUserProfile() {
        var keySearch = $("#txtSearch").val();
        $("#" + global.Element.JtableUserProfile).jtable("load", { 'keyword': keySearch });
    }
    /*function show Popup*/
    function showPopupUserProfile() {
        $("#" + global.Element.PopupUserProfile).modal("show");
    }
    /*End*/
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
    function saveGenaral() {
        var userId = $("#userId").val();
        var firstName = $("#f-name").val();
        var lastName = $("#l-name").val();
        var email = $("#email").val();
        var mobile = $("#mobile").val();
        $.ajax({
            url: global.UrlAction.SaveUserGeneral + "?firstName=" + firstName + "&lastName=" + lastName + "&email=" + email + "&mobile=" + mobile + "&userId=" + userId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Cập nhật Thành Công");
                        window.location.href = "/Dashboard/Index";
                    }
                    if (result.Result === "ERROR") {
                        toastr.warning(result.Errors.Message);
                    }
                }, false, global.Element.PopupUserProfile, true, true, function () {
                    toastr.warning(result.ErrorMessages[0].Message);
                });
            }
        });
    }
    function saveUserName() {
        var userId = $("#userId").val();
        var oldUser = $("#f-olduser").val();
        var newUserName = $("#l-newname").val();
        var comfirmUserName = $("#l-comfirmname").val();
        var password = $("#l-password").val();
        $.ajax({
            url: global.UrlAction.SaveUserUserName + "?oldUser=" + oldUser + "&newUserName=" + newUserName + "&comfirmUserName=" + comfirmUserName + "&password=" + password + "&userId=" + userId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Đổi Tên Đăng Nhập Thành Công");
                        window.location.href = "/Dashboard/Index";
                    }
                    if (result.Result === "ERROR") {
                        toastr.warning(result.ErrorMessages.Message);
                    }
                }, false, global.Element.PopupUserProfile, true, true, function () {
                    toastr.warning(result.ErrorMessages[0].Message);
                });
            }
        });
    }
    function savePassWord() {
        var userId = $("#userId").val();
        var oldPassword = $("#f-oldpassword").val();
        var newPassword = $("#l-newpassword").val();
        var comfirmPassword = $("#c-comfirmpassword").val();
        $.ajax({
            url: global.UrlAction.SaveUserPassWord + "?oldPassword=" + oldPassword + "&newPassword=" + newPassword + "&comfirmPassword=" + comfirmPassword + "&userId=" + userId,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                $('#loading').hide();
                GlobalCommon.CallbackProcess(result, function () {
                    if (result.Result === "OK") {
                        toastr.success("Đổi Password Thành Công");
                        window.location.href = "/Dashboard/Index";
                    }
                    if (result.Result === "ERROR") {
                        toastr.warning(result.Errors.Message);
                    }
                }, false, global.Element.PopupUserProfile, true, true, function () {
                    toastr.warning(result.ErrorMessages[0].Message);
                });
            }
        });
    }
    /*End Save */
    var registerEvent = function () {
        $("[savegernaral]").click(function () {
            saveGenaral();
        });
        $("[saveusername]").click(function () {
            saveUserName();
        });
        $("[savepassword]").click(function () {
            savePassWord();
        });
    };
    this.Init = function () {
        global.Data.ClientId = document.getElementById("ClientName").innerHTML;
        registerEvent();
        reloadListUserProfile();
    };
};
/*End Region*/
$(document).ready(function () {
    var userProfile = new VINASIC.UserProfile();
    userProfile.Init();
});
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode === 59 || charCode === 46)
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    { GlobalCommon.ShowMessageDialog("Vui lòng nhập số.", function () { }, "Lỗi Nhập liệu"); }
    return true;
}