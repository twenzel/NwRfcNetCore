using System.Text.RegularExpressions;
using NwRfcNetCore.Interop;
using NwRfcNetCore.Util;

namespace NwRfcNetCore.RfcTypes;

/// <summary>
/// Represents an RFC Date. Format (YYYYMMDD)
/// </summary>
public partial class RfcDate
{
	private const string YEAR_FORMAT = "YYYY";
	private const string MONTH_FORMAT = "MM";
	private const string DAY_FORMAT = "DD";

	private const string RFC_DATE_TEMPLATE = "YYYYMMDD";

	// Null RFC date in string format
	public static readonly string NullRfcDateString = string.Empty.PadRight(RFC_DATE_TEMPLATE.Length);
	public static readonly string ZerolRfcDateString = string.Empty.PadRight(RFC_DATE_TEMPLATE.Length, '0');

	private static readonly Regex _rfcDateFormat = RfcDateFormatExpression();

	/// <summary>
	/// Creates a new RfcDate with format YYYMMDD
	/// </summary>
	/// <param name="date">YYYYMMDD RFC Date</param>
	public RfcDate(string date)
	{
		if (string.IsNullOrEmpty(date) || date == NullRfcDateString || date == ZerolRfcDateString)
		{
			RfcValue = null;
			return;
		}

		if (!_rfcDateFormat.IsMatch(date))
			throw new ArgumentException($"Invalid RFC Date {date}");

		var year = int.Parse(date[..YEAR_FORMAT.Length]);
		var month = int.Parse(date.Substring(YEAR_FORMAT.Length, MONTH_FORMAT.Length));
		var day = int.Parse(date.Substring(YEAR_FORMAT.Length + MONTH_FORMAT.Length, DAY_FORMAT.Length));

		RfcValue = new DateTime(year, month, day);
	}

	/// <summary>
	/// Creates a new RfcDate with format YYYMMDD
	/// </summary>
	/// <param name="date">YYYYMMDD RFC Date</param>
	public RfcDate(DateTime? date) => RfcValue = date?.Date;

	/// <summary>
	/// Date
	/// </summary>
	public DateTime? RfcValue { get; }

	/// <summary>
	/// Converts date to RFC Date format
	/// </summary>
	/// <returns></returns>
	public override string ToString()
		=> RfcValue?.ToString("yyyyMMdd") ?? ZerolRfcDateString;

	public char[] ToBuffer() => ToString().ToCharArray();


	#region Interop

	/// <summary>
	/// sets the value of a RFC date field
	/// </summary>
	/// <param name="dataHandle">handle to container</param>
	/// <param name="name">field name</param>
	internal void SetFieldValue(IntPtr dataHandle, string name)
	{
		if (RfcValue != null)
		{
			var rc = RfcInterop.RfcSetDate(dataHandle, name, ToBuffer(), out var errorInfo);
			rc.OnErrorThrowException(errorInfo);
		}
	}

	/// <summary>
	/// Gets the value of a RFC date field
	/// </summary>
	/// <param name="dataHandle">handle to container</param>
	/// <param name="name">field name</param>
	/// <returns></returns>
	internal static RfcDate? GetFieldValue(IntPtr dataHandle, string name)
	{
		var buffer = new char[RFC_DATE_TEMPLATE.Length]; //   = new StringBuilder(YearFormat.Length + MonthFormat.Length + DayFormat.Length);
		buffer.FillAll(' ');

		var rc = RfcInterop.RfcGetDate(dataHandle, name, buffer, out var errorInfo);
		rc.OnErrorThrowException(errorInfo);

		var date = new string(buffer);

		if ((date == NullRfcDateString) || (date == ZerolRfcDateString))
			return null;

		return new RfcDate(date);
	}
	#endregion

	[GeneratedRegex("^[0-9]{8}$", RegexOptions.Compiled)]
	private static partial Regex RfcDateFormatExpression();
}
