using NwRfcNetCore.Interop;
using NwRfcNetCore.TypeMapper;

namespace NwRfcNetCore;

/// <summary>
/// RFC paramter handler base class
/// </summary>
internal abstract class RfcParameter
{
	/// <summary>
	/// Returns the value of the specified field as a pointer to RFCTYPE_STRUCTURE 
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="map"></param>
	/// <returns></returns>
	protected static IntPtr GetStructure(IntPtr handler, PropertyMap map)
	{
		var rc = RfcInterop.RfcGetStructure(handler, map.RfcParameterName, out var structHandle, out var errorInfo);
		rc.OnErrorThrowException(errorInfo);
		return structHandle;
	}
}