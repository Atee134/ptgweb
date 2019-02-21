using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class DiamondSquareGenerator : IDiamondSquareGenerator
    {
        private static readonly Random random = new Random();

        public DiamondSquareGenerator()
        {

        }

        public HeightmapDto Generate(int width, int height, byte initialCornerValue, float offsetRange)
        {
            //  let A = a width*height 2D array of 0s
            //  pre - seed four corners of A with a value
            byte[,] heightmapData = GenerateInitialHeightmapData(width, height, initialCornerValue);

            //  let step_size = width - 1
            //  let r = a random number within a range

            int stepSize = width - 1;


            //  while step_size > 1:
            //  loop over A
            //  do diamond_step for each square in A

            //  loop over A
            //  do square_step for each diamond in A

            //  step_size /= 2
            //  reduce random range for r

            int offset;

            while (stepSize > 1)
            {
                for (int x = stepSize / 2; x < width - 1; x += stepSize)
                {
                    for (int y = stepSize / 2; y < height - 1; y += stepSize)
                    {
                        offset = random.Next((int)(-offsetRange / 2), (int)offsetRange);
                        DiamondStep(x, y, heightmapData, stepSize, offset);
                    }
                }

                stepSize /= 2;

                for (int x = 0; x < width - 1; x += stepSize)
                {
                    for (int y = 0; y < height - 1; y += stepSize)
                    {
                        offset = random.Next((int)(-offsetRange / 2), (int)offsetRange);
                        SquareStep(x, y, heightmapData, stepSize, offset);
                    }
                }

                offsetRange *= 0.7f;
            }

            byte[] heightmapByteArray = BitmapHelper.WriteToByteArray(heightmapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                Heightmap = heightmapByteArray
            };
        }

        private byte[,] GenerateInitialHeightmapData(int width, int height, byte initialCornerValue)
        {
            byte[,] heightmapDataArray = new byte[width, height];

            for (int x = 0; x < heightmapDataArray.GetLength(0); x++)
            {
                for (int y = 0; y < heightmapDataArray.GetLength(1); y++)
                {
                    heightmapDataArray[x, y] = 0;
                }
            }

            heightmapDataArray[0, 0] = 240;
            heightmapDataArray[0, height - 1] = 60;
            heightmapDataArray[width - 1, 0] = 120;
            heightmapDataArray[width - 1, height - 1] = 50;

            return heightmapDataArray;
        }

        private void DiamondStep(int x, int y, byte[,] heightmapDataArray, int stepSize, int offset)
        {
            if (heightmapDataArray[x, y] != 0) return;

            stepSize /= 2;

            int topLeft = heightmapDataArray[x - stepSize, y + stepSize];
            int topRight = heightmapDataArray[x + stepSize, y + stepSize];
            int bottomLeft = heightmapDataArray[x - stepSize, y - stepSize];
            int bottomRight = heightmapDataArray[x + stepSize, y - stepSize];

            int value = (((topLeft + topRight + bottomLeft + bottomRight) / 4) + offset);

            if (value > byte.MaxValue) value = byte.MaxValue;
            else if (value < byte.MinValue) value = byte.MinValue;

            heightmapDataArray[x, y] = (byte)value;
        }

        private void SquareStep(int x, int y, byte[,] heightmapDataArray, int stepSize, int offset)
        {
            if (heightmapDataArray[x, y] != 0) return;

            int width = heightmapDataArray.GetLength(0);
            int height = heightmapDataArray.GetLength(1);

            int left;
            int top;
            int right;
            int bottom;

            if (x - stepSize >= 0) left = heightmapDataArray[x - stepSize, y];
            else left = heightmapDataArray[x + stepSize, y];

            if (x + stepSize <= width - 1) right = heightmapDataArray[x + stepSize, y];
            else right = heightmapDataArray[x - stepSize, y];

            if (y - stepSize >= 0) bottom = heightmapDataArray[x, y - stepSize];
            else bottom = heightmapDataArray[x, y + stepSize];

            if (y + stepSize <= height - 1) top = heightmapDataArray[x, y + stepSize];
            else top = heightmapDataArray[x, y - stepSize];

            byte value = (byte)(((left + top + right + bottom) / 4) + offset);

            if (value > byte.MaxValue) value = byte.MaxValue;
            else if (value < byte.MinValue) value = byte.MinValue;

            heightmapDataArray[x, y] = (byte)value;
        }
    }
}
