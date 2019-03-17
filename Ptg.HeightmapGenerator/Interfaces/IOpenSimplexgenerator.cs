namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IOpenSimplexGenerator
    {
        byte[] Generate(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity);
    }
}
