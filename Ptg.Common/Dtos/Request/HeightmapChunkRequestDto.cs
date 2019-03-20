using System;

namespace Ptg.Common.Dtos.Request
{
    public class HeightmapChunkRequestDto
    {
        public Guid BaseHeightmapChunkId { get; set; }

        public int OffsetX { get; set; }

        public int OffsetZ { get; set; }
    }
}
