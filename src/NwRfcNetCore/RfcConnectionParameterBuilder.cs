using System.Collections.Specialized;
using NwRfcNetCore.Util;

namespace NwRfcNetCore;

/// <summary>
/// Helps to create the parameters to build a RFC connection.
/// </summary>
public class RfcConnectionParameterBuilder
{
	private readonly RfcConnectionParameters _parameters;
	private const string CONNECTION_URI_SCHEME = "sap";
	private const char CONNECTION_STRING_ITEM_SEPARATOR = ';';
	private const char CONNECTION_STRING_KEY_VALUE_SEPARATOR = '=';
	private static readonly char[] _connectionStringKeyValueSeparatorSplit = [CONNECTION_STRING_KEY_VALUE_SEPARATOR];
	private static readonly string[] _userNameKeyAliases = ["userName", "userId", "uid", "user", "u"];
	private static readonly string[] _passwordKeyAliases = ["password", "passwd", "pass", "pwd", "p"];
	private static readonly string[] _targetHostKeyAliases = ["target_host", "targetHost", "host", "server", "h"];
	private static readonly string[] _logonLanguageKeyAliases = ["language", "lang", "l"];
	private static readonly string[] _logonClientKeyAliases = ["client", "cl", "c"];
	private static readonly string[] _systemNumberKeyAliases = ["system_number", "systemnumber", "sysnr"];
	private static readonly string[] _systemIdKeyAliases = ["system_id", "systemid", "sysid"];
	private static readonly string[] _traceKeyAliases = ["trace", "tr", "RfcSdkTrace"];
	private static readonly string[] _poolSizeKeyAliases = ["connection_pool_size", "pool_size", "ps"];
	private static readonly string[] _sapRouterKeyAliases = ["sap_router_name", "sap_router", "router_name", "router", "rn"];
	private static readonly string[] _sncModeKeyAliases = ["snc_mode", "sncmode", "UseSnc", "snc"];
	private static readonly string[] _sncQopKeyAliases = ["snc_qop", "sncqop"];
	private static readonly string[] _sncMyNameKeyAliases = ["snc_myname", "sncmyname"];
	private static readonly string[] _sncPartnerKeyAliases = ["snc_partnername", "sncpartnername", "snc_partner", "sncpartner"];
	private static readonly string[] _sncLibKeyAliases = ["snc_library", "snc_lib", "snclib"];

	private static readonly string[] _activationAliases = ["1", "enabled", "On", "true", "yes"];
	private static readonly string[] _deactivationAliases = ["0", "disabled", "Off", "false", "no"];

	/// <summary>
	/// Creates a new instance of the <see cref="RfcConnectionParameterBuilder"/> class.
	/// </summary>
	public RfcConnectionParameterBuilder()
	{
		_parameters = new RfcConnectionParameters();
	}

	/// <summary>
	/// Sets the user name for the RFC connection.
	/// </summary>
	/// <param name="userName">The user name.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseLogonUserName(string userName, string key = RfcConnectionParameters.DEFAULT_USER_NAME_PARAMETER_KEY) =>
	  SetParameter(key, userName);

	/// <summary>
	/// Sets the password for the RFC connection.
	/// </summary>
	/// <param name="password">The password of the connection.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseLogonPassword(string password, string key = RfcConnectionParameters.DEFAULT_PASSWORD_PARAMETER_KEY) =>
	  SetParameter(key, password);

	/// <summary>
	/// Sets the connection client.
	/// </summary>
	/// <param name="client">The sap server connection client.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseLogonClient(string client, string key = RfcConnectionParameters.DEFAULT_CLIENT_PARAMETER_KEY) =>
	  SetParameter(key, client);

	/// <summary>
	/// Sets the connection language.
	/// </summary>
	/// <param name="connectionLanguage">The host name of the sap server.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseLogonLanguage(string connectionLanguage, string key = RfcConnectionParameters.DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY) =>
	  SetParameter(key, connectionLanguage);

	/// <summary>
	/// Sets the host name of the sap server.
	/// </summary>
	/// <param name="host">The host name of the sap server.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseConnectionHost(string host, string key = RfcConnectionParameters.DEFAULT_HOST_PARAMETER_KEY) =>
	  SetParameter(key, host);

