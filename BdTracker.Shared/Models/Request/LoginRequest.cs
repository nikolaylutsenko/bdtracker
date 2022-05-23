namespace BdTracker.Shared.Models.Request;

public record LoginRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}