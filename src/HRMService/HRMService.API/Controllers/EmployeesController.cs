using Asp.Versioning;
using HRMService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMService.API.Controllers
{
    [Route("api/hrm/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        // Dữ liệu giả
        private static readonly List<EmployeeDto> _mockEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, FullName = "Hà Việt Uyên Synh", Email = "hvusynh@hcmiu.edu.vn", Position = "Giảng viên" },
            new EmployeeDto { Id = 2, FullName = "Đoàn Mậu Hiển", Email = "dmhien@hcmiu.edu.vn", Position = "Phó phòng" },
            new EmployeeDto { Id = 3, FullName = "Hồ Đặng Hữu Trọng", Email = "hdhtrong@hcmiu.edu.vn", Position = "Chuyên viên" },
            new EmployeeDto { Id = 4, FullName = "Nguyễn Thị Mai Phương", Email = "ntmphuong@hcmiu.edu.vn", Position = "Giảng viên" },
            new EmployeeDto { Id = 5, FullName = "Trần Văn Khải", Email = "tvkhai@hcmiu.edu.vn", Position = "Trưởng phòng" },
            new EmployeeDto { Id = 6, FullName = "Phạm Hồng Ngọc", Email = "phngoc@hcmiu.edu.vn", Position = "Thư ký khoa" },
            new EmployeeDto { Id = 7, FullName = "Lê Thị Bích Ngọc", Email = "ltbngoc@hcmiu.edu.vn", Position = "Chuyên viên" },
            new EmployeeDto { Id = 8, FullName = "Nguyễn Văn Tài", Email = "nvtai@hcmiu.edu.vn", Position = "Giảng viên" },
            new EmployeeDto { Id = 9, FullName = "Đỗ Hoàng Yến", Email = "dhyen@hcmiu.edu.vn", Position = "Giảng viên" },
            new EmployeeDto { Id = 10, FullName = "Nguyễn Trọng Nghĩa", Email = "nghiant@hcmiu.edu.vn", Position = "Chuyên viên" },
            new EmployeeDto { Id = 11, FullName = "Ngô Quang Huy", Email = "tqhuy@hcmiu.edu.vn", Position = "Chuyên viên" },
        };

        // GET: api/hrm/v1/Employees
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDto>> GetAll()
        {
            return Ok(_mockEmployees);
        }

        // GET: api/hrm/v1/Employees/1
        [HttpGet("{id}")]
        public ActionResult<EmployeeDto> GetById(int id)
        {
            var employee = _mockEmployees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound($"Không tìm thấy nhân sự với Id = {id}");

            return Ok(employee);
        }
    }
}
