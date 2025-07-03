namespace Domain.Exceptions;

/// <summary>
/// This exception throw by developer
/// </summary>
public class IntentionalException : Exception
{
    public IntentionalException(string msg) : base(msg)
    {
    }
}

/// <summary>
/// Known exception for logic error. eg. ValidationException, UniqueException etc.
/// </summary>
public class LogicException : Exception
{
    public string? Code { get; set; }
    public object? Model { get; set; }

    protected string? _message;
    public LogicException()
    {
    }

    public LogicException(string message) : base(message)
    {
        _message = message;
    }

    public LogicException(string message, object? model)
        : this(message) => Model = model;

    public override string Message => _message ?? string.Empty;
}
