using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByCompanyAsync(int companyId);
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
    }
}
