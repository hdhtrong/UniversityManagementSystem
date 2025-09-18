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
    public class TuitionFeesController : ControllerBase
    {
        private readonly IEduTuitionFeeService _tuitionFeeService;
        private readonly IMapper _mapper;

        public TuitionFeesController(IEduTuitionFeeService tuitionFeeService, IMapper mapper)
        {
            _tuitionFeeService = tuitionFeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var fees = await _tuitionFeeService.GetAll();
            var result = _mapper.Map<IEnumerable<EduTuitionFeeDto>>(fees);
            return Ok(new ApiResponse("Fetched all tuition fees successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var fee = await _tuitionFeeService.GetById(id);
            if (fee == null)
                return NotFound(new ApiResponse("Tuition fee not found"));

            var dto = _mapper.Map<EduTuitionFeeDto>(fee);
            return Ok(new ApiResponse("Fetched tuition fee successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduTuitionFeeDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Tuition fee data is required"));

            var entity = _mapper.Map<EduTuitionFee>(dto);
            var result = await _tuitionFeeService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Tuition fee created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create tuition fee"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduTuitionFeeDto dto)
        {
            if (dto == null || id != dto.TuitionFeeID.ToString())
                return BadRequest(new ApiResponse("Invalid tuition fee data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _tuitionFeeService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Tuition fee not found"));

            var entity = _mapper.Map<EduTuitionFee>(dto);
            var result = await _tuitionFeeService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Tuition fee updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update tuition fee"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _tuitionFeeService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Tuition fee not found"));

            var result = await _tuitionFeeService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Tuition fee deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete tuition fee"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var fees = _tuitionFeeService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduTuitionFeeDto>>(fees);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched tuition fees with filter successfully", responseData));
        }
    }
}
