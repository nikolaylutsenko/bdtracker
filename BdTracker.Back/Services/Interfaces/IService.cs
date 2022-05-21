using BdTracker.Shared.Entities;

namespace BdTracker.Back.Services.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(string id);
        Task AddAsync(T item);
        Task UpdateAsync(T item);
    }
}