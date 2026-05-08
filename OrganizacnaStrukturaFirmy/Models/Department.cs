namespace OrganizacnaStrukturaFirmy.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int? ManagerId { get; set; }

        public Project? Project { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
