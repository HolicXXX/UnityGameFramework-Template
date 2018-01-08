using System;
using System.Globalization;
using UnityEngine;

namespace GameMain {
	public static class TimeUtility {
		
		public static DateTime UTCStart = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime UTCStart_LocalTimeZone = TimeZone.CurrentTimeZone.ToLocalTime (UTCStart);

		public static GregorianCalendar G_Calender = new GregorianCalendar ();

		public static ChineseLunisolarCalendar LunisolarCalender = new ChineseLunisolarCalendar ();

		public static long GetTimeStamp (DateTime time)
		{
			time = TimeZone.CurrentTimeZone.ToLocalTime (time);
			var delta = time - UTCStart_LocalTimeZone;
			return Convert.ToInt64 (delta.TotalSeconds);
		}

		public static long GetNowTimeStamp ()
		{
			return GetTimeStamp (DateTime.Now);
		}

		public static long GetTimeStampDelta (DateTime small, DateTime big)
		{
			small = TimeZone.CurrentTimeZone.ToLocalTime (small);
			big = TimeZone.CurrentTimeZone.ToLocalTime (big);
			return Convert.ToInt64 ((big - small).TotalSeconds);
		}

		public static DateTime GetDateTime (long timeStamp_s)
		{
			TimeSpan ts = TimeSpan.FromSeconds (Convert.ToDouble (timeStamp_s));
			return new DateTime (ts.Ticks);
		}

		public static TimeSpan GetTimeSpan (long timeStamp_s)
		{
			return TimeSpan.FromSeconds (timeStamp_s);
		}

		public static DateTime GetDayStartTime (DateTime day)
		{
			return new DateTime (day.Year, day.Month, day.Day, 0, 0, 0, 0, day.Kind);
		}

		public static DateTime GetDayEndTime (DateTime day)
		{
			return new DateTime (day.Year, day.Month, day.Day, 23, 59, 59, 999, day.Kind);
		}

		public static DateTime GetMonthStartTime (DateTime month)
		{
			return new DateTime (month.Year, month.Month, 1, 0, 0, 0, 0, month.Kind);
		}

		public static DateTime GetMonthEndTime (DateTime month)
		{
			return new DateTime (month.Year, month.Month, G_Calender.GetDaysInMonth (month.Year, month.Month), 23, 59, 59, 999, month.Kind);
		}

		public static DateTime GetWeekStartTime (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			day = day.AddDays (weekStart - day.DayOfWeek + day.DayOfWeek > weekStart ? 0 : -7);
			return GetDayStartTime (day);
		}

		public static DateTime GetWeekEndTime (DateTime day, DayOfWeek weekEnd = DayOfWeek.Saturday)
		{
			day = day.AddDays (weekEnd - day.DayOfWeek + day.DayOfWeek > weekEnd ? 7 : 0);
			return GetDayEndTime (day);
		}

		public static DateTime GetYearStartTime (DateTime year)
		{
			return new DateTime (year.Year, 1, 1, 0, 0, 0, 0, year.Kind);
		}

		public static DateTime GetYearEndTime (DateTime year)
		{
			return new DateTime (year.Year, 12, G_Calender.GetDaysInYear (year.Year), 23, 59, 59, 999, year.Kind);
		}

		public static int GetWeekOfMonth (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			var mStart = GetMonthStartTime (day);
			int pass = day.Day - mStart.Day;
			return pass / 7 + mStart.DayOfWeek > weekStart ? 1 : 2;
		}

		public static int GetWeekOfYear (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			var yStart = GetYearStartTime (day);
			int pass = day.Day - yStart.Day;
			return pass / 7 + yStart.DayOfWeek > weekStart ? 1 : 2;
		}

		public static int MonthToQuarter (int month)
		{
			month = Mathf.Clamp (month, 1, 12);
			return (month - 1) / 3 + 1;
		}

		public static bool IsLeapYear (int year)
		{
			return G_Calender.IsLeapYear (year);
		}
	}
}
