using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Service.implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDivisionRepository _divisionRepository;

        public DepartmentService(IDepartmentRepository departmentRepository, IProjectRepository projectRepository, IEmployeeRepository employeeRepository, IDivisionRepository divisionRepository)
        {
            _departmentRepository = departmentRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _divisionRepository = divisionRepository;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(MapToDto);
        }

        public async Task<IEnumerable<DepartmentDto>> GetByProjectAsync(int projectId)
        {
            var departments = await _departmentRepository.GetByProjectAsync(projectId);
            return departments.Select(MapToDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return null;
            return MapToDto(department);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
            if (project == null)
                throw new ArgumentException($"Project with id {dto.ProjectId} not found.");

            if (await _departmentRepository.CodeExistsAsync(dto.Code))
                throw new ArgumentException($"Department with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager == null)
                    throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

                var division = await _projectRepository.GetByIdAsync(project.DivisionId);
                if (manager.CompanyId != division!.DivisionId)
                    throw new ArgumentException("Manager must be an employee of the company.");
            }

            var department = new Department
            {
                Code = dto.Code,
                Name = dto.Name,
                ProjectId = dto.ProjectId,
                ManagerId = dto.ManagerId
            };

            var created = await _departmentRepository.CreateAsync(department);
            return MapToDto(created);
        }

        public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException($"Department with id {id} not found.");

            if (await _departmentRepository.CodeExistsAsync(dto.Code, id))
                throw new ArgumentException($"Department with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager == null)
                    throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

                var project = await _projectRepository.GetByIdAsync(department.ProjectId);
                var division = await _divisionRepository.GetByIdAsync(project!.DivisionId);
                if (manager.CompanyId != division!.CompanyId)
                    throw new ArgumentException("Manager must be an employee of the company.");
            }

            department.Code = dto.Code;
            department.Name = dto.Name;
            department.ManagerId = dto.ManagerId;

            var updated = await _departmentRepository.UpdateAsync(department);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException($"Department with id {id} not found.");

            await _departmentRepository.DeleteAsync(department);
        }

        private static DepartmentDto MapToDto(Department department) => new()
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            ProjectId = department.ProjectId,
            ManagerId = department.ManagerId
        };
    }
}
