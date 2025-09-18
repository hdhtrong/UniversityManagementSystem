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
            return Ok(new ApiResponse("Fetched all academic years", result));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var year = await _yearService.GetByIdAsync(id);
            if (year == null)
                return NotFound(new ApiResponse("Academic year not found"));

            var dto = _mapper.Map<EduAcademicYearDto>(year);
            return Ok(new ApiResponse("Academic year fetched successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduAcademicYearDto dto)
        {
            var entity = _mapper.Map<EduAcademicYear>(dto);
            var result = await _yearService.Create(entity);

            if (!result)
                return StatusCode(500, new ApiResponse("Failed to create academic year"));

            return CreatedAtAction(nameof(GetById), new { id = entity.YearID },
                new ApiResponse("Academic year created successfully", dto));
        }

        [HttpPut("{id:guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EduAcademicYearDto dto)
        {
            if (id != dto.YearID)
                return BadRequest(new ApiResponse("ID mismatch"));

            var existing = await _yearService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse("Academic year not found"));

            var entity = _mapper.Map<EduAcademicYear>(dto);
            var result = await _yearService.Update(entity);

            if (!result)
                return StatusCode(500, new ApiResponse("Failed to update academic year"));

            return Ok(new ApiResponse("Academic year updated successfully", dto));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _yearService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse("Academic year not found"));

            var result = await _yearService.Delete(id);
            if (!result)
                return StatusCode(500, new ApiResponse("Failed to delete academic year"));

            return Ok(new ApiResponse("Academic year deleted successfully"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            var years = _yearService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduAcademicYearDto>>(years);

            var response = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Paged academic years fetched successfully", response));
        }
    }
}
