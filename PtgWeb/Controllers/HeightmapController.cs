using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private static readonly Random random = new Random();

        private byte[][] testHeightMap;
        private readonly int heightMapWidth = 512;
        private readonly int heightMapHeight = 512;

        public HeightmapController()
        {
            testHeightMap = new byte[heightMapWidth][];

            for (int i = 0; i < heightMapWidth; i++)
            {
                testHeightMap[i] = new byte[heightMapHeight];
                random.NextBytes(testHeightMap[i]);
            }
        }

        [HttpGet]
        public IActionResult GetHeightMap()
        {
            byte[] fileContent;
            using (var stream = new MemoryStream())
            {
                var bitmap = new Bitmap(heightMapWidth, heightMapHeight);

                for (int i = 0; i < heightMapWidth; i++)
                {
                    for (int j = 0; j < heightMapHeight; j++)
                    {
                        var value = testHeightMap[i][j];
                        bitmap.SetPixel(i, j, Color.FromArgb(value, value, value));
                    }
                }

                bitmap.Save(stream, ImageFormat.Bmp);
                fileContent = stream.ToArray();
            }

            return File(fileContent, "image/bmp");
        }
    }
}