using Microsoft.AspNetCore.SignalR;
using Ptg.Common.Dtos.Signalr;
using Ptg.Services.Interfaces;
using PtgWeb.HubServices;
using System;
using System.Threading.Tasks;

namespace PtgWeb.Hubs
{
    public class GameManagerHub : Hub
    {
        private static readonly object startGameLockObj = new object();
        private readonly IGameManagerService gameManagerService;
        private readonly ILocationChangedBroadcasterService locationChangedBroadcasterService;

        public GameManagerHub(IGameManagerService gameManagerService, ILocationChangedBroadcasterService locationChangedBroadcasterService)
        {
            this.gameManagerService = gameManagerService;
            this.locationChangedBroadcasterService = locationChangedBroadcasterService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var player = gameManagerService.GetPlayer(Context.ConnectionId);
            if (player != null)
            {
                gameManagerService.RemovePlayer(player.SessionId, player.Name);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, player.SessionId.ToString());
                await Clients.Group(player.SessionId.ToString()).SendAsync("playerLeft", player.Name);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task JoinSession(JoinGameSessionMessage message)
        {
            gameManagerService.AddSignalrConnectionIdToPlayer(Guid.Parse(message.SessionId), message.PlayerName, Context.ConnectionId);
            await Clients.Group(message.SessionId).SendAsync("playerJoined", message.PlayerName);
            await Groups.AddToGroupAsync(Context.ConnectionId, message.SessionId);
        }

        public async Task MapLoaded(MapLoadedMessage message)
        {
            gameManagerService.PlayerLoadedMap(Context.ConnectionId, message.Location);
            var player = gameManagerService.GetPlayer(Context.ConnectionId);

            await Clients.Client(Context.ConnectionId).SendAsync("receivePlayerId", player.Id);

            Guid sessionId = Guid.Parse(message.SessionId);

            Task messageTask = null;
            lock (startGameLockObj)
            {
                if (!gameManagerService.IsSessionInGame(sessionId) && gameManagerService.IsEveryoneReadyInSession(sessionId))
                {
                    gameManagerService.UpdateSessionState(sessionId, true);
                    messageTask = Clients.Group(message.SessionId).SendAsync("startGame", gameManagerService.GetLocationsInSession(sessionId));
                }
            }

            if (messageTask != null)
            {
                await messageTask;
                locationChangedBroadcasterService.StartBroadcasting(sessionId);
            }
        }

        public void LocationChanged(LocationChangedMessage message)
        {
            gameManagerService.PlayerChangedLocation(Context.ConnectionId, message.Location);
        }
    }
}
