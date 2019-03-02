using Ptg.Common.Dtos;
using System;

namespace Ptg.DataAccess
{
    public interface IRepository
    {
        void AddHeightmap(HeightmapDto heightmapDto);
        void AddSplatmap(SplatmapDto splatmapDto);
        int AddPlayer(PlayerDto playerDto);
        void AddSession(Guid sessionId);
        HeightmapDto GetHeightmap(Guid id);
        SplatmapDto Getsplatmap(Guid id);
        bool IsInSessions(Guid sessionId);
        void RemoveSession(Guid sessionId);
        void SaveChanges();
    }
}
