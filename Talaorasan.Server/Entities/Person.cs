using System.ComponentModel.DataAnnotations;

namespace Talaorasan.Server.Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = null;
        public string? ExtensionName { get; set; } = null;
        public string? Prefix { get; set; } = null;
        public string? Suffix { get; set; } = null;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<FaceTemplate> FaceTemplates { get; set; } = new List<FaceTemplate>();
        public ICollection<EnrollmentImage> EnrollmentImages { get; set; } = new List<EnrollmentImage>();
        public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    }
}
