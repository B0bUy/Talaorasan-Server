using Microsoft.AspNetCore.Mvc;
using Talaorasan.Server.Data;
using Talaorasan.Shared.Transaction;
using Talaorasan.Shared.Response;

namespace Talaorasan.Server.Logic
{
    public interface IEnrollmentService
    {
        Task<TransactionResponseDto<Guid>> EnrollImage(EnrollmentDto enrollment, CancellationToken token);
    }
    public class EnrollmentService : IEnrollmentService
    {
        private readonly TalaorasanDbContext _db;
        private readonly FileManagementService _file;
        public EnrollmentService(TalaorasanDbContext db, FileManagementService file)
        {
            _db = db;
            _file = file;
        }

        public async Task<TransactionResponseDto<Guid>> EnrollImage(EnrollmentDto enrollment, CancellationToken token)
        {
            try
            {
                if (enrollment.PersonId == Guid.Empty ||
                  !_db.Persons.Any(c => c.PersonId == enrollment.PersonId))
                    return new TransactionResponseDto<Guid>()
                    {
                        Success = false,
                        Message = "Person not found..."
                    };
                var uploadImage = await _file.Upload(new FileDto()
                {
                    Content = enrollment.Image,
                    Length = enrollment.Image.Length,
                    FileName = enrollment.FileName,
                }, GetFileName(enrollment));
                if(!uploadImage.Success)
                    return new TransactionResponseDto<Guid>()
                    {
                        Success = false,
                        Message = "Something went wrong while enrolling person..."
                    };
                _db.EnrollmentImages.Add(new Entities.EnrollmentImage()
                {
                    PersonId = enrollment.PersonId,
                    ImagePath = uploadImage.Output,
                    ImageSizeBytes = File.ReadAllBytes(uploadImage.Output).Length,
                    MimeType = enrollment.MimeType,
                    Pose = enrollment.Pose,
                    CapturedUtc = DateTime.UtcNow
                });
                await _db.SaveChangesAsync(token);
                return new TransactionResponseDto<Guid>()
                {
                    Success = false,
                    Message = "Person not found."
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TransactionResponseDto<Guid>() { Success = false, Message = e.Message };
            }
        }
        [NonAction]
        private static string GetFileName(EnrollmentDto enrollment) 
        {

            return string.Empty;
        }
    }
}
