using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace PtgWeb.Hubs
{
    public class GameManagerHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var asd = 5;
            await base.OnConnectedAsync();
        }

        public async Task JoinSession(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task SendTerrainDataIdToSession(Guid terrainDataId, Guid sessionId)
        {
            await Clients.Group(sessionId.ToString()).SendAsync("receiveTerrainDataGuid", terrainDataId);
        }
    }
}
