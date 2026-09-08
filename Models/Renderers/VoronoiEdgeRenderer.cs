using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public class VoronoiEdgeRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new(typeof(VoronoiEdgeRenderer), "Voronoi Edge");

    public WriteableBitmap Render(WriteableBitmap source)
    {
        throw new System.NotImplementedException();
    }
}
