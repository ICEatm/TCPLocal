namespace TCPLocal.Client.Models
{
    /// <summary>
    /// Represents the configuration model containing server and client configurations.
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// Gets or sets the server configuration.
        /// </summary>
        public ServerConfig Server { get; set; }

        /// <summary>
        /// Gets or sets the client configuration.
        /// </summary>
        public ClientConfig Client { get; set; }
    }

    /// <summary>
    /// Represents the server configuration.
    /// </summary>
    public class ServerConfig
    {
        /// <summary>
        /// Gets or sets the IP address of the server.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the port number of the server.
        /// </summary>
        public int Port { get; set; }
    }

    /// <summary>
    /// Represents the client configuration.
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// Gets or sets the interval (in milliseconds) at which messages are sent by the client.
        /// </summary>
        public int MessageSendInterval { get; set; }
    }
}
