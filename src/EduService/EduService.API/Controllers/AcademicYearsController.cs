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
    public class AcademicYearsController : ControllerBase
    {
        private readonly IEduAcademicYearService _yearService;
        private readonly IMapper _mapper;

        public AcademicYearsController(IEduAcademicYearService yearService, IMapper mapper)
        {
            _yearService = yearService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var years = await _yearService.GetAll();
            var result = _mapper.Map<IEnumerable<EduAcademicYearDto>>(years);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var year = await _yearService.GetByIdAsync(id);
            if (year == null)
                return NotFound();

            var dto = _mapper.Map<EduAcademicYearDto>(year);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduAcademicYearDto dto)
        {
            if (dto == null)
                return BadRequest("Academic year data is required");

            var entity = _mapper.Map<EduAcademicYear>(dto);
            var result = await _yearService.Create(entity);

            if (result)
                return Ok("Academic year created successfully");

            return StatusCode(500, "Failed to create academic year");
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduAcademicYearDto dto)
        {
            if (dto == null || id != dto.YearID.ToString())
                return BadRequest("Invalid academic year data");

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _yearService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var entity = _mapper.Map<EduAcademicYear>(dto);
            var result = await _yearService.Update(entity);

            if (result)
                return Ok("Academic year updated successfully");

            return StatusCode(500, "Failed to update academic year");
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest("Invalid GUID format");

            var existing = await _yearService.GetById(guidId);
            if (existing == null)
                return NotFound();

            var result = await _yearService.Delete(guidId);
            if (result)
                return Ok("Academic year deleted successfully");

            return StatusCode(500, "Failed to delete academic year");
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest("Filter is null");

            var years = _yearService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduAcademicYearDto>>(years);

            return Ok(new
            {
                Total = total,
                Data = dtoList
            });
        }
    }
}
