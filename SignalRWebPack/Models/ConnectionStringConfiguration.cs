namespace AspDotNetCore.RestFull.Api.Models
{
    public class ConnectionStringConfiguration
    {
        public string Default { get; set; }
        public int? CommandTimeoutInSeconds { get; set; }
    }
}
