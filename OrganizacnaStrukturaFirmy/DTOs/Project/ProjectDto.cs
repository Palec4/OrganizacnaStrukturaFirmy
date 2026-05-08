namespace OrganizacnaStrukturaFirmy.DTOs.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DivisionId { get; set; }
        public int? ManagerId { get; set; }
    }
}
