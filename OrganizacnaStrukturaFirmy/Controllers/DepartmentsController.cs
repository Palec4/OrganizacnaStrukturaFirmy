using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _context.Departments.ToListAsync();

            return Ok(departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                ProjectId = d.ProjectId,
                ManagerId = d.ManagerId
            }));
        }

        // GET: api/departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound($"Department with id {id} not found.");

            return Ok(new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId
            });
        }

        // POST: api/departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            var project = await _context.Projects.FindAsync(dto.ProjectId);
            if (project == null)
                return BadRequest($"Project with id {dto.ProjectId} not found.");

            if (await _context.Departments.AnyAsync(d => d.Code == dto.Code))
                return BadRequest($"Department with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
                if (manager == null)
                    return BadRequest($"Employee with id {dto.ManagerId} not found.");

                var division = await _context.Divisions.FindAsync(project.DivisionId);
                if (manager.CompanyId != division!.CompanyId)
                    return BadRequest("Manager must be an employee of the company.");
            }

            var department = new Department
            {
                Code = dto.Code,
                Name = dto.Name,
                ProjectId = dto.ProjectId,
                ManagerId = dto.ManagerId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = department.Id }, new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId
            });
        }

        // PUT: api/departments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentDto>> Update(int id, UpdateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            if (!dto.ManagerId.HasValue)
                return BadRequest("ManagerId is required when updating a department.");

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound($"Department with id {id} not found.");

            if (await _context.Departments.AnyAsync(d => d.Code == dto.Code && d.Id != id))
                return BadRequest($"Department with code '{dto.Code}' already exists.");

            var project = await _context.Projects.FindAsync(department.ProjectId);
            var division = await _context.Divisions.FindAsync(project!.DivisionId);

            var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
            if (manager == null)
                return BadRequest($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != division!.CompanyId)
                return BadRequest("Manager must be an employee of the company.");

            department.Code = dto.Code;
            department.Name = dto.Name;
            department.ManagerId = dto.ManagerId;

            await _context.SaveChangesAsync();

            return Ok(new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                ProjectId = department.ProjectId,
                ManagerId = department.ManagerId
            });
        }

        // DELETE: api/departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound($"Department with id {id} not found.");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
