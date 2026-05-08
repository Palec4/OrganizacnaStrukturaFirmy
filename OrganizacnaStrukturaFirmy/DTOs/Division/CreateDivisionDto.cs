namespace OrganizacnaStrukturaFirmy.DTOs.Division
{
    public class CreateDivisionDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }
    }
}
