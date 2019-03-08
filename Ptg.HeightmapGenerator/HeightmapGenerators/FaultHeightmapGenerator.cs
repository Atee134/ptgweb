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

        public HeightmapDto GenerateHeightmap(int width, int height, int iterationCount, float offsetPerIteration)
        {
            float[,] heightmapData = InitHeightmapData(width, height);

            for (int i = 0; i < iterationCount; i++)
            {
                (Point start, Point end) linePoints = GenerateLinePoints(width, height);
                RecalculateHeightmapData(heightmapData, offsetPerIteration, linePoints.start, linePoints.end);
            }

            return new HeightmapDto
            {
                Width = width,
                Height = height,
                HeightmapOriginalArray = heightmapData,
                HeightmapByteArray = BitmapHelper.WriteToByteArray(heightmapData)
            };
        }

        private void RecalculateHeightmapData(float[,] heightmapData, float offset, Point lineStart, Point lineEnd)
        {
            for (int x = 0; x < heightmapData.GetLength(0); x++)
            {
                for (int y = 0; y < heightmapData.GetLength(1); y++)
                {
                    float currentHeightmapValue = heightmapData[x, y];

                    if (IsLeft(lineStart, lineEnd, x, y))
                    {
                        if ((int)currentHeightmapValue + offset > byte.MaxValue) currentHeightmapValue = byte.MaxValue;
                        else currentHeightmapValue += offset;
                    }
                    else
                    {
                        if ((int)currentHeightmapValue - offset < byte.MinValue) currentHeightmapValue = byte.MinValue;
                        else currentHeightmapValue -= offset;
                    }

                    heightmapData[x, y] = currentHeightmapValue;
                }
            }
        }

        private float[,] InitHeightmapData(int width, int height)
        {
            float[,] heightmapData = new float[width, height];

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
            Side startSide = (Side)random.Next(0, 4);
            bool oppositeSide = random.Next(0, 2) == 1 ? true : false;

            Side endSide;

            switch (startSide)
            {
                case Side.Left:
                    endSide = oppositeSide ? Side.Right : random.Next(0, 2) == 1 ? Side.Top : Side.Bottom;
                    break;
                case Side.Top:
                    endSide = oppositeSide ? Side.Bottom : random.Next(0, 2) == 1 ? Side.Left : Side.Right;
                    break;
                case Side.Right:
                    endSide = oppositeSide ? Side.Left : random.Next(0, 2) == 1 ? Side.Top : Side.Bottom;
                    break;
                case Side.Bottom:
                    endSide = oppositeSide ? Side.Top : random.Next(0, 2) == 1 ? Side.Left : Side.Right;
                    break;
                default:
                    endSide = Side.Left;
                    break;
            }

            Point lineStart = GenerateRandomPoint(startSide, width, height);
            Point lineEnd = GenerateRandomPoint(endSide, width, height);

            return (start: lineStart, end: lineEnd);
        }

        private Point GenerateRandomPoint(Side side, int width, int height)
        {
            switch (side)
            {
                case Side.Left:
                    return new Point(0, random.Next(0, height));
                case Side.Top:
                    return new Point(random.Next(0, width), height - 1);
                case Side.Right:
                    return new Point(width - 1, random.Next(0, height));
                case Side.Bottom:
                    return new Point(random.Next(0, width), 0);
                default:
                    return new Point();
            }
        }

        private bool IsLeft(Point lineStart, Point lineEnd, int x, int y)
        {
            return ((lineEnd.X - lineStart.X) * (y - lineStart.Y) - (lineEnd.Y - lineStart.Y) * (x - lineStart.X)) > 0;
        }
    }
}
