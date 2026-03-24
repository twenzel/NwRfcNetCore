namespace NwRfcNetCore.Hosting;

public class RfcConnectionSettings : RfcConnectionParameters
{
	public string? SysId { get => Get(DEFAULT_SYSTEM_ID_KEY); set => Set(DEFAULT_SYSTEM_ID_KEY, value); }
	public string? AppServerHost { get => Get(DEFAULT_HOST_PARAMETER_KEY); set => Set(DEFAULT_HOST_PARAMETER_KEY, value); }
	public string? SysNumber { get => Get(DEFAULT_SYSTEM_NUMBER_KEY); set => Set(DEFAULT_SYSTEM_NUMBER_KEY, value); }
	public string? Client { get => Get(DEFAULT_CLIENT_PARAMETER_KEY); set => Set(DEFAULT_CLIENT_PARAMETER_KEY, value); }
	public string? User { get => Get(DEFAULT_USER_NAME_PARAMETER_KEY); set => Set(DEFAULT_USER_NAME_PARAMETER_KEY, value); }
	public string? Password { get => Get(DEFAULT_PASSWORD_PARAMETER_KEY); set => Set(DEFAULT_PASSWORD_PARAMETER_KEY, value); }
	public string? Language { get => Get(DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY); set => Set(DEFAULT_CONNECTION_LANGUAGE_PARAMETER_KEY, value); }

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
}
