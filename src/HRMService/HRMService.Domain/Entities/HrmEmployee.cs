using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace HRMService.Domain.Entities
{
    [Table("Employees")]
    [Index(nameof(Code), IsUnique = true)]
    public class HrmEmployee : AuditableEntity
    {
        public HrmEmployee()
        {
            ID = Guid.NewGuid();
        }

        public HrmEmployee(string code)
        {
            ID = Guid.NewGuid();
            Code = code;
        }

        [Key]
        public Guid ID { get; set; }

        [MaxLength(20)]
        public required string Code { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; } // Tên

        [MaxLength(100)]
        public string? LastName { get; set; } // Họ và tên đệm

        [MaxLength(100)]
        public required string Fullname { get; set; } // Họ và tên đầy đủ

        [MaxLength(50)]
        public required string DOB { get; set; } // Ngày tháng năm sinh

        [MaxLength(50)]
        public required string Gender { get; set; } // Giới tính

        [MaxLength(150)]
        public required string WorkEmail { get; set; } // Email

        [MaxLength(150)]
        public string? PersonalEmail { get; set; } // Email

        [MaxLength(100)]
        public string? Phone { get; set; } // Điện thoại

        [MaxLength(50)]
        public required string IdentityNumber { get; set; } // Số CCCD/Hộ chiếu

        [MaxLength(50)]
        public string? SocialInsuranceNumber { get; set; } // Số sổ bảo hiểm

        [MaxLength(100)]
        public string? BankBranch { get; set; } // Chi nhánh Đông Sài Gòn

        [MaxLength(100)]
        public string? BankName { get; set; } // BIDV, Techcombank

        [MaxLength(100)]
        public string? BankAccountNumber { get; set; } // 3141498058

        [MaxLength(100)]
        public string? Nationality { get; set; } // Quốc tịch

        [MaxLength(50)]
        public string? Ethnicity { get; set; } // Dân tộc

        [MaxLength(100)]
        public string? Religion { get; set; } // Tôn giáo

        [MaxLength(100)]
        public string? ProvincePermanentAddress { get; set; } // Tỉnh/thành phố (thường trú)
        [MaxLength(100)]
        public string? DistrictPermanentAddress { get; set; } // Quận/Huyện (thường trú)
        [MaxLength(100)]
        public string? WardPermanentAddress { get; set; } // Xã/Phường (thường trú)
        [MaxLength(100)]
        public string? HouseNumberPermanentAddress { get; set; } // Số nhà (thường trú)

        [MaxLength(100)]
        public string? ProvincePlaceOfBirth { get; set; } // Tỉnh/thành phố (Nơi sinh)
        [MaxLength(100)]
        public string? DistrictPlaceOfBirth { get; set; } // Quận/Huyện (Nơi sinh)
        [MaxLength(100)]
        public string? WardPlaceOfBirth { get; set; } // Xã/Phường (Nơi sinh)

        [MaxLength(100)]
        public string? ProvinceHometown { get; set; } // Tỉnh/thành phố (Quê quán)
        [MaxLength(100)]
        public string? DistrictHometown { get; set; } // Quận/Huyện (Quê quán)
        [MaxLength(100)]
        public string? WardHometown { get; set; } // Xã/Phường (Quê quán)

        [MaxLength(50)]
        public string? AcademicDegree { get; set; } // Học vị

        [MaxLength(50)]
        public string? AcademicTitle { get; set; } // Học hàm

        [MaxLength(50)]
        public string? JoinYouthUnionDate { get; set; } // Ngày vào đoàn
        [MaxLength(50)]
        public string? JoinPartyDate { get; set; } // Ngày vào đảng
        [MaxLength(50)]
        public string? OfficialPartyDate { get; set; } // Ngày vào đảng chính thức
        [MaxLength(50)]
        public string? StartedWorkDate { get; set; } // Ngày vào trường

        [MaxLength(100)]
        public string? Position { get; set; } // Chức vụ

        [Required]
        public Guid? DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public virtual HrmDepartment? Department { get; set; }
    }
}
