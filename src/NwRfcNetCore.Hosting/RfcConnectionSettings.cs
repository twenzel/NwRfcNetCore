namespace NwRfcNetCore.Hosting;

public class RfcConnectionSettings : RfcConnectionParameters
{
	/// <summary>
	/// The backend's system ID. (E.g. "H9C"). If this parameter is not specified, the value of DEST is used instead.
	/// </summary>
	public string? SysId { get => Get(DEFAULT_SYSTEM_ID_KEY); set => Set(DEFAULT_SYSTEM_ID_KEY, value); }

	/// <summary>
	/// Hostname of the application server
	/// </summary>
	public string? AppServerHost { get => Get(DEFAULT_HOST_PARAMETER_KEY); set => Set(DEFAULT_HOST_PARAMETER_KEY, value); }

	/// <summary>
	/// The backend's system number. (E.g. "01")
	/// </summary>
	public string? SysNumber { get => Get(DEFAULT_SYSTEM_NUMBER_KEY); set => Set(DEFAULT_SYSTEM_NUMBER_KEY, value); }

	/// <summary>
	/// The Client or "Mandant" to which to logon.
	/// </summary>
	public string? Client { get => Get(DEFAULT_CLIENT_PARAMETER_KEY); set => Set(DEFAULT_CLIENT_PARAMETER_KEY, value); }
	public string? User { get => Get(DEFAULT_USER_NAME_PARAMETER_KEY); set => Set(DEFAULT_USER_NAME_PARAMETER_KEY, value); }
	public string? Password { get => Get(DEFAULT_PASSWORD_PARAMETER_KEY); set => Set(DEFAULT_PASSWORD_PARAMETER_KEY, value); }

	/// <summary>
	/// Logon Language. Either specify the two-character ISO-Code (like EN for English,
	/// KO for Korean) or the one - character SAP-specific code(like E for English,
	/// 3 for Korean). Note that the ISO-codes are case-insensitiv, while the SAP
	/// codes are not! So 'D' logs you on in German, while 'd' logs you on in
	/// Serbian, if that language is installed in your system...
	/// </summary>
	public string? Language { get => Get(DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY); set => Set(DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY, value); }

	/// <summary>
	/// One of 0(off), 1(brief), 2(verbose), 3(detailed), 4(full)
	/// </summary>
	public TraceLevel Trace { get => (TraceLevel)GetInt(DEFAULT_TRACE_PARAMETER_KEY); set => Set(DEFAULT_TRACE_PARAMETER_KEY, ((int)value).ToString()); }

	private void Set(string key, string? value)
	{
		if (value != null)
			_connectionParameters[key] = value;
		else
			_connectionParameters.Remove(key);
	}

	private string? Get(string key)
	{
		if (Parameters.TryGetValue(key, out var value))
			return value;

		return null;
	}

	private int GetInt(string key)
	{
		if (Parameters.TryGetValue(key, out var value))
			return int.Parse(value);

		return 0;
	}
}
