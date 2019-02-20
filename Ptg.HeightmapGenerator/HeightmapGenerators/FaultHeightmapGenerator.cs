using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.HeightmapGenerator.Interfaces;
using System;
using System.Drawing;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class FaultHeightmapGenerator : IFaultHeightmapGenerator
    {
        private enum Side
        {
            Left,
            Top,
            Right,
            Bottom
        }

        private readonly static Random random = new Random();

        public HeightmapDto GenerateHeightmap(int width, int height, int iterationCount, int offSetInOneIteration)
        {
            byte[,] heightmapData = InitHeightmapData(width, height);

            for (int i = 0; i < iterationCount; i++)
            {
                (Point start, Point end) linePoints = GenerateLinePoints(width, height);
                RecalculateHeightmapData(heightmapData, offSetInOneIteration, linePoints.start, linePoints.end);
            }

            byte[] heightmapByteArray = BitmapHelper.WriteToByteArray(heightmapData);

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                Heightmap = heightmapByteArray
            };
        }

        private void RecalculateHeightmapData(byte[,] heightmapData, int offset, Point lineStart, Point lineEnd)
        {
            for (int x = 0; x < heightmapData.GetLength(0); x++)
            {
                for (int y = 0; y < heightmapData.GetLength(1); y++)
                {
                    byte currentHeightmapValue = heightmapData[x, y];

                    if (IsLeft(lineStart, lineEnd, x, y))
                    {
                        if ((int)currentHeightmapValue + offset > byte.MaxValue) currentHeightmapValue = byte.MaxValue;
                        else currentHeightmapValue += (byte)offset;
                    }
                    else
                    {
                        if ((int)currentHeightmapValue - offset < byte.MinValue) currentHeightmapValue = byte.MinValue;
                        else currentHeightmapValue -= (byte)offset;
                    }

                    heightmapData[x, y] = currentHeightmapValue;
                }
            }
        }

        private byte[,] InitHeightmapData(int width, int height)
        {
            byte[,] heightmapData = new byte[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightmapData[x, y] = 127;
                }
            }

            return heightmapData;
        }

        private (Point start, Point end) GenerateLinePoints(int width, int height)
        {
            //Side startingSide = (Side)random.Next(0, 4);
            bool horizontal = random.Next(0, 2) == 1 ? true : false;



            Point lineStart;
            Point lineEnd;

            if (horizontal)
            {
                lineStart = new Point(0, random.Next(0, height));
                lineEnd = new Point(width - 1, random.Next(0, height));
            }
            else
            {
                lineStart = new Point(random.Next(0, width), 0);
                lineEnd = new Point(random.Next(0, width), height - 1);
            }

            bool positiveDirection = random.Next(0, 2) == 1 ? true : false;

            if (positiveDirection)
            {
                return (start: lineStart, end: lineEnd);
            }
            else
            {
                return (start: lineEnd, end: lineStart);
            }
        }

        private bool IsLeft(Point lineStart, Point lineEnd, int x, int y)
        {
            return ((lineEnd.X - lineStart.X) * (y - lineStart.Y) - (lineEnd.Y - lineStart.Y) * (x - lineStart.X)) > 0;
        }
    }
}
