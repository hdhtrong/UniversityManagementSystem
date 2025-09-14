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
    public class GradesController : ControllerBase
    {
        private readonly IEduGradeService _gradeService;
        private readonly IMapper _mapper;

        public GradesController(IEduGradeService gradeService, IMapper mapper)
        {
            _gradeService = gradeService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var grades = await _gradeService.GetAll();
            var result = _mapper.Map<IEnumerable<EduGradeDto>>(grades);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var grade = await _gradeService.GetById(id);
            if (grade == null)
                return NotFound();

            var dto = _mapper.Map<EduGradeDto>(grade);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduGradeDto dto)
        {
            if (dto == null)
                return BadRequest("Grade data is required");

            var entity = _mapper.Map<EduGrade>(dto);
            var result = await _gradeService.Create(entity);
            if (result)
                return Ok("Grade created successfully");

            return StatusCode(500, "Failed to create grade");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduGradeDto dto)
        {
            if (dto == null || id != dto.GradeID.ToString())
                return BadRequest("Invalid grade data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _gradeService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduGrade>(dto);
            var result = await _gradeService.Update(entity);
            if (result)
                return Ok("Grade updated successfully");

            return StatusCode(500, "Failed to update grade");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _gradeService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _gradeService.Delete(guidId);
            if (result)
                return Ok("Grade deleted successfully");

            return StatusCode(500, "Failed to delete grade");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var grades = _gradeService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduGradeDto>>(grades);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
