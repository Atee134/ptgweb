using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.SplatmapGenerator.Interfaces;
using System;
using System.Drawing;

namespace Ptg.SplatmapGenerator.SplatmapGenerators
{
    public class RandomSplatmapGenerator : IRandomSplatmapGenerator
    {
        private static readonly Random random = new Random();

        public SplatmapDto Generate(HeightmapDto heightmapDto)
        {
            Color[,] splatmapData = Generate(heightmapDto.Width, heightmapDto.Height);

            byte[] splatmapByteArray = BitmapHelper.WriteToByteArray(splatmapData);

            return new SplatmapDto
            {
                SplatmapByteArray = splatmapByteArray
            };
        }

        private Color[,] Generate(int width, int height)
        {
            Color[,] heightMapData = new Color[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int color = random.Next(0, 3);

                    switch (color)
                    {
                        case (0):
                            heightMapData[x, y] = Color.Red;
                            break;
                        case (1):
                            heightMapData[x, y] = Color.Green;
                            break;
                        case (2):
                            heightMapData[x, y] = Color.Blue;
                            break;
                        default:
                            break;
                    }
                }
            }

            return heightMapData;
        }
    }
}
