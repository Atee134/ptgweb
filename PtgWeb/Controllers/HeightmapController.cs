using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptg.Common;
using Ptg.Common.Dtos.Request;
using Ptg.Common.Dtos.Response;
using Ptg.DataAccess;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.Services.Interfaces;
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

        [HttpPost("random")]
        public IActionResult CreateRandomHeightmap([FromBody] RandomHeightmapRequestDto requestDto)
        {
            var result = terrainService.Generate(requestDto);

            return Ok(result);
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
            //var result = terrainService.Generate(requestDto);

            var result = terrainService.GenerateTEST(requestDto);

            return File(BitmapHelper.WriteToByteArray(result.HeightmapOriginalArray), "image/bmp");
        }

        [HttpGet("{id}")]
        public IActionResult GetHeightmap(Guid id)
        {
            var result = repository.GetHeightmap(id);

            var response = new HeightmapResponseDto
            {
                Id = result.Id,
                Width = result.Width,
                Height = result.Height,
                HeightmapCoords = result.HeightmapCoords
            };

            return Ok(response);
        }

        [HttpGet("~/api/splatmap/{id}")]
        public IActionResult GetSplatmap(Guid id)
        {
            var result = repository.Getsplatmap(id);

            return File(result.SplatmapByteArray, "image/bmp");
        }
    }
}