using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace TCPLocal.Client.Models
{
    /// <summary>
    /// Represents a TCP client model for managing TCP connections and communications.
    /// </summary>
    public class TcpClientModel
    {
        /// <summary>
        /// Gets the TCP client.
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Gets the network stream associated with the TCP client.
        /// </summary>
        public NetworkStream Stream { get; private set; }

        /// <summary>
        /// Gets the server IP address.
        /// </summary>
        public string ServerIp { get; private set; }

        /// <summary>
        /// Gets the server port number.
        /// </summary>
        public int ServerPort { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client is connected.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Gets the unique identifier for the client.
        /// </summary>
        public string ClientGuid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientModel"/> class with the specified server IP address and port number.
        /// </summary>
        /// <param name="ip">The server IP address.</param>
        /// <param name="port">The server port number.</param>
        public TcpClientModel(string ip, int port)
        {
            ServerIp = ip;
            ServerPort = port;
            IsConnected = false;
            ClientGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Connects to the server.
        /// </summary>
        public void Connect()
        {
            if (!IsConnected)
            {
                Client = new TcpClient(); // Initialize the TcpClient
                Client.Connect(ServerIp, ServerPort); // Connect to the server
                Stream = Client.GetStream(); // Get the network stream for reading and writing
                IsConnected = true; // Set the connection status to true
            }
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                Stream.Close(); // Close the network stream
                Client.Close(); // Close the TCP client
                IsConnected = false; // Set the connection status to false
            }
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(object data)
        {
            if (IsConnected && Stream != null)
            {
                string jsonData = JsonConvert.SerializeObject(data);
                byte[] jsonBytes = Encoding.ASCII.GetBytes(jsonData);

                Stream.Write(jsonBytes, 0, jsonBytes.Length);
            }
        }

        /// <summary>
        /// Receives a message from the server.
        /// </summary>
        /// <returns>The message received from the server.</returns>
        public string ReceiveMessage()
        {
            if (IsConnected && Stream != null)
            {
                byte[] buffer = new byte[1024]; // Buffer to store the received data
                int bytesRead = Stream.Read(buffer, 0, buffer.Length); // Read the data from the network stream
                return Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert the byte array to a string and return
            }
            return null!;
        }
    }
}
