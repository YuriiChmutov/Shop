namespace BuildingBlocks.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }

    public BadRequestException(string name, string details) : base(name)
    {
        Details = details;
    }

    public string? Details { get; }
}