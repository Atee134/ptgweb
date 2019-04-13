namespace Ptg.Common.Dtos.Request
{
    public class FaultHeightmapRequestDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int IterationCount { get; set; }
        public float OffsetPerIteration { get; set; }
        public int? Seed { get; set; }
    }
}
