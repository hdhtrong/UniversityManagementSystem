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
            return Ok(new ApiResponse("Fetched curriculums successfully", result));
        }

        [HttpGet("{programId}/{subjectId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid programId, Guid subjectId)
        {
            var curriculum = await _curriculumService.GetById(programId, subjectId);
            if (curriculum == null)
                return NotFound(new ApiResponse("Curriculum not found"));

            var dto = _mapper.Map<EduCurriculumDto>(curriculum);
            return Ok(new ApiResponse("Fetched curriculum successfully", dto));
        }

        [HttpGet("programs/{programId}/subjects")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectsByProgram(Guid programId)
        {
            var curriculums = await _curriculumService.GetSubjectsByProgram(programId);
            var result = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(new ApiResponse("Fetched subjects by program successfully", result));
        }

        [HttpGet("programs/{programId}/subjects/semester/{semesterOrder}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectsByProgramAndSemester(Guid programId, int semesterOrder)
        {
            var curriculums = await _curriculumService.GetSubjectsByProgramAndSemester(programId, semesterOrder);
            var result = _mapper.Map<IEnumerable<CurriculumSubjectDto>>(curriculums);
            return Ok(new ApiResponse("Fetched subjects by program and semester successfully", result));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduCurriculumDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Curriculum data is required"));

            var entity = _mapper.Map<EduCurriculum>(dto);
            var success = await _curriculumService.Create(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to create curriculum"));

            var createdDto = _mapper.Map<EduCurriculumDto>(entity);
            return Ok(new ApiResponse("Curriculum created successfully", createdDto));
        }

        [HttpPut("{programId}/{subjectId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid programId, Guid subjectId, [FromBody] EduCurriculumDto dto)
        {
            if (dto == null || dto.ProgramID != programId || dto.SubjectID != subjectId)
                return BadRequest(new ApiResponse("Invalid curriculum data"));

            var existing = await _curriculumService.GetById(programId, subjectId);
            if (existing == null)
                return NotFound(new ApiResponse("Curriculum not found"));

            var entity = _mapper.Map<EduCurriculum>(dto);
            var success = await _curriculumService.Update(entity);

            if (!success)
                return StatusCode(500, new ApiResponse("Failed to update curriculum"));

            var updatedDto = _mapper.Map<EduCurriculumDto>(entity);
            return Ok(new ApiResponse("Curriculum updated successfully", updatedDto));
        }

        [HttpDelete("{programId}/{subjectId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(Guid programId, Guid subjectId)
        {
            var existing = await _curriculumService.GetById(programId, subjectId);
            if (existing == null)
                return NotFound(new ApiResponse("Curriculum not found"));

            var success = await _curriculumService.Delete(programId, subjectId);
            if (!success)
                return StatusCode(500, new ApiResponse("Failed to delete curriculum"));

            return Ok(new ApiResponse("Curriculum deleted successfully"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var curriculums = _curriculumService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduCurriculumDto>>(curriculums);

            var response = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched curriculums with filter successfully", response));
        }
    }
}
