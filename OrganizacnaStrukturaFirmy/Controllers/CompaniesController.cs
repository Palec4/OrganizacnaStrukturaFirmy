using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Company;
using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.DTOs.Employee;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Service.Interface;


namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            return Ok(company);
        }
        // POST: api/companies
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto) 
        {
            try
            {
                var company = await _companyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT: api/companies/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDto>> Update(int id, UpdateCompanyDto dto)
        {
            try
            {
                var company = await _companyService.UpdateAsync(id, dto);
                return Ok(company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _companyService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
