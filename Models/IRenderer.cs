using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public interface IRenderer
{
    RendererDescriptor Descriptor { get; }
    WriteableBitmap Render(WriteableBitmap source);
}
