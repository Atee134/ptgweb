using Microsoft.AspNetCore.Mvc;
using Ptg.Common.Dtos.Request;
using Ptg.Common.Dtos.Response;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
using System;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private readonly ITerrainService terrainService;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;
        private readonly IOpenSimplexGenerator openSimplexGenerator;

        public HeightmapController(ITerrainService terrainService, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator, IOpenSimplexGenerator openSimplexGenerator)
        {
            this.terrainService = terrainService;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
            this.openSimplexGenerator = openSimplexGenerator;
        }

        [HttpPost("fault")]
        public IActionResult CreateFaultHeightmap([FromBody] FaultHeightmapRequestDto requestDto)
        {
            var result = terrainService.Generate(requestDto);

            return Ok(result);
        }

        [HttpPost("diamondSquare")]
        public IActionResult CreateDiamondSquareHeightmap([FromBody] DiamondSquareHeightmapRequestDto requestDto)
        {
            var result = terrainService.Generate(requestDto);

            return Ok(result);
        }

        [HttpPost("simplex")]
        public IActionResult CreateSimplexHeightmap([FromBody] OpenSimplexRequestDto requestDto)
        {
            var result = terrainService.Generate(requestDto);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetHeightmap(Guid id)
        {
            var result = terrainService.GetHeightmap(id);

            return File(result, "image/bmp");
        }

        [HttpGet("{baseChunkId}/{offsetX}/{offsetZ}")]
        public IActionResult GetHeightmapChunk(Guid baseChunkId, int offsetX, int offsetZ)
        {
            var result = terrainService.GetHeightmapChunk(baseChunkId, offsetX, offsetZ);

            return File(result, "image/bmp");
        }

        [HttpGet("{id}/info")]
        public IActionResult GetHeightmapInfo(Guid id)
        {
            var result = terrainService.GetHeightmapInfo(id);

            var response = new HeightmapInfoResponseDto
            {
                Id = result.Id,
                Width = result.Width,
                Height = result.Height,
                OverlappedSize = result.OverlappedSize
            };

            return Ok(response);
        }

        [HttpGet("~/api/splatmap/{id}")]
        public IActionResult GetSplatmap(Guid id)
        {
            var result = terrainService.GetSplatmap(id);

            return File(result, "image/bmp");
        }
    }
}