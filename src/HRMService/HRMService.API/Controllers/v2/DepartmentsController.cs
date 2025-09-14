
using Asp.Versioning;
using HRMService.API.Models;
using HRMService.Domain.Entities;
using HRMService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMService.API.Controllers.v2
{
    [Route("api/hrm/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IHrmDepartmentService _departmentService;

        public DepartmentsController(IHrmDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _departmentService.GetAll();
            var result = departments.Select(d => ToDto(d));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(Guid id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null) return NotFound();

            return Ok(ToDto(department));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DepartmentDto dto)
        {
            if (dto == null) return BadRequest();

            var department = ToEntity(dto);
            department.ID = Guid.NewGuid();

            var success = await _departmentService.Create(department);
            if (!success) return StatusCode(500, "Unable to create department");

            return CreatedAtAction(nameof(GetById), new { id = department.ID }, ToDto(department));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] DepartmentDto dto)
        {
            if (dto == null || id != dto.ID) return BadRequest();

            var existing = await _departmentService.GetById(id);
            if (existing == null) return NotFound();

            var updated = ToEntity(dto);
            updated.ID = id;

            var success = await _departmentService.Update(updated);
            if (!success) return StatusCode(500, "Unable to update department");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var success = await _departmentService.Delete(id);
            if (!success) return NotFound();

            return NoContent();
        }

        // Helper methods to map between DTO and Entity

        private DepartmentDto ToDto(HrmDepartment d) => new DepartmentDto
        {
            ID = d.ID,
            Order = d.Order,
            Code = d.Code,
            Name = d.Name,
            EnglishName = d.EnglishName,
            ShortName = d.ShortName,
            Description = d.Description,
            Category = d.Category,
            ParentCode = d.ParentCode,
            Level = d.Level
        };

        private HrmDepartment ToEntity(DepartmentDto dto) => new HrmDepartment
        {
            ID = dto.ID,
            Order = dto.Order,
            Code = dto.Code,
            Name = dto.Name,
            EnglishName = dto.EnglishName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Category = dto.Category,
            ParentCode = dto.ParentCode,
            Level = dto.Level
        };
    }
}
