using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Interface for a renderer that can modify a bitmap.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// The renderer descriptor, which uniquely identifies the renderer.
    /// </summary>
    RendererDescriptor Descriptor { get; }

    /// <summary>
    /// Renders the specified settings onto the target bitmap.
    /// </summary>
    /// <param name="target">The bitmap to modify.</param>
    /// <param name="settings">The settings to use for rendering.</param>
    void Render(WriteableBitmap target, PassSettings settings);
}