	/// <summary>
	/// Sets the sap system number.
	/// </summary>
	/// <param name="systemNumber">The sap system number.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSystemNumber(string systemNumber, string key = RfcConnectionParameters.DEFAULT_SYSTEM_NUMBER_KEY) =>
	  SetParameter(key, systemNumber);

	/// <summary>
	/// Sets the id of the sap system.
	/// </summary>
	/// <param name="systemId">The id of the sap system.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSystemId(string systemId, string key = RfcConnectionParameters.DEFAULT_SYSTEM_ID_KEY) =>
	  SetParameter(key, systemId);

	/// <summary>
	/// Sets the trace state of the connection (This must be 'true/false', '0/1', 'On/Off', 'enabled/disabled', 'yes/no').
	/// </summary>
	/// <param name="trace">The trace activation state of the connection.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseTrace(string trace = "0", string key = RfcConnectionParameters.DEFAULT_TRACE_PARAMETER_KEY)
	{
		ArgumentNullException.ThrowIfNull(trace);

		if (_activationAliases.Any(aa => trace.IndexOf(aa, StringComparison.OrdinalIgnoreCase) > -1))
			SetParameter(key, "1");
		else if (_deactivationAliases.Any(da => trace.IndexOf(da, StringComparison.OrdinalIgnoreCase) > -1))
			SetParameter(key, "0");
		else
			throw new ArgumentException(paramName: nameof(trace), message: $"The give value '{trace}' was invalid. Permitted values are: 'true/false', '0/1', 'On/Off', 'enabled/disabled', 'yes/no'");

		return this;
	}

	/// <summary>
	/// Sets size of the connection pool (This must be a positive value).
	/// </summary>
	/// <param name="connectionPoolSize">The size of the connection pool.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseConnectionPooling(int connectionPoolSize = 10, string key = RfcConnectionParameters.DEFAULT_CONNECTION_POOL_SIZE_PARAMETER_KEY)
	{
		if (connectionPoolSize < 1)
			throw new ArgumentException(paramName: nameof(connectionPoolSize), message: "The connection pool size must be at greater than zero.");

		return SetParameter(key, connectionPoolSize.ToString());
	}

	/// <summary>
	/// Sets the name of the sap router for the RFC connection.
	/// </summary>
	/// <param name="sapRouter">The name of the sap router.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSapRouter(string sapRouter, string key = RfcConnectionParameters.DEFAULT_SAP_ROUTER_PARAMETER_KEY) =>
	   SetParameter(key, sapRouter);

	/// <summary>
	/// Sets the secure network communication quality of protection (This must be one of the values '1','2','3','8','9').
	/// </summary>
	/// <param name="sncQop">The SNC quality of protection (protection level).</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSecureNetworkCommunicationQop(string sncQop = "3", string key = RfcConnectionParameters.DEFAULT_SNC_QOP_PARAMETER_KEY)
	{
		if (int.TryParse(sncQop, out var parsed) == false || (parsed != 1 && parsed != 2 && parsed != 3 && parsed != 8 && parsed != 9))
			throw new ArgumentException(paramName: nameof(sncQop), message: "The only permitted values for the quality of protection are: '1','2','3','8','9'.");

		return SetParameter(key, parsed.ToString());
	}

	/// <summary>
	/// Sets the secure network communication mode (This should be one of the values '0'->SNC-Disabled, '1'->SNC-Enabled).
	/// </summary>
	/// <param name="sncMode">The SNC mode ('0'->SNC-Disabled, '1'->SNC-Enabled).</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSecureNetworkCommunicationMode(string sncMode, string key = RfcConnectionParameters.DEFAULT_SNC_MODE_PARAMETER_KEY)
	{
		if (int.TryParse(sncMode, out var parsed) == false || (parsed != 0 && parsed != 1))
			throw new ArgumentException(paramName: nameof(sncMode), message: "The only permitted values for the snc-mode are: '0' for SNC-Disabled or'1' for SNC-Enabled.");

		return SetParameter(key, parsed.ToString());
	}

	/// <summary>
	/// Sets the secure network communication name of the RFC server program.
	/// </summary>
	/// <param name="sncMyName">The SNC name of the RFC server program.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSecureNetworkCommunicationMyName(string sncMyName, string key = RfcConnectionParameters.DEFAULT_SNC_MY_NAME_PARAMETER_KEY) =>
	   SetParameter(key, sncMyName);

