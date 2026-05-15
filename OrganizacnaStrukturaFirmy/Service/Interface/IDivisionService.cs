using OrganizacnaStrukturaFirmy.DTOs.Division;

namespace OrganizacnaStrukturaFirmy.Service.Interface
{
    public interface IDivisionService
    {
        Task<IEnumerable<DivisionDto>> GetAllAsync();
        Task<IEnumerable<DivisionDto>> GetByCompanyAsync(int companyId);
        Task<DivisionDto?> GetByIdAsync(int id);
        Task<DivisionDto> CreateAsync(CreateDivisionDto dto);
        Task<DivisionDto> UpdateAsync(int id, UpdateDivisionDto dto);
        Task DeleteAsync(int id);
    }
}
