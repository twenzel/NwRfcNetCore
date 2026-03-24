using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace NwRfcNetCore.Util;

internal static partial class UriExtensions
{
	private const string URI_QUERY_PARAMETERS_PREFIX = "?";
	private const string URI_QUERY_PARAMETERS_SEPARATOR = "&";
	private const string URI_QUERY_PARAMETERS_VALUE_SEPARATOR = "=";

	public static NameValueCollection ParseQueryString(this string queryString)
	{
		var nvc = new NameValueCollection();

		if (string.IsNullOrWhiteSpace(queryString))
		{
			return nvc;
		}

		// remove anything other than query string from url
		if (queryString.Contains(URI_QUERY_PARAMETERS_PREFIX))
		{
			queryString = queryString[(queryString.IndexOf(URI_QUERY_PARAMETERS_PREFIX) + URI_QUERY_PARAMETERS_PREFIX.Length)..];
		}

		foreach (var vp in QueryParameterExpression().Split(queryString))
		{
			var singlePair = QueryParameterValuesExpression().Split(vp);
			if (singlePair.Length == 2)
			{
				nvc.Add(singlePair[0], singlePair[1]);
			}
			else
			{
				// only one key with no value specified in query string
				nvc.Add(singlePair[0], string.Empty);
			}
		}

		return nvc;
	}

	[GeneratedRegex(URI_QUERY_PARAMETERS_SEPARATOR)]
	private static partial Regex QueryParameterExpression();

	[GeneratedRegex(URI_QUERY_PARAMETERS_VALUE_SEPARATOR)]
	private static partial Regex QueryParameterValuesExpression();
}
