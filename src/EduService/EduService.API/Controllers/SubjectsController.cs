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
            return Ok(new ApiResponse("Fetched all subjects successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var subject = await _subjectService.GetById(id);
            if (subject == null)
                return NotFound(new ApiResponse("Subject not found"));

            var dto = _mapper.Map<EduSubjectDto>(subject);
            return Ok(new ApiResponse("Fetched subject successfully", dto));
        }

        [HttpGet("{id}/Prerequisites")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubjectPrerequisites(Guid id)
        {
            var subjects = await _subjectService.GetPrerequisitesAsync(id);
            var result = _mapper.Map<IEnumerable<EduSubjectDto>>(subjects);
            return Ok(new ApiResponse("Fetched prerequisites successfully", result));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduSubjectDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Subject data is required"));

            var entity = _mapper.Map<EduSubject>(dto);
            var result = await _subjectService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Subject created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create subject"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduSubjectDto dto)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            if (dto == null || guidId != dto.SubjectID)
                return BadRequest(new ApiResponse("Invalid subject data"));

            var existing = await _subjectService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Subject not found"));

            var entity = _mapper.Map<EduSubject>(dto);
            var result = await _subjectService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Subject updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update subject"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _subjectService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Subject not found"));

            var result = await _subjectService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Subject deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete subject"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var subjects = _subjectService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduSubjectDto>>(subjects);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched subjects with filter successfully", responseData));
        }
    }
}
