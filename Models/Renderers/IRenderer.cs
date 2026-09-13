namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Interface for a renderer that can modify a bitmap.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Renders the specified settings onto the target bitmap.
    /// </summary>
    /// <param name="target">The bitmap to modify.</param>
    /// <param name="settings">The settings to use for rendering.</param>
    void Render(TextureBuffer target, PassSettings settings);
}
