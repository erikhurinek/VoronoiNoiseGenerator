using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that fills the bitmap with a solid colour.
/// </summary>
public sealed class SolidColourRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new RendererDescriptor("Solid Colour", GetType(), typeof(SolidColourRendererSettings));

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