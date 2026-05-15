using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;

namespace OrganizacnaStrukturaFirmy.Repository.Implementation
{
    public class DivisionRepository : IDivisionRepository
    {
        private readonly AppDbContext _context;

        public DivisionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Division>> GetAllAsync()
            => await _context.Divisions.ToListAsync();

        public async Task<IEnumerable<Division>> GetByCompanyAsync(int companyId)
            => await _context.Divisions.Where(d => d.CompanyId == companyId).ToListAsync();

        public async Task<Division?> GetByIdAsync(int id)
            => await _context.Divisions.FindAsync(id);

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
            => await _context.Divisions.AnyAsync(d => d.Code == code && d.Id != excludeId);

        public async Task<Division> CreateAsync(Division division)
        {
            _context.Divisions.Add(division);
            await _context.SaveChangesAsync();
            return division;
        }

        public async Task<Division> UpdateAsync(Division division)
        {
            await _context.SaveChangesAsync();
            return division;
        }

        public async Task DeleteAsync(Division division)
        {
            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasProjectsAsync(int divisionId)
            => await _context.Projects.AnyAsync(p => p.DivisionId == divisionId);
    }
}
