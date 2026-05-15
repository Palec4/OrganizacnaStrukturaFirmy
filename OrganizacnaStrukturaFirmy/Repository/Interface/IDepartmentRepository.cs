using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<IEnumerable<Department>> GetByProjectAsync(int projectId);
        Task<Department?> GetByIdAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<Department> CreateAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task DeleteAsync(Department department);
    }
}
