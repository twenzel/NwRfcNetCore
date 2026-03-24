namespace NwRfcNetCore;

/// <summary>
/// Factory to create a new RFC connection
/// </summary>
public interface IRfcConnectionFactory
{
	/// <summary>
	/// Creates a new RFC connection
	/// </summary>
	/// <returns></returns>
	RfcConnection CreateConnection();
}
