using System;

namespace Dynamic.Framework.Mvc.Extension
{
    public class Calendar
    {
        private double timeZone = 7.0;

        public long getJulius(int intNgay, int intThang, int intNam)
        {
            int num1 = (14 - intThang) / 12;
            int num2 = intNam + 4800 - num1;
            int num3 = intThang + 12 * num1 - 3;
            int num4 = intNgay + (153 * num3 + 2) / 5 + 365 * num2 + num2 / 4 - num2 / 100 + num2 / 400 - 32045;
            if (num4 < 2299161)
                num4 = intNgay + (153 * num3 + 2) / 5 + 365 * num2 + num2 / 4 - 32083;
            return (long)num4;
        }

        public string jdTosDate(int jd)
        {
            int num1;
            int num2;
            if (jd > 2299160)
            {
                int num3 = jd + 32044;
                num1 = (4 * num3 + 3) / 146097;
                num2 = num3 - num1 * 146097 / 4;
            }
            else
            {
                num1 = 0;
                num2 = jd + 32082;
            }
            int num4 = (4 * num2 + 3) / 1461;
            int num5 = num2 - 1461 * num4 / 4;
            int num6 = (5 * num5 + 2) / 153;
            return (num5 - (153 * num6 + 2) / 5 + 1).ToString() + "/" + (num6 + 3 - 12 * (num6 / 10)).ToString() + "/" + (num1 * 100 + num4 - 4800 + num6 / 10).ToString();
        }

        public int getNewMoonDay(int k)
        {
            double num1 = (double)k / 1236.85;
            double num2 = num1 * num1;
            double num3 = num2 * num1;
            double num4 = Math.PI / 180.0;
            double num5 = 2415020.75933 + 29.53058868 * (double)k + 0.0001178 * num2 - 1.55E-07 * num3 + 0.00033 * Math.Sin((166.56 + 132.87 * num1 - 0.009173 * num2) * num4);
            double num6 = 359.2242 + 29.10535608 * (double)k - 3.33E-05 * num2 - 3.47E-06 * num3;
            double num7 = 306.0253 + 385.81691806 * (double)k + 0.0107306 * num2 + 1.236E-05 * num3;
            double num8 = 21.2964 + 390.67050646 * (double)k - 0.0016528 * num2 - 2.39E-06 * num3;
            double num9 = (0.1734 - 0.000393 * num1) * Math.Sin(num6 * num4) + 0.0021 * Math.Sin(2.0 * num4 * num6) - 0.4068 * Math.Sin(num7 * num4) + 0.0161 * Math.Sin(num4 * 2.0 * num7) - 0.0004 * Math.Sin(num4 * 3.0 * num7) + 0.0104 * Math.Sin(num4 * 2.0 * num8) - 0.0051 * Math.Sin(num4 * (num6 + num7)) - 0.0074 * Math.Sin(num4 * (num6 - num7)) + 0.0004 * Math.Sin(num4 * (2.0 * num8 + num6)) - 0.0004 * Math.Sin(num4 * (2.0 * num8 - num6)) - 0.0006 * Math.Sin(num4 * (2.0 * num8 + num7)) + 0.001 * Math.Sin(num4 * (2.0 * num8 - num7)) + 0.0005 * Math.Sin(num4 * (2.0 * num7 + num6));
            double num10 = num1 >= -11.0 ? 0.000265 * num1 - 0.000278 + 0.000262 * num2 : 0.001 + 0.000839 * num1 + 0.0002261 * num2 - 8.45E-06 * num3 - 8.1E-08 * num1 * num3;
            return (int)(num5 + num9 - num10 + 0.5 + this.timeZone / 24.0);
        }

        public int getSunLongitude(int jdn)
        {
            double num1 = ((double)jdn - 2451545.5 - this.timeZone / 24.0) / 36525.0;
            double num2 = num1 * num1;
            double num3 = Math.PI / 180.0;
            double num4 = 357.5291 + 35999.0503 * num1 - 0.0001559 * num2 - 4.8E-07 * num1 * num2;
            double num5 = (280.46645 + 36000.76983 * num1 + 0.0003032 * num2 + ((1.9146 - 0.004817 * num1 - 1.4E-05 * num2) * Math.Sin(num3 * num4) + (0.019993 - 0.000101 * num1) * Math.Sin(num3 * 2.0 * num4) + 0.00029 * Math.Sin(num3 * 3.0 * num4))) * num3;
            return (int)((num5 - 2.0 * Math.PI * (double)(int)(num5 / (2.0 * Math.PI))) / Math.PI * 6.0);
        }

        public int getLunarMonthll(int intNam)
        {
            double num1 = (double)(int)((double)(this.getJulius(31, 12, intNam) - 2415021L) / 29.530588853);
            double num2 = (double)this.getNewMoonDay((int)num1);
            if ((double)this.getSunLongitude((int)num2) >= 9.0)
                num2 = (double)this.getNewMoonDay((int)num1 - 1);
            return (int)num2;
        }

        public int getLeapMonthOffset(double a11)
        {
            int num1 = (int)((a11 - 2415021.07699869) / 29.530588853 + 0.5);
            int num2 = 1;
            double num3 = (double)this.getSunLongitude(this.getNewMoonDay(num1 + num2));
            double num4;
            do
            {
                num4 = num3;
                ++num2;
                num3 = (double)this.getSunLongitude(this.getNewMoonDay(num1 + num2));
            }
            while (num3 != num4 && num2 < 14);
            return num2 - 1;
        }

        public string convertSolar2Lunar(int intNgay, int intThang, int intNam)
        {
            double num1 = (double)this.getJulius(intNgay, intThang, intNam);
            int k = (int)((num1 - 2415021.07699869) / 29.530588853);
            double num2 = (double)this.getNewMoonDay(k + 1);
            if (num2 > num1)
                num2 = (double)this.getNewMoonDay(k);
            double a11 = (double)this.getLunarMonthll(intNam);
            double num3 = a11;
            double num4;
            if (a11 >= num2)
            {
                num4 = (double)intNam;
                a11 = (double)this.getLunarMonthll(intNam - 1);
            }
            else
            {
                num4 = (double)(intNam + 1);
                num3 = (double)this.getLunarMonthll(intNam + 1);
            }
            double num5 = num1 - num2 + 1.0;
            int num6 = (int)((num2 - a11) / 29.0);
            double num7 = (double)(num6 + 11);
            if (num3 - a11 > 365.0)
            {
                int leapMonthOffset = this.getLeapMonthOffset(a11);
                if (num6 >= leapMonthOffset)
                {
                    num7 = (double)(num6 + 10);
                    if (num6 != leapMonthOffset)
                        ;
                }
            }
            if (num7 > 12.0)
                num7 -= 12.0;
            if (num7 >= 11.0 && num6 < 4)
                --num4;
            string str1 = num5.ToString();
            string str2 = num7.ToString();
            num4.ToString();
            return str1 + "/" + str2;
        }
    }
}
