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
    public class CurriculumsController : ControllerBase
    {
        private readonly IEduCurriculumService _curriculumService;
        private readonly IMapper _mapper;

        public CurriculumsController(IEduCurriculumService curriculumService, IMapper mapper)
        {
            _curriculumService = curriculumService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var curriculums = await _curriculumService.GetAll();
            var result = _mapper.Map<IEnumerable<EduCurriculumDto>>(curriculums);
            return Ok(result);
        }

        [HttpGet("{programId}/{subjectId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid programId, Guid subjectId)
        {
            var curriculum = await _curriculumService.GetById(programId, subjectId);
            if (curriculum == null)
                return NotFound();

            var dto = _mapper.Map<EduCurriculumDto>(curriculum);
            return Ok(dto);
        }

        [HttpGet("programs/{programId}/subjects")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectsByProgram(Guid programId)
        {
            var curriculums = await _curriculumService.GetSubjectsByProgram(programId);
            var result = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(result);
        }

        [HttpGet("programs/{programId}/subjects/semester/{semesterOrder}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectsByProgramAndSemester(Guid programId, int semesterOrder)
        {
            var curriculums = await _curriculumService.GetSubjectsByProgramAndSemester(programId, semesterOrder);
            var result = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduCurriculumDto dto)
        {
            if (dto == null)
                return BadRequest("Curriculum data is required");

            var entity = _mapper.Map<EduCurriculum>(dto);
            var result = await _curriculumService.Create(entity);

            if (result)
                return Ok("Curriculum created successfully");

            return StatusCode(500, "Failed to create curriculum");
        }

        [HttpPut("{programId}/{subjectId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid programId, Guid subjectId, [FromBody] EduCurriculumDto dto)
        {
            if (dto == null || dto.ProgramID != programId || dto.SubjectID != subjectId)
                return BadRequest("Invalid curriculum data");

            var existing = await _curriculumService.GetById(programId, subjectId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduCurriculum>(dto);
            var result = await _curriculumService.Update(entity);

            if (result)
                return Ok("Curriculum updated successfully");

            return StatusCode(500, "Failed to update curriculum");
        }

        [HttpDelete("{programId}/{subjectId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(Guid programId, Guid subjectId)
        {
            var existing = await _curriculumService.GetById(programId, subjectId);
            if (existing == null)
                return NotFound();

            var result = await _curriculumService.Delete(programId, subjectId);
            if (result)
                return Ok("Curriculum deleted successfully");

            return StatusCode(500, "Failed to delete curriculum");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var curriculums = _curriculumService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduCurriculumDto>>(curriculums);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
