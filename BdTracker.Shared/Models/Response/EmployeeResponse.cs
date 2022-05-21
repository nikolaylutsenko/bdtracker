using BdTracker.Shared.Entities;

namespace BdTracker.Shared.Models.Response;

public record EmployeeResponse
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PositionName { get; set; }
    public string? CompanyId { get; set; }
    public Sex Sex { get; set; }
    public DateTime BirthDay { get; set; }
}