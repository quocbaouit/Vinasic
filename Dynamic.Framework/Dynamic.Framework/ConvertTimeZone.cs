using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dynamic.Framework
{
    static class TimeConvert
    {
        public static DateTime GetClientTime(this DateTime obj)
        {
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(obj, cstZone);
                return cstTime;
            }
            catch (TimeZoneNotFoundException)
            {
                return obj;             
            }
            catch (InvalidTimeZoneException)
            {
                return obj;
            }           
        }
    }
}
