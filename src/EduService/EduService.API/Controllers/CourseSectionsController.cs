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
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var section = await _sectionService.GetById(id);
            if (section == null)
                return NotFound();

            var dto = _mapper.Map<EduCourseSectionDto>(section);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduCourseSectionDto dto)
        {
            if (dto == null)
                return BadRequest("Course section data is required");

            var entity = _mapper.Map<EduCourseSection>(dto);
            var result = await _sectionService.Create(entity);
            if (result)
                return Ok("Course section created successfully");

            return StatusCode(500, "Failed to create course section");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduCourseSectionDto dto)
        {
            if (dto == null || id != dto.SectionID.ToString())
                return BadRequest("Invalid course section data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _sectionService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduCourseSection>(dto);
            var result = await _sectionService.Update(entity);
            if (result)
                return Ok("Course section updated successfully");

            return StatusCode(500, "Failed to update course section");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _sectionService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _sectionService.Delete(guidId);
            if (result)
                return Ok("Course section deleted successfully");

            return StatusCode(500, "Failed to delete course section");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var sections = _sectionService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduCourseSectionDto>>(sections);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
