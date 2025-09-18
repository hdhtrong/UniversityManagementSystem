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
            if (currentSemester == null)
                return NotFound(new ApiResponse("No current semester found"));

            var dto = _mapper.Map<EduSemesterDto>(currentSemester);
            return Ok(new ApiResponse("Fetched current semester successfully", dto));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var semesters = await _semesterService.GetAll();
            var result = _mapper.Map<IEnumerable<EduSemesterDto>>(semesters);
            return Ok(new ApiResponse("Fetched all semesters successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var semester = await _semesterService.GetByIdAsync(id);
            if (semester == null)
                return NotFound(new ApiResponse("Semester not found"));

            var dto = _mapper.Map<EduSemesterDto>(semester);
            return Ok(new ApiResponse("Fetched semester successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduSemesterDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Semester data is required"));

            var entity = _mapper.Map<EduSemester>(dto);
            var result = await _semesterService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Semester created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create semester"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduSemesterDto dto)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            if (dto == null || guidId != dto.SemesterID)
                return BadRequest(new ApiResponse("Invalid semester data"));

            var existing = await _semesterService.GetByIdAsync(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Semester not found"));

            var entity = _mapper.Map<EduSemester>(dto);
            var result = await _semesterService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Semester updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update semester"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _semesterService.GetByIdAsync(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Semester not found"));

            var result = await _semesterService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Semester deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete semester"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var semesters = _semesterService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduSemesterDto>>(semesters);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched semesters with filter successfully", responseData));
        }
    }
}
