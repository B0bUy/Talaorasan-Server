using Microsoft.AspNetCore.Mvc;
using Talaorasan.Server.Data;
using Talaorasan.Shared.Response;
using Talaorasan.Shared.Transaction;

namespace Talaorasan.Server.Logic
{
    public interface IDeviceManagementService
    {
    }
    public class DeviceManagementService : IDeviceManagementService
    {
        private readonly TalaorasanDbContext _db;
        public DeviceManagementService(TalaorasanDbContext db)
        {
            _db = db;
        }
        public async Task<TransactionResponseDto> AddDevice(DeviceDto device, CancellationToken token)
        {
            try
            {
                return new TransactionResponseDto() { Success = false, Message = "" };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TransactionResponseDto() { Success = false, Message = e.Message};
            }
        }
    }
}
