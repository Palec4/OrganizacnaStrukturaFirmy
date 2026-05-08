namespace OrganizacnaStrukturaFirmy.DTOs.Company
{
    public class CreateCompanyDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
    }
}
