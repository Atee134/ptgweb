using Microsoft.AspNetCore.SignalR;
using Ptg.Common.Dtos.Signalr;
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

        public async Task JoinSession(JoinGameSessionMessage message)
        {
            await Clients.Group(message.SessionId).SendAsync("playerJoined", message.PlayerName);
            await Groups.AddToGroupAsync(Context.ConnectionId, message.SessionId);
        }

        public async Task SendTerrainDataIdToSession(Guid terrainDataId, Guid sessionId)
        {
            await Clients.Group(sessionId.ToString()).SendAsync("receiveTerrainDataGuid", terrainDataId);
        }
    }
}
