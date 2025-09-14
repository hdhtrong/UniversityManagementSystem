using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;

namespace EduService.API.Controllers
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IEduDepartmentService _departmentService;
        private readonly IEduInstructorService _instructorService;
        private readonly IMapper _mapper;

        public DepartmentsController(IEduInstructorService instructorService, IEduDepartmentService departmentService, IMapper mapper)
        {
            _instructorService = instructorService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduDepartmentDto>>(departments);
            return Ok(result);
        }

        [HttpGet("{id}/Instructors")]
        [AllowAnonymous]
        public IActionResult GetInstructors(Guid id)
        {
            var instructors = _instructorService.GetByDepartmentId(id);
            if (instructors == null)
                return NotFound();
            var dto = _mapper.Map<IEnumerable<EduInstructorDto>>(instructors);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
                return NotFound();

            var dto = _mapper.Map<EduDepartmentDto>(department);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduDepartmentDto dto)
        {
            if (dto == null)
                return BadRequest("Department data is required");

            var entity = _mapper.Map<EduDepartment>(dto);
            var result = await _departmentService.Create(entity);
            if (result)
                return Ok("Department created successfully");

            return StatusCode(500, "Failed to create department");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduDepartmentDto dto)
        {
            if (dto == null || id != dto.DepartmentID.ToString())
                return BadRequest("Invalid department data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _departmentService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduDepartment>(dto);
            var result = await _departmentService.Update(entity);
            if (result)
                return Ok("Department updated successfully");

            return StatusCode(500, "Failed to update department");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _departmentService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _departmentService.Delete(guidId);
            if (result)
                return Ok("Department deleted successfully");

            return StatusCode(500, "Failed to delete department");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var list = _departmentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduDepartmentDto>>(list);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
