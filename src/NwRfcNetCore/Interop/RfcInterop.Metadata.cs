using System.Runtime.InteropServices;

namespace NwRfcNetCore.Interop;

internal static partial class RfcInterop
{
	[DllImport(NW_RFC_LIB, CharSet = CharSet.Unicode)]
	public static extern IntPtr RfcGetFunctionDesc(IntPtr rfcHandle, string funcName, out RFC_ERROR_INFO errorInfo);
}