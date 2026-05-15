using Microsoft.AspNetCore.Mvc;
using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.DTOs.Project;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IDepartmentService _departmentService;

        public ProjectsController(IProjectService projectService, IDepartmentService departmentService)
        {
            _projectService = projectService;
            _departmentService = departmentService;
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

        // GET: api/projects/5/departments
        [HttpGet("{id}/departments")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound($"Project with id {id} not found.");

            var departments = await _departmentService.GetByProjectAsync(id);
            return Ok(departments);
        }
    }
}
