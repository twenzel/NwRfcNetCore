namespace NwRfcNetCore.RfcTypes;

internal interface IRfcType<T>
{
	T RfcValue { get; }
}
