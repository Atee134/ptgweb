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
        private readonly static object chunkGenerationLockObj = new object();

        private readonly IRepository repository;
        private readonly IRandomSplatmapGenerator randomSplatmapGenerator;
        private readonly IHeightBasedSplatmapGenerator heightBasedSplatmapGenerator;
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;
        private readonly IOpenSimplexGenerator openSimplexGenerator;

        public TerrainService(IRepository repository, IRandomSplatmapGenerator randomSplatmapGenerator, IHeightBasedSplatmapGenerator heightBasedSplatmapGenerator, IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator, IOpenSimplexGenerator openSimplexGenerator)
        {
            this.repository = repository;
            this.randomSplatmapGenerator = randomSplatmapGenerator;
            this.heightBasedSplatmapGenerator = heightBasedSplatmapGenerator;
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
            this.openSimplexGenerator = openSimplexGenerator;
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

        public Guid Generate(OpenSimplexRequestDto requestDto)
        {
            var heightmapDto = openSimplexGenerator.Generate(
                requestDto.Width,
                requestDto.Height,
                requestDto.Seed,
                requestDto.Scale,
                requestDto.Octaves,
                requestDto.Persistance,
                requestDto.Lacunarity
            );

            var id = CreateHeightmap(heightmapDto);

            if (requestDto.Infinite)
            {
                repository.AddBaseHeightmapChunk(new BaseHeightmapChunkDto
                {
                    Id = id,
                    Width = requestDto.Width,
                    Height = requestDto.Height,
                    Seed = requestDto.Seed,
                    Scale = requestDto.Scale,
                    Octaves = requestDto.Octaves,
                    Persistance = requestDto.Persistance,
                    Lacunarity = requestDto.Lacunarity,
                    Heightmap = heightmapDto
                });

                repository.SaveChanges();
            }

            return id;
        }

        public byte[] GetHeightmapChunk(HeightmapChunkRequestDto requestDto)
        {
            lock (chunkGenerationLockObj)
            {
                if (repository.IsHeightmapChunkExists(requestDto.BaseHeightmapChunkId, requestDto.OffsetX, requestDto.OffsetZ))
                {
                    return repository.GetHeightmapChunk(requestDto.BaseHeightmapChunkId, requestDto.OffsetX, requestDto.OffsetZ);
                }
                else
                {
                    var heightmapDto = CreateHeightmapChunk(requestDto.BaseHeightmapChunkId, requestDto.OffsetX, requestDto.OffsetZ);

                    repository.AddHeightmapChunk(requestDto.BaseHeightmapChunkId, requestDto.OffsetX, requestDto.OffsetZ, heightmapDto);

                    repository.SaveChanges();

                    return heightmapDto.HeightmapByteArray;
                }
            }
        }

        private HeightmapDto CreateHeightmapChunk(Guid baseChunkId, int offsetX, int offsetZ)
        {
            var baseChunk = repository.GetBaseHeightmapChunk(baseChunkId);

            var heightmapDto = openSimplexGenerator.Generate(
                   baseChunk.Width,
                   baseChunk.Height,
                   baseChunk.Seed,
                   baseChunk.Scale,
                   baseChunk.Octaves,
                   baseChunk.Persistance,
                   baseChunk.Lacunarity,
                   baseChunk.Width * offsetX,
                   baseChunk.Height * offsetZ
               );

            heightmapDto.Id = Guid.NewGuid();

            return heightmapDto;
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
            var splatmap = heightBasedSplatmapGenerator.Generate(heightmapDto.HeightmapOriginalArray, 0.3f, 0.17f, 0.16f);
            splatmap.Id = heightmapDto.Id;

            repository.AddSplatmap(splatmap);
        }
    }
}