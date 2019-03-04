using Ptg.Common.Dtos;
using System;
using System.Collections.Generic;

namespace Ptg.DataAccess
{
    public interface IRepository
    {
        void AddHeightmap(HeightmapDto heightmapDto);
        void AddSplatmap(SplatmapDto splatmapDto);
        void AddPlayer(PlayerDto playerDto);
        void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId);
        void RemovePlayer(Guid sessionId, string playerName);
        PlayerDto GetPlayer(Guid sessionId, string playerName);
        PlayerDto GetPlayer(string signalrConnectionId);
        void AddSession(Guid sessionId);
        HeightmapDto GetHeightmap(Guid id);
        SplatmapDto Getsplatmap(Guid id);
        List<PlayerDto> GetPlayers(Guid sessionId);
        bool HeightmapExists(Guid id);
        bool SplatmapExists(Guid id);
        bool SessionExists(Guid sessionId);
        int PlayerCountInSession(Guid sessionId);
        void RemoveSession(Guid sessionId);
        void SaveChanges();
    }
}
