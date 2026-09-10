using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer for the Euclidean distance to the nearest sample.
/// </summary>
public class VoronoiDistanceRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Voronoi Distance", typeof(VoronoiDistanceRenderer), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(WriteableBitmap target, PassSettings settings)
    {
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings;

        if (rendererSettings is null)
            throw new ArgumentException("Invalid renderer settings for VoronoiDistanceRenderer.");

        ISampler sampler = SamplerFactory.CreatePoissonDiscSampler(
            target.PixelSize.Width,
            target.PixelSize.Height,
            rendererSettings.Radius,
            rendererSettings.MaxSamples,
            rendererSettings.Subsamples,
            rendererSettings.Wrap
        );
        var samples = sampler.GenerateSamples();
        PixelIterator.IteratePixels(target, (x, y) =>
        {
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius * 2);
            if (neighbours.Any())
            {
                Neighbour minSample = neighbours.OrderBy(n => n.Distance).First();

                byte intensity = (byte)(Math.Min(minSample.Distance / rendererSettings.Radius, 1.0) * 255);
                return RGBA.FromRgba(intensity, intensity, intensity, 255);
            }
            else
            {
                return RGBA.Transparent;
            }
        });
    }
}
