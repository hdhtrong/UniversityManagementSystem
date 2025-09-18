
using Asp.Versioning;
using AutoMapper;
using HRMService.API.Models;
using HRMService.Domain.Entities;
using HRMService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;

namespace HRMService.API.Controllers.v2
{
    [Route("api/hrm/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IHrmDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentsController(IHrmDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAll();
            var result = _mapper.Map<IEnumerable<HrmDepartmentDto>>(departments);
            return Ok(new ApiResponse("Fetched all departments successfully", result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
                return NotFound(new ApiResponse($"Department with id '{id}' not found"));

            var dto = _mapper.Map<HrmDepartmentDto>(department);
            return Ok(new ApiResponse("Fetched department successfully", dto));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HrmDepartmentDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Invalid department data"));

            var department = _mapper.Map<HrmDepartment>(dto);
            department.ID = Guid.NewGuid();

            var success = await _departmentService.Create(department);
            if (!success)
                return StatusCode(500, new ApiResponse("Unable to create department"));

            var createdDto = _mapper.Map<HrmDepartmentDto>(department);
            return Ok(new ApiResponse("Department created successfully", createdDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HrmDepartmentDto dto)
        {
            if (dto == null || id != dto.ID)
                return BadRequest(new ApiResponse("Invalid department data"));

            var existing = await _departmentService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse($"Department with id '{id}' not found"));

            var updated = _mapper.Map<HrmDepartment>(dto);
            updated.ID = id;

            var success = await _departmentService.Update(updated);
            if (!success)
                return StatusCode(500, new ApiResponse("Unable to update department"));

            var updatedDto = _mapper.Map<HrmDepartmentDto>(updated);
            return Ok(new ApiResponse("Department updated successfully", updatedDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _departmentService.Delete(id);
            if (!success)
                return NotFound(new ApiResponse($"Department with id '{id}' not found"));

            return Ok(new ApiResponse("Department deleted successfully"));
        }
    }
}
