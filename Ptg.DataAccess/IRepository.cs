using Ptg.Common.Dtos;
using System;
using System.Collections.Generic;

namespace Ptg.DataAccess
{
    public interface IRepository
    {
        void AddHeightmap(HeightmapDto heightmapDto);
        void AddBaseHeightmapChunk(BaseHeightmapChunkDto baseChunkDto);
        void AddHeightmapChunk(Guid baseChunkId, int offsetX, int offsetZ, HeightmapDto heightmapDto);
        bool IsHeightmapChunkExists(Guid baseChunkId, int offsetX, int offsetZ);
        void AddSplatmap(SplatmapDto splatmapDto);
        void AddPlayer(PlayerDto playerDto);
        void AddPlayerLocation(string signalrConnectionId, LocationDto location);
        void UpdatePlayerLocation(string signalrConnectionId, LocationDto location);
        void SetPlayerLoaded(string signalrConnectionId, bool loaded);
        void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId);
        void RemovePlayer(Guid sessionId, string playerName);
        int PlayersInSessionNotReady(Guid sessionId);
        PlayerDto GetPlayer(Guid sessionId, string playerName);
        PlayerDto GetPlayer(string signalrConnectionId);
        List<LocationDto> GetLocationsInSession(Guid sessionId);
        void AddSession(Guid sessionId);
        byte[] GetHeightmap(Guid id);
        BaseHeightmapChunkDto GetBaseHeightmapChunk(Guid id);
        byte[] GetHeightmapChunk(Guid baseChunkId, int offsetX, int offsetZ);
        HeightmapInfoDto GetHeightmapInfo(Guid id);
        SplatmapDto GetSplatmap(Guid id);
        List<PlayerDto> GetPlayers(Guid sessionId);
        bool HeightmapExists(Guid id);
        bool SplatmapExists(Guid id);
        bool SessionExists(Guid sessionId);
        int PlayerCountInSession(Guid sessionId);
        void RemoveSession(Guid sessionId);
        void UpdateSessionState(Guid sessionId, bool inGame);
        bool IsSessionInGame(Guid sessionId);
        void SaveChanges();
    }
}
