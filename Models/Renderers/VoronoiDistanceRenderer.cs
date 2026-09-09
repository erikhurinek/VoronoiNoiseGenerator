using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;


public class VoronoiDistanceRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new(typeof(VoronoiDistanceRenderer), "Voronoi Distance");

    public WriteableBitmap Render(WriteableBitmap source, PassSettings settings)
    {
        PoissonDiscSampler sampler = new(source.PixelSize.Width, source.PixelSize.Height);
        var samples = sampler.GenerateSamples(settings.Radius, settings.Samples, settings.Subsamples, settings.Wrap);
        PixelIterator.IteratePixels(source, (x, y) =>
        {
            var neighbours = samples.Neighbours(x, y, settings.Radius * 2);
            if (neighbours.Any())
            {
                Neighbour minSample = neighbours.OrderBy(n => n.Distance).First();

                byte intensity = (byte)(Math.Min(minSample.Distance / settings.Radius, 1.0) * 255);
                return RGBA.FromRgba(intensity, intensity, intensity, 255);
            }
            else
            {
                return RGBA.Transparent;
            }
        });

        return source;
    }
}
