using System;
using System.Linq;
using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that displays the distance to the nearest perpendicular bisector between the closest two samples.
/// Unlike <see cref="VoronoiEdgeRenderer"/>, this renderer calculates the distance to all perpendicular bisectors 
/// for all nearby neighbours and returns the minimum. The result is therefore smoother but more expensive.
/// </summary>
public sealed class VoronoiEdgeRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new RendererDescriptor("Voronoi Edge", GetType(), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings
            ?? throw new ArgumentException("Invalid renderer settings for VoronoiDistanceRenderer.");

        // We want neighbour coordinates to exist beyond the border of our grid, so that edges are calculated correctly.
        // Therefore, we want them to be unwrapped.
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

        // Iterate over each pixel, and set the color based the nearest sample.
        TextureBufferIterator.Iterate(target, settings.SelectedColourMixer, (x, y) =>
        {
            // Get all neighbours.
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius)
                                    .OrderBy(n => n.DistanceSquared)
                                    .ToList();

            // Return black if there are fewer than two neighbours.
            if (neighbours.Count < 2)
                return Colour.Black;

            Neighbour nearest = neighbours[0];
            float nearestDistance = MathF.Sqrt(nearest.DistanceSquared);

            float minEdgeDistance = float.PositiveInfinity;

            for (int i = 1; i < neighbours.Count; i++)
            {
                Neighbour other = neighbours[i];
                float otherDistance = MathF.Sqrt(other.DistanceSquared);

                float lowerBound = (otherDistance - nearestDistance) * 0.5f;
                if (lowerBound >= minEdgeDistance)
                    break;

                float dx = nearest.X - other.X;
                float dy = nearest.Y - other.Y;
                float siteDistance = MathF.Sqrt(dx * dx + dy * dy);

                if (siteDistance == 0.0f)
                    continue;

                float edgeDistance = (other.DistanceSquared - nearest.DistanceSquared) / (2.0f * siteDistance);

                if (edgeDistance < minEdgeDistance)
                    minEdgeDistance = edgeDistance;
            }

            float intensity = minEdgeDistance / (rendererSettings.Radius * 2.0f);
            return new Vector4(intensity);
        });
    }
}
