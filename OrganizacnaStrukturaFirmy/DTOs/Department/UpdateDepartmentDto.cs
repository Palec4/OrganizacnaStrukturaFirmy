namespace OrganizacnaStrukturaFirmy.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
    }
}
