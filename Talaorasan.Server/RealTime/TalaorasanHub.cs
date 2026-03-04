using Microsoft.AspNetCore.SignalR;
using Talaorasan.Server.RealTime.Connection;

namespace Talaorasan.Server.RealTime
{
    public class TalaorasanHub : Hub
    {
        private readonly IConnectionManager _manager;
        public TalaorasanHub(IConnectionManager manager)
        {
            _manager = manager;
        }
        public override Task OnConnectedAsync()
        {
            var deviceId = Context.GetHttpContext()?.Request.Query["deviceId"].ToString();
            if (!string.IsNullOrEmpty(deviceId))
                _manager.AddConnection(deviceId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
