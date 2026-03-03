using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Talaorasan.Server.Entities
{
    public class AttendanceLog
    {
        [Key]
        public Guid AttendanceLogId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Device))]
        public Guid DeviceId { get; set; }

        public Device Device { get; set; } = null!;

        [ForeignKey(nameof(Person))]
        public Guid? MatchedPersonId { get; set; } // nullable for UNKNOWN

        public Person? Person { get; set; }

        [Required]
        public LogType Action { get; set; } = LogType.None; // TIME_IN / TIME_OUT

        [Required]
        public StatusType Status { get; set; } = StatusType.Unknown; // MATCHED / UNKNOWN / OVERRIDDEN

        public double BestScore { get; set; }
        public double SecondBestScore { get; set; }
        public double ThresholdUsed { get; set; }
        public double MarginUsed { get; set; }

        public byte[]? ProbeEmbedding { get; set; } // optional

        [MaxLength(500)]
        public string? ProbeImagePath { get; set; }
        public DateTime CapturedUtc { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }

    public enum LogType
    {
        None = 0,
        TimeIn,
        TimeOut
    }
    public enum StatusType
    {
        Unknown = 0,
        Matched,
        Overridden
    }
}
