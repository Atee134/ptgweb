using System;
using System.Collections.Generic;

namespace Ptg.Services.Interfaces
{
    public interface IGameManagerService
    {
        Guid CreateGameSession();
        int AddPlayer(Guid sessionId, string playerName);
        List<string> GetPlayerNamesInSession(Guid sessionId);
        void ValidateGameSessionStart(Guid sessionId, Guid terrainDataId);
    }
}
