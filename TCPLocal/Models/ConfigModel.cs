namespace TCPLocal.Server.Models
{
    /// <summary>
    /// Represents the server configuration settings.
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// Gets or sets the server configuration.
        /// </summary>
        public ServerConfig Server { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigModel"/> class.
        /// </summary>
        public ConfigModel()
        {
            Server = new ServerConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigModel"/> class with the specified server configuration.
        /// </summary>
        /// <param name="server">The server configuration.</param>
        public ConfigModel(ServerConfig server)
        {
            Server = server;
        }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class.
        /// </summary>
        public ServerConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfig"/> class with the specified IP address and port number.
        /// </summary>
        /// <param name="ip">The server IP address.</param>
        /// <param name="port">The server port number.</param>
        public ServerConfig(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}
