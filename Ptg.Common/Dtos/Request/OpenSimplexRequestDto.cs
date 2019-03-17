namespace Ptg.Common.Dtos.Request
{
    public class OpenSimplexRequestDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Seed { get; set; }
        public float Scale { get; set; }
        public int Octaves { get; set; }
        public float Persistance { get; set; }
        public float Lacunarity { get; set; }
    }
}
