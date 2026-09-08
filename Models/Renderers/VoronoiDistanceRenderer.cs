using System.ComponentModel;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;


public class VoronoiDistanceRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new(typeof(VoronoiDistanceRenderer), "Voronoi Distance");

    public WriteableBitmap Render(WriteableBitmap source)
    {
        throw new System.NotImplementedException();
    }
}
