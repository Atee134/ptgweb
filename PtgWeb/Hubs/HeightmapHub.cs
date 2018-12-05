using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace PtgWeb.Hubs
{
    public class HeightmapHub : Hub
    {
        double[][] testHeightMap = new double[][] { new[] { 1d, 2d }, new[] { 3d, 4d }, new[] { 5d, 6d }, };

        public async Task PositionChanged(Vector3 position)
        {
            await Clients.All.SendAsync("heightmapData", testHeightMap);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
