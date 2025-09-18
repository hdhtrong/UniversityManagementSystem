using System.ComponentModel.DataAnnotations;

namespace HRMService.API.Models
{
    public class HrmDepartmentDto
    {
        public Guid ID { get; set; }

        public int Order { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public string Category { get; set; } // Khoa, phòng ban, Trung tâm

        public string ParentCode { get; set; } // Đơn vị quản lý trực tiếp

        public int Level { get; set; } // phân cấp đơn vị
    }
}
