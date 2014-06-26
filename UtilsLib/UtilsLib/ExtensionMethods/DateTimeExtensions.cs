using System;
using System.Globalization;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks if a DateTime is valid enforcing Sql Date range standards
        /// </summary>
        /// <param name="date">The date to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidWithSqlDateStandards(this DateTime date)
        {
            bool valid = false;

            DateTime sqlMinDate = DateTime.Parse("1/1/1753");
            DateTime sqlMaxDate = DateTime.Parse("12/31/9999");
            if (date >= sqlMinDate && date <= sqlMaxDate)
            {
                valid = true;
            }

            return valid;
        }

        /// <summary>
        /// Checks if a DateTime is equal to the default date time value
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDefault(this DateTime date)
        {
            return date.Equals(default(DateTime)) || date.Equals(DateTime.Parse("1/1/1900"));
        }

        /// <summary>
        /// Returns the week of year for the current DateTime object
        /// </summary>
        /// <param name="date" />
        /// <returns />
        public static int WeekOfYear(this DateTime date)
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.CurrentInfo;
            if (info != null)
            {
                Calendar cal = info.Calendar;
                return cal.GetWeekOfYear(date, info.CalendarWeekRule, info.FirstDayOfWeek);
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string MonthName(this DateTime date)
        {
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            string strMonthName = mfi.GetMonthName(date.Month);

            return strMonthName;
        }

        /// <summary>
        /// Gets the DateTime instance represented by the beginning of the month of the current DateTime
        /// </summary>
        /// <param name="date" />
        /// <returns>A DateTime</returns>
        public static DateTime BeginningOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the DateTime instance represented by the end of the month of the current DateTime
        /// </summary>
        /// <param name="date" />
        /// <returns>A DateTime</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.GetLastDayOfMonth());
        }

        /// <summary>
        /// Returns the first day of the week that the specified date is in. 
        /// </summary>
        public static DateTime BeginningOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }
            return firstDayInWeek;
        }

        /// <summary>
        /// Returns the first day of the week that the specified date is in. 
        /// </summary>
        public static DateTime BeginningOfWeek(this DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return BeginningOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the last day of the week that the specified date is in. 
        /// </summary>
        public static DateTime EndOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date.AddDays(7);
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }
            return firstDayInWeek.AddDays(-1);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date is in. 
        /// </summary>
        public static DateTime EndOfWeek(this DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return EndOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns an integer value representing the last day of the month of the current DateTime
        /// </summary>
        /// <param name="date" />
        /// <returns />
        public static int GetLastDayOfMonth(this DateTime date)
        {
            int currentMonth = date.Month;
            int day = 0;
            while (date.Month == currentMonth)
            {
                day = date.Day;
                date = date.AddDays(1);
            }
            return day;
        }

        /// <summary>
        /// Gets the First day of the week (Sunday)
        /// </summary>
        /// <param name="date" />
        /// <returns>get sunday of the week</returns>
        public static DateTime GetSundayOfWeek(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        /// <summary>
        /// Gets the Last day of the week (Saturday)
        /// </summary>
        /// <param name="date" />
        /// <returns>gets saturday of the week</returns>
        public static DateTime GetSaturdayOfWeek(this DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Saturday)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        /// <summary>
        /// Gets to corresponding day of the week
        /// </summary>
        /// <param name="date" />
        /// <param name="dayOfWeek" />
        /// <returns>Returns the specified day of the week</returns>
        public static DateTime GetDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            int counter = 1;
            if ((int)date.DayOfWeek < (int)dayOfWeek)
            {
                counter = 1;
            }
            else if ((int)date.DayOfWeek > (int)dayOfWeek)
            {
                counter = -1;
            }

            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(counter);
            }

            return date;
        }

        /// <summary>
        /// Returns the day before the current date
        /// </summary>
        /// <param name="date" />
        /// <param name="useSuppliedDate" />
        /// <returns>A DateTime</returns>
        public static DateTime Yesterday(this DateTime date, bool useSuppliedDate = false)
        {
            return (useSuppliedDate ? date.Date : DateTime.Today).AddDays(-1);
        }

        /// <summary>
        /// Returns the day after the current date
        /// </summary>
        /// <param name="date" />
        /// <param name="useSuppliedDate" />
        /// <returns>A DateTime</returns>
        public static DateTime Tomorrow(this DateTime date, bool useSuppliedDate = false)
        {
            return (useSuppliedDate ? date.Date : DateTime.Today).AddDays(1);
        }

        /// <summary>
        /// Checks if a DateTime is after another date
        /// </summary>
        /// <param name="dateToValidate" />
        /// <param name="startDate" />
        /// <returns>True if date is after specified date, false otherwise</returns>
        public static bool After(this DateTime dateToValidate, DateTime startDate)
        {
            return dateToValidate > startDate;
        }

        /// <summary>
        /// Checks if a DateTime is on or after another date
        /// </summary>
        /// <param name="dateToValidate" />
        /// <param name="startDate" />
        /// <returns>True if date is after specified date, false otherwise</returns>
        public static bool OnAndAfter(this DateTime dateToValidate, DateTime startDate)
        {
            return dateToValidate >= startDate;
        }

        /// <summary>
        /// Checks if a DateTime is before another date
        /// </summary>
        /// <param name="dateToValidate" />
        /// <param name="startDate" />
        /// <returns>True if date is after specified date, false otherwise</returns>
        public static bool Before(this DateTime dateToValidate, DateTime startDate)
        {
            return dateToValidate < startDate;
        }

        /// <summary>
        /// Checks if a DateTime is within 2 dates
        /// </summary>
        /// <param name="dateToValidate" />
        /// <param name="startDate" />
        /// <param name="endDate" />
        /// <returns>True if date is within dates, false otherwise</returns>
        public static bool Between(this DateTime dateToValidate, DateTime startDate, DateTime? endDate)
        {
            return DoDatesOverlap(dateToValidate, dateToValidate, startDate, endDate);
        }

        /// <summary>
        /// Check if the 4 dates overlap. This is used for the contract computation EffectiveDate
        /// </summary>
        /// <param name="startFirstDate">Start of First Date</param>
        /// <param name="endFirstDate">End of First Date</param>
        /// <param name="startSecondDate">Start of Second Date</param>
        /// <param name="endSecondDate">End of Second Date</param>
        /// <returns>true if the dates overlap</returns>
        public static bool DoDatesOverlap(DateTime startFirstDate, DateTime? endFirstDate, DateTime startSecondDate, DateTime? endSecondDate)
        {
            bool datesOverlap = false;

            DateTime endDate1 = !endFirstDate.HasValue ? DateTime.MaxValue : endFirstDate.Value;
            DateTime endDate2 = !endSecondDate.HasValue ? DateTime.MaxValue : endSecondDate.Value;

            if (startFirstDate <= startSecondDate && startSecondDate <= endDate1)
            {
                //start date of 2nd span within range of first span
                datesOverlap = true;
            }

            if (startFirstDate <= endDate2 && endDate2 <= endDate1)
            {
                //end date of 2nd span within range of first span
                datesOverlap = true;
            }

            if (startSecondDate <= startFirstDate && endDate1 <= endDate2)
            {
                //Second date starts before and esnds after first date has ended
                datesOverlap = true;
            }

            return datesOverlap;
        }

        /// <summary>
        /// Returns true, if second date supplied is at least specific number of days after the first date
        /// </summary>
        /// <param name="date" />
        /// <param name="dateToCompare" />
        /// <param name="numberOfDays" />
        /// <returns />
        public static bool DaysAfter(this DateTime date, DateTime dateToCompare, int numberOfDays)
        {
            return date.AddDays(numberOfDays.NegativeValue()).After(dateToCompare);
        }

        /// <summary>
        /// Returns true, if second date supplied is at least specific number of days after the first date
        /// </summary>
        /// <param name="date" />
        /// <param name="dateToCompare" />
        /// <param name="numberOfDays" />
        /// <returns />
        public static bool DaysOnAndAfter(this DateTime date, DateTime dateToCompare, int numberOfDays)
        {
            return date.AddDays(numberOfDays.NegativeValue()).OnAndAfter(dateToCompare);
        }

        /// <summary>
        /// Returns true, if second date supplied is at most specific number of days before the first date
        /// </summary>
        /// <param name="date" />
        /// <param name="dateToCompare" />
        /// <param name="numberOfDays" />
        /// <returns />
        public static bool DaysBefore(this DateTime date, DateTime dateToCompare, int numberOfDays)
        {
            return date.AddDays(numberOfDays.AbsoluteValue()).Before(dateToCompare);
        }

        /// <summary>
        /// Returns true, if date and dateToCompare are within numberOfDays
        /// </summary>
        /// <param name="date" />
        /// <param name="dateToCompare" />
        /// <param name="numberOfDays" />
        /// <returns>True, if date and dateToCompare are within numberOfDays</returns>
        public static bool Within(this DateTime date, DateTime dateToCompare, int numberOfDays)
        {
            return (date - dateToCompare).Days.AbsoluteValue() <= numberOfDays;
        }

        /// <summary>
        /// Formats Date for display
        /// </summary>
        /// <param name="date" />
        /// <returns>Formatted String yyyy-MM-dd hh:mm:ss tt</returns>
        public static string FormatLocalDateTime(this DateTime date)
        {
            DateTime convertedTime = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return convertedTime.ToLocalTime().ToString("MMM dd, yyyy - HH:mm:ss tt");
        }

        /// <summary>
        /// Formats the Last Login for Display
        /// </summary>
        /// <param name="createDate">Date user was created</param>
        /// <param name="lastLoginDate">Date user last logged in</param>
        /// <returns>Formatted String For User's Last Login</returns>
        public static string FormatLocalLastLoginDateTime(DateTime createDate, DateTime lastLoginDate)
        {
            return lastLoginDate == createDate ? "Never" : FormatLocalDateTime(lastLoginDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }


    }
}
