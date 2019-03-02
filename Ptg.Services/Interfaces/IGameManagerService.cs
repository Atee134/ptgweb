using System;

namespace Ptg.Services.Interfaces
{
    public interface IGameManagerService
    {
        Guid CreateGameSession();
        int AddPlayer(Guid sessionId, string playerName);
        void ValidateGameSessionStart(Guid sessionId, Guid terrainDataId);
    }
}
