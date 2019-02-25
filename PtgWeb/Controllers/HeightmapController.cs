using Microsoft.AspNetCore.Mvc;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
using PtgWeb.Common.Dtos.Request;
using System;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ITerrainService terrainService;
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;

        public HeightmapController(IRepository repository, ITerrainService terrainService, IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator)
        {
            this.repository = repository;
            this.terrainService = terrainService;
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
        }

        [HttpGet("random")]
        public IActionResult GetRandomHeightmap(int width, int height)
        {
            var result = randomHeightmapGenerator.GenerateHeightmap(width, height);

            return File(result.HeightmapByteArray, "image/bmp");
        }

        [HttpGet("fault")]
        public IActionResult GetFaultHeightmap([FromQuery] FaultHeightmapRequestDto requestDto)
        {
            var result = faultHeightmapGenerator.GenerateHeightmap(requestDto.Width, requestDto.Height, requestDto.IterationCount, requestDto.OffsetPerIteration);

            return File(result.HeightmapByteArray, "image/bmp");
        }

        [HttpPost("diamondSquare")]
        public IActionResult CreateDiamondSquareHeightmap([FromBody] DiamondSquareHeightmapRequestDto requestDto)
        {
            var result = terrainService.GenerateDiamondSquareTerrain(requestDto);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetHeightmap(Guid id)
        {
            var result = repository.GetHeightmap(id);

            return File(result.HeightmapByteArray, "image/bmp");
        }

        [HttpGet("~/api/splatmap")]
        public IActionResult GetSplatmap(Guid id)
        {
            var result = repository.Getsplatmap(id);

            return File(result.SplatmapByteArray, "image/bmp");
        }
    }
}