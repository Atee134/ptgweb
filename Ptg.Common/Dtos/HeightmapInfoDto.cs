using System;

namespace Ptg.Common.Dtos
{
    public class HeightmapInfoDto
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int OverlappedSize { get; set; }
    }
}
