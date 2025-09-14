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
            var currentSemester = await _weekService.GetCurrentWeek();
            var dto = _mapper.Map<EduWeekDto>(currentSemester);
            return Ok(dto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var weeks = await _weekService.GetAll();
            var result = _mapper.Map<IEnumerable<EduWeekDto>>(weeks);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var week = await _weekService.GetById(id);
            if (week == null)
                return NotFound();

            var dto = _mapper.Map<EduWeekDto>(week);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduWeekDto dto)
        {
            if (dto == null)
                return BadRequest("Week data is required");

            var entity = _mapper.Map<EduWeek>(dto);
            var result = await _weekService.Create(entity);

            if (result)
                return Ok("Week created successfully");

            return StatusCode(500, "Failed to create week");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduWeekDto dto)
        {
            if (dto == null || id != dto.WeekID.ToString())
                return BadRequest("Invalid week data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _weekService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduWeek>(dto);
            var result = await _weekService.Update(entity);

            if (result)
                return Ok("Week updated successfully");

            return StatusCode(500, "Failed to update week");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _weekService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _weekService.Delete(guidId);
            if (result)
                return Ok("Week deleted successfully");

            return StatusCode(500, "Failed to delete week");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var weeks = _weekService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduWeekDto>>(weeks);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
