using System;

namespace Ptg.Common.Dtos.Response
{
    public class HeightmapResponseDto
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public float[] HeightmapCoords { get; set; }
    }
}
