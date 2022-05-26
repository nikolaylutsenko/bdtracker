using BdTracker.Shared.Entities;

namespace BdTracker.Shared.Models.Request;

public record UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDay { get; set; }
    public string? PositionName { get; set; }
    public string? Email { get; set; }
    public Sex Sex { get; set; }
}
