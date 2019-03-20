using System;

namespace Ptg.DataAccess.Models
{
    public class HeightmapChunk
    {
        public Heightmap Heightmap { get; set; }

        public int OffsetX { get; set; }

        public int OffsetZ { get; set; }
    }
}
