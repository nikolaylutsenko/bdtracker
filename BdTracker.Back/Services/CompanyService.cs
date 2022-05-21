using AutoMapper;
using BdTracker.Back.Data;
using BdTracker.Shared.Entities;
using BdTracker.Back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BdTracker.Back.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CompanyService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id);

            if (company != null)
                throw new Exception($"Company with id {id} not found");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetAsync(string id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
                throw new Exception($"Company with id {id} not found");

            return company;
        }

        public async Task<Company> GetByOwnerIdAsync(string ownerId)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyOwnerId == ownerId);

            if (company == null)
                throw new Exception($"Company with provided owner id {ownerId} not found");

            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            var oldCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);

            if (oldCompany == null)
                throw new Exception($"Company with id {company.Id} not exist");

            var updatedCompany = _mapper.Map(company, oldCompany);

            _context.Companies.Update(updatedCompany);
            await _context.SaveChangesAsync();
        }
    }
}
