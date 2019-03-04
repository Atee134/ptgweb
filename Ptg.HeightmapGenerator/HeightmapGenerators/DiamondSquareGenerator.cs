using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class DiamondSquareGenerator : IDiamondSquareGenerator
    {
        private static readonly Random random = new Random();

        public HeightmapDto Generate(int size, float offsetRange, float offsetReductionRate)
        {
            float[,] heightmapData = GenerateInitialHeightmapData(size);
            int stepSize = size - 1;

            while (stepSize > 1)
            {
                float offset;

                for (int x = stepSize / 2; x < size - 1; x += stepSize)
                {
                    for (int y = stepSize / 2; y < size - 1; y += stepSize)
                    {
                        offset = GenerateOffset(offsetRange);
                        DiamondStep(x, y, heightmapData, stepSize, offset);
                    }
                }

                stepSize /= 2;

                for (int x = 0; x < size; x += stepSize)
                {
                    for (int y = 0; y < size; y += stepSize)
                    {
                        offset = GenerateOffset(offsetRange);
                        SquareStep(x, y, heightmapData, stepSize, offset);
                    }
                }

                offsetRange *= offsetReductionRate;
            }

            byte[] heightmapByteArray = BitmapHelper.WriteToByteArray(heightmapData);

            return new HeightmapDto
            {
                Width = size,
                Height = size,
                HeightmapFloatArray = heightmapData,
                HeightmapByteArray = heightmapByteArray
            };
        }

        private float[,] GenerateInitialHeightmapData(int size)
        {
            float[,] heightmapDataArray = new float[size, size];

            for (int x = 0; x < heightmapDataArray.GetLength(0); x++)
            {
                for (int y = 0; y < heightmapDataArray.GetLength(1); y++)
                {
                    heightmapDataArray[x, y] = 0;
                }
            }

            heightmapDataArray[0, 0] = random.Next(0, 256);
            heightmapDataArray[0, size - 1] = random.Next(0, 256);
            heightmapDataArray[size - 1, 0] = random.Next(0, 256);
            heightmapDataArray[size - 1, size - 1] = random.Next(0, 256);

            return heightmapDataArray;
        }

        private float GenerateOffset(float offsetRange)
        {
            return Convert.ToSingle(random.NextDouble() * offsetRange * 2 - offsetRange);
        }

        private void DiamondStep(int x, int y, float[,] heightmapDataArray, int stepSize, float offset)
        {
            if (heightmapDataArray[x, y] != 0) return;

            int halfStep = stepSize / 2;

            float topLeft = heightmapDataArray[x - halfStep, y + halfStep];
            float topRight = heightmapDataArray[x + halfStep, y + halfStep];
            float bottomLeft = heightmapDataArray[x - halfStep, y - halfStep];
            float bottomRight = heightmapDataArray[x + halfStep, y - halfStep];

            float value = (topLeft + topRight + bottomLeft + bottomRight) / 4 + offset;

            if (value > byte.MaxValue) value = byte.MaxValue;
            else if (value < byte.MinValue) value = byte.MinValue;

            heightmapDataArray[x, y] = value;
        }

        private void SquareStep(int x, int y, float[,] heightmapDataArray, int stepSize, float offset)
        {
            if (heightmapDataArray[x, y] != 0) return;

            int size = heightmapDataArray.GetLength(0);

            float left;
            float top;
            float right;
            float bottom;

            if (x - stepSize >= 0) left = heightmapDataArray[x - stepSize, y];
            else left = heightmapDataArray[x + stepSize, y];

            if (x + stepSize <= size - 1) right = heightmapDataArray[x + stepSize, y];
            else right = heightmapDataArray[x - stepSize, y];

            if (y - stepSize >= 0) bottom = heightmapDataArray[x, y - stepSize];
            else bottom = heightmapDataArray[x, y + stepSize];

            if (y + stepSize <= size - 1) top = heightmapDataArray[x, y + stepSize];
            else top = heightmapDataArray[x, y - stepSize];

            float value = (left + top + right + bottom) / 4 + offset;

            if (value > byte.MaxValue) value = byte.MaxValue;
            else if (value < byte.MinValue) value = byte.MinValue;

            heightmapDataArray[x, y] = value;
        }
    }
}
