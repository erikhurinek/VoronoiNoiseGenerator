using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public class VoronoiCellRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new(typeof(VoronoiCellRenderer), "Voronoi Cell");

    public WriteableBitmap Render(WriteableBitmap source, PassSettings settings)
    {
        throw new NotImplementedException();
    }
}
