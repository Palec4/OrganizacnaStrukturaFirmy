using OrganizacnaStrukturaFirmy.DTOs.Company;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Service.implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public CompanyService(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                ManagerId = c.ManagerId
            });
        }

        public async Task<CompanyDto?> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return null;

            return new CompanyDto
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                ManagerId = company.ManagerId
            };
        }

        public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            if (await _companyRepository.CodeExistsAsync(dto.Code))
                throw new ArgumentException($"Company with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
                if (manager == null)
                    throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");
            }

            var company = new Company
            {
                Code = dto.Code,
                Name = dto.Name,
                ManagerId = dto.ManagerId
            };

            var created = await _companyRepository.CreateAsync(company);
            return new CompanyDto
            {
                Id = created.Id,
                Code = created.Code,
                Name = created.Name,
                ManagerId = created.ManagerId
            };
        }

        public async Task<CompanyDto> UpdateAsync(int id, UpdateCompanyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            if (!dto.ManagerId.HasValue)
                throw new ArgumentException("ManagerId is required when updating a company.");

            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                throw new KeyNotFoundException($"Company with id {id} not found.");

            if (await _companyRepository.CodeExistsAsync(dto.Code, id))
                throw new ArgumentException($"Company with code '{dto.Code}' already exists.");

            var manager = await _employeeRepository.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new ArgumentException($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != id)
                throw new ArgumentException("Manager must be an employee of this company.");

            company.Code = dto.Code;
            company.Name = dto.Name;
            company.ManagerId = dto.ManagerId;

            var updated = await _companyRepository.UpdateAsync(company);
            return new CompanyDto
            {
                Id = updated.Id,
                Code = updated.Code,
                Name = updated.Name,
                ManagerId = updated.ManagerId
            };
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                throw new KeyNotFoundException($"Company with id {id} not found.");

            await _companyRepository.DeleteAsync(company);
        }
    }
}
