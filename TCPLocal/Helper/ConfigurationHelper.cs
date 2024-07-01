using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using TCPLocal.Server.Models;

namespace TCPLocal.Server.Helper
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Loads the configuration from the specified YAML file.
        /// </summary>
        /// <param name="filePath">The path to the YAML configuration file.</param>
        /// <returns>The deserialized <see cref="ConfigModel"/> object.</returns>
        public static ConfigModel LoadConfig(string filePath)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                using (var reader = new StreamReader(filePath))
                {
                    return deserializer.Deserialize<ConfigModel>(reader);
                }
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
