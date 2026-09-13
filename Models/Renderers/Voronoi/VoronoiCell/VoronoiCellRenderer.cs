using System;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that generates a Voronoi cell pattern based on the nearest sample point.
/// </summary>
public sealed class VoronoiCellRenderer : IRenderer
{
    /// <summary>
    /// Gets the descriptor for this renderer.
    /// </summary>
    public static RendererDescriptor Descriptor => new("Voronoi Cell", typeof(VoronoiCellRenderer), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings
            ?? throw new ArgumentException("Invalid renderer settings for VoronoiDistanceRenderer.");

        // We consider the cell coordinates in this renderer, and we want to wrap them if necessary.
        WrapBehaviour wrapBehaviour = rendererSettings.Wrap ? WrapBehaviour.WrapDistanceAndCoordinates : WrapBehaviour.NoWrap;

        // Create a Poisson disc sampler.
        ISampler sampler = SamplerFactory.CreatePoissonDiscSampler(
            target.Width,
            target.Height,
            rendererSettings.Radius,
            rendererSettings.MaxSamples,
            rendererSettings.Subsamples,
            rendererSettings.Seed,
            wrapBehaviour
        );

        // Generate the samples.
        ISampleCollection samples = sampler.GenerateSamples();

        // Create a colour cache to store the colours of the samples.
        CoordinateHasher hasher = new();

        // Iterate over each pixel, and set the color based the nearest sample.
        TextureBufferIterator.Iterate(target, settings.SelectedColourMixer, (x, y) =>
        {
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius).OrderBy(n => n.DistanceSquared);

            if (!neighbours.Any())
                return Colour.Black;

            Neighbour nearestSample = neighbours.First();

            return hasher.GetColour(nearestSample.X, nearestSample.Y);
        });
    }
}
