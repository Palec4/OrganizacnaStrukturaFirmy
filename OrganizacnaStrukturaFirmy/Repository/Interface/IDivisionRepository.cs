using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Repository.Interface
{
    public interface IDivisionRepository
    {
        Task<IEnumerable<Division>> GetAllAsync();
        Task<IEnumerable<Division>> GetByCompanyAsync(int companyId);
        Task<Division?> GetByIdAsync(int id);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<Division> CreateAsync(Division division);
        Task<Division> UpdateAsync(Division division);
        Task DeleteAsync(Division division);
        Task<bool> HasProjectsAsync(int divisionId);
    }
}
