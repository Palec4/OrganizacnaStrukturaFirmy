using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _context.Projects.ToListAsync();

            return Ok(projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DivisionId = p.DivisionId,
                ManagerId = p.ManagerId
            }));
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound($"Project with id {id} not found.");

            return Ok(new ProjectDto
            {
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                DivisionId = project.DivisionId,
                ManagerId = project.ManagerId
            });
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            var division = await _context.Divisions.FindAsync(dto.DivisionId);
            if (division == null)
                return BadRequest($"Division with id {dto.DivisionId} not found.");

            if (await _context.Projects.AnyAsync(p => p.Code == dto.Code))
                return BadRequest($"Project with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
                if (manager == null)
                    return BadRequest($"Employee with id {dto.ManagerId} not found.");

                if (manager.CompanyId != division.CompanyId)
                    return BadRequest("Manager must be an employee of the company.");
            }

            var project = new Project
            {
                Code = dto.Code,
                Name = dto.Name,
                DivisionId = dto.DivisionId,
                ManagerId = dto.ManagerId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, new ProjectDto
            {
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                DivisionId = project.DivisionId,
                ManagerId = project.ManagerId
            });
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(int id, UpdateProjectDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            if (!dto.ManagerId.HasValue)
                return BadRequest("ManagerId is required when updating a project.");

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project with id {id} not found.");

            if (await _context.Projects.AnyAsync(p => p.Code == dto.Code && p.Id != id))
                return BadRequest($"Project with code '{dto.Code}' already exists.");

            var division = await _context.Divisions.FindAsync(project.DivisionId);

            var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
            if (manager == null)
                return BadRequest($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != division!.CompanyId)
                return BadRequest("Manager must be an employee of the company.");

            project.Code = dto.Code;
            project.Name = dto.Name;
            project.ManagerId = dto.ManagerId;

            await _context.SaveChangesAsync();

            return Ok(new ProjectDto
            {
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                DivisionId = project.DivisionId,
                ManagerId = project.ManagerId
            });
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project with id {id} not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/projects/5/departments
        [HttpGet("{id}/departments")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project with id {id} not found.");

            var departments = await _context.Departments
                .Where(d => d.ProjectId == id)
                .ToListAsync();

            return Ok(departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                ProjectId = d.ProjectId,
                ManagerId = d.ManagerId
            }));
        }
    }
}
