namespace BdTracker.Shared.Entities;

public record Company
{
    public string Id { get; set; }
    public string Name { get; init; }
    public string CompanyOwnerId { get; set; }

    // navigation prop
    public ICollection<AppUser> Employees { get; set; } = new List<AppUser>();
}
