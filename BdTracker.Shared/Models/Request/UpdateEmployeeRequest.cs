namespace BdTracker.Shared.Models.Request;

public record UpdateEmployeeRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDay { get; set; }
    public string Email { get; set; }
    public string PositionName { get; set; }
    public string CompanyName { get; set; }
    public string Password { get; set; }
}
