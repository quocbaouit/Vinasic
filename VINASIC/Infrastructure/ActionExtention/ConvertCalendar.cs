using System;
using GPRO.Ultilities;

namespace VINASIC.Infrastructure.ActionExtention
{
    public class ConvertCalendar
    {
        public static DateTime ConvertLunar2Solar(DateTime date)
        {
            return VietnameseCalendar.Lunar2Solar(date.Day,date.Month,date.Year);
        }

        public static DateTime ConvertSolar2Lunar(DateTime date)
        {
            int[] arr = VietnameseCalendar.Solar2Lunar(date);
            DateTime dateResult = new DateTime(arr[2], arr[1], arr[0]);
            return dateResult;
        }
    }
}