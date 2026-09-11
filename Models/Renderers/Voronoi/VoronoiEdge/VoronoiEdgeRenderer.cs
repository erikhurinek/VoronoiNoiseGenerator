using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that displays the distance to the perpendicular bisector between the closest two samples.
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
            // Get the two nearest neighbours.
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius).OrderBy(n => n.DistanceSquared).Take(2);

            // If there are fewer than two neighbours, return black.
            if (!neighbours.Any() || neighbours.Count() < 2)
                return Colour.Black;

            // Get the two nearest neighbours.
            Neighbour neighbour1 = neighbours.First();
            Neighbour neighbour2 = neighbours.Skip(1).First();

            // Calculate the distance to the perpendicular bisector between the two nearest neighbours.
            (double x1, double y1) = (neighbour1.X, neighbour1.Y);
            (double x2, double y2) = (neighbour2.X, neighbour2.Y);
            double neighbourDistance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            double distance = (neighbour1.DistanceSquared - neighbour2.DistanceSquared) / (2.0 * neighbourDistance);
            double intensity = Math.Clamp(Math.Abs(distance) / (rendererSettings.Radius * 2.0), 0.0, 1.0);

            return new Colour(intensity, intensity, intensity, intensity);
        });
    }
}
