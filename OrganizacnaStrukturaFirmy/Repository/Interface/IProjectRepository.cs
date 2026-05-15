using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Repository.Interface
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByDivisionAsync(int divisionId);
        Task<Project?> GetByIdAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<bool> HasDepartmentsAsync(int projectId);
        Task<Project> CreateAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
