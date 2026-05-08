namespace OrganizacnaStrukturaFirmy.DTOs.Division
{
    public class UpdateDivisionDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
    }
}
