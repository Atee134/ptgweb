namespace PtgWeb.Common.Dtos.Request
{
    public class FaultHeightmapRequestDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int IterationCount { get; set; }
        public int OffsetPerIteration { get; set; }
    }
}
