namespace NwRfcNetCore.TypeMapper;

/// <summary>
/// Map Properties to RFC function parameters
/// </summary>
public class PropertyMap
{
	/// <summary>
	/// RFC Field Name
	/// </summary>
	public string RfcParameterName { get; internal set; } = string.Empty;

	/// <summary>
	/// Property name
	/// </summary>
	public required string PropertyName { get; init; }

	/// <summary>
	/// RFC Field Type
	/// </summary>
	public RfcFieldType? ParameterType { get; internal set; }

	/// <summary>
	/// Type of Property
	/// </summary>
	public required Type PropertyType { get; init; }

	/// <summary>
	/// Define the RFC Field Length. 
	/// This is only required for some RFC Field Types
	/// </summary>
	public int Length { get; internal set; }

	/// <summary>
	/// Defines padding character for character type fields
	/// </summary>
	public char PaddingCharacter { get; internal set; } = ' ';

	/// <summary>
	/// Defines padding for character type fields
	/// </summary>
	public StringAlignment Alignment { get; internal set; } = StringAlignment.Right;
}
