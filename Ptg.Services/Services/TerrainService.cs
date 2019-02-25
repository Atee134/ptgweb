using System;
using Ptg.Common.Dtos;
using Ptg.Common.Dtos.Request;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
using Ptg.SplatmapGenerator.Interfaces;

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

        public Guid Generate(DiamondSquareHeightmapRequestDto requestDto)
        {
            var heightmapDto = diamondSquareGenerator.Generate(requestDto.Size, requestDto.OffsetRange, requestDto.OffsetReductionRate);

            return CreateHeightmap(heightmapDto);
        }

        public Guid Generate(FaultHeightmapRequestDto requestDto)
        {
            var heightmapDto = faultHeightmapGenerator.GenerateHeightmap(requestDto.Width, requestDto.Height, requestDto.IterationCount, requestDto.OffsetPerIteration);

            return CreateHeightmap(heightmapDto);
        }

        public Guid Generate(RandomHeightmapRequestDto requestDto)
        {
            var heightmapDto = randomHeightmapGenerator.GenerateHeightmap(requestDto.Width, requestDto.Height);

            return CreateHeightmap(heightmapDto);
        }

        private Guid CreateHeightmap(HeightmapDto heightmapDto)
        {
            var id = Guid.NewGuid();

            heightmapDto.Id = id;

            repository.AddHeightmap(heightmapDto);

            repository.SaveChanges();

            return id;
        }

        private void GenerateSplatmap(HeightmapDto heightmapDto)
        {
            //var splatmap = randomSplatmapGenerator.Generate(heightmap);

            //repository.AddSplatmap(splatmap);
        }

    }
}