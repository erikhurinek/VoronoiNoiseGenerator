using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that fills the bitmap with a solid colour.
/// </summary>
public sealed class SolidColourRenderer : IRenderer
{
    /// <summary>
    /// Gets the descriptor for this renderer.
    /// </summary>
    public static RendererDescriptor Descriptor => new("Solid Colour", typeof(SolidColourRenderer), typeof(SolidColourRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Get the colour mixer
        IColourMixer colourMixer = settings.SelectedColourMixer;

        // Cast the renderer settings to the expected type.
        SolidColourRendererSettings? rendererSettings = settings.RendererSettings as SolidColourRendererSettings
            ?? throw new ArgumentException("Invalid renderer settings for SolidColourRenderer.");

        // Fill the bitmap with the solid colour.
        TextureBufferIterator.Iterate(target, colourMixer, (x, y) => rendererSettings.Colour);
    }
}