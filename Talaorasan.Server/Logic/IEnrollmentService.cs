using Talaorasan.Server.Data;
using Talaorasan.Shared.Response;

namespace Talaorasan.Server.Logic
{
    public interface IEnrollmentService
    {
    }
    public class EnrollmentService : IEnrollmentService
    {
        private readonly TalaorasanDbContext _db;
        public EnrollmentService(TalaorasanDbContext db)
        {
            _db = db;
        }

        //public async Task<TransactionResponseDto<Guid>> ManageEnrollmentImage()
        //{
        //
        //}
    }
}
