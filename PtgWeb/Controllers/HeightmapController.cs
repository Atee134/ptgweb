using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptg.Common.Dtos.Request;
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
            var result = terrainService.Generate(requestDto);

            var what = HttpContext.Session.GetString("Test");

            if (what == null)
            {
                HttpContext.Session.SetString("Test", Guid.NewGuid().ToString());
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetHeightmap(Guid id)
        {
            var result = repository.GetHeightmap(id);

            var what = HttpContext.Session.GetString("Test");

            return File(result.HeightmapByteArray, "image/bmp");
        }

        [HttpGet("~/api/splatmap/{id}")]
        public IActionResult GetSplatmap(Guid id)
        {
            var result = repository.Getsplatmap(id);

            return File(result.SplatmapByteArray, "image/bmp");
        }
    }
}