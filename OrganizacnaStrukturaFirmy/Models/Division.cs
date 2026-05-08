namespace OrganizacnaStrukturaFirmy.Models
{
    public class Division
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }


        public Company? Company { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
