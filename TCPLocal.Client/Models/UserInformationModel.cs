namespace TCPLocal.Client.Models
{
    public class UserInformationModel
    {
        public string? Guid { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string DomainName { get; set; }
        public string OperatingSystem { get; set; }
        public string LocalIp { get; set; }
        public string ExternalIp { get; set; }
        public bool Is64BitOs { get; set; }
    }
}
