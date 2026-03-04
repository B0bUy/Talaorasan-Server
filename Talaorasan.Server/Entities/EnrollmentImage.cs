using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Talaorasan.Shared.Transaction;

namespace Talaorasan.Server.Entities
{
    public class EnrollmentImage
    {
        [Key]
        public Guid EnrollmentImageId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; set; }
        public Person Person { get; set; } = null!;

        [Required]
        public string ImagePath { get; set; } = string.Empty;
        public string? MimeType { get; set; }
        public FacePose Pose { get; set; }

        public int? ImageSizeBytes { get; set; }
        public DateTime CapturedUtc { get; set; } = DateTime.UtcNow;
    }
}
