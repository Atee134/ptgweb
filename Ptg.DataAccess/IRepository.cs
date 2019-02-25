using Ptg.Common.Dtos;
using System;

namespace Ptg.DataAccess
{
    public interface IRepository
    {
        void AddHeightmap(HeightmapDto heightmapDto);
        void AddSplatmap(SplatmapDto splatmapDto);
        HeightmapDto GetHeightmap(Guid id);
        SplatmapDto Getsplatmap(Guid id);
        void SaveChanges();
    }
}
