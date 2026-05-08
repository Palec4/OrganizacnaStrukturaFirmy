namespace OrganizacnaStrukturaFirmy.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Title { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation properties
        public Company? Company { get; set; }
        public Department? Department { get; set; }
    }
}
