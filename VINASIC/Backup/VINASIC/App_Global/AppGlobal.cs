using System;
using System.Configuration;
using System.Web;
namespace SystemAccount.App_Global
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
    }
}