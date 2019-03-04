﻿using Microsoft.AspNetCore.SignalR;
using Ptg.Common.Dtos.Signalr;
using Ptg.Common.Exceptions;
using Ptg.DataAccess;
using Ptg.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PtgWeb.Hubs
{
    public class GameManagerHub : Hub
    {
        private readonly IGameManagerService gameManagerService;

        public GameManagerHub(IGameManagerService gameManagerService)
        {
            this.gameManagerService = gameManagerService;
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

        public async Task SendTerrainDataIdToSession(Guid terrainDataId, Guid sessionId)
        {
            await Clients.Group(sessionId.ToString()).SendAsync("receiveTerrainDataGuid", terrainDataId);
        }
    }
}
