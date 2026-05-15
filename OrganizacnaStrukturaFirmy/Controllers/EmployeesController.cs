using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Employee;
using OrganizacnaStrukturaFirmy.Models;


namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await _context.Employees.ToListAsync();

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

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }
            return Ok(new EmployeeDto
            {
                Id = employee.Id,
                CompanyId = employee.CompanyId,
                Title = employee.Title,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Email = employee.Email
            });
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest("FirstName is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest("LastName is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                return BadRequest("Phone is required.");

            var company = await _context.Companies.FindAsync(dto.CompanyId);
            if (company == null)
                return BadRequest($"Company with id {dto.CompanyId} not found.");

            var employee = new Employee
            {
                CompanyId = dto.CompanyId,
                Title = dto.Title,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, new EmployeeDto
            {
                Id = employee.Id,
                CompanyId = employee.CompanyId,
                Title = employee.Title,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Email = employee.Email
            });
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest("FirstName is required.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest("LastName is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                return BadRequest("Phone is required.");

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound($"Employee with id {id} not found.");

            employee.Title = dto.Title;
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new EmployeeDto
            {
                Id = employee.Id,
                CompanyId = employee.CompanyId,
                Title = employee.Title,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                Email = employee.Email
            });
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound($"Employee with id {id} not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
