using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.SplatmapGenerator.Interfaces;
using System.Drawing;
using System.Linq;

namespace Ptg.SplatmapGenerator.SplatmapGenerators
{
    public class HeightBasedSplatmapGenerator : IHeightBasedSplatmapGenerator
    {
        public SplatmapDto Generate(HeightmapDto heightmapDto, float lowPercent, float highPercent, float transitionPercent)
        {
            float midPercent = 1 - (lowPercent + highPercent);

            var allValues = heightmapDto.HeightmapFloatArray.Cast<float>();
            float minHeight = allValues.Min();
            float maxHeight = allValues.Max();

            float totalValueRange = maxHeight - minHeight;

            float lowMinValue = minHeight;
            float lowMaxValue = minHeight + totalValueRange * lowPercent;

            float midMinValue = lowMaxValue;
            float midMaxValue = lowMaxValue + totalValueRange * midPercent;

            float highMinValue = midMaxValue;
            float highMaxValue = midMaxValue + totalValueRange * highPercent;

            Color[,] colors = new Color[heightmapDto.Width, heightmapDto.Height];

            for (int x = 0; x < heightmapDto.Width; x++)
            {
                for (int y = 0; y < heightmapDto.Height; y++)
                {
                    float value = heightmapDto.HeightmapFloatArray[x, y];

                    if (value >= lowMinValue && value < lowMaxValue)
                    {
                        colors[x, y] = Color.Red;
                    }
                    else if (value >= midMinValue && value < midMaxValue)
                    {
                        colors[x, y] = Color.Green;
                    }
                    else if (value >= highMinValue)
                    {
                        colors[x, y] = Color.Blue;
                    }
                }
            }

            byte[] splatmapByteArray = BitmapHelper.WriteToByteArray(colors);

            return new SplatmapDto
            {
                Width = heightmapDto.Width,
                Height = heightmapDto.Height,
                SplatmapByteArray = splatmapByteArray
            };
        }
    }
}
