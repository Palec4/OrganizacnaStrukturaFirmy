namespace OrganizacnaStrukturaFirmy.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }


        public Employee? Manager { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }
}
