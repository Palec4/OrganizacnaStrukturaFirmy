namespace OrganizacnaStrukturaFirmy.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public int? DepartmentId { get; set; }
        public string? Title { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
