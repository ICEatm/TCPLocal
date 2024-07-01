using System.Net.Sockets;

namespace TCPLocal.Server.Models
{
    public class ClientRemoteData
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string DomainName { get; set; }
        public bool Is64BitOs { get; set; }
        public string ExternalIp { get; set; }
        public string OperatingSystem { get; set; }
        public string LocalIp { get; set; }
    }

    /// <summary>
    /// Represents information about a connected client.
    /// </summary>
    public class ClientInfoModel
    {
        /// <summary>
        /// Gets the TCP client.
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Gets or sets the last activity time of the client.
        /// </summary>
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInfo"/> class with the specified TCP client and last activity time.
        /// </summary>
        /// <param name="client">The TCP client.</param>
        /// <param name="lastActivity">The last activity time.</param>
        public ClientInfoModel(TcpClient client, DateTime lastActivity)
        {
            Client = client;
            LastActivity = lastActivity;
        }
    }
}
