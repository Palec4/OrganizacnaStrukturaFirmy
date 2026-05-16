using Microsoft.AspNetCore.Mvc;
using OrganizacnaStrukturaFirmy.DTOs.Company;
using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.DTOs.Employee;
using OrganizacnaStrukturaFirmy.Service.implementation;
using OrganizacnaStrukturaFirmy.Service.Interface;


namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IDivisionService _divisionService;

        public CompaniesController(ICompanyService companyService, IEmployeeService employeeService, IDivisionService divisionService)
        {
            _companyService = companyService;
            _employeeService = employeeService;
            _divisionService = divisionService;
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
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // GET: api/companies/5/employees
        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            var employees = await _employeeService.GetByCompanyAsync(id);
            return Ok(employees);
        }

        // GET: api/companies/5/divisions
        [HttpGet("{id}/divisions")]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetByCompany(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
                return NotFound($"Company with id {id} not found.");

            var divisions = await _divisionService.GetByCompanyAsync(id);
            return Ok(divisions);
        }
    }
}
