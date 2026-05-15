using OrganizacnaStrukturaFirmy.DTOs.Company;

namespace OrganizacnaStrukturaFirmy.Service.Interface
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto?> GetByIdAsync(int id);
        Task<CompanyDto> CreateAsync(CreateCompanyDto dto);
        Task<CompanyDto> UpdateAsync(int id, UpdateCompanyDto dto);
        Task DeleteAsync(int id);

    }
}
