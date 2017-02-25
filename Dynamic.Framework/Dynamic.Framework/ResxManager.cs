using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace Dynamic.Framework
{
    public class ResxManager
    {
        public static readonly ResxType Vi = new ResxType()
        {
            Name = "VN",
            UICulture = "vi-VN",
            IsDefault = true
        };
        public static readonly ResxType En = new ResxType()
        {
            Name = "US",
            UICulture = "en-US",
            IsDefault = false
        };
        private static ResourceManager resourceMan;
        private static Dictionary<string, CultureInfo> _dicCulture;

        public static ResourceManager ResourceManager
        {
            get
            {
                ResxType resx = ResxManager.GetResx(ResxManager.CurrentLanguage);
                if (object.ReferenceEquals((object)ResxManager.resourceMan, (object)null) || ResxManager.resourceMan.BaseName != string.Format("Resources.{0}", (object)resx.Name))
                    ResxManager.resourceMan = new ResourceManager(string.Format("Resources.{0}", (object)resx.Name), Assembly.Load("App_GlobalResources"));
                return ResxManager.resourceMan;
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["SS_LANGUAGE"] != null)
                    return HttpContext.Current.Session["SS_LANGUAGE"].ToString();
                return "vi-VN";
            }
        }

        private static Dictionary<string, CultureInfo> DicCulture
        {
            get
            {
                if (ResxManager._dicCulture == null)
                    ResxManager._dicCulture = new Dictionary<string, CultureInfo>();
                return ResxManager._dicCulture;
            }
        }

        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                return ResxManager.GetCulture(ResxManager.CurrentLanguage);
            }
        }

        private static CultureInfo GetCulture(string pLangName)
        {
            if (!ResxManager.DicCulture.ContainsKey(pLangName))
                ResxManager.DicCulture.Add(pLangName, new CultureInfo(pLangName));
            return ResxManager.DicCulture[pLangName];
        }

        public static void SetLanguage(eLanguage language)
        {
            switch (language)
            {
                case eLanguage.Vi:
                    HttpContext.Current.Session["SS_LANGUAGE"] = (object)ResxManager.Vi.UICulture;
                    break;
                case eLanguage.En:
                    HttpContext.Current.Session["SS_LANGUAGE"] = (object)ResxManager.En.UICulture;
                    break;
                default:
                    HttpContext.Current.Session["SS_LANGUAGE"] = (object)ResxManager.Vi.UICulture;
                    break;
            }
        }

        public static void SetLanguage(string uICulture)
        {
            HttpContext.Current.Session["SS_LANGUAGE"] = (object)uICulture;
        }

        public static string GetString(string pResxName, bool pLowerFirstKey = false)
        {
            string str = "";
            try
            {
                str = ResxManager.ResourceManager.GetString(pResxName, ResxManager.CurrentCultureInfo);
                if (pLowerFirstKey)
                    str = str.Substring(0, 1).ToLower() + str.Substring(1);
            }
            catch
            {
            }
            return str;
        }

        public static IEnumerable<ResxType> GetResx()
        {
            return (IEnumerable<ResxType>)new ResxType[2]
      {
        ResxManager.Vi,
        ResxManager.En
      };
        }

        public static ResxType GetResx(string uiCulture)
        {
            return Enumerable.FirstOrDefault<ResxType>(Enumerable.Where<ResxType>(ResxManager.GetResx(), (Func<ResxType, bool>)(p => p.UICulture == uiCulture))) ?? Enumerable.FirstOrDefault<ResxType>(Enumerable.Where<ResxType>(ResxManager.GetResx(), (Func<ResxType, bool>)(p => p.IsDefault))) ?? ResxManager.Vi;
        }
    }
}
