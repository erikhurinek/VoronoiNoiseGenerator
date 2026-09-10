using System;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public class VoronoiCellRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new("Voronoi Cell", typeof(VoronoiCellRenderer), typeof(VoronoiRendererSettings));

    public void Render(WriteableBitmap target, PassSettings settings)
    {
        throw new NotImplementedException();
    }
}
