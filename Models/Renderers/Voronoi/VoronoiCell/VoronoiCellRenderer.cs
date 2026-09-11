using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that generates a Voronoi cell pattern based on the nearest sample point.
/// </summary>
public sealed class VoronoiCellRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Voronoi Cell", GetType(), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(WriteableBitmap target, PassSettings settings)
    {
        // Get the colour mixer
        IColourMixer colourMixer = settings.ColourMixer;

        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings;

        if (rendererSettings is null)
            throw new ArgumentException("Invalid renderer settings for VoronoiDistanceRenderer.");

        // Create a Poisson disc sampler.
        ISampler sampler = SamplerFactory.CreatePoissonDiscSampler(
            target.PixelSize.Width,
            target.PixelSize.Height,
            rendererSettings.Radius,
            rendererSettings.MaxSamples,
            rendererSettings.Subsamples,
            rendererSettings.Wrap,
            rendererSettings.Seed
        );

        // Generate the samples.
        ISampleCollection samples = sampler.GenerateSamples();

        // Create a colour cache to store the colours of the samples.
        CoordinateHasher hasher = new();

        // Iterate over each pixel, and set the color based the nearest sample.
        BitmapWriter.Write(target, colourMixer, (x, y) =>
        {
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius).OrderBy(n => n.DistanceSquared);

            if (!neighbours.Any())
                return Colour.Black;

            Neighbour nearestSample = neighbours.First();

            return hasher.GetColour(nearestSample.X, nearestSample.Y);
        });
    }
}
