﻿using Ptg.Common.Dtos;
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
        bool HeightmapExists(Guid id);
        bool SplatmapExists(Guid id);
        bool SessionExists(Guid sessionId);
        void RemoveSession(Guid sessionId);
        void SaveChanges();
    }
}
