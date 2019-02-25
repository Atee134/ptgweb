using System;

namespace Ptg.Common.Dtos
{
    public class SplatmapDto
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] SplatmapByteArray { get; set; }
    }
}
