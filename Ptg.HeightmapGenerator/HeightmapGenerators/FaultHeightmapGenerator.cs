using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class FaultHeightmapGenerator : IFaultHeightmapGenerator
    {
        private readonly static Random random = new Random();
        private Point lineStart;
        private Point lineEnd;

        public HeightmapDto GenerateHeightmap(int width, int height)
        {
            GenerateLinePoints(width, height);

            byte[,] heightMapData = Generate(width, height);

            //TODO move WriteToByteArray to a common helper method
            byte[] heightmapByteArray = WriteToByteArray(heightMapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                Heightmap = heightmapByteArray
            };
        }

        private byte[,] Generate(int width, int height)
        {
            byte[,] heightMapData = new byte[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = isLeft(x, y) ? (byte)0 : (byte)255;
                }
            }

            return heightMapData;
        }

        private void GenerateLinePoints(int width, int height)
        {
            bool leftToRight = random.Next(0, 2) == 1 ? true : false;

            if (leftToRight)
            {
                lineStart = new Point(0, random.Next(0, height));
                lineEnd = new Point(width - 1, random.Next(0, height));
            }
            else
            {
                lineStart = new Point(random.Next(0, width), 0);
                lineEnd = new Point(random.Next(0, width), height - 1);
            }
        }

        private bool isLeft(int x, int y)
        {
            return ((lineEnd.X - lineStart.X) * (y - lineStart.Y) - (lineEnd.Y - lineStart.Y) * (x - lineStart.X)) > 0;
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
                        var value = heightMapdata[x, y];
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
