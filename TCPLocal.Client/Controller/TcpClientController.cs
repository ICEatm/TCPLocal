using Microsoft.Extensions.Logging;
using TCPLocal.Client.Extensions;
using TCPLocal.Client.Models;

namespace TCPLocal.Client.Controllers
{
    /// <summary>
    /// Manages the TCP client operations, including connection, disconnection, sending, and receiving messages.
    /// </summary>
    public class TcpClientController
    {
        private readonly UserInformationModel _userInformationModel;
        private readonly TcpClientModel _clientModel;
        private readonly int _messageSendInterval;
        private Thread? _receiveThread;
        private Thread? _sendThread;
        private readonly ILogger<TcpClientController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientController"/> class with the specified client model and logger.
        /// </summary>
        /// <param name="clientModel">The TCP client model.</param>
        /// <param name="messageSendInterval">The interval in milliseconds for sending messages.</param>
        /// <param name="logger">The logger.</param>
        public TcpClientController(TcpClientModel clientModel, int messageSendInterval, UserInformationModel userInformationModel, ILogger<TcpClientController> logger)
        {
            _clientModel = clientModel;
            _userInformationModel = userInformationModel;
            _messageSendInterval = messageSendInterval;
            _logger = logger;
        }

        /// <summary>
        /// Starts the TCP client controller by attempting to connect to the server.
        /// </summary>
        public void Start()
        {
            Console.Title = $"TCPLocal Client ...waiting for Server";
            Connect();
        }

        /// <summary>
        /// Connects to the server and starts the threads for sending and receiving data.
        /// </summary>
        private void Connect()
        {
            while (!_clientModel.IsConnected)
            {
                try
                {
                    _logger.LogInformation("Attempting to connect to server...");
                    _clientModel.Connect();
                    _logger.LogInformation("Connected to server!");

                    // Send client's GUID to server
                    _clientModel.SendMessage(_clientModel.ClientGuid);

                    // Read GUID from server
                    string serverGuid = _clientModel.ReceiveMessage();
                    _logger.LogInformation($"Received GUID from server: {serverGuid}");

                    Console.Title = $"TCPLocal Client - Connected to Server (ClientID: {_clientModel.ClientGuid})";

                    _userInformationModel.Guid = _clientModel.ClientGuid;

                    _receiveThread = new Thread(ReceiveData);
                    _receiveThread.Start();

                    _sendThread = new Thread(SendMessagePeriodically);
                    _sendThread.Start();
                }
                catch (Exception ex)
                {
                    _logger.LogErrorWithoutStackTrace("Failed to connect, retrying in 10 seconds...", ex);
                    Thread.Sleep(10000);
                }
            }
        }

        /// <summary>
        /// Periodically sends a message to the server every configured interval.
        /// </summary>
        private void SendMessagePeriodically()
        {            
            while (_clientModel.IsConnected)
            {
                try
                {
                    _clientModel.SendMessage(_userInformationModel);
                    _logger.LogInformation("Sent UserInformation to the server");
                    Thread.Sleep(_messageSendInterval);
                }
                catch (Exception ex)
                {
                    _logger.LogErrorWithoutStackTrace("Lost connection to server.", ex);
                    _clientModel.Disconnect();
                    Connect();
                }
            }
        }

        /// <summary>
        /// Continuously receives data from the server.
        /// </summary>
        private void ReceiveData()
        {
            try
            {
                while (_clientModel.IsConnected)
                {
                    string message = _clientModel.ReceiveMessage();
                    if (!string.IsNullOrEmpty(message))
                    {
                        _logger.LogInformation($"Received from server: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithoutStackTrace("Server has closed the connection.", ex);
            }
            finally
            {
                _clientModel.Disconnect();
                Connect();
            }
        }
    }
}