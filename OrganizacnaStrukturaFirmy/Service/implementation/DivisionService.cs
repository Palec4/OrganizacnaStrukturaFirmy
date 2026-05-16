using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Service.implementation
{
    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public DivisionService(IDivisionRepository divisionRepository, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
        {
            _divisionRepository = divisionRepository;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<DivisionDto>> GetAllAsync()
        {
            var divisions = await _divisionRepository.GetAllAsync();
            return divisions.Select(MapToDto);
        }

        public async Task<IEnumerable<DivisionDto>> GetByCompanyAsync(int companyId)
        {
            var divisions = await _divisionRepository.GetByCompanyAsync(companyId);
            return divisions.Select(MapToDto);
        }

        public async Task<DivisionDto?> GetByIdAsync(int id)
        {
            var division = await _divisionRepository.GetByIdAsync(id);
            if (division == null) return null;
            return MapToDto(division);
        }

        public async Task<DivisionDto> CreateAsync(CreateDivisionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);
            if (company == null)
                throw new ArgumentException($"Company with id {dto.CompanyId} not found.");

            if (await _divisionRepository.CodeExistsAsync(dto.Code))
                throw new ArgumentException($"Division with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager == null)
                    throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

                if (manager.CompanyId != dto.CompanyId)
                    throw new ArgumentException("Manager must be an employee of this company.");
            }

            var division = new Division
            {
                Code = dto.Code,
                Name = dto.Name,
                CompanyId = dto.CompanyId,
                ManagerId = dto.ManagerId
            };

            var created = await _divisionRepository.CreateAsync(division);
            return MapToDto(created);
        }

        public async Task<DivisionDto> UpdateAsync(int id, UpdateDivisionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            var division = await _divisionRepository.GetByIdAsync(id);
            if (division == null)
                throw new KeyNotFoundException($"Division with id {id} not found.");

            if (await _divisionRepository.CodeExistsAsync(dto.Code, id))
                throw new ArgumentException($"Division with code '{dto.Code}' already exists.");

            var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != division.CompanyId)
                throw new ArgumentException("Manager must be an employee of this company.");

            division.Code = dto.Code;
            division.Name = dto.Name;
            division.ManagerId = dto.ManagerId;

            var updated = await _divisionRepository.UpdateAsync(division);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id)
        {
            var division = await _divisionRepository.GetByIdAsync(id);
            if (division == null)
                throw new KeyNotFoundException($"Division with id {id} not found.");

            if (await _divisionRepository.HasProjectsAsync(id))
                throw new InvalidOperationException("Cannot delete division that has projects.");

            await _divisionRepository.DeleteAsync(division);
        }

        private static DivisionDto MapToDto(Division division) => new()
        {
            Id = division.Id,
            Code = division.Code,
            Name = division.Name,
            CompanyId = division.CompanyId,
            ManagerId = division.ManagerId
        };
    }
}
