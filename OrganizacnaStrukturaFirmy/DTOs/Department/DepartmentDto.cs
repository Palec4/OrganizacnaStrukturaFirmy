namespace OrganizacnaStrukturaFirmy.DTOs.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int? ManagerId { get; set; }
    }
}
