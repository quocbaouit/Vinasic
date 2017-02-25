using System;
using System.Web.Mvc;
using Dynamic.Framework.Security;

namespace Dynamic.Framework.Mvc.Extension
{
    public static class Extension
    {
        public static string UserId(this HtmlHelper helper)
        {
            return Authentication.UserId.ToString();
        }

        public static string FullName(string firstName, string lastName)
        {
            if (ResxManager.CurrentLanguage == ResxManager.Vi.UICulture)
                return string.Format("{0} {1}", (object)lastName, (object)firstName);
            return string.Format("{0} {1}", (object)firstName, (object)lastName);
        }

        public static string FullName(this HtmlHelper helper, string firstName, string lastName)
        {
            return FullName(firstName, lastName);
        }

        public static string FullName(this HtmlHelper helper)
        {
            throw new NotImplementedException();
        }

        public static string Email(this HtmlHelper helper)
        {
            return Authentication.User.Email;
        }

        public static string DateTimeViFormatFull(this HtmlHelper helper, DateTime? dateTime = null)
        {
            if (!dateTime.HasValue)
                dateTime = new DateTime?(DateTime.Now);
            return string.Format("{0:dd/MM/yyyy HH:mm:ss}", (object)dateTime);
        }

        public static MvcHtmlString LogoImage(this HtmlHelper helper, string configKeyPath)
        {
            string format = "<img src='/resource/ImageHandler.ashx?w=61&h=61&imageUrl={0}&configKey={1}&df={2}'/>";
            string str1 = "";
            if (Authentication.IsAuthenticated)
            {
                string logoCompany = Authentication.User.LogoCompany;
                string str2 = "/Files/Images/defaultImg.png";
                str1 = string.Format(format, (object)logoCompany, (object)configKeyPath, (object)str2);
            }
            return MvcHtmlString.Create(str1);
        }

        public static MvcHtmlString AvatarImage(this HtmlHelper helper, string configKeyPath, object attributes)
        {
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString AvatarImage(this HtmlHelper helper, string configKeyPath)
        {
            string format = "<img src='/resource/ImageHandler.ashx?w=45&h=45&imageUrl={0}&configKey={1}&df=Image_Upload/avatar.png'/>";
            string str = "";
            if (Authentication.IsAuthenticated)
            {
                string imagePath = Authentication.User.ImagePath;
                if (!string.IsNullOrEmpty(imagePath))
                    ;
                str = string.Format(format, (object)imagePath, (object)configKeyPath);
            }
            return MvcHtmlString.Create(str);
        }

        public static string DayOfWeekText(this HtmlHelper helper, DateTime datetime)
        {
            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "CN";
                case DayOfWeek.Monday:
                    return "Hai";
                case DayOfWeek.Tuesday:
                    return "Ba";
                case DayOfWeek.Wednesday:
                    return "Tư";
                case DayOfWeek.Thursday:
                    return "Năm";
                case DayOfWeek.Friday:
                    return "Sáu";
                case DayOfWeek.Saturday:
                    return "Bảy";
                default:
                    return "";
            }
        }
    }
}
