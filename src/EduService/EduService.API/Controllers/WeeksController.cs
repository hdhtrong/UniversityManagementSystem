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
    public class WeeksController : ControllerBase
    {
        private readonly IEduWeekService _weekService;
        private readonly IMapper _mapper;

        public WeeksController(IEduWeekService weekService, IMapper mapper)
        {
            _weekService = weekService;
            _mapper = mapper;
        }

        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrentWeek()
        {
            var currentWeek = await _weekService.GetCurrentWeek();
            if (currentWeek == null)
                return NotFound(new ApiResponse("No current week found"));

            var dto = _mapper.Map<EduWeekDto>(currentWeek);
            return Ok(new ApiResponse("Fetched current week successfully", dto));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var weeks = await _weekService.GetAll();
            var result = _mapper.Map<IEnumerable<EduWeekDto>>(weeks);
            return Ok(new ApiResponse("Fetched all weeks successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var week = await _weekService.GetById(id);
            if (week == null)
                return NotFound(new ApiResponse("Week not found"));

            var dto = _mapper.Map<EduWeekDto>(week);
            return Ok(new ApiResponse("Fetched week successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduWeekDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Week data is required"));

            var entity = _mapper.Map<EduWeek>(dto);
            var result = await _weekService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Week created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create week"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduWeekDto dto)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            if (dto == null || guidId != dto.WeekID)
                return BadRequest(new ApiResponse("Invalid week data"));

            var existing = await _weekService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Week not found"));

            var entity = _mapper.Map<EduWeek>(dto);
            var result = await _weekService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Week updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update week"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _weekService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Week not found"));

            var result = await _weekService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Week deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete week"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var weeks = _weekService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduWeekDto>>(weeks);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched weeks with filter successfully", responseData));
        }
    }
}
