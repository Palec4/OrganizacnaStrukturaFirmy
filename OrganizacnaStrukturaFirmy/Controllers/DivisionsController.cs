using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Division;
using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionService _divisionService;

        public DivisionsController(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        // GET: api/divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetAll()
        {
            var divisions = await _divisionService.GetAllAsync();
            return Ok(divisions);
        }

        // GET: api/divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDto>> GetById(int id)
        {
            var division = await _divisionService.GetByIdAsync(id);
            if (division == null)
                return NotFound($"Division with id {id} not found.");

            return Ok(division);
        }

        // POST: api/divisions
        [HttpPost]
        public async Task<ActionResult<DivisionDto>> Create(CreateDivisionDto dto)
        {
            try
            {
                var division = await _divisionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = division.Id }, division);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/divisions/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DivisionDto>> Update(int id, UpdateDivisionDto dto)
        {
            try
            {
                var division = await _divisionService.UpdateAsync(id, dto);
                return Ok(division);
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

        // DELETE: api/divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _divisionService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //// GET: api/divisions/5/projects
        //[HttpGet("{id}/projects")]
        //public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects(int id)
        //{
        //    var division = await _context.Divisions.FindAsync(id);
        //    if (division == null)
        //        return NotFound($"Division with id {id} not found.");

        //    var projects = await _context.Projects
        //        .Where(p => p.DivisionId == id)
        //        .ToListAsync();

        //    return Ok(projects.Select(p => new ProjectDto
        //    {
        //        Id = p.Id,
        //        Code = p.Code,
        //        Name = p.Name,
        //        DivisionId = p.DivisionId,
        //        ManagerId = p.ManagerId
        //    }));
        //}
    }
}
