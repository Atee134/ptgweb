using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Ptg.Services.Interfaces;
using Ptg.Services.Models;
using PtgWeb.Hubs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PtgWeb.HubServices
{
    public class LocationChangedBroadcasterService : ILocationChangedBroadcasterService
    {
        private readonly static int TICK_RATE = 75;

        private readonly List<Broadcaster> broadcasters = new List<Broadcaster>();
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IHubContext<GameManagerHub> gameManagerHubContext;

        public LocationChangedBroadcasterService(IServiceScopeFactory serviceScopeFactory, IHubContext<GameManagerHub> gameManagerHubContext)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.gameManagerHubContext = gameManagerHubContext;
        }

        public void StartBroadcasting(Guid sessionId)
        {
            if (broadcasters.Find(b => b.SessionId == sessionId) != null) return;

            Broadcaster broadcaster = new Broadcaster();
            broadcaster.SessionId = sessionId;

            CancellationTokenSource source = new CancellationTokenSource();
            broadcaster.CancellationTokenSource = source;

            Task task = Task.Run(() => Broadcast(sessionId, source.Token));
            broadcaster.Task = task;

            broadcasters.Add(broadcaster);
        }

        private async Task Broadcast(Guid sessionId, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    IGameManagerService gameManagerService = scope.ServiceProvider.GetService<IGameManagerService>();
                    gameManagerHubContext.Clients.Group(sessionId.ToString()).SendAsync("locationsChanged", gameManagerService.GetLocationsInSession(sessionId));
                }

                await Task.Delay(1000 / TICK_RATE);
            }
        }
    }
}
