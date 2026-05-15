using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;

namespace OrganizacnaStrukturaFirmy.Repository.Implementation
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
            => await _context.Projects.ToListAsync();

        public async Task<IEnumerable<Project>> GetByDivisionAsync(int divisionId)
            => await _context.Projects.Where(p => p.DivisionId == divisionId).ToListAsync();

        public async Task<Project?> GetByIdAsync(int id)
            => await _context.Projects.FindAsync(id);

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
            => await _context.Projects.AnyAsync(p => p.Code == code && p.Id != excludeId);

        public async Task<bool> HasDepartmentsAsync(int projectId)
            => await _context.Departments.AnyAsync(d => d.ProjectId == projectId);

        public async Task<Project> CreateAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
