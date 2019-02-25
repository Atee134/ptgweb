using System;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
using Ptg.SplatmapGenerator.Interfaces;
using PtgWeb.Common.Dtos.Request;

namespace Ptg.Services.Services
{
    public class TerrainService : ITerrainService
    {
        private readonly IRepository repository;
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IRandomSplatmapGenerator randomSplatmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;

        public TerrainService(IRepository repository, IRandomHeightmapGenerator randomHeightmapGenerator, IRandomSplatmapGenerator randomSplatmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator)
        {
            this.repository = repository;
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.randomSplatmapGenerator = randomSplatmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
        }

        public Guid GenerateDiamondSquareTerrain(DiamondSquareHeightmapRequestDto requestDto)
        {
            var id = Guid.NewGuid();

            var heightmap = diamondSquareGenerator.Generate(requestDto.Size, requestDto.OffsetRange, requestDto.OffsetReductionRate);
            heightmap.Id = id;

            repository.AddHeightmap(heightmap);

            //var splatmap = randomSplatmapGenerator.Generate(heightmap);

            //repository.AddSplatmap(splatmap);

            repository.SaveChanges();

            return id;
        }
    }
}