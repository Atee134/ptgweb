using System;

namespace Ptg.DataAccess.Models
{
    public class Splatmap
    {
        public Guid Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] SplatmapByteArray { get; set; }
    }
}
