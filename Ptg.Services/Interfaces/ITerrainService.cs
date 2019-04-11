using Ptg.Common.Dtos;
using Ptg.Common.Dtos.Request;
using System;

namespace Ptg.Services.Interfaces
{
    public interface ITerrainService
    {
        Guid Generate(DiamondSquareHeightmapRequestDto requestDto);
        Guid Generate(FaultHeightmapRequestDto requestDto);
        Guid Generate(OpenSimplexRequestDto requestDto);
        byte[] GetHeightmapChunk(Guid baseHeightmapChunkId, int offsetX, int offsetZ);
        byte[] GetHeightmap(Guid id);
        byte[] GetSplatmap(Guid id);
        HeightmapInfoDto GetHeightmapInfo(Guid id);
    }
}
