using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightmapController : ControllerBase
    {
        private static readonly Random random = new Random();

        private byte[][] testHeightMap;
        private readonly int heightMapWidth = 128;
        private readonly int heightMapHeight = 128;

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
            var bmp = new Bitmap(heightMapWidth, heightMapHeight);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            int counter = 0;

            for (int i = 0; i < heightMapWidth; i++)
            {
                for (int j = 0; j < heightMapHeight; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        rgbValues[counter++] = testHeightMap[i][j];
                    }
                }
            }
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            bmp.Save("test134.bmp");

            return Ok();
        }
    }
}