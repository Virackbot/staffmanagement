using System.Text.Json;

namespace Domain.Helpers;

public class HttpClientInfo
{
    public string? IpAddress { get; set; }
    public string? MacAddress { get; set; }
    public string? HostName { get; set; }
    public string? OSVersion { get; set; }
    public string? UserAgent { get; set; }
    public string? Version { get; set; }
    public DateTime Time { get; set; }
    public string ToJson() => JsonSerializer.Serialize(this);
}
