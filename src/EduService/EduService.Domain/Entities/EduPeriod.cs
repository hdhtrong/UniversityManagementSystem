using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("Periods")]
    public class EduPeriod : AuditableEntity
    {
        [Key]
        public int PeriodNumber { get; set; }

        public TimeSpan? StartTime { get; set; }
        
        public TimeSpan? EndTime { get; set; }
    }
}
