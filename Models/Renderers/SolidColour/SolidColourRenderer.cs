using System;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that fills the bitmap with a solid colour.
/// </summary>
public sealed class SolidColourRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Solid Colour", GetType(), typeof(SolidColourRendererSettings));

    /// <inheritdoc/>
    public void Render(WriteableBitmap target, PassSettings settings)
    {
        // Get the colour mixer
        IColourMixer colourMixer = settings.ColourMixer;

        // Cast the renderer settings to the expected type.
        SolidColourRendererSettings? rendererSettings = settings.RendererSettings as SolidColourRendererSettings
            ?? throw new ArgumentException("Invalid renderer settings for SolidColourRenderer.");

        // Fill the bitmap with the solid colour.
        BitmapWriter.Write(target, colourMixer, (x, y) => rendererSettings.Colour);
    }
}