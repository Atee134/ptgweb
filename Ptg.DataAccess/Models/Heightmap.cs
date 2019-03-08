using System;

namespace Ptg.DataAccess.Models
{
    public class Heightmap
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] HeightmapByteArray { get; set; }
    }
}
