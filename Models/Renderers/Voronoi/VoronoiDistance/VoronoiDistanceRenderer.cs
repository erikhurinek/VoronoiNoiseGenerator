using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer for the Euclidean distance to the nearest sample.
/// </summary>
public sealed class VoronoiDistanceRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Voronoi Distance", GetType(), typeof(VoronoiRendererSettings));

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

        // Iterate over each pixel, and set the color based on the distance.
        BitmapWriter.Write(target, colourMixer, (x, y) =>
        {
            // Get all relevant samples.
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius * 2);

            // There should usually be a sample, but if the settings were misconfigured, 
            // return a transparent pixel.
            if (!neighbours.Any())
                return Colour.Black;

            // Get the nearest sample.
            Neighbour minSample = neighbours.OrderBy(n => n.DistanceSquared).First();

            // Calculate and set the intensity.
            double intensity = Math.Min(minSample.Distance / rendererSettings.Radius, 1.0);
            return new Colour(intensity, intensity, intensity, intensity);
        });
    }
}
