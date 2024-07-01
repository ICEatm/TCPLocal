using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TCPLocal.Client.Controllers;
using TCPLocal.Client.Helper;
using TCPLocal.Client.Models;

/// <summary>
/// Main entry point of the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the program.</param>
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<TcpClientController>>();
        var config = ConfigurationHelper.LoadConfig("config.yaml", logger);

        if (config != null)
        {
            UserInformationModel userInformation = new UserInformationModel
            {
                UserName = Environment.UserName,
                MachineName = Environment.MachineName,
                DomainName = Environment.UserDomainName,
                Is64BitOs = Environment.Is64BitOperatingSystem,
                ExternalIp = UserInformationHelper.GetExternalIp(),
                OperatingSystem = Environment.OSVersion.VersionString,
                LocalIp = UserInformationHelper.GetEthernetIpAddress(),
            };

            TcpClientModel clientModel = new TcpClientModel(config.Server.Ip, config.Server.Port);
            TcpClientController clientController = new TcpClientController(clientModel, config.Client.MessageSendInterval, userInformation, logger);
            clientController.Start();
        }
    }

    /// <summary>
    /// Configures services for dependency injection.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
                .AddTransient<TcpClientController>();
    }
}
