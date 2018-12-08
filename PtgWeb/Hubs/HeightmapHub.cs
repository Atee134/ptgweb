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
        private static readonly Random random = new Random();

        private double[][] testHeightMap;
        private readonly int heightMapWidth = 128;
        private readonly int heightMapHeight = 128;

        public HeightmapHub()
        {
            testHeightMap = new double[heightMapWidth][];

            for (int i = 0; i < heightMapWidth; i++)
            {
                testHeightMap[i] = new double[heightMapHeight];

                for (int j = 0; j < heightMapHeight; j++)
                {
                    testHeightMap[i][j] = random.NextDouble() * 100;
                }
            }
        }

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
