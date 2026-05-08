namespace OrganizacnaStrukturaFirmy.DTOs.Project
{
    public class UpdateProjectDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
    }
}
