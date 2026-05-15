using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Data;
using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Models;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound($"Project with id {id} not found.");

            return Ok(project);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            try
            {
                var project = await _projectService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(int id, UpdateProjectDto dto)
        {
            try
            {
                var project = await _projectService.UpdateAsync(id, dto);
                return Ok(project);
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

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectService.DeleteAsync(id);
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

        //// GET: api/projects/5/departments
        //[HttpGet("{id}/departments")]
        //public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments(int id)
        //{
        //    var project = await _context.Projects.FindAsync(id);
        //    if (project == null)
        //        return NotFound($"Project with id {id} not found.");

        //    var departments = await _context.Departments
        //        .Where(d => d.ProjectId == id)
        //        .ToListAsync();

        //    return Ok(departments.Select(d => new DepartmentDto
        //    {
        //        Id = d.Id,
        //        Code = d.Code,
        //        Name = d.Name,
        //        ProjectId = d.ProjectId,
        //        ManagerId = d.ManagerId
        //    }));
        //}
    }
}
