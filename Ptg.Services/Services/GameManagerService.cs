using Ptg.Common.Dtos;
using Ptg.Common.Exceptions;
using Ptg.DataAccess;
using Ptg.Services.Interfaces;
using System;

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

        public int AddPlayer(Guid sessionId, string playerName)
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

            int playerId = repository.AddPlayer(playerDto);

            repository.SaveChanges();

            return playerId;
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
