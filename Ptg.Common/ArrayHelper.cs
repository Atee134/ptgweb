using System;

namespace Ptg.Common
{
    public static class ArrayHelper
    {
        private static readonly int DISTANCE_BETWEEN_POINTS = 10;

        public static float[] ConvertToFlatCoordsArray(float[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            float[] flattenedArray = new float[width * height * 3];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xCoord = Convert.ToSingle(x - width * 0.5) * DISTANCE_BETWEEN_POINTS;
                    float zCoord = Convert.ToSingle(y - height * 0.5) * DISTANCE_BETWEEN_POINTS;
                    float noiseValue = array[x,y];

                    int currentIdx = 3 * (y * width + x);

                    flattenedArray[currentIdx] = xCoord;
                    flattenedArray[currentIdx + 1] = noiseValue;
                    flattenedArray[currentIdx + 2] = zCoord;
                }
            }

            return flattenedArray;
        }
    }
}
