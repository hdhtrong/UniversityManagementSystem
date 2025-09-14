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
    public class PeriodsController : ControllerBase
    {
        private readonly IEduPeriodService _periodService;
        private readonly IMapper _mapper;

        public PeriodsController(IEduPeriodService periodService, IMapper mapper)
        {
            _periodService = periodService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var periods = await _periodService.GetAll();
            var result = _mapper.Map<IEnumerable<EduPeriodDto>>(periods);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var period = await _periodService.GetById(id);
            if (period == null)
                return NotFound();

            var dto = _mapper.Map<EduPeriodDto>(period);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduPeriodDto dto)
        {
            if (dto == null)
                return BadRequest("Period data is required");

            var entity = _mapper.Map<EduPeriod>(dto);
            var result = await _periodService.Create(entity);

            if (result)
                return Ok("Period created successfully");

            return StatusCode(500, "Failed to create period");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(int id, [FromBody] EduPeriodDto dto)
        {
            if (dto == null || id != dto.PeriodNumber)
                return BadRequest("Invalid period data");

            var existing = await _periodService.GetById(id);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduPeriod>(dto);
            var result = await _periodService.Update(entity);

            if (result)
                return Ok("Period updated successfully");

            return StatusCode(500, "Failed to update period");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _periodService.GetById(id);
            if (existing == null)
                return NotFound();

            var result = await _periodService.Delete(id);
            if (result)
                return Ok("Period deleted successfully");

            return StatusCode(500, "Failed to delete period");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var periods = _periodService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduPeriodDto>>(periods);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
