using Microsoft.AspNetCore.Mvc;
using Ptg.HeightmapGenerator.Interfaces;
using PtgWeb.Dtos.Request;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;
        private readonly IDiamondSquareGenerator diamondSquareGenerator;

        public HeightmapController(IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator, IDiamondSquareGenerator diamondSquareGenerator)
        {
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
            this.diamondSquareGenerator = diamondSquareGenerator;
        }

        [HttpGet("random")]
        public IActionResult GetRandomHeightmap(int width, int height)
        {
            var result = randomHeightmapGenerator.GenerateHeightmap(width, height);

            return File(result.Heightmap, "image/bmp");
        }

        [HttpGet("fault")]
        public IActionResult GetFaultHeightmap([FromQuery] FaultHeightmapRequestDto requestDto)
        {
            var result = faultHeightmapGenerator.GenerateHeightmap(requestDto.Width, requestDto.Height, requestDto.IterationCount, requestDto.OffsetPerIteration);

            return File(result.Heightmap, "image/bmp");
        }

        [HttpGet("diamondSquare")]
        public IActionResult GetDiamondSquareHeightmap([FromQuery] FaultHeightmapRequestDto requestDto)
        {
            var result = diamondSquareGenerator.Generate(257, 257, 100, 50);

            return File(result.Heightmap, "image/bmp");
        }
    }
}