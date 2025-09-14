namespace EduService.API.Models
{
    public class EduInvoiceDto
    {
        public Guid InvoiceID { get; set; }
        public string StudentID { get; set; } = string.Empty;
        public string SemesterCode { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Note { get; set; }

        public Guid TuitionFeeID { get; set; }
    }
}
