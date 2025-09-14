using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("TuitionFees")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduTuitionFee : AuditableEntity
    {
        [Key]
        public Guid TuitionFeeID { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; } // StudentID_SemesterCode

        public required Guid StudentID { get; set; } // FK tới Student

        public required Guid SemesterID { get; set; } // FK tới Semester

        [Column(TypeName = "decimal(18,2)")]
        public decimal TuitionAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountToPay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Debt { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        // Navigation properties
        [ForeignKey(nameof(StudentID))]
        public EduStudent? Student { get; set; }

        [ForeignKey(nameof(SemesterID))]
        public EduSemester? Semester { get; set; }

        public ICollection<EduInvoice>? Invoices { get; set; }
    }
}
