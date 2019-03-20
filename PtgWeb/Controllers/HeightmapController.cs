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
        private readonly IOpenSimplexGenerator openSimplexGenerator;

        public HeightmapController(IRepository repository, ITerrainService terrainService, IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator, IOpenSimplexGenerator openSimplexGenerator)
        {
            this.repository = repository;
            this.terrainService = terrainService;
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
            this.openSimplexGenerator = openSimplexGenerator;
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
            var result = repository.GetHeightmap(id);

            return File(result, "image/bmp");
        }

        //[HttpGet("{id}")]
        //public IActionResult GetHeightmap(Guid id)
        //{
        //    var result = repository.GetHeightmap(id);

        //    return File(result, "image/bmp");
        //}

        [HttpGet("{id}/info")]
        public IActionResult GetHeightmapInfo(Guid id)
        {
            var result = repository.GetHeightmapInfo(id);

            var response = new HeightmapInfoResponseDto
            {
                Id = result.Id,
                Width = result.Width,
                Height = result.Height,
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