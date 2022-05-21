namespace BdTracker.Shared.Models.Request;

public record UpdateUserRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDay { get; set; }
    public string PositionName { get; set; }
    public string CompanyName { get; set; }
}
