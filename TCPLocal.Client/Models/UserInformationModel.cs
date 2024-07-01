namespace TCPLocal.Client.Models
{
    /// <summary>
    /// Represents user information including system details.
    /// </summary>
    public class UserInformationModel
    {
        /// <summary>
        /// Gets or sets the GUID of the user/client.
        /// </summary>
        public string? Guid { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the operating system name and version.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the local IP address.
        /// </summary>
        public string LocalIp { get; set; }

        /// <summary>
        /// Gets or sets the external/public IP address.
        /// </summary>
        public string ExternalIp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operating system is 64-bit.
        /// </summary>
        public bool Is64BitOs { get; set; }
    }
}
