using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Domain.Entities
{
    [Table("Rooms")]
    [Index(nameof(RoomName), IsUnique = true)]
    public class EduRoom : AuditableEntity
    {
        [Key]
        public Guid RoomID { get; set; }

        [MaxLength(50)]
        public string? Building { get; set; }

        public int Floor { get; set; }

        [MaxLength(50)]
        public string? RoomName { get; set; }

        public int Capacity { get; set; }
    }
}
