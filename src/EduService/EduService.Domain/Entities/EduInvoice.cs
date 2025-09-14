using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("Invoices")]
    [Index(nameof(InvoiceNumber), IsUnique = true)]
    public class EduInvoice : AuditableEntity
    {
        [Key]
        public Guid InvoiceID { get; set; }

        [MaxLength(50)]
        public string? StudentID { get; set; } = string.Empty;

        public string? SemesterCode{ get; set; }

        [MaxLength(50)]
        public required string InvoiceNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public required decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        // FK tới TuitionFee
        public Guid TuitionFeeID { get; set; }

        [ForeignKey(nameof(TuitionFeeID))]
        public EduTuitionFee? TuitionFee { get; set; }
    }
}
