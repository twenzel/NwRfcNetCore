using System.Collections.ObjectModel;

namespace NwRfcNetCore;

/// <summary>
/// Base implementation of te connection parameters
/// </summary>
public class RfcConnectionParameters : IRfcConnectionParameters
{
	public const string DEFAULT_USER_NAME_PARAMETER_KEY = "user";
	public const string DEFAULT_PASSWORD_PARAMETER_KEY = "passwd";
	public const string DEFAULT_HOST_PARAMETER_KEY = "ASHOST";
	public const string DEFAULT_CLIENT_PARAMETER_KEY = "client";

	public const string DEFAULT_SYSTEM_NUMBER_KEY = "sysnr";
	public const string DEFAULT_SYSTEM_ID_KEY = "SYSID";
	public const string DEFAULT_CONNECTION_NAME_PARAMETER_KEY = "name";
	public const string DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY = "lang";
	public const string DEFAULT_TRACE_PARAMETER_KEY = "trace";
	public const string DEFAULT_CONNECTION_POOL_SIZE_PARAMETER_KEY = "POOL_SIZE";
	public const string DEFAULT_SAP_ROUTER_PARAMETER_KEY = "saprouter";

	public const string DEFAULT_SNC_QOP_PARAMETER_KEY = "snc_qop";
	public const string DEFAULT_SNC_MY_NAME_PARAMETER_KEY = "snc_myname";
	public const string DEFAULT_SNC_PARTNER_NAME_PARAMETER_KEY = "snc_partnername";
	public const string DEFAULT_SNC_LIB_PARAMETER_KEY = "snc_lib";
	public const string DEFAULT_SNC_MODE_PARAMETER_KEY = "snc_mode";

	protected readonly Dictionary<string, string> _connectionParameters = new(StringComparer.OrdinalIgnoreCase);

	public IReadOnlyDictionary<string, string> Parameters =>
		new ReadOnlyDictionary<string, string>(_connectionParameters);

	public RfcConnectionParameters SetParameter(string key, string value)
	{
		ArgumentNullException.ThrowIfNull(key);
		ArgumentNullException.ThrowIfNull(value);

		_connectionParameters[key] = value;

		return this;
	}
}
