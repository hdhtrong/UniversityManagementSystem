using Asp.Versioning;
using AutoMapper;
using EduService.API.Models;
using EduService.Application.Services;
using EduService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;
using SharedKernel.Models;

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
            return Ok(new ApiResponse("Fetched departments successfully", result));
        }

        [HttpGet("{id}/instructors")]
        [AllowAnonymous]
        public IActionResult GetInstructors(Guid id)
        {
            var instructors = _instructorService.GetByDepartmentId(id);
            if (instructors == null)
                return NotFound(new ApiResponse("No instructors found for this department"));

            var dto = _mapper.Map<IEnumerable<EduInstructorDto>>(instructors);
            return Ok(new ApiResponse("Fetched instructors successfully", dto));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
                return NotFound(new ApiResponse("Department not found"));

            var dto = _mapper.Map<EduDepartmentDto>(department);
            return Ok(new ApiResponse("Fetched department successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduDepartmentDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Department data is required"));

            var entity = _mapper.Map<EduDepartment>(dto);
            var success = await _departmentService.Create(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to create department"));

            var createdDto = _mapper.Map<EduDepartmentDto>(entity);
            return Ok(new ApiResponse("Department created successfully", createdDto));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduDepartmentDto dto)
        {
            if (dto == null || id != dto.DepartmentID.ToString())
                return BadRequest(new ApiResponse("Invalid department data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _departmentService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Department not found"));

            var entity = _mapper.Map<EduDepartment>(dto);
            var success = await _departmentService.Update(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to update department"));

            var updatedDto = _mapper.Map<EduDepartmentDto>(entity);
            return Ok(new ApiResponse("Department updated successfully", updatedDto));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _departmentService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Department not found"));

            var success = await _departmentService.Delete(guidId);
            if (!success)
                return StatusCode(500, new ApiResponse("Failed to delete department"));

            return Ok(new ApiResponse("Department deleted successfully"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var list = _departmentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduDepartmentDto>>(list);

            var response = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched departments with filter successfully", response));
        }
    }
}
