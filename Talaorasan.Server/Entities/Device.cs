using System.ComponentModel.DataAnnotations;

namespace Talaorasan.Server.Entities
{
    public class Device
    {
        [Key]
        public Guid DeviceId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(80)]
        public string StationCode { get; set; } = string.Empty;
        [MaxLength(150)]
        public string? DeviceName { get; set; }
        [MaxLength(50)]
        public string? Platform { get; set; } // Android, Windows
        [MaxLength(200)]
        public string? InstallIdHash { get; set; }
        [MaxLength(200)]
        public string DeviceTokenHash { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public DateTime? ApprovedUtc { get; set; }
        public DateTime? LastSeenUtc { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    }
}
