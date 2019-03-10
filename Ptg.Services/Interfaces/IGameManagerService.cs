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
        List<LocationDto> GetLocationsInSession(Guid sessionId);
        void PlayerLoadedMap(string signalrConnectionId, LocationDto location);
        void PlayerChangedLocation(string signalrConnectionId, LocationDto location);
        bool IsEveryoneReadyInSession(Guid sessionId);
        void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId);
        List<string> GetPlayerNamesInSession(Guid sessionId);
        void ValidateGameSessionStart(Guid sessionId, Guid terrainDataId);
    }
}
