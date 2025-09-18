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
    public class MajorsController : ControllerBase
    {
        private readonly IEduMajorService _majorService;
        private readonly IMapper _mapper;

        public MajorsController(IEduMajorService majorService, IMapper mapper)
        {
            _majorService = majorService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var majors = await _majorService.GetAll();
            var result = _mapper.Map<IEnumerable<EduMajorDto>>(majors);
            return Ok(new ApiResponse("Fetched all majors successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var major = await _majorService.GetById(id);
            if (major == null)
                return NotFound(new ApiResponse("Major not found"));

            var dto = _mapper.Map<EduMajorDto>(major);
            return Ok(new ApiResponse("Fetched major successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduMajorDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Major data is required"));

            var entity = _mapper.Map<EduMajor>(dto);
            var result = await _majorService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Major created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create major"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduMajorDto dto)
        {
            if (dto == null || id != dto.MajorID.ToString())
                return BadRequest(new ApiResponse("Invalid major data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existingMajor = await _majorService.GetById(guidId);
            if (existingMajor == null)
                return NotFound(new ApiResponse("Major not found"));

            var entity = _mapper.Map<EduMajor>(dto);
            var result = await _majorService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Major updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update major"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existingMajor = await _majorService.GetById(guidId);
            if (existingMajor == null)
                return NotFound(new ApiResponse("Major not found"));

            var result = await _majorService.Delete(guidId);

            if (result)
                return Ok(new ApiResponse("Major deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete major"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var majors = _majorService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduMajorDto>>(majors);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched majors with filter successfully", responseData));
        }
    }
}
