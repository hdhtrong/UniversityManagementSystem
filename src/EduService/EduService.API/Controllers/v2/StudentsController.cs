using Asp.Versioning;
using EduService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EduService.Application.Services;

namespace EduService.API.Controllers.v2
{
    [Route("api/edu/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IEduStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentsController(IEduStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAll();
            var result = _mapper.Map<IEnumerable<EduStudentDto>>(students);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetById(id);
            if (student == null)
                return NotFound();

            var dto = _mapper.Map<EduStudentDto>(student);
            return Ok(dto);
        }
    }
}
