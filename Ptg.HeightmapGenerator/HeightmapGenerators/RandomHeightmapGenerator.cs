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
            float[,] heightmapData = Generate(width, height);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                HeightmapOriginalArray = heightmapData,
                HeightmapCoords = ArrayHelper.ConvertToFlatCoordsArray(heightmapData)
            };
        }

        private float[,] Generate(int width, int height)
        {
            float[,] heightMapData = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = random.Next(0, 256);
                }
            }

            return heightMapData;
        }
    }
}