using Ptg.Common;
using Ptg.HeightmapGenerator.Interfaces;
using Ptg.HeightmapGenerator.NoiseFunctions;

namespace Ptg.HeightmapGenerator.HeightmapGenerators
{
    public class OpenSimplexGenerator : IOpenSimplexGenerator
    {
        public byte[] Generate()
        {
            var noise = new OpenSimplexNoise();

            float[,] floatArrayHuh = new float[1024, 1024];

            for (int x = 0; x < floatArrayHuh.GetLength(0); x++)
            {
                for (int y = 0; y < floatArrayHuh.GetLength(1); y++)
                {
                    floatArrayHuh[x, y] = (float)noise.Evaluate(x * 0.1, y * 0.1);
                }
            }

            var byteArray = BitmapHelper.WriteToByteArray(floatArrayHuh);

            return byteArray;
        }
    }
}
