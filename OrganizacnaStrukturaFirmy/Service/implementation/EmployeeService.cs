using OrganizacnaStrukturaFirmy.DTOs.Employee;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Repository.Interface;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Service.implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByCompanyAsync(int companyId)
        {
            var employees = await _employeeRepository.GetByCompanyAsync(companyId);
            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("FirstName is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("LastName is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new ArgumentException("Phone is required.");

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);
            if (company == null)
                throw new ArgumentException($"Company with id {dto.CompanyId} not found.");

            var employee = new Employee
            {
                CompanyId = dto.CompanyId,
                Title = dto.Title,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email
            };

            var created = await _employeeRepository.CreateAsync(employee);
            return MapToDto(created);
        }

        public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new ArgumentException("FirstName is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ArgumentException("LastName is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new ArgumentException("Phone is required.");

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with id {id} not found.");

            employee.Title = dto.Title;
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;

            var updated = await _employeeRepository.UpdateAsync(employee);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with id {id} not found.");

            await _employeeRepository.DeleteAsync(employee);
        }

        private static EmployeeDto MapToDto(Employee employee) => new()
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            Title = employee.Title,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Phone = employee.Phone,
            Email = employee.Email
        };
    }
}
