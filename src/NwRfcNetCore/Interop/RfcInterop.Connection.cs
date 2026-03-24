using System.Runtime.InteropServices;

namespace NwRfcNetCore.Interop;

internal static partial class RfcInterop
{
	[DllImport(NW_RFC_LIB, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr RfcOpenConnection(RFC_CONNECTION_PARAMETER[] connectionParams, uint paramCount, out RFC_ERROR_INFO errorInfo);

	[DllImport(NW_RFC_LIB, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public static extern RFC_RC RfcCloseConnection(IntPtr rfcHandle, out RFC_ERROR_INFO errorInfo);

	[DllImport(NW_RFC_LIB, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public static extern RFC_RC RfcInvoke(IntPtr rfcHandle, IntPtr funcHandle, out RFC_ERROR_INFO errorInfo);

	[DllImport(NW_RFC_LIB, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	internal static extern RFC_RC RfcPing(IntPtr rfcHandle, out RFC_ERROR_INFO errorInfo);

	[DllImport(NW_RFC_LIB, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	internal static extern RFC_RC RfcIsConnectionHandleValid(IntPtr rfcHandle, ref int isValid, out RFC_ERROR_INFO errorInfo);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	internal struct RFC_CONNECTION_PARAMETER
	{
		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Value;
	}
}