using Ptg.Common;
using Ptg.Common.Dtos;
using Ptg.SplatmapGenerator.Interfaces;
using System.Drawing;
using System.Linq;

namespace Ptg.SplatmapGenerator.SplatmapGenerators
{
    public class HeightBasedSplatmapGenerator : IHeightBasedSplatmapGenerator
    {
        public SplatmapDto Generate(HeightmapDto heightmapDto, float lowPercent, float midPercent, float highPercent)
        {
            float transitionPercent = 1 - (lowPercent + midPercent + highPercent);

            var allValues = heightmapDto.HeightmapFloatArray.Cast<float>();
            float minHeight = allValues.Min();
            float maxHeight = allValues.Max();

            float totalValueRange = maxHeight - minHeight;

            float lowMinValue = minHeight;
            float lowMaxValue = minHeight + totalValueRange * lowPercent;

            float lowMidTransitionMinValue = lowMaxValue;
            float lowMidTransitionMaxValue = lowMaxValue + totalValueRange * transitionPercent / 2;
            float lowMidTransitionRange = lowMidTransitionMaxValue - lowMidTransitionMinValue;

            float midMinValue = lowMidTransitionMaxValue;
            float midMaxValue = lowMidTransitionMaxValue + totalValueRange * midPercent;

            float midHighTransitionMinValue = midMaxValue;
            float midHighTransitionMaxValue = midMaxValue + totalValueRange * transitionPercent / 2;
            float midHighTransitionRange = midHighTransitionMaxValue - midHighTransitionMinValue;

            float highMinValue = midHighTransitionMaxValue;
            float highMaxValue = midHighTransitionMaxValue + totalValueRange * highPercent;

            Color[,] colors = new Color[heightmapDto.Width, heightmapDto.Height];

            for (int x = 0; x < heightmapDto.Width; x++)
            {
                for (int y = 0; y < heightmapDto.Height; y++)
                {
                    float value = heightmapDto.HeightmapFloatArray[x, y];

                    if (value >= lowMinValue && value < lowMaxValue)
                    {
                        colors[x, y] = Color.FromArgb(255, 0, 0);
                    }
                    else if (value >= lowMidTransitionMinValue && value < lowMidTransitionMaxValue)
                    {
                        float currentPercent = (value - lowMidTransitionMinValue) / lowMidTransitionRange;
                        int green = (int)(255 * currentPercent);
                        int red = 255 - green;
                        colors[x, y] = Color.FromArgb(red, green, 0);
                    }
                    else if (value >= midMinValue && value < midMaxValue)
                    {
                        colors[x, y] = Color.FromArgb(0, 255, 0);
                    }
                    else if (value >= midHighTransitionMinValue && value < midHighTransitionMaxValue)
                    {
                        float currentPercent = (value - midHighTransitionMinValue) / midHighTransitionRange;
                        int blue = (int)(255 * currentPercent);
                        int green = 255 - blue;
                        colors[x, y] = Color.FromArgb(0, green, blue);
                    }
                    else if (value >= highMinValue)
                    {
                        colors[x, y] = Color.FromArgb(0, 0, 255);
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
