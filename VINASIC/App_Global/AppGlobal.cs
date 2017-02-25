using System;
using System.Configuration;

namespace VINASIC.App_Global
{
    public static partial class AppGlobal
    {
        private static string _vinasicConnectionstring;
        public static string VinasicConnectionstring
        {
            get
            {
                if (string.IsNullOrEmpty(_vinasicConnectionstring))
                {
                    _vinasicConnectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["VINASIC"].ConnectionString;
                }
                return _vinasicConnectionstring;
            }
        }
        public static class EmailConfig
        {
            public const string EMAIL_SUPPORT = "EmailSupport";
            public const string EMAIL_FEEDBACK = "EmailFeedBack";
            public const string EMAIL_HOST = "EmailHost";
            public const string EMAIL_HOST_PORT = "EmailHostPort";
            public const string EMAIL_PASS_HOST = "EmailPassHost";
            public const string EMAIL_USER_HOST = "EmailUserHost";
            public const string ENABLE_SSL = "EnableSsl";
        }

        public static class EmailHostSetting
        {
            private const string EMAIL_SUPPORT = EmailConfig.EMAIL_SUPPORT;
            private const string EMAIL_FEEDBACK = EmailConfig.EMAIL_FEEDBACK;
            private const string EMAIL_HOST = EmailConfig.EMAIL_HOST;
            private const string EMAIL_PASS_HOST = EmailConfig.EMAIL_PASS_HOST;
            private const string EMAIL_USER_HOST = EmailConfig.EMAIL_USER_HOST;
            private const string EMAIL_HOST_PORT = EmailConfig.EMAIL_HOST_PORT;
            private const string ENABLE_SSL = EmailConfig.ENABLE_SSL;

            public static string EmailSupport
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_SUPPORT];
                    return value ?? string.Empty;
                }
            }
            public static string EmailFeedBack
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_FEEDBACK];
                    return value ?? string.Empty;
                }
            }
            public static string EmailHost
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_HOST];
                    return value ?? string.Empty;
                }
            }
            public static string EmailPassHost
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_PASS_HOST];
                    return value ?? string.Empty;
                }
            }
            public static string EmailUserHost
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_USER_HOST];
                    return value ?? string.Empty;
                }
            }
            public static int EmailHostPort
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[EMAIL_HOST_PORT];
                    int _tmp = 0;
                    int.TryParse(value, out _tmp);
                    return _tmp;
                }
            }
            public static bool EnableSsl
            {
                get
                {
                    var value = ConfigurationManager.AppSettings[ENABLE_SSL];
                    bool _tmp = false;
                    bool.TryParse(value, out _tmp);
                    return _tmp;
                }
            }


        }

        public static class LoginConfig
        {

            public static int GetLoginCount()
            {

                int value = 0;
                int.TryParse(ConfigurationManager.AppSettings["LoginCount"], out value);
                return value;

            }

            public static int GetTimeLock()
            {

                int value = 0;
                int.TryParse(ConfigurationManager.AppSettings["TimeLock"], out value);
                return value;

            }

        }
        public class FileExtension
        {
            public const string Image = "*.jpg,*.gif,*.png,*.bmp";
            public const string Icon = "*.ico";
            public const string File = "*.zip,*.rar,*.xls,*.xlsx,*.pdf,*.txt,*.exe,*.doc,*.docx,*.pdf";
        }

        public enum eFileMaxSize
        {
            Image = 2048,
            Icon = 1024,
            File = 10240
        }

        public class InsertText
        {
            public const string Image = "Chọn Hình(Max 2Mb)";
            public const string Image1 = "Chọn Hình";
            public const string Icon = "Chọn Icon (Max 1Mb)";
            public const string File = "Chọn File (Max 10Mb)";
        }

        [Serializable]
        public class UploadFileConfig
        {
            public string FileExtension { get; set; }
            public eFileMaxSize MaxSize { get; set; }
            public bool MultipleType { get; set; }
            public string InsertText { get; set; }
            public string UploadControlId { get; set; }
            public string FileListId { get; set; }
            public string SubmitUploadId { get; set; }
        }
    }
}