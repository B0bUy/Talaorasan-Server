using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Talaorasan.Server.Entities
{
    public class DeviceRegistrationRequest
    {
        [Key]
        public Guid DeviceRegistrationRequestId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Device))]
        public Guid DeviceId { get; set; }

        public Device Device { get; set; } = null!;

        [Required]
        public DeviceRegistrationRequestStatus RequestStatus { get; set; } = DeviceRegistrationRequestStatus.Pending; // Pending/Approved/Rejected

        [MaxLength(500)]
        public string? RequestedByInfo { get; set; }

        public DateTime RequestedUtc { get; set; } = DateTime.UtcNow;
        public string? ApprovedByUserId { get; set; } // AspNetUsers.Id
        public DateTime? ApprovedUtc { get; set; }

        [MaxLength(300)]
        public string? RejectionReason { get; set; }
    }

    public enum DeviceRegistrationRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
