using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class RandomHeightmapGenerator : IRandomHeightmapGenerator
    {
        private static readonly Random random = new Random();

        public RandomHeightmapGenerator()
        {

        }

        public HeightmapDto GenerateHeightmap(int width, int height)
        {
            byte[,] heightMapData = Generate(width, height);

            byte[] heightMap = WriteToByteArray(heightMapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                Heightmap = heightMap
            };
        }

        private byte[,] Generate(int width, int height)
        {
            byte[,] heightMapData = new byte[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = (byte)random.Next(0, 256);
                }
            }

            return heightMapData;
        }

        private byte[] WriteToByteArray(byte[,] heightMapdata)
        {
            byte[] content;
            int width = heightMapdata.GetLength(0);
            int height = heightMapdata.GetLength(1);
            using (var stream = new MemoryStream())
            {
                var bitmap = new Bitmap(width, height); // TODO put this back here PixelFormat.Format16bppGrayScale);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var value = heightMapdata[x,y];
                        bitmap.SetPixel(x, y, Color.FromArgb(value, value, value)); // TODO with 16bit grayscale format this throws exception, use lockbits instead of setpixel anyway
                    }
                }

                bitmap.Save(stream, ImageFormat.Bmp);
                content = stream.ToArray();
            }

            return content;
        }
    }
}