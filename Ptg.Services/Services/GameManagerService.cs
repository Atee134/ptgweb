using Ptg.Common.Dtos;
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
            var playerDto = new PlayerDto
            {
                SessionId = sessionId,
                Name = playerName
            };

            int playerId = repository.AddPlayer(playerDto);

            repository.SaveChanges();

            return playerId;
        }
    }
}
