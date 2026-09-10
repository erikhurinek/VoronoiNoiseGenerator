using System;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public class VoronoiEdgeRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new("Voronoi Edge", typeof(VoronoiEdgeRenderer), typeof(VoronoiRendererSettings));

    public void Render(WriteableBitmap target, PassSettings settings)
    {
        throw new NotImplementedException();
    }
}
