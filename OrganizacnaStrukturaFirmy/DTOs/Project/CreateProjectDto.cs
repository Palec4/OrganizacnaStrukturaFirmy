namespace OrganizacnaStrukturaFirmy.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DivisionId { get; set; }
        public int? ManagerId { get; set; }
    }
}
