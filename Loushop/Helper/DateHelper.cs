using System;
using System.Globalization;

namespace Loushop.Helper
{
    public static class DateHelper
    {
        public static string ToPersianDate(DateTime gregorianDate)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(gregorianDate);
            int month = pc.GetMonth(gregorianDate);
            int day = pc.GetDayOfMonth(gregorianDate);

            return $"{year}/{month:D2}/{day:D2}";
        }
    }

}
