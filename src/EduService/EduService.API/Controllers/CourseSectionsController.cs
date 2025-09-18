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
    public class CourseSectionsController : ControllerBase
    {
        private readonly IEduCourseSectionService _sectionService;
        private readonly IMapper _mapper;

        public CourseSectionsController(IEduCourseSectionService sectionService, IMapper mapper)
        {
            _sectionService = sectionService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var sections = await _sectionService.GetAll();
            var result = _mapper.Map<IEnumerable<EduCourseSectionDto>>(sections);
            return Ok(new ApiResponse("Fetched course sections successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var section = await _sectionService.GetById(id);
            if (section == null)
                return NotFound(new ApiResponse("Course section not found"));

            var dto = _mapper.Map<EduCourseSectionDto>(section);
            return Ok(new ApiResponse("Fetched course section successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduCourseSectionDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Course section data is required"));

            var entity = _mapper.Map<EduCourseSection>(dto);
            var success = await _sectionService.Create(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to create course section"));

            var createdDto = _mapper.Map<EduCourseSectionDto>(entity);
            return Ok(new ApiResponse("Course section created successfully", createdDto));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduCourseSectionDto dto)
        {
            if (dto == null || id != dto.SectionID.ToString())
                return BadRequest(new ApiResponse("Invalid course section data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _sectionService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Course section not found"));

            var entity = _mapper.Map<EduCourseSection>(dto);
            var success = await _sectionService.Update(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to update course section"));

            var updatedDto = _mapper.Map<EduCourseSectionDto>(entity);
            return Ok(new ApiResponse("Course section updated successfully", updatedDto));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _sectionService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Course section not found"));

            var success = await _sectionService.Delete(guidId);
            if (!success)
                return StatusCode(500, new ApiResponse("Failed to delete course section"));

            return Ok(new ApiResponse("Course section deleted successfully"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var sections = _sectionService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduCourseSectionDto>>(sections);

            var response = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched course sections with filter successfully", response));
        }
    }
}
