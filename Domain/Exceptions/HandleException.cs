namespace Domain.Exceptions;

public class HandleException
{
    public string? Error { get; set; }
    public string? Type { get; set; }
    public string? Stack { get; set; }
    public string? Code { get; set; }
}

public class HandleFluentException
{
    public Dictionary<string, List<string>>? Errors { get; set; }
    public string? Title { get; set; }
    public int Status { get; set; }
    public string? TraceId { get; set; }
}
