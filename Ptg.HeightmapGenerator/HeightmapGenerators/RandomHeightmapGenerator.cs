using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;

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

            byte[] heightmapByteArray = BitmapHelper.WriteToByteArray(heightMapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                HeightmapByteArray = heightmapByteArray
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
    }
}