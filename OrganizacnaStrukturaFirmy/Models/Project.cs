namespace OrganizacnaStrukturaFirmy.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DivisionId { get; set; }
        public int? ManagerId { get; set; }


        public Division? Division { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}
