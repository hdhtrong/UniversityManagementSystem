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
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEduEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;

        public EnrollmentsController(IEduEnrollmentService enrollmentService, IMapper mapper)
        {
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduEnrollmentDto>>(enrollments);
            return Ok(new ApiResponse("Fetched all enrollments successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var enrollment = await _enrollmentService.GetById(id);
            if (enrollment == null)
                return NotFound(new ApiResponse("Enrollment not found"));

            var dto = _mapper.Map<EduEnrollmentDto>(enrollment);
            return Ok(new ApiResponse("Fetched enrollment successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduEnrollmentDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Enrollment data is required"));

            var entity = _mapper.Map<EduEnrollment>(dto);
            var result = await _enrollmentService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Enrollment created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create enrollment"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduEnrollmentDto dto)
        {
            if (dto == null || id != dto.EnrollmentID.ToString())
                return BadRequest(new ApiResponse("Invalid enrollment data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _enrollmentService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Enrollment not found"));

            var entity = _mapper.Map<EduEnrollment>(dto);
            var result = await _enrollmentService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Enrollment updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update enrollment"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _enrollmentService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Enrollment not found"));

            var result = await _enrollmentService.Delete(guidId);

            if (result)
                return Ok(new ApiResponse("Enrollment deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete enrollment"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var enrollments = _enrollmentService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduEnrollmentDto>>(enrollments);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched enrollments with filter successfully", responseData));
        }
    }
}
