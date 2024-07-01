using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Extensions.Logging;
using TCPLocal.Client.Extensions;
using YamlDotNet.Serialization;
using TCPLocal.Client.Models;

namespace TCPLocal.Client.Helper
{
    /// <summary>
    /// Helper class for loading configuration from YAML files.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Loads configuration from a YAML file.
        /// </summary>
        /// <param name="filePath">The path to the YAML file.</param>
        /// <param name="logger">The logger instance for logging errors.</param>
        /// <returns>The loaded configuration model or null if an error occurs.</returns>
        public static ConfigModel LoadConfig(string filePath, ILogger logger)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var config = deserializer.Deserialize<ConfigModel>(reader);
                    return config;
                }
            }
            catch (Exception ex)
            {
                logger.LogErrorWithoutStackTrace($"Error loading configuration file from path '{filePath}'", ex);
                return null!;
            }
        }
    }
}
