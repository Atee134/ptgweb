using System;
using System.Linq;

namespace Ptg.Common
{
    public static class ArrayHelper
    {
        private static readonly float DISTANCE_BETWEEN_POINTS = 2f;
        private static readonly float HEIGHT_MULTIPLIER = 0.8f;

        public static float[] ConvertToFlatCoordsArray(float[,] heightmapData)
        {
            int width = heightmapData.GetLength(0);
            int height = heightmapData.GetLength(1);

            float[] coordsArray = new float[width * height * 3];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xCoord = Convert.ToSingle(x - width * 0.5) * DISTANCE_BETWEEN_POINTS;
                    float zCoord = Convert.ToSingle(y - height * 0.5) * DISTANCE_BETWEEN_POINTS;
                    float yValue = heightmapData[x, y] * HEIGHT_MULTIPLIER;

                    int currentIdx = 3 * (y * width + x);

                    coordsArray[currentIdx] = xCoord;
                    coordsArray[currentIdx + 1] = yValue;
                    coordsArray[currentIdx + 2] = zCoord;
                }
            }

            return coordsArray;
        }
    }
}
