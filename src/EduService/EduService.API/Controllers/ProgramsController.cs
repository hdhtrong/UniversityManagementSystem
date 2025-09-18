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
            return Ok(new ApiResponse("Fetched all programs successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var program = await _programService.GetById(id);
            if (program == null)
                return NotFound(new ApiResponse("Program not found"));

            var dto = _mapper.Map<EduProgramDto>(program);
            return Ok(new ApiResponse("Fetched program successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduProgramDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Program data is required"));

            var entity = _mapper.Map<EduProgram>(dto);
            var result = await _programService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Program created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create program"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduProgramDto dto)
        {
            if (dto == null || id != dto.ProgramID.ToString())
                return BadRequest(new ApiResponse("Invalid program data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _programService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Program not found"));

            var entity = _mapper.Map<EduProgram>(dto);
            var result = await _programService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Program updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update program"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _programService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Program not found"));

            var result = await _programService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Program deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete program"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var programs = _programService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduProgramDto>>(programs);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched programs with filter successfully", responseData));
        }
    }
}
