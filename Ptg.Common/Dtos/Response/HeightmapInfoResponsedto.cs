using System;

namespace Ptg.Common.Dtos.Response
{
    public class HeightmapInfoResponseDto
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int OverlappedSize { get; set; }
    }
}
