using BdTracker.Shared.Entities;

namespace BdTracker.Shared.Models.Request;
public class RegisterOwnerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? CompanyName { get; set; }
    public DateTime Birthday { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Sex Sex { get; set; }
}