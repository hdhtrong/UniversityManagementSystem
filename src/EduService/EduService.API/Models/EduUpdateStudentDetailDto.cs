using System.ComponentModel.DataAnnotations;

namespace EduService.API.Models
{
    public class EduUpdateStudentDetailDto
    {
        public required string DOB { get; set; }
        public string? Email { get; set; } // Email
        public string? Phone { get; set; } // Điện thoại
        public string? IdentityNumber { get; set; }
        public string? ProvincePermanentAddress { get; set; }
        public string? DistrictPermanentAddress { get; set; } // Quận/Huyện (thường trú)
        public string? WardPermanentAddress { get; set; } // Xã/Phường (thường trú)
        public string? HouseNumberPermanentAddress { get; set; } // Số nhà (thường trú)

        [MaxLength(250)]
        public string? TemporaryAddress { get; set; } // Nơi tạm trú (nếu không nội trú)

        [MaxLength(100)]
        public string? ProvincePlaceOfBirth { get; set; } // Tỉnh/thành phố (Nơi sinh)

        [MaxLength(100)]
        public string? ProvinceHometown { get; set; } // Tỉnh/thành phố (Quê quán)

        [MaxLength(100)]
        public string? Nationality { get; set; } // Quốc tịch

        [MaxLength(50)]
        public string? Ethnicity { get; set; } // Dân tộc

        [MaxLength(100)]
        public string? Religion { get; set; } // Tôn giáo
    }
}
