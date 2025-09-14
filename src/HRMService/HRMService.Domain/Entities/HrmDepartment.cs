using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HRMService.Domain.Entities
{
    [Table("Departments")]
    [Index(nameof(Code), IsUnique = true)]
    [Index(nameof(ShortName), IsUnique = true)]
    public class HrmDepartment
    {
        [Key]
        public Guid ID { get; set; }
    
        public int Order { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string EnglishName { get; set; }

        [MaxLength(100)]
        public string ShortName { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; } // Khoa, phòng ban, Trung tâm

        public string ParentCode { get; set; } // Đơn vị quản lý trực tiếp

        public int Level { get; set; } // phân cấp đơn vị
    }
}
