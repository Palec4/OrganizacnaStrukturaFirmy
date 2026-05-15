using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;

namespace OrganizacnaStrukturaFirmy.Repository.Implementation
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
            => await _context.Departments.ToListAsync();

        public async Task<IEnumerable<Department>> GetByProjectAsync(int projectId)
            => await _context.Departments.Where(d => d.ProjectId == projectId).ToListAsync();

        public async Task<Department?> GetByIdAsync(int id)
            => await _context.Departments.FindAsync(id);

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
            => await _context.Departments.AnyAsync(d => d.Code == code && d.Id != excludeId);

        public async Task<Department> CreateAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
