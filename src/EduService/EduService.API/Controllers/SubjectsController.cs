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
    public class SubjectsController : ControllerBase
    {
        private readonly IEduSubjectService _subjectService;
        private readonly IMapper _mapper;

        public SubjectsController(IEduSubjectService subjectService, IMapper mapper)
        {
            _subjectService = subjectService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAll();
            var result = _mapper.Map<IEnumerable<EduSubjectDto>>(subjects);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var subject = await _subjectService.GetById(id);
            if (subject == null)
                return NotFound();

            var dto = _mapper.Map<EduSubjectDto>(subject);
            return Ok(dto);
        }

        [HttpGet("{id}/Prerequisites")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectPrerequisites(Guid id)
        {
            var subjects = await _subjectService.GetPrerequisitesAsync(id);
            var result = _mapper.Map<IEnumerable<EduSubjectDto>>(subjects);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduSubjectDto dto)
        {
            if (dto == null)
                return BadRequest("Subject data is required");

            var entity = _mapper.Map<EduSubject>(dto);
            var result = await _subjectService.Create(entity);

            if (result)
                return Ok("Subject created successfully");

            return StatusCode(500, "Failed to create subject");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduSubjectDto dto)
        {
            if (dto == null || id != dto.SubjectID.ToString())
                return BadRequest("Invalid subject data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _subjectService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduSubject>(dto);
            var result = await _subjectService.Update(entity);

            if (result)
                return Ok("Subject updated successfully");

            return StatusCode(500, "Failed to update subject");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _subjectService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _subjectService.Delete(guidId);
            if (result)
                return Ok("Subject deleted successfully");

            return StatusCode(500, "Failed to delete subject");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var subjects = _subjectService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduSubjectDto>>(subjects);

            return Ok(new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            });
        }
    }
}
