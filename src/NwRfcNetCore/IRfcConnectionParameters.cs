namespace NwRfcNetCore;

/// <summary>
/// Represents a collection of all parameters used to create a RFC connection.
/// </summary>
public interface IRfcConnectionParameters
{
	/// <summary>
	/// Gets the parameters used to create a RFC connection.
	/// </summary>
	IReadOnlyDictionary<string, string> Parameters { get; }
}
