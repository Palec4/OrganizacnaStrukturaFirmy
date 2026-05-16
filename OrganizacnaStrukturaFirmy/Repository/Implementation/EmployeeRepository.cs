using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;

namespace OrganizacnaStrukturaFirmy.Repository.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
            => await _context.Employees.ToListAsync();

        public async Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId)
            => await _context.Employees.Where(e => e.CompanyId == companyId).ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsManagerAnywhereAsync(int employeeId)
    => await _context.Companies.AnyAsync(c => c.ManagerId == employeeId) ||
       await _context.Divisions.AnyAsync(d => d.ManagerId == employeeId) ||
       await _context.Projects.AnyAsync(p => p.ManagerId == employeeId) ||
       await _context.Departments.AnyAsync(d => d.ManagerId == employeeId);
    }
}
