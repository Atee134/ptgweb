using Ptg.Common.Dtos;
using Ptg.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace Ptg.DataAccess
{
    public class RepositoryInMemory : IRepository
    {
        private readonly Dictionary<Guid, Heightmap> heightmaps;
        private readonly Dictionary<Guid, Splatmap> splatmaps;

        public RepositoryInMemory()
        {
            heightmaps = new Dictionary<Guid, Heightmap>();
            splatmaps = new Dictionary<Guid, Splatmap>();
        }

        // TODO use automapper

        public void AddHeightmap(HeightmapDto heightmapDto)
        {
            var heightmap = new Heightmap
            {
                Id = heightmapDto.Id,
                Width = heightmapDto.Width,
                Height = heightmapDto.Height,
                HeightmapFloatArray = heightmapDto.HeightmapFloatArray,
                HeightmapByteArray = heightmapDto.HeightmapByteArray
            };

            heightmaps.Add(heightmapDto.Id, heightmap);
        }

        public void AddSplatmap(SplatmapDto splatmapDto)
        {
            var splatmap = new Splatmap
            {
                Id = splatmapDto.Id,
                Width = splatmapDto.Width,
                Height = splatmapDto.Height,
                SplatmapByteArray = splatmapDto.SplatmapByteArray
            };

            splatmaps.Add(splatmapDto.Id, splatmap);
        }

        public HeightmapDto GetHeightmap(Guid id)
        {
            var heightmap = heightmaps[id];

            return new HeightmapDto
            {
                Id = heightmap.Id,
                Width = heightmap.Width,
                Height = heightmap.Height,
                HeightmapFloatArray = heightmap.HeightmapFloatArray,
                HeightmapByteArray = heightmap.HeightmapByteArray
            };
        }

        public SplatmapDto Getsplatmap(Guid id)
        {
            var splatmap = splatmaps[id];

            return new SplatmapDto
            {
                Id = splatmap.Id,
                Width = splatmap.Width,
                Height = splatmap.Height,
                SplatmapByteArray = splatmap.SplatmapByteArray
            };
        }

        public void SaveChanges()
        {
            return;
        }
    }
}
