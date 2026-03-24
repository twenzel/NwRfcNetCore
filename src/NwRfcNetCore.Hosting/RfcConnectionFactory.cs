namespace NwRfcNetCore.Hosting;

/// <summary>
/// Implementation of the <see cref="IRfcConnectionFactory"/>
/// </summary>
public class RfcConnectionFactory : IRfcConnectionFactory
{
	private readonly IRfcConnectionParameters _rfcConnectionParameters;

	/// <summary>
	/// Initializes a new instance.
	/// </summary>
	/// <param name="rfcConnectionParameters">The connection parameters.</param>
	/// <exception cref="ArgumentNullException"></exception>
	public RfcConnectionFactory(IRfcConnectionParameters rfcConnectionParameters)
	{
		_rfcConnectionParameters = rfcConnectionParameters ?? throw new ArgumentNullException(nameof(rfcConnectionParameters));
	}

	/// <inheritdoc />
	public RfcConnection CreateConnection()
	{
		return new RfcConnection(_rfcConnectionParameters);
	}
}
