using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TCPLocal.Client.Helper
{
    public static class UserInformationHelper
    {
        /// <summary>
        /// Gets the local Ethernet IP address.
        /// </summary>
        /// <returns>The local Ethernet IP address as a string, or "n/a" if not found.</returns>
        public static string GetEthernetIpAddress()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                                n.OperationalStatus == OperationalStatus.Up);

                foreach (var networkInterface in networkInterfaces)
                {
                    var ipProperties = networkInterface.GetIPProperties();
                    var unicastAddresses = ipProperties.UnicastAddresses;

                    foreach (var unicastAddress in unicastAddresses)
                    {
                        if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return unicastAddress.Address.ToString();
                        }
                    }
                }

                return "n/a"; // Return "n/a" if no Ethernet IP address found
            }
            catch (Exception)
            {
                return "n/a"; // Return "n/a" if any exception occurs
            }
        }

        /// <summary>
        /// Fetches the external IP address from a web service synchronously.
        /// </summary>
        /// <returns>The external IP address as a string, or "n/a" if fetching fails.</returns>
        public static string GetExternalIp()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync("https://www.myexternalip.com/raw").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        return content.Trim();
                    }
                    else
                    {
                        return "n/a";
                    }
                }
            }
            catch (Exception)
            {
                return "n/a";
            }
        }
    }
}