	/// <summary>
	/// Sets the secure network communication library.
	/// </summary>
	/// <param name="sncLibName">The path and file name of the 'gssapi' library.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSecureNetworkCommunicationLibrary(string sncLibName, string key = RfcConnectionParameters.DEFAULT_SNC_LIB_PARAMETER_KEY) =>
	   SetParameter(key, sncLibName);

	/// <summary>
	/// Sets the secure network communication partner.
	/// </summary>
	/// <param name="sncPartnerName">The partner name.</param>
	/// <param name="key">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseSecureNetworkCommunicationPartner(string sncPartnerName, string key = RfcConnectionParameters.DEFAULT_SNC_PARTNER_NAME_PARAMETER_KEY) =>
	   SetParameter(key, sncPartnerName);

	/// <summary>
	/// Sets an additional connection parameter.
	/// </summary>
	/// <param name="parameterValue">The partner name.</param>
	/// <param name="parameterKey">The key for the parameter.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseAdditionalParameter(string parameterValue, string parameterKey) =>
	   SetParameter(parameterKey, parameterValue);

	/// <summary>
	/// Sets multiple logon parameters for the connection creation.
	/// </summary>
	/// <param name="language">The connection language (This is normally a 2-letter country key like EN,DE or FR).</param>
	/// <param name="client">The client identifier (This is normally a 3 digit number with leading zeros like '001').</param>
	/// <param name="userName">The user name.</param>
	/// <param name="password">The password.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder UseLogonInformation(string language, string client, string userName, string password)
	{
		ArgumentNullException.ThrowIfNull(language);
		ArgumentNullException.ThrowIfNull(client);
		ArgumentNullException.ThrowIfNull(userName);
		ArgumentNullException.ThrowIfNull(password);

		UseLogonLanguage(language);
		UseLogonClient(client);
		UseLogonUserName(userName);
		UseLogonPassword(password);
		return this;
	}

	/// <summary>
	/// Use an <see cref="Uri"/> instance to build a connection (must be in form 'scheme://userinfoparams@hostinfoparams?query_string').
	/// </summary>
	/// <param name="connectionUri">The uri representing the connection parameters.</param>
	/// <returns>The connection parameter builder.</returns>
	/// <remarks>
	/// A complete uri wourld be:
	/// <para>
	/// sap://user=[USER_NAME];passwd=[PASSWORD];Client=[CLIENT];lang=[LANGUAGE];[UseSnc]=[true|false]@connectiontype/conndetail1/conndetail2?GwHost=[GWHOST]?GwServ=[GWSERV]?MsServ=[MSSERV]?Group=[GROUP]?ListenerDest=[LISTENERDEST]?ListenerGwHost=[LISTENERGWHOST]?ListenerGwServ=[LISTENERGWSERV]?ListenerProgramId=[LISTENERPROGRAMID]?RfcSdkTrace=[true/false]?AbapDebug=[true/false]
	/// </para>
	/// A sample would be:
	/// <para>
	/// sap://Client=800;lang=EN@A/YourSAPHOST/00 
	/// </para>
	/// </remarks>
	public RfcConnectionParameterBuilder FromConnectionUri(Uri connectionUri)
	{
		ArgumentNullException.ThrowIfNull(connectionUri);

		if (!connectionUri.Scheme.Equals(CONNECTION_URI_SCHEME, StringComparison.OrdinalIgnoreCase))
			throw new ArgumentException(paramName: nameof(connectionUri), message: $"The connection string uri must have the scheme '{CONNECTION_URI_SCHEME}'.");

		var userInfo = connectionUri.UserInfo.Split([CONNECTION_STRING_ITEM_SEPARATOR], StringSplitOptions.RemoveEmptyEntries) ?? [];
		var userInfoKvp = userInfo
		  .Where(kvp => kvp.Contains(CONNECTION_STRING_KEY_VALUE_SEPARATOR))
		  .Select(kvp => kvp.Split(_connectionStringKeyValueSeparatorSplit, 2))
		  .ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.OrdinalIgnoreCase);

		var userName = GetWhenPresent(userInfoKvp, _userNameKeyAliases);
		userName = GetUnescapedWhenPresent(userName);

		var password = GetWhenPresent(userInfoKvp, _passwordKeyAliases);
		password = GetUnescapedWhenPresent(password);

