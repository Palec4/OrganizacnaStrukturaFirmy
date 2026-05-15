using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;

namespace OrganizacnaStrukturaFirmy.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
            => await _context.Companies.Include(c => c.Manager).ToListAsync();

        public async Task<Company?> GetByIdAsync(int id)
            => await _context.Companies.Include(c => c.Manager).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
            => await _context.Companies.AnyAsync(c => c.Code == code && c.Id != excludeId);

        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
