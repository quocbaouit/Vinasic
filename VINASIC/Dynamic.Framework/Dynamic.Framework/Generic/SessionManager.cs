using System.Web;

namespace Dynamic.Framework.Generic
{
    public class SessionManager
    {
        public static int LoginCount
        {
            get
            {
                if (HttpContext.Current.Session == null)
                    return 0;
                return HttpContext.Current.Session["SS_LOGIN_COUNT"] == null ? 0 : int.Parse(HttpContext.Current.Session["SS_LOGIN_COUNT"].ToString());
            }
            set
            {
                if (value <= 0)
                    HttpContext.Current.Session["SS_LOGIN_COUNT"] = (object)null;
                else
                    HttpContext.Current.Session["SS_LOGIN_COUNT"] = (object)value;
            }
        }

        public static bool RequireChangePassword
        {
            get
            {
                if (HttpContext.Current.Session == null)
                    return false;
                return HttpContext.Current.Session["SS_REQUIRE_CHANGE_PASSWORD"] != null && (bool)HttpContext.Current.Session["SS_REQUIRE_CHANGE_PASSWORD"];
            }
            set
            {
                if (!value)
                    HttpContext.Current.Session["SS_REQUIRE_CHANGE_PASSWORD"] = (object)null;
                else
                    HttpContext.Current.Session["SS_REQUIRE_CHANGE_PASSWORD"] = (object)(int)(value ? 1 : 0);
            }
        }
    }
}