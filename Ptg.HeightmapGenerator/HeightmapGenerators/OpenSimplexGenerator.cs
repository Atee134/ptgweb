using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.HeightmapGenerator.NoiseFunctions;
using System.Drawing;
using System.Linq;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class OpenSimplexGenerator : IOpenSimplexGenerator
    {
        public HeightmapDto Generate(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity)
        {
            var noise = new OpenSimplexNoise();

            float[,] heightmapData = GenerateHeightmap(width, height, seed, scale, octaves, persistance, lacunarity);

            var heightmapByteArray = BitmapHelper.WriteToByteArray(heightmapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                HeightmapOriginalArray = heightmapData,
                HeightmapByteArray = heightmapByteArray
            };
        }

        public float[,] GenerateHeightmap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity)
        {
            var noise = new OpenSimplexNoise(seed);
            System.Random rnd = new System.Random(seed);

            float amplitude = 1f;
            float frequency = 1f;
            float maxPossibleHeight = 0f;

            Point[] octaveOffsets = new Point[octaves];
            for (int i = 0; i < octaves; i++)
            {
                int offsetX = rnd.Next(-100000, 100000);
                int offsetY = rnd.Next(-100000, 100000);

                octaveOffsets[i] = new Point(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                maxPossibleHeight *= persistance;
            }

            float[,] heightmap = new float[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    amplitude = 1f;
                    frequency = 1f;
                    float noiseValueSum = 0f;

                    for (int i = 0; i < octaves; i++)
                    {
                        float samplePointX = (x + octaveOffsets[i].X) * scale * frequency;
                        float samplePointY = (y + octaveOffsets[i].Y) * scale * frequency;

                        float noisevalue = (float)noise.Evaluate(samplePointX, samplePointY);

                        noiseValueSum += noisevalue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    heightmap[x, y] = (noiseValueSum + 1.5f) * 85;
                }
            }

            var allValues = heightmap.Cast<float>();
            float minHeight = allValues.Min();
            float maxHeight = allValues.Max();

            return heightmap;
        }
    }
}
