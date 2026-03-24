using NwRfcNetCore.Interop;
using NwRfcNetCore.RfcTypes;
using NwRfcNetCore.TypeMapper;

namespace NwRfcNetCore;

/// <summary>
/// RFC Output paramter handler
/// </summary>
internal class RfcParameterOutput : RfcParameter
{
	private readonly RfcMapper _mapper;

	internal RfcParameterOutput(RfcMapper mapper) =>
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

	/// <summary>
	/// Get Return Parameters from RFC
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	internal object GetReturnParameters(IntPtr handler, Type type)
	{
		var returnValue = Activator.CreateInstance(type)
			?? throw new InvalidOperationException($"Could not create instance of type {type}");

		foreach (var propMap in _mapper[type])
		{
			var map = propMap.Value;

			var prop = returnValue.GetType().GetProperty(map.PropertyName)
				?? throw new InvalidOperationException($"Could not find property {map.PropertyName} on {returnValue.GetType()}");

			object? objectValue = null;

			switch (map.ParameterType)
			{
				case RfcFieldType.Char:
					objectValue = RfcChar.GetFieldValue(handler, map.RfcParameterName, map.Length)?.RfcValue;
					break;

				case RfcFieldType.Int:
					objectValue = GetInt(handler, map);
					break;

				case RfcFieldType.Table:
					objectValue = GetTable(handler, map, prop.PropertyType);
					break;

				case RfcFieldType.Structure:
					var structHandle = GetStructure(handler, map);
					objectValue = GetReturnParameters(structHandle, map.PropertyType);
					break;

				case RfcFieldType.Date:
					objectValue = RfcDate.GetFieldValue(handler, map.RfcParameterName)?.RfcValue;
					break;

				case RfcFieldType.Time:
					objectValue = RfcTime.GetFieldValue(handler, map.RfcParameterName)?.RfcValue;
					break;

				case RfcFieldType.Int8:
					objectValue = RfcInt8.GetFieldValue(handler, map.RfcParameterName).RfcValue;
					break;

				case RfcFieldType.Bcd:
					objectValue = RfcBcd.GetFieldValue(handler, map.RfcParameterName).RfcValue;
					break;

				case RfcFieldType.String:
					objectValue = RfcString.GetFieldValue(handler, map.RfcParameterName)?.RfcValue;
					break;

				case RfcFieldType.Byte:
					objectValue = RfcXString.GetFieldValue(handler, map.RfcParameterName)?.RfcValue;
					break;

				default:
					throw new RfcException("Rfc Type not handled");
			}

			if (objectValue != null)
				prop.SetValue(returnValue, objectValue);
		}

		return returnValue;
	}

	/// <summary>
	///  Returns the value of the specified field. as INT32
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="map"></param>
	/// <returns></returns>
	private static int GetInt(IntPtr handler, PropertyMap map)
	{
		var rc = RfcInterop.RfcGetInt(handler, map.RfcParameterName, out var intValue, out var errorInfo);
		rc.OnErrorThrowException(errorInfo);
		return intValue;
	}

	/// <summary>
	/// Returns the value of the specified field as a pointer to a RFCTYPE_TABLE
	/// </summary>
	/// <param name="handler"></param>
	/// <param name="map"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	private System.Array GetTable(IntPtr handler, PropertyMap map, Type t)
	{
		if (!t.IsArray)
			throw new RfcException($"Type {t.FullName} is not array");

		var rc = RfcInterop.RfcGetTable(handler, map.RfcParameterName, out IntPtr tableHandle, out var errorInfo);
		rc.OnErrorThrowException(errorInfo);

		rc = RfcInterop.RfcGetRowCount(tableHandle, out var rowCount, out errorInfo);
		rc.OnErrorThrowException(errorInfo);

		var elementType = t.GetElementType() ?? throw new InvalidOperationException($"Could not get element type of array {t}");

		var arr = Array.CreateInstance(elementType, rowCount);
		for (var i = 0; i < rowCount; i++)
		{
			rc = RfcInterop.RfcMoveTo(tableHandle, (uint)i, out errorInfo);
			rc.OnErrorThrowException(errorInfo);

			var rowHandle = RfcInterop.RfcGetCurrentRow(tableHandle, out errorInfo);
			rc.OnErrorThrowException(errorInfo);

			var value = GetReturnParameters(rowHandle, elementType);
			arr.SetValue(value, i);
		}

		return arr;
	}
}
