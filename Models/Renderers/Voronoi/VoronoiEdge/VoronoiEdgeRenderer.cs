using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that displays the distance to the nearest perpendicular bisector between the closest two samples.
/// Unlike <see cref="VoronoiEdgeRenderer"/>, this renderer calculates the distance to all perpendicular bisectors 
/// for all nearby neighbours and returns the minimum. The result is therefore smoother but more expensive.
/// </summary>
public sealed class VoronoiEdgeRenderer : IRenderer
{
    /// <inheritdoc/>
    public RendererDescriptor Descriptor => new("Voronoi Edge", GetType(), typeof(VoronoiRendererSettings));

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

        // Iterate over each pixel, and set the color based the nearest sample.
        BitmapWriter.Write(target, colourMixer, (x, y) =>
        {
            // Get all neighbours.
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius)
                                    .OrderBy(n => n.DistanceSquared)
                                    .ToList();

            // Return black if there are fewer than two neighbours.
            if (neighbours.Count < 2)
                return Colour.Black;

            Neighbour nearest = neighbours[0];
            double nearestDistance = Math.Sqrt(nearest.DistanceSquared);

            double minEdgeDistance = double.PositiveInfinity;

            for (int i = 1; i < neighbours.Count; i++)
            {
                Neighbour other = neighbours[i];
                double otherDistance = Math.Sqrt(other.DistanceSquared);

                double lowerBound = (otherDistance - nearestDistance) * 0.5;
                if (lowerBound >= minEdgeDistance)
                    break;

                double dx = nearest.X - other.X;
                double dy = nearest.Y - other.Y;
                double siteDistance = Math.Sqrt(dx * dx + dy * dy);

                if (siteDistance == 0.0)
                    continue;

                double edgeDistance = (other.DistanceSquared - nearest.DistanceSquared) / (2.0 * siteDistance);

                if (edgeDistance < minEdgeDistance)
                    minEdgeDistance = edgeDistance;
            }

            double intensity = Math.Clamp(minEdgeDistance / (rendererSettings.Radius * 2.0), 0.0, 1.0);
            return new Colour(intensity, intensity, intensity, intensity);
        });
    }
}
