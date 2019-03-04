using Ptg.Common.Dtos;
using System;
using System.Collections.Generic;

namespace Ptg.Services.Interfaces
{
    public interface IGameManagerService
    {
        Guid CreateGameSession();
        void AddPlayer(Guid sessionId, string playerName);
        void RemovePlayer(Guid sessionId, string playerName);
        PlayerDto GetPlayer(Guid sessionId, string playerName);
        PlayerDto GetPlayer(string signalrConnectionId);
        void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId);
        List<string> GetPlayerNamesInSession(Guid sessionId);
        void ValidateGameSessionStart(Guid sessionId, Guid terrainDataId);
    }
}
