namespace Domain.Models;

public class HttpClientInfo
{
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
}