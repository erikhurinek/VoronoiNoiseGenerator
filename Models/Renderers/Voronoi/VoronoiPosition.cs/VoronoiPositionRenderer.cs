using System;
using System.Linq;
using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that displays the distance to the nearest perpendicular bisector between the closest two samples.
/// Unlike <see cref="VoronoiEdgeRenderer"/>, this renderer calculates the distance to all perpendicular bisectors 
/// for all nearby neighbours and returns the minimum. The result is therefore smoother but more expensive.
/// </summary>
public sealed class VoronoiPositionRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Voronoi Position", GetType(), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings
            ?? throw new ArgumentException($"Invalid renderer settings for {nameof(VoronoiPositionRenderer)}.");

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

        // Iterate over each pixel, and set the color based the position of the nearest sample.
        TextureBufferIterator.Iterate(target, settings.ColourMixer, (x, y) =>
        {
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius).OrderBy(n => n.DistanceSquared);

            if (!neighbours.Any())
                return Colour.Black;

            Neighbour nearestSample = neighbours.First();


            const float oneThird = 1.0f / 3.0f;
            float factorX = oneThird + oneThird * nearestSample.X / target.Width;
            float factorY = oneThird + oneThird * nearestSample.Y / target.Height;

            return new Vector4(factorX, factorY, 1.0f - factorX, 1.0f - factorY);
        });
    }
}
