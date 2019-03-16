using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Ptg.DataAccess;
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
        private readonly IRepository repository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IHubContext<GameManagerHub> gameManagerHubContext;

        public LocationChangedBroadcasterService(IRepository repository, IServiceScopeFactory serviceScopeFactory, IHubContext<GameManagerHub> gameManagerHubContext)
        {
            this.repository = repository;
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
                gameManagerHubContext.Clients.Group(sessionId.ToString()).SendAsync("locationsChanged", repository.GetLocationsInSession(sessionId));
                await Task.Delay(1000 / TICK_RATE);
            }
        }
    }
}
