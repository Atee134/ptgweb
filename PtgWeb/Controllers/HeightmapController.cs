﻿using Microsoft.AspNetCore.Mvc;
using Ptg.HeightmapGenerator.Interfaces;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private readonly IRandomHeightmapGenerator randomHeightmapGenerator;
        private readonly IFaultHeightmapGenerator faultHeightmapGenerator;

        public HeightmapController(IRandomHeightmapGenerator randomHeightmapGenerator, IFaultHeightmapGenerator faultHeightmapGenerator)
        {
            this.randomHeightmapGenerator = randomHeightmapGenerator;
            this.faultHeightmapGenerator = faultHeightmapGenerator;
        }

        [HttpGet("random")]
        public IActionResult GetRandomHeightmap(int width, int height)
        {
            var result = randomHeightmapGenerator.GenerateHeightmap(width, height);

            return File(result.Heightmap, "image/bmp");
        }

        [HttpGet("fault")]
        public IActionResult GetFaultHeightmap(int width, int height)
        {
            var result = faultHeightmapGenerator.GenerateHeightmap(width, height);

            return File(result.Heightmap, "image/bmp");
        }
    }
}