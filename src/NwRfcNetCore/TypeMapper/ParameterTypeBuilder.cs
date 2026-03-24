using System.Linq.Expressions;
using System.Reflection;
using NwRfcNetCore.Util;

namespace NwRfcNetCore.TypeMapper;

public class ParameterTypeBuilder<TEntity>
{
	private readonly PropertyInternalStorage _internalStorage;

	internal ParameterTypeBuilder(PropertyInternalStorage internalStorage)
		=> _internalStorage = internalStorage;

	public PropertyBuilder Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
		=> new PropertyBuilder(GetMappingOrAdd(propertyExpression));

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="propertyLambda"></param>
	/// <returns></returns>
	private PropertyMap GetMappingOrAdd<TProperty>(Expression<Func<TEntity, TProperty>> propertyLambda)
	{
		var prop = GetPropertyInfo(propertyLambda);
		return _internalStorage.GetOrAdd(prop.Name,
			() => new PropertyMap()
			{
				PropertyName = prop.Name,
				RfcParameterName = prop.Name.ToUpper(),
				ParameterType = null,
				PropertyType = prop.PropertyType
			}
		);
	}

	// ideia from : https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
	/// <summary>
	/// Retive property name from a lambda expression
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="propertyLambda"></param>
	/// <returns></returns>
	private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
	{
		ArgumentNullException.ThrowIfNull(propertyLambda);

		if (!(propertyLambda.Body is MemberExpression member))
			throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

		var propInfo = member.Member as PropertyInfo
			?? throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

		if (propInfo.ReflectedType == null)
			throw new ArgumentException($"Expression '{propertyLambda}' does not return a reflected type.");

		var type = typeof(TEntity);
		if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
			throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

		return propInfo;
	}
}
