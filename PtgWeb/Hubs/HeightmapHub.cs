using Microsoft.AspNetCore.SignalR;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace PtgWeb.Hubs
{
    public class HeightmapHub : Hub
    {
        public HeightmapHub()
        {
         
        }

        public async Task PositionChanged(Vector3 position)
        {
            await Clients.All.SendAsync("receiveTerrainDataGuid", new Guid());
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
