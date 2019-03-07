using System;

namespace Ptg.Common.Dtos
{
    public class HeightmapDto
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public float[,] HeightmapOriginalArray { get; set; }

        public float[] HeightmapCoords { get; set; }
    }
}