		var client = GetWhenPresent(userInfoKvp, _logonClientKeyAliases);
		client = GetUnescapedWhenPresent(client);

		var language = GetWhenPresent(userInfoKvp, _logonLanguageKeyAliases);
		language = GetUnescapedWhenPresent(language);
		;

		var sncMode = GetWhenPresent(userInfoKvp, _sncModeKeyAliases);
		sncMode = GetUnescapedWhenPresent(sncMode);

		var param = connectionUri.Query.ParseQueryString();

		param.Add(RfcConnectionParameters.DEFAULT_USER_NAME_PARAMETER_KEY, userName);
		param.Add(RfcConnectionParameters.DEFAULT_PASSWORD_PARAMETER_KEY, password);
		param.Add(RfcConnectionParameters.DEFAULT_CLIENT_PARAMETER_KEY, client);
		param.Add(RfcConnectionParameters.DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY, language);
		param.Add(RfcConnectionParameters.DEFAULT_SNC_MODE_PARAMETER_KEY, sncMode);

		if (connectionUri.Host.Equals("A", StringComparison.OrdinalIgnoreCase))
		{
			var detail1 = (connectionUri.Segments.Length > 1 ? connectionUri.Segments[1] : null)
				?? throw new ArgumentException(paramName: nameof(connectionUri), message: "The uri must contain the sap application server host (ASHOST) in the second segment.");

			param.Add(RfcConnectionParameters.DEFAULT_HOST_PARAMETER_KEY, detail1);

			var detail2 = connectionUri.Segments.Length > 2 ? connectionUri.Segments[2] : null;
			if (!string.IsNullOrWhiteSpace(detail2))
				param.Add(RfcConnectionParameters.DEFAULT_SYSTEM_NUMBER_KEY, detail2);
		}
		else
		{
			throw new NotSupportedException($"The given connection type '{connectionUri.Host}' is not supported yet.");
		}

