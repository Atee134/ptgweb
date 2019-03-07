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
        private readonly IRandomSplatmapGenerator randomSplatmapGenerator;
        private readonly IHeightBasedSplatmapGenerator heightBasedSplatmapGenerator;
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;

        public TerrainService(IRepository repository, IRandomSplatmapGenerator randomSplatmapGenerator, IHeightBasedSplatmapGenerator heightBasedSplatmapGenerator, IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator)
        {
            this.repository = repository;
            this.randomSplatmapGenerator = randomSplatmapGenerator;
            this.heightBasedSplatmapGenerator = heightBasedSplatmapGenerator;
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
        }

        public Guid Generate(DiamondSquareHeightmapRequestDto requestDto)
        {
            var heightmapDto = diamondSquareGenerator.Generate(requestDto.Size, requestDto.OffsetRange, requestDto.OffsetReductionRate);

            return CreateHeightmap(heightmapDto);
        }

        public HeightmapDto GenerateTEST(DiamondSquareHeightmapRequestDto requestDto)
        {
            var heightmapDto = diamondSquareGenerator.Generate(requestDto.Size, requestDto.OffsetRange, requestDto.OffsetReductionRate);

            return heightmapDto;
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
            GenerateSplatmap(heightmapDto);

            repository.SaveChanges();

            return id;
        }

        private void GenerateSplatmap(HeightmapDto heightmapDto)
        {
            var splatmap = heightBasedSplatmapGenerator.Generate(heightmapDto, 0.3f, 0.17f, 0.16f);
            splatmap.Id = heightmapDto.Id;

            repository.AddSplatmap(splatmap);
        }
    }
}