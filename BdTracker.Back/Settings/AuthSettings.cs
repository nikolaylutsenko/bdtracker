namespace BdTracker.Back.Settings;
public interface IAuthSettings
{
    string? Key { get; init; }
    string? Issuer { get; set; }
    string? Audience { get; set; }
}

public record AuthSettings : IAuthSettings
{
    public const string SectionName = "Authentication";
    public string? Key { get; init; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}