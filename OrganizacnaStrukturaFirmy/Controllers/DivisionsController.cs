using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Models;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DivisionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetAll()
        {
            var divisions = await _context.Divisions.ToListAsync();

            return Ok(divisions.Select(d => new DivisionDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                CompanyId = d.CompanyId,
                ManagerId = d.ManagerId
            }));
        }

        // GET: api/divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDto>> GetById(int id)
        {
            var division = await _context.Divisions.FindAsync(id);

            if (division == null)
                return NotFound($"Division with id {id} not found.");

            return Ok(new DivisionDto
            {
                Id = division.Id,
                Code = division.Code,
                Name = division.Name,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            });
        }

        // POST: api/divisions
        [HttpPost]
        public async Task<ActionResult<DivisionDto>> Create(CreateDivisionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            var company = await _context.Companies.FindAsync(dto.CompanyId);
            if (company == null)
                return BadRequest($"Company with id {dto.CompanyId} not found.");

            if (await _context.Divisions.AnyAsync(d => d.Code == dto.Code))
                return BadRequest($"Division with code '{dto.Code}' already exists.");

            if (dto.ManagerId.HasValue)
            {
                var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
                if (manager == null)
                    return BadRequest($"Employee with id {dto.ManagerId} not found.");

                if (manager.CompanyId != dto.CompanyId)
                    return BadRequest("Manager must be an employee of this company.");
            }

            var division = new Division
            {
                Code = dto.Code,
                Name = dto.Name,
                CompanyId = dto.CompanyId,
                ManagerId = dto.ManagerId
            };

            _context.Divisions.Add(division);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = division.Id }, new DivisionDto
            {
                Id = division.Id,
                Code = division.Code,
                Name = division.Name,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            });
        }

        // PUT: api/divisions/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DivisionDto>> Update(int id, UpdateDivisionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest("Code is required.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            if (!dto.ManagerId.HasValue)
                return BadRequest("ManagerId is required when updating a division.");

            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
                return NotFound($"Division with id {id} not found.");

            if (await _context.Divisions.AnyAsync(d => d.Code == dto.Code && d.Id != id))
                return BadRequest($"Division with code '{dto.Code}' already exists.");

            var manager = await _context.Employees.FindAsync(dto.ManagerId.Value);
            if (manager == null)
                return BadRequest($"Employee with id {dto.ManagerId} not found.");

            if (manager.CompanyId != division.CompanyId)
                return BadRequest("Manager must be an employee of this company.");

            division.Code = dto.Code;
            division.Name = dto.Name;
            division.ManagerId = dto.ManagerId;

            await _context.SaveChangesAsync();

            return Ok(new DivisionDto
            {
                Id = division.Id,
                Code = division.Code,
                Name = division.Name,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            });
        }

        // DELETE: api/divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
                return NotFound($"Division with id {id} not found.");

            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/divisions/5/projects
        [HttpGet("{id}/projects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
                return NotFound($"Division with id {id} not found.");

            var projects = await _context.Projects
                .Where(p => p.DivisionId == id)
                .ToListAsync();

            return Ok(projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DivisionId = p.DivisionId,
                ManagerId = p.ManagerId
            }));
        }
    }
}
