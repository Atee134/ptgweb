using Ptg.Common.Dtos;
using Ptg.Common.Exceptions;
using Ptg.DataAccess;
using Ptg.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ptg.Services.Services
{
    public class GameManagerService : IGameManagerService
    {
        private readonly IRepository repository;

        public GameManagerService(IRepository repository)
        {
            this.repository = repository;
        }

        public Guid CreateGameSession()
        {
            Guid sessionId = Guid.NewGuid();

            repository.AddSession(sessionId);

            return sessionId;
        }

        public void AddPlayer(Guid sessionId, string playerName)
        {
            if (!repository.SessionExists(sessionId))
            {
                throw new PtgNotFoundException($"Session with ID: {sessionId} does not exist.");
            }

            var playerDto = new PlayerDto
            {
                SessionId = sessionId,
                Name = playerName
            };

            repository.AddPlayer(playerDto);

            repository.SaveChanges();
        }

        public void PlayerLoadedMap(string signalrConnectionId, LocationDto location)
        {
            repository.AddPlayerLocation(signalrConnectionId, location);
            repository.SetPlayerLoaded(signalrConnectionId, true);

            repository.SaveChanges();
        }

        public void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId)
        {
            repository.AddSignalrConnectionIdToPlayer(sessionId, playerName, connectionId);

            repository.SaveChanges();
        }

        public bool IsEveryoneReadyInSession(Guid sessionId)
        {
            return repository.PlayersInSessionNotReady(sessionId) == 0;
        }

        public void RemovePlayer(Guid sessionId, string playerName)
        {
            repository.RemovePlayer(sessionId, playerName);

            if (repository.PlayerCountInSession(sessionId) == 0)
            {
                repository.RemoveSession(sessionId);
            }
        }

        public PlayerDto GetPlayer(Guid sessionId, string playerName)
        {
            return repository.GetPlayer(sessionId, playerName);
        }

        public PlayerDto GetPlayer(string signalrConnectionId)
        {
            return repository.GetPlayer(signalrConnectionId);
        }

        public List<LocationDto> GetLocationsInSession(Guid sessionId)
        {
            return repository.GetLocationsInSession(sessionId);
        }

        public List<string> GetPlayerNamesInSession(Guid sessionId)
        {
            var players = repository.GetPlayers(sessionId);

            return players.Select(p => p.Name).ToList();
        }

        public void ValidateGameSessionStart(Guid sessionId, Guid terrainDataId)
        {
            if (!repository.SessionExists(sessionId) || !repository.HeightmapExists(terrainDataId) || !repository.SplatmapExists(terrainDataId))
            {
                throw new PtgNotFoundException($"Game session not found, or terrain data is not ready yet");
            }
        }
    }
}
