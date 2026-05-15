using OrganizacnaStrukturaFirmy.DTOs.Project;

namespace OrganizacnaStrukturaFirmy.Service.Interface
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<IEnumerable<ProjectDto>> GetByDivisionAsync(int divisionId);
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
    }
}
