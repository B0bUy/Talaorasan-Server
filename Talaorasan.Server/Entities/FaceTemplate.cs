using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Talaorasan.Server.Entities
{
    public class FaceTemplate
    {
        [Key]
        public Guid FaceTemplateId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; set; }
        public Person Person { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = "insightface_arcface";

        [Required]
        public byte[] Embedding { get; set; } = Array.Empty<byte>(); // float32[512]
        public int EmbeddingDim { get; set; } = 512;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
