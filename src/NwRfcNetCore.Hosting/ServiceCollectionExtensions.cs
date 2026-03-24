using NwRfcNetCore;
using NwRfcNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions to add the RFC services to the <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRfcConnector(this IServiceCollection services, Action<RfcConnectionSettings> rfcConnectionSettingsSetup)
	{
		var settings = new RfcConnectionSettings();
		rfcConnectionSettingsSetup(settings);

		return AddRfcConnector(services, settings);
	}

	public static IServiceCollection AddRfcConnector(this IServiceCollection services, RfcConnectionSettings rfcConnectionSettings)
	{
		services.AddSingleton<IRfcConnectionFactory, RfcConnectionFactory>();
		services.AddSingleton(rfcConnectionSettings);
		services.AddSingleton<IRfcConnectionParameters>(rfcConnectionSettings);
		services.AddTransient(sp => sp.GetRequiredService<IRfcConnectionFactory>().CreateConnection());

		return services;
	}
}
