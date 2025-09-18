using Asp.Versioning;
using AutoMapper;
using HRMService.API.Models;
using HRMService.Domain.Entities;
using HRMService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.SharedKernel.Models;
using SharedKernel.Models;

namespace HRMService.API.Controllers
{
    [Route("api/hrm/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "HrmManager")]
    public class EmployeesController : ControllerBase
    {
        private readonly IHrmEmployeeService _employeeService;
        private readonly IHrmDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public EmployeesController(
            IHrmEmployeeService employeeService,
            IHrmDepartmentService departmentService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            var result = _mapper.Map<IEnumerable<HrmEmployeeDto>>(employees);
            return Ok(new ApiResponse("Fetched all employees successfully", result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null)
                return NotFound(new ApiResponse($"Employee with id '{id}' not found"));

            var dto = _mapper.Map<HrmEmployeeDto>(employee);
            return Ok(new ApiResponse("Fetched employee successfully", dto));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HrmEmployeeDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse("Invalid employee data"));

            var employee = _mapper.Map<HrmEmployee>(dto);
            employee.ID = Guid.NewGuid();

            if (dto.DepartmentID.HasValue)
            {
                var dept = await _departmentService.GetById(dto.DepartmentID.Value);
                if (dept == null)
                    return BadRequest(new ApiResponse("Department not found"));

                employee.DepartmentID = dept.ID;
                employee.Department = dept;
            }

            var success = await _employeeService.Create(employee);
            if (!success)
                return StatusCode(500, new ApiResponse("Unable to create employee"));

            var createdDto = _mapper.Map<HrmEmployeeDto>(employee);
            return Ok(new ApiResponse("Employee created successfully", createdDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HrmEmployeeDto dto)
        {
            if (dto == null || id != dto.ID)
                return BadRequest(new ApiResponse("Invalid employee data"));

            var existing = await _employeeService.GetById(id);
            if (existing == null)
                return NotFound(new ApiResponse($"Employee with id '{id}' not found"));

            var updated = _mapper.Map<HrmEmployee>(dto);
            updated.ID = id;

            if (dto.DepartmentID.HasValue)
            {
                var dept = await _departmentService.GetById(dto.DepartmentID.Value);
                if (dept == null)
                    return BadRequest(new ApiResponse("Department not found"));

                updated.DepartmentID = dept.ID;
                updated.Department = dept;
            }

            var success = await _employeeService.Update(updated);
            if (!success)
                return StatusCode(500, new ApiResponse("Unable to update employee"));

            var updatedDto = _mapper.Map<HrmEmployeeDto>(updated);
            return Ok(new ApiResponse("Employee updated successfully", updatedDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _employeeService.Delete(id);
            if (!success)
                return NotFound(new ApiResponse($"Employee with id '{id}' not found"));

            return Ok(new ApiResponse("Employee deleted successfully"));
        }

        [HttpPost("filter")]
        public IActionResult GetByFilterPaging([FromBody] FilterRequest filter)
        {
            if (filter == null)
                return BadRequest(new ApiResponse("Filter is null"));

            var employees = _employeeService.GetByFilterPaging(filter, out int total).ToList();
            var dtoList = _mapper.Map<List<HrmEmployeeDto>>(employees);

            var pagingResult = new
            {
                filter.PageIndex,
                filter.PageSize,
                Total = total,
                Data = dtoList
            };

            return Ok(new ApiResponse("Fetched employees by filter successfully", pagingResult));
        }
    }
}
