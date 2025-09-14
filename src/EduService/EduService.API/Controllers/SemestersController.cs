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
    public class SemestersController : ControllerBase
    {
        private readonly IEduSemesterService _semesterService;
        private readonly IMapper _mapper;

        public SemestersController(IEduSemesterService semesterService, IMapper mapper)
        {
            _semesterService = semesterService;
            _mapper = mapper;
        }

        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrentSemester()
        {
            var currentSemester = await _semesterService.GetCurrentSemester();
            var dto = _mapper.Map<EduSemesterDto>(currentSemester);
            return Ok(dto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var semesters = await _semesterService.GetAll();
            var result = _mapper.Map<IEnumerable<EduSemesterDto>>(semesters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var semester = await _semesterService.GetByIdAsync(id);
            if (semester == null)
                return NotFound();

            var dto = _mapper.Map<EduSemesterDto>(semester);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduSemesterDto dto)
        {
            if (dto == null)
                return BadRequest("Semester data is required");

            var entity = _mapper.Map<EduSemester>(dto);
            var result = await _semesterService.Create(entity);

            if (result)
                return Ok("Semester created successfully");

            return StatusCode(500, "Failed to create semester");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduSemesterDto dto)
        {
            if (dto == null || id != dto.SemesterID.ToString())
                return BadRequest("Invalid semester data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _semesterService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduSemester>(dto);
            var result = await _semesterService.Update(entity);

            if (result)
                return Ok("Semester updated successfully");

            return StatusCode(500, "Failed to update semester");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _semesterService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _semesterService.Delete(guidId);
            if (result)
                return Ok("Semester deleted successfully");

            return StatusCode(500, "Failed to delete semester");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var semesters = _semesterService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduSemesterDto>>(semesters);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
