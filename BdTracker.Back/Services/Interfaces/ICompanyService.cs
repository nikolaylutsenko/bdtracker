using BdTracker.Shared.Entities;

namespace BdTracker.Back.Services.Interfaces;

public interface ICompanyService
{
    Task<Company> GetAsync(string id);
    Task<Company> GetByOwnerIdAsync(string ownerId);
    Task<IEnumerable<Company>> GetAllAsync();
    Task DeleteAsync(string id);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
}
