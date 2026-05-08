namespace OrganizacnaStrukturaFirmy.DTOs.Division
{
    public class DivisionDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }
    }
}
