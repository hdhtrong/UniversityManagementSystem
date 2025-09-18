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
    public class RoomsController : ControllerBase
    {
        private readonly IEduRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomsController(IEduRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAll();
            var result = _mapper.Map<IEnumerable<EduRoomDto>>(rooms);
            return Ok(new ApiResponse("Fetched all rooms successfully", result));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetById(id);
            if (room == null)
                return NotFound(new ApiResponse("Room not found"));

            var dto = _mapper.Map<EduRoomDto>(room);
            return Ok(new ApiResponse("Fetched room successfully", dto));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Create([FromBody] EduRoomDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Room data is required"));

            var entity = _mapper.Map<EduRoom>(dto);
            var result = await _roomService.Create(entity);

            if (result)
                return Ok(new ApiResponse("Room created successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to create room"));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Update(string id, [FromBody] EduRoomDto dto)
        {
            if (dto == null || id != dto.RoomID.ToString())
                return BadRequest(new ApiResponse("Invalid room data"));

            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _roomService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Room not found"));

            var entity = _mapper.Map<EduRoom>(dto);
            var result = await _roomService.Update(entity);

            if (result)
                return Ok(new ApiResponse("Room updated successfully", dto));

            return StatusCode(500, new ApiResponse("Failed to update room"));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return BadRequest(new ApiResponse("Invalid GUID format"));

            var existing = await _roomService.GetById(guidId);
            if (existing == null)
                return NotFound(new ApiResponse("Room not found"));

            var result = await _roomService.Delete(guidId);
            if (result)
                return Ok(new ApiResponse("Room deleted successfully"));

            return StatusCode(500, new ApiResponse("Failed to delete room"));
        }

        [HttpPost("filter")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EduManager")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var rooms = _roomService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<EduRoomDto>>(rooms);

            var responseData = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched rooms with filter successfully", responseData));
        }
    }
}
