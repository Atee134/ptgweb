using Moq;
using NUnit.Framework;
using Ptg.Common.Dtos;
using Ptg.Common.Dtos.Request;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Services;
using Ptg.SplatmapGenerator.Interfaces;
using System;

namespace Ptg.Services.Tests
{
    [TestFixture()]
    public class TerrainServiceTests
    {
        [TestCase(TestName = "Is ID being generated")]
        public void IsIdGenerated()
        {
            var mockHeightmapGenerator = new Mock<IDiamondSquareGenerator>();
            mockHeightmapGenerator.Setup(x => x.Generate(
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<int?>()
                )).Returns(new HeightmapDto());

            var mockSplatmapGenerator = new Mock<IHeightBasedSplatmapGenerator>();
            mockSplatmapGenerator.Setup(x => x.Generate(
                It.IsAny<float[,]>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<float>()
                )).Returns(new SplatmapDto());

            var mockRepository = new Mock<IRepository>();

            var terrainService = new TerrainService(mockRepository.Object, mockSplatmapGenerator.Object, null, mockHeightmapGenerator.Object, null);

            var id = terrainService.Generate(new DiamondSquareHeightmapRequestDto { Size = 17, OffsetRange = 10, OffsetReductionRate = 0.5f });

            Assert.AreNotEqual(id, Guid.Empty);
        }

        [TestCase(TestName = "Is heightmap stored")]
        public void IsHeightmapStored()
        {
            var heightmap = new HeightmapDto();
            var splatmap = new SplatmapDto();

            var mockHeightmapGenerator = new Mock<IDiamondSquareGenerator>();
            mockHeightmapGenerator.Setup(x => x.Generate(
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<int?>()
                )).Returns(heightmap);

            var mockSplatmapGenerator = new Mock<IHeightBasedSplatmapGenerator>();
            mockSplatmapGenerator.Setup(x => x.Generate(
                It.IsAny<float[,]>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<float>()
                )).Returns(splatmap);

            var mockRepository = new Mock<IRepository>();

            var terrainService = new TerrainService(mockRepository.Object, mockSplatmapGenerator.Object, null, mockHeightmapGenerator.Object, null);

            var id = terrainService.Generate(new DiamondSquareHeightmapRequestDto { Size = 17, OffsetRange = 10, OffsetReductionRate = 0.5f });
            mockRepository.Verify(mock => mock.AddHeightmap(heightmap), Times.Once());
        }

        [TestCase(TestName = "Is splatmap stored")]
        public void IsSplatmapStored()
        {
            var heightmap = new HeightmapDto();
            var splatmap = new SplatmapDto();

            var mockHeightmapGenerator = new Mock<IDiamondSquareGenerator>();
            mockHeightmapGenerator.Setup(x => x.Generate(
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<int?>()
                )).Returns(heightmap);

            var mockSplatmapGenerator = new Mock<IHeightBasedSplatmapGenerator>();
            mockSplatmapGenerator.Setup(x => x.Generate(
                It.IsAny<float[,]>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<float>()
                )).Returns(splatmap);

            var mockRepository = new Mock<IRepository>();

            var terrainService = new TerrainService(mockRepository.Object, mockSplatmapGenerator.Object, null, mockHeightmapGenerator.Object, null);

            var id = terrainService.Generate(new DiamondSquareHeightmapRequestDto { Size = 17, OffsetRange = 10, OffsetReductionRate = 0.5f });
            mockRepository.Verify(mock => mock.AddSplatmap(splatmap), Times.Once());
        }

        [TestCase(TestName = "Get HeightmapChunk returns stored chunk")]
        public void IsHeightmapChunkReturned()
        {
            Guid id = Guid.NewGuid();
            int offsetX = 5;
            int offsetZ = 6;
            byte[] heightmapChunk = new byte[] { 1, 3, 4 };

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.IsHeightmapChunkExists(
                It.Is<Guid>(i => i == id),
                It.Is<int>(i => i == offsetX),
               It.Is<int>(i => i == offsetZ)
                )).Returns(true);

            mockRepository.Setup(x => x.GetHeightmapChunk(
              It.Is<Guid>(i => i == id),
                It.Is<int>(i => i == offsetX),
               It.Is<int>(i => i == offsetZ)
                )).Returns(heightmapChunk);

            var terrainService = new TerrainService(mockRepository.Object, null, null, null, null);

            var result = terrainService.GetHeightmapChunk(id, offsetX, offsetZ);

            Assert.AreEqual(result, heightmapChunk);
        }

        [TestCase(TestName = "Get HeightmapChunk generates new chunk")]
        public void IsHeightmapChunkGenerated()
        {
            Guid id = Guid.NewGuid();
            int offsetX = 5;
            int offsetZ = 6;

            HeightmapDto heightmapDto = new HeightmapDto();

            var mockGenerator = new Mock<IOpenSimplexGenerator>();
            mockGenerator.Setup(x => x.Generate(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<int>(),
                It.IsAny<int>()
                )).Returns(heightmapDto);

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.IsHeightmapChunkExists(
                It.Is<Guid>(i => i == id),
                It.Is<int>(i => i == offsetX),
               It.Is<int>(i => i == offsetZ)
                )).Returns(false);

            mockRepository.Setup(x => x.GetBaseHeightmapChunk(
                It.Is<Guid>(i => i == id)
                )).Returns(new BaseHeightmapChunkDto());

            var terrainService = new TerrainService(mockRepository.Object, null, null, null, mockGenerator.Object);

            terrainService.GetHeightmapChunk(id, offsetX, offsetZ);

            mockRepository.Verify(mock => mock.AddHeightmapChunk(id, offsetX, offsetZ, heightmapDto), Times.Once());
        }
    }
}
