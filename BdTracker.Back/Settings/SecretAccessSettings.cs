namespace BdTracker.Back.Settings;

public interface ISecretAccessSettings
{
    string? Key { get; init; }
    string? Value { get; init; }
}
public class SecretAccessSettings : ISecretAccessSettings
{
    public const string SectionName = "SecretAccess";
    public string? Key { get; init; }
    public string? Value { get; init; }
}