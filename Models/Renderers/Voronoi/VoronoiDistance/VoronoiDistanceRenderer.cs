using System;
using System.Linq;
using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer for the Euclidean distance to the nearest sample.
/// </summary>
public sealed class VoronoiDistanceRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new RendererDescriptor("Voronoi Distance", GetType(), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings
            ?? throw new ArgumentException("Invalid renderer settings for VoronoiDistanceRenderer.");

        // It doesn't really matter whether sample coordinates are wrapped or not, since we only care about distance.
        WrapBehaviour wrapBehaviour = rendererSettings.Wrap ? WrapBehaviour.WrapDistanceUnwrapCoordinates : WrapBehaviour.NoWrap;

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

        // Iterate over each pixel, and set the color based on the distance.
        TextureBufferIterator.Iterate(target, settings.SelectedColourMixer, (x, y) =>
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
            float intensity = MathF.Min(minSample.Distance / rendererSettings.Radius / 2.0f, 1.0f);
            return new Vector4(intensity);
        });
    }
}
