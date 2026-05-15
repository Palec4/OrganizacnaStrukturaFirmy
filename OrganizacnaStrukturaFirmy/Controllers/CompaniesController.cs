using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Company;
using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.DTOs.Employee;
using OrganizacnaStrukturaFirmy.Models;


namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _context;

        public CompaniesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var companies = await _context.Companies
                .Include(c => c.Manager)
                .ToListAsync();

            return Ok(companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                ManagerId = c.ManagerId
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(int id)
        {
            var company = await _context.Companies
             .Include(c => c.Manager)
             .FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound($"Company with id {id} not found");
            }

            return Ok(new CompanyDto
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                ManagerId = company.ManagerId
            });
        }
        // POST: api/companies
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto) 
        { 
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");
            if (await _context.Companies.AnyAsync(c => c.Code == dto.Code))
                return BadRequest($"Company with code '{dto.Code}' already exists.");
            if (dto.ManagerId.HasValue)
            { 
                var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
                if (manager == null)
                    return BadRequest($"Employee with id {dto.ManagerId.Value} not found.");
            }

            var company = new Company
            {
                Code = dto.Code,
                Name = dto.Name,
                ManagerId = dto.ManagerId
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = company.Id }, new CompanyDto
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                ManagerId = company.ManagerId
            });
        }
        // PUT: api/companies/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDto>> Update(int id, UpdateCompanyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            if (!dto.ManagerId.HasValue)
                return BadRequest("ManagerId is required when updating a company.");

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            if (await _context.Companies.AnyAsync(c => c.Code == dto.Code && c.Id != id))
                return BadRequest($"Company with code '{dto.Code}' already exists.");

            var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
            if (manager == null)
                return BadRequest($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != id)
                return BadRequest("Manager must be an employee of this company.");

            company.Code = dto.Code;
            company.Name = dto.Name;
            company.ManagerId = dto.ManagerId;

            await _context.SaveChangesAsync();

            return Ok(new CompanyDto
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                ManagerId = company.ManagerId
            });
        }

        // DELETE: api/companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/companies/5/employees
        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            var employees = await _context.Employees
                .Where(e => e.CompanyId == id)
                .ToListAsync();

            return Ok(employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                Title = e.Title,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Phone = e.Phone,
                Email = e.Email
            }));
        }

        // GET: api/companies/5/divisions
        [HttpGet("{companyId}/divisions")]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetByCompany(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                return NotFound($"Company with id {companyId} not found.");

            var divisions = await _context.Divisions
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();

            return Ok(divisions.Select(d => new DivisionDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                CompanyId = d.CompanyId,
                ManagerId = d.ManagerId
            }));
        }
    }
}
