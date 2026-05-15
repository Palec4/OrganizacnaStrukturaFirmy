using OrganizacnaStrukturaFirmy.DTOs.Department;

namespace OrganizacnaStrukturaFirmy.Service.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<IEnumerable<DepartmentDto>> GetByProjectAsync(int projectId);
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
