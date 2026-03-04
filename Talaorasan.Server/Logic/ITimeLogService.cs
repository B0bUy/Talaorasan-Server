using Talaorasan.Server.Data;

namespace Talaorasan.Server.Logic
{
    public interface ITimeLogService
    {
    }
    public class TimeLogService : ITimeLogService
    {
        private readonly TalaorasanDbContext _db;
        public TimeLogService(TalaorasanDbContext db)
        {
            _db = db;
        } 
    }
}
