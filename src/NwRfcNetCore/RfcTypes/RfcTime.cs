using System.Text.RegularExpressions;
using NwRfcNetCore.Interop;
using NwRfcNetCore.Util;

namespace NwRfcNetCore.RfcTypes;

/// <summary>
/// Represents an RFC Time. Format (HHMMSS)
/// </summary>
public partial class RfcTime
{
	private const string HOURS_FORMAT = "HH";
	private const string MINUTES_FORMAT = "MM";
	private const string SECONDS_FORMAT = "SS";

	private const string RFC_TIME_TEMPLATE = "HHMMSS";

	// Null RFC date in string format
	public static readonly string NullRfcTimeString = string.Empty.PadRight(RFC_TIME_TEMPLATE.Length);
	public static readonly string ZeroRfcTimeString = string.Empty.PadRight(RFC_TIME_TEMPLATE.Length, '0');

	private static readonly Regex _rfcTimeFormat = RfcTimeFormatExpression();

	/// <summary>
	/// Creates a new RfcDate with format YYYMMDD
	/// </summary>
	/// <param name="date">YYYYMMDD RFC Date</param>
	public RfcTime(string time)
	{
		if (string.IsNullOrEmpty(time) || time == NullRfcTimeString || time == ZeroRfcTimeString)
		{
			RfcValue = null;
			return;
		}

		if (!_rfcTimeFormat.IsMatch(time))
			throw new ArgumentException($"Invalid RFC Time {time}");

		var hours = int.Parse(time[..HOURS_FORMAT.Length]);
		var minutes = int.Parse(time.Substring(HOURS_FORMAT.Length, MINUTES_FORMAT.Length));
		var seconds = int.Parse(time.Substring(HOURS_FORMAT.Length + MINUTES_FORMAT.Length, SECONDS_FORMAT.Length));

		RfcValue = new TimeSpan(hours, minutes, seconds);
	}

	/// <summary>
	/// Creates a new RfcDate with format YYYMMDD
	/// </summary>
	/// <param name="date">YYYYMMDD RFC Date</param>
	public RfcTime(TimeSpan? time) => RfcValue = time;

	/// <summary>
	/// Time
	/// </summary>
	public TimeSpan? RfcValue { get; }

	/// <summary>
	/// Converts date to RFC Time format
	/// </summary>
	/// <returns></returns>
	public override string ToString()
		=> RfcValue?.ToString("hhmmss") ?? ZeroRfcTimeString;

	public char[] ToBuffer() => ToString().ToCharArray();

	/// <summary>
	/// Sets the value of a RFC time field
	/// </summary>
	/// <param name="dataHandle">handle to container</param>
	/// <param name="name">field name</param>
	internal void SetFieldValue(IntPtr dataHandle, string name)
	{
		if (RfcValue != null)
		{
			var rc = RfcInterop.RfcSetTime(dataHandle, name, ToBuffer(), out var errorInfo);
			rc.OnErrorThrowException(errorInfo);
		}
	}

	/// <summary>
	/// Gets the value of a RFC date field
	/// </summary>
	/// <param name="dataHandle">handle to container</param>
	/// <param name="name">field name</param>
	/// <returns></returns>
	internal static RfcTime? GetFieldValue(IntPtr dataHandle, string name)
	{
		var buffer = new char[RFC_TIME_TEMPLATE.Length]; //   = new StringBuilder(YearFormat.Length + MonthFormat.Length + DayFormat.Length);
		buffer.FillAll(' ');

		var rc = RfcInterop.RfcGetTime(dataHandle, name, buffer, out var errorInfo);
		rc.OnErrorThrowException(errorInfo);

		var time = new string(buffer);

		if ((time == NullRfcTimeString) || (time == ZeroRfcTimeString))
			return null;

		return new RfcTime(time);
	}

	[GeneratedRegex("^[0-9]{6}$", RegexOptions.Compiled)]
	private static partial Regex RfcTimeFormatExpression();
}
