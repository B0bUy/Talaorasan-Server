using Microsoft.AspNetCore.SignalR;

namespace Talaorasan.Server.RealTime
{
    public class TalaorasanHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var stationId = Context.GetHttpContext()?.Request.Query["stationId"].ToString();
            return base.OnConnectedAsync();
        }
    }
}
