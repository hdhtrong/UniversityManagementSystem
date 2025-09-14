namespace EduService.API.Models
{
    public class EduTuitionFeeDto
    {
        public Guid TuitionFeeID { get; set; }
        public string StudentID { get; set; } = string.Empty;
        public Guid SemesterID { get; set; }
        public decimal TuitionAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Debt { get; set; }
        public string? Note { get; set; }
    }
}
