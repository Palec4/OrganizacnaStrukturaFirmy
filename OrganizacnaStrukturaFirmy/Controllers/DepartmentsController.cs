using Microsoft.AspNetCore.Mvc;
using OrganizacnaStrukturaFirmy.DTOs.Department;
using OrganizacnaStrukturaFirmy.Service.Interface;

namespace OrganizacnaStrukturaFirmy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }

        // GET: api/departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound($"Department with id {id} not found.");

            return Ok(department);
        }

        // POST: api/departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
        {
            try
            {
                var department = await _departmentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/departments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentDto>> Update(int id, UpdateDepartmentDto dto)
        {
            try
            {
                var department = await _departmentService.UpdateAsync(id, dto);
                return Ok(department);
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

        // DELETE: api/departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
