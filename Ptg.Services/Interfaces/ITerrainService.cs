using PtgWeb.Common.Dtos.Request;
using System;

namespace Ptg.Services.Interfaces
{
    public interface ITerrainService
    {
        Guid GenerateDiamondSquareTerrain(DiamondSquareHeightmapRequestDto requestDto);
    }
}
