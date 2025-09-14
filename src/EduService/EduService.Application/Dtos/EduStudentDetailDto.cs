
namespace EduService.Application.Dtos
{
    public class EduStudentDetailDto
    {
        public string Fullname { get; set; }
        public string DOB { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } 
        public string? StudentID { get; set; }
        public string? IdentityNumber { get; set; }
        public string? Nationality { get; set; }
        public string? Ethnicity { get; set; }
        public string? Religion { get; set; }
        public string? ProvincePermanentAddress { get; set; } // Tỉnh/thành phố (thường trú)
        public string? DistrictPermanentAddress { get; set; } // Quận/Huyện (thường trú)
        public string? WardPermanentAddress { get; set; } // Xã/Phường (thường trú)
        public string? HouseNumberPermanentAddress { get; set; } // Số nhà (thường trú)
        public string? TemporaryAddress { get; set; } // Địa chỉ tạm trú
        public string? EducationType { get; set; } // Loại hình đào tạo
        public string? MajorName { get; set; }
        public string? MajorCode { get; set; }
        public string? ClassCode { get; set; }
        public string? ClassName { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramCode { get; set; }
        public int? TrainingFromYear { get; set; } // Đào tạo từ năm
        public string? DepartmentName { get; set; }
        public string? InstructorEmail { get; set; }
        public string? InstructorName { get; set; }
    }
}
