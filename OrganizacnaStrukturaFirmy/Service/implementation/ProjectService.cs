using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Service.implementation
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ProjectService(IProjectRepository projectRepository, IDivisionRepository divisionRepository, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _divisionRepository = divisionRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(MapToDto);
        }

        public async Task<IEnumerable<ProjectDto>> GetByDivisionAsync(int divisionId)
        {
            var projects = await _projectRepository.GetByDivisionAsync(divisionId);
            return projects.Select(MapToDto);
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;
            return MapToDto(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            var division = await _divisionRepository.GetByIdAsync(dto.DivisionId);
            if (division == null)
                throw new ArgumentException($"Division with id {dto.DivisionId} not found.");

            if (await _projectRepository.CodeExistsAsync(dto.Code))
                throw new ArgumentException($"Project with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager == null)
                    throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

                if (manager.CompanyId != division.CompanyId)
                    throw new ArgumentException("Manager must be an employee of the company.");
            }

            var project = new Project
            {
                Code = dto.Code,
                Name = dto.Name,
                DivisionId = dto.DivisionId,
                ManagerId = dto.ManagerId
            };

            var created = await _projectRepository.CreateAsync(project);
            return MapToDto(created);
        }

        public async Task<ProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            if (!dto.ManagerId.HasValue)
                throw new ArgumentException("ManagerId is required when updating a project.");

            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new KeyNotFoundException($"Project with id {id} not found.");

            if (await _projectRepository.CodeExistsAsync(dto.Code, id))
                throw new ArgumentException($"Project with code '{dto.Code}' already exists.");

            var division = await _divisionRepository.GetByIdAsync(project.DivisionId);

            var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != division!.CompanyId)
                throw new ArgumentException("Manager must be an employee of the company.");

            project.Code = dto.Code;
            project.Name = dto.Name;
            project.ManagerId = dto.ManagerId;

            var updated = await _projectRepository.UpdateAsync(project);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new KeyNotFoundException($"Project with id {id} not found.");

            if (await _projectRepository.HasDepartmentsAsync(id))
                throw new InvalidOperationException("Cannot delete project that has departments.");

            await _projectRepository.DeleteAsync(project);
        }

        private static ProjectDto MapToDto(Project project) => new()
        {
            Id = project.Id,
            Code = project.Code,
            Name = project.Name,
            DivisionId = project.DivisionId,
            ManagerId = project.ManagerId
        };
    }
}
