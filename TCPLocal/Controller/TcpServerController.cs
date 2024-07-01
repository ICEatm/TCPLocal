using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TCPLocal.Server.Models;

namespace TCPLocal.Server.Controllers
{
    /// <summary>
    /// Represents the TCP server controller for managing TCP connections and communications.
    /// </summary>
    public class TcpServerController
    {
        private TcpListener listener;
        private bool isRunning;
        private string serverGuid;
        private Dictionary<string, ClientInfoModel> connectedClients = new Dictionary<string, ClientInfoModel>();
        private HashSet<string> connectedClientGuids = new HashSet<string>();
        private Action<string> _removeClientFromListView;
        private Action<string> _addClientToListView;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServerController"/> class with the specified server configuration.
        /// </summary>
        /// <param name="serverConfig">The server configuration.</param>
        public TcpServerController(ServerConfig serverConfig, Action<string> addClientToListView, Action<string> removeClientFromListView)
        {
            listener = new TcpListener(IPAddress.Parse(serverConfig.Ip), serverConfig.Port);
            serverGuid = Guid.NewGuid().ToString();
            _addClientToListView = addClientToListView;
            _removeClientFromListView = removeClientFromListView;
        }

        /// <summary>
        /// Starts the TCP server.
        /// </summary>
        public void Start()
        {
            listener.Start();
            isRunning = true;
            Console.WriteLine($"Server started with GUID: {serverGuid}");

            while (isRunning)
            {
                if (listener.Pending())
                {
                    var client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(() => HandleClient(client))
                    {
                        IsBackground = true
                    };
                    clientThread.Start();
                }

                RemoveInactiveClients();
                Thread.Sleep(100); // Add a small delay to avoid busy waiting
            }
        }

        /// <summary>
        /// Handles a connected client.
        /// </summary>
        /// <param name="client">The connected TCP client.</param>
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                // Receive client's GUID
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string clientGuid = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client connected with GUID: {clientGuid}");

                lock (connectedClients)
                {
                    connectedClients[clientGuid] = new ClientInfoModel(client, DateTime.Now);
                }

                // Send server's GUID to the client
                byte[] guidMessage = Encoding.ASCII.GetBytes(serverGuid);
                stream.Write(guidMessage, 0, guidMessage.Length);

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from {clientGuid}: {message}");

                    // Execute _updateClientListAction only once per client
                    if (connectedClientGuids.Add(clientGuid))
                    {
                        _addClientToListView?.Invoke(message);
                    }

                    lock (connectedClients)
                    {
                        connectedClients[clientGuid].LastActivity = DateTime.Now;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Client disconnected. Error: {e.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        /// <summary>
        /// Removes inactive clients.
        /// </summary>
        private void RemoveInactiveClients()
        {
            lock (connectedClients)
            {
                var inactiveClients = new List<string>();
                foreach (var client in connectedClients)
                {
                    if ((DateTime.Now - client.Value.LastActivity).TotalSeconds > 10)
                    {
                        inactiveClients.Add(client.Key);
                    }
                }

                foreach (var clientGuid in inactiveClients)
                {
                    Console.WriteLine($"Removing inactive client: {clientGuid}");
                    connectedClients[clientGuid].Client.Close();
                    connectedClients.Remove(clientGuid);

                    _removeClientFromListView?.Invoke(clientGuid);
                }
            }
        }

        /// <summary>
        /// Stops the TCP server.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            listener.Stop();

            // Close all connected clients
            lock (connectedClients)
            {
                foreach (var client in connectedClients.Values)
                {
                    client.Client.Close();
                }
                connectedClients.Clear();
            }
        }
    }
}