		FromNamedValues(param);
		return this;
	}

	/// <summary>
	/// Use a connection string to build a connection (must be in form 'Server=[host]; UserName=[userName]; Password=[password]; Client=[Client]...').
	/// </summary>
	/// <param name="connectionString">The string representing the connection parameters.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder FromConnectionString(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentException(paramName: nameof(connectionString), message: "The connection string must not be null or empty.");

		var keyValuePairs = connectionString.Split(CONNECTION_STRING_ITEM_SEPARATOR)
		  .Where(kvp => kvp.Contains(CONNECTION_STRING_KEY_VALUE_SEPARATOR))
		  .Select(kvp => kvp.Split(_connectionStringKeyValueSeparatorSplit, 2))
		  .Select(kvp => new { Key = kvp[0].Trim(), Value = kvp[1].Trim() });

		var collection = new NameValueCollection();
		foreach (var kvp in keyValuePairs)
		{
			collection.Add(kvp.Key, kvp.Value);
		}

		return FromNameValueCollection(collection);
	}

	/// <summary>
	/// Use a <see cref="NameValueCollection"/> instance to build a connection.
	/// </summary>
	/// <param name="connectionParameters">The collection containing the connection parameters.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder FromNameValueCollection(NameValueCollection connectionParameters)
	{
		FromNamedValues(connectionParameters);
		return this;
	}

	/// <summary>
	/// Use a <see cref="IDictionary{TKey, TValue}"/> instance to build a connection.
	/// </summary>
	/// <param name="connectionParameters">The dictionary containing the connection parameters.</param>
	/// <returns>The connection parameter builder.</returns>
	public RfcConnectionParameterBuilder FromDictionary(IDictionary<string, string> connectionParameters)
	{
		ArgumentNullException.ThrowIfNull(connectionParameters);

		var collection = new NameValueCollection();
		foreach (var kvp in connectionParameters)
			collection.Add(kvp.Key, kvp.Value);

		return FromNameValueCollection(collection);
	}

	private RfcConnectionParameterBuilder SetParameter(string key, string value)
	{
		_parameters.SetParameter(key, value);
		return this;
	}

	private static void SetWhenPresent(string? value, Action<string> setAction)
	{
		if (!string.IsNullOrEmpty(value))
			setAction(value);
	}

	private static string? GetAndRemoveWhenPresent(NameValueCollection source, IEnumerable<string> aliases)
	{
		string? foundItem = null;

		foreach (var alias in aliases)
		{
			var item = source.Get(alias);
			if (item != null)
				foundItem = item;

			source.Remove(alias);
		}

		return foundItem;
	}

	private static string? GetWhenPresent(Dictionary<string, string> source, IEnumerable<string> aliases)
	{
		foreach (var alias in aliases)
		{
			if (source.TryGetValue(alias, out var value))
				return value;

		}

		return null;
	}

	private static string? GetUnescapedWhenPresent(string? value)
	{
		return (value != null) ? Uri.UnescapeDataString(value) : value;
	}

	private void FromValues(string? userName,
	  string? password,
	  string? host,
	  string? language,
	  string? client,
	  string? systemNumber,
	  string? systemId,
	  string? trace,
	  string? poolSize,
	  string? saprouter,
	  string? sncMode,
	  string? sncQop,
	  string? sncMyName,
	  string? sncPartner,
	  string? sncLib)
	{
		SetWhenPresent(userName, val => UseLogonUserName(val));
		SetWhenPresent(password, val => UseLogonPassword(val));
		SetWhenPresent(host, val => UseConnectionHost(val));
		SetWhenPresent(language, val => UseLogonLanguage(val));
		SetWhenPresent(client, val => UseLogonClient(val));
		SetWhenPresent(systemNumber, val => UseSystemNumber(val));
		SetWhenPresent(systemId, val => UseSystemId(val));
		SetWhenPresent(trace, val => UseTrace(val));
		SetWhenPresent(poolSize, val => UseConnectionPooling(int.Parse(val)));
		SetWhenPresent(saprouter, val => UseSapRouter(val));
		SetWhenPresent(sncMode, val => UseSecureNetworkCommunicationMode(val));
		SetWhenPresent(sncQop, val => UseSecureNetworkCommunicationQop(val));
		SetWhenPresent(sncMyName, val => UseSecureNetworkCommunicationMyName(val));
		SetWhenPresent(sncPartner, val => UseSecureNetworkCommunicationPartner(val));
		SetWhenPresent(sncLib, val => UseSecureNetworkCommunicationLibrary(val));
	}

	private void FromNamedValues(NameValueCollection param)
	{
		var localCopy = new NameValueCollection(param);

		var userName = GetAndRemoveWhenPresent(localCopy, _userNameKeyAliases);
		var password = GetAndRemoveWhenPresent(localCopy, _passwordKeyAliases);
		var host = GetAndRemoveWhenPresent(localCopy, _targetHostKeyAliases);
		var language = GetAndRemoveWhenPresent(localCopy, _logonLanguageKeyAliases);
		var client = GetAndRemoveWhenPresent(localCopy, _logonClientKeyAliases);
		var systemNumber = GetAndRemoveWhenPresent(localCopy, _systemNumberKeyAliases);
		var systemId = GetAndRemoveWhenPresent(localCopy, _systemIdKeyAliases);
		var trace = GetAndRemoveWhenPresent(localCopy, _traceKeyAliases);
		var poolSize = GetAndRemoveWhenPresent(localCopy, _poolSizeKeyAliases);
		var saprouter = GetAndRemoveWhenPresent(localCopy, _sapRouterKeyAliases);

		var sncMode = GetAndRemoveWhenPresent(localCopy, _sncModeKeyAliases);
		var sncQop = GetAndRemoveWhenPresent(localCopy, _sncQopKeyAliases);
		var sncMyName = GetAndRemoveWhenPresent(localCopy, _sncMyNameKeyAliases);
		var sncPartner = GetAndRemoveWhenPresent(localCopy, _sncPartnerKeyAliases);
		var sncLib = GetAndRemoveWhenPresent(localCopy, _sncLibKeyAliases);

		FromValues(userName, password, host, language, client, systemNumber, systemId, trace, poolSize, saprouter, sncMode, sncQop, sncMyName, sncPartner, sncLib);

		foreach (var additionalParameter in localCopy.AllKeys)
		{
			if (additionalParameter != null)
			{
				var value = localCopy.Get(additionalParameter);
				if (value != null)
					_parameters.SetParameter(additionalParameter, value);
			}
		}
	}

	internal RfcConnectionParameters Build() => _parameters;
}
