using System;
using System.Globalization;
using UnityEngine;

namespace GameMain {
	public static class TimeUtility {

		/// <summary>
		/// The UTC Start Time.
		/// </summary>
		public static DateTime UTCStart = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// The UTC Start time on local time zone.
		/// </summary>
		public static DateTime UTCStart_LocalTimeZone = TimeZone.CurrentTimeZone.ToLocalTime (UTCStart);

		/// <summary>
		/// The Gregorian Calender.
		/// </summary>
		public static GregorianCalendar G_Calender = new GregorianCalendar ();

		/// <summary>
		/// The Lunisolar Calender.
		/// </summary>
		public static ChineseLunisolarCalendar LunisolarCalender = new ChineseLunisolarCalendar ();

		/// <summary>
		/// Get the time stamp.
		/// </summary>
		/// <returns>The time stamp.</returns>
		/// <param name="time">DateTime.</param>
		public static long GetTimeStamp (DateTime time)
		{
			time = TimeZone.CurrentTimeZone.ToLocalTime (time);
			var delta = time - UTCStart_LocalTimeZone;
			return Convert.ToInt64 (delta.TotalSeconds);
		}

		/// <summary>
		/// Get the time stamp on local time Now.
		/// </summary>
		/// <returns>The time stamp.</returns>
		public static long GetNowTimeStamp ()
		{
			return GetTimeStamp (DateTime.Now);
		}

		/// <summary>
		/// Get the difference of two given time stamp.
		/// </summary>
		/// <returns>The difference.</returns>
		/// <param name="small">The Smaller time.</param>
		/// <param name="big">The Bigger time.</param>
		public static long GetTimeStampDelta (DateTime small, DateTime big)
		{
			small = TimeZone.CurrentTimeZone.ToLocalTime (small);
			big = TimeZone.CurrentTimeZone.ToLocalTime (big);
			return Convert.ToInt64 ((big - small).TotalSeconds);
		}

		/// <summary>
		/// Get the date time with a given time stamp seconds.
		/// </summary>
		/// <returns>The Datetime.</returns>
		/// <param name="timeStamp_s">Time stamp seconds.</param>
		public static DateTime GetDateTime (long timeStamp_s)
		{
			TimeSpan ts = TimeSpan.FromSeconds (Convert.ToDouble (timeStamp_s));
			return new DateTime (ts.Ticks);
		}

		/// <summary>
		/// Get the time span with a given time stamp seconds.
		/// </summary>
		/// <returns>The TimeSpan.</returns>
		/// <param name="timeStamp_s">Time stamp seconds.</param>
		public static TimeSpan GetTimeSpan (long timeStamp_s)
		{
			return TimeSpan.FromSeconds (timeStamp_s);
		}

		/// <summary>
		/// Get the given day's start time.
		/// </summary>
		/// <returns>The start DateTime.</returns>
		/// <param name="day">Day.</param>
		public static DateTime GetDayStartTime (DateTime day)
		{
			return new DateTime (day.Year, day.Month, day.Day, 0, 0, 0, 0, day.Kind);
		}

		/// <summary>
		/// Get the given day's end time.
		/// </summary>
		/// <returns>The end time DateTime.</returns>
		/// <param name="day">Day.</param>
		public static DateTime GetDayEndTime (DateTime day)
		{
			return new DateTime (day.Year, day.Month, day.Day, 23, 59, 59, 999, day.Kind);
		}

		/// <summary>
		/// Get the given month's start time.
		/// </summary>
		/// <returns>The start time DateTime.</returns>
		/// <param name="month">Month.</param>
		public static DateTime GetMonthStartTime (DateTime month)
		{
			return new DateTime (month.Year, month.Month, 1, 0, 0, 0, 0, month.Kind);
		}

		/// <summary>
		/// Get the given month's end time.
		/// </summary>
		/// <returns>The end time DateTime.</returns>
		/// <param name="month">Month.</param>
		public static DateTime GetMonthEndTime (DateTime month)
		{
			return new DateTime (month.Year, month.Month, G_Calender.GetDaysInMonth (month.Year, month.Month), 23, 59, 59, 999, month.Kind);
		}

		/// <summary>
		/// Get the given day's current week's start time.
		/// </summary>
		/// <returns>The week's start time DateTime.</returns>
		/// <param name="day">Day.</param>
		/// <param name="weekStart">Week start Day.</param>
		public static DateTime GetWeekStartTime (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			day = day.AddDays (weekStart - day.DayOfWeek + day.DayOfWeek > weekStart ? 0 : -7);
			return GetDayStartTime (day);
		}

		/// <summary>
		/// Get the given day's current week's end time.
		/// </summary>
		/// <returns>The week's end time DateTime.</returns>
		/// <param name="day">Day.</param>
		/// <param name="weekEnd">Week end Day.</param>
		public static DateTime GetWeekEndTime (DateTime day, DayOfWeek weekEnd = DayOfWeek.Saturday)
		{
			day = day.AddDays (weekEnd - day.DayOfWeek + day.DayOfWeek > weekEnd ? 7 : 0);
			return GetDayEndTime (day);
		}

		/// <summary>
		/// Get the given year's start time.
		/// </summary>
		/// <returns>The start time DateTime.</returns>
		/// <param name="year">Year.</param>
		public static DateTime GetYearStartTime (DateTime year)
		{
			return new DateTime (year.Year, 1, 1, 0, 0, 0, 0, year.Kind);
		}

		/// <summary>
		/// Get the given year's end time.
		/// </summary>
		/// <returns>The end time DateTime.</returns>
		/// <param name="year">Year.</param>
		public static DateTime GetYearEndTime (DateTime year)
		{
			return new DateTime (year.Year, 12, G_Calender.GetDaysInYear (year.Year), 23, 59, 59, 999, year.Kind);
		}

		/// <summary>
		/// Get the given day's current week number of month. Start from 1.
		/// </summary>
		/// <returns>The week number of month.</returns>
		/// <param name="day">Day.</param>
		/// <param name="weekStart">Week start day.</param>
		public static int GetWeekOfMonth (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			var mStart = GetMonthStartTime (day);
			int pass = day.Day - mStart.Day;
			return pass / 7 + mStart.DayOfWeek > weekStart ? 1 : 2;
		}

		/// <summary>
		/// Get the given day's current week number of year. Start from 1.
		/// </summary>
		/// <returns>The week number of year.</returns>
		/// <param name="day">Day.</param>
		/// <param name="weekStart">Week start Day.</param>
		public static int GetWeekOfYear (DateTime day, DayOfWeek weekStart = DayOfWeek.Sunday)
		{
			var yStart = GetYearStartTime (day);
			int pass = day.Day - yStart.Day;
			return pass / 7 + yStart.DayOfWeek > weekStart ? 1 : 2;
		}

		/// <summary>
		/// Convert Month number to Quarter number(1-4).
		/// </summary>
		/// <returns>The to Quarter number.</returns>
		/// <param name="month">Month number.</param>
		public static int MonthToQuarter (int month)
		{
			month = Mathf.Clamp (month, 1, 12);
			return (month - 1) / 3 + 1;
		}

		/// <summary>
		/// Is the given year a leap year.
		/// </summary>
		/// <returns><c>true</c>, if it's a leap year, <c>false</c> otherwise.</returns>
		/// <param name="year">Year.</param>
		public static bool IsLeapYear (int year)
		{
			return G_Calender.IsLeapYear (year);
		}
	}
}
