using System;

namespace Ptg.DataAccess.Models
{
    public class Heightmap
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] HeightmapByteArray { get; set; }

        public int? ChunkOffsetX { get; set; }

        public int? ChunkOffsetZ { get; set; }
    }
}
