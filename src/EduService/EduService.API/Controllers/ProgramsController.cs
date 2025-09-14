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
    public class ProgramsController : ControllerBase
    {
        private readonly IEduProgramService _programService;
        private readonly IMapper _mapper;

        public ProgramsController(IEduProgramService programService, IMapper mapper)
        {
            _programService = programService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var programs = await _programService.GetAll();
            var result = _mapper.Map<IEnumerable<EduProgramDto>>(programs);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var program = await _programService.GetById(id);
            if (program == null)
                return NotFound();

            var dto = _mapper.Map<EduProgramDto>(program);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduProgramDto dto)
        {
            if (dto == null)
                return BadRequest("Program data is required");

            var entity = _mapper.Map<EduProgram>(dto);
            var result = await _programService.Create(entity);
            if (result)
                return Ok("Program created successfully");

            return StatusCode(500, "Failed to create program");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduProgramDto dto)
        {
            if (dto == null || id != dto.ProgramID.ToString())
                return BadRequest("Invalid program data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _programService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduProgram>(dto);
            var result = await _programService.Update(entity);
            if (result)
                return Ok("Program updated successfully");

            return StatusCode(500, "Failed to update program");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _programService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _programService.Delete(guidId);
            if (result)
                return Ok("Program deleted successfully");

            return StatusCode(500, "Failed to delete program");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var programs = _programService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduProgramDto>>(programs);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
