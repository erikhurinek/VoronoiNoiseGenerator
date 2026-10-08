using System;
using System.Linq;
using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A renderer that displays the position of the nearest sample.
/// The RGBA channels returned are (x, y, 1-x, 1-y) where x and y are the scaled coordinates of the nearest sample.
/// The coordinates will be remapped (unclamped) from the range [-1,2] to [0,1] to ensure that samples outside of the texture do not overflow.
/// Therefore (1/3,1/3) corresponds to a sample in the top left corner, and (2/3,2/3) corresponds to one in the bottom right.
/// </summary>
public sealed class VoronoiPositionRenderer : IRenderer
{
    /// <summary>
    /// Gets the descriptor for this renderer.
    /// </summary>
    public static RendererDescriptor Descriptor => new("Voronoi Position", typeof(VoronoiPositionRenderer), typeof(VoronoiRendererSettings));

    /// <inheritdoc/>
    public void Render(TextureBuffer target, PassSettings settings)
    {
        // Cast the renderer settings to the expected type.
        VoronoiRendererSettings? rendererSettings = settings.RendererSettings as VoronoiRendererSettings
            ?? throw new ArgumentException($"Invalid renderer settings for {nameof(VoronoiPositionRenderer)}.");

        // We consider the cell coordinates in this renderer, and we want to wrap them if necessary.
        WrapBehaviour wrapBehaviour = rendererSettings.Wrap ? WrapBehaviour.WrapDistanceAndCoordinates : WrapBehaviour.NoWrap;

        // Create a Poisson disc sampler and generate the samples.
        ISampleCollection samples = SamplerFactory.CreatePoissonDiscSampler(
            target.Width,
            target.Height,
            rendererSettings.Radius,
            rendererSettings.MaxSamples,
            rendererSettings.Subsamples,
            rendererSettings.Seed,
            wrapBehaviour
        ).GenerateSamples();

        // Iterate over each pixel, and set the color based the position of the nearest sample.
        TextureBufferIterator.Iterate(target, settings.SelectedColourMixer, (x, y) =>
        {
            // Get all relevant samples.
            var neighbours = samples.Neighbours(x, y, rendererSettings.Radius).OrderBy(n => n.DistanceSquared);

            // There should usually be a sample, but if the settings were misconfigured, 
            // return a transparent pixel.
            if (!neighbours.Any())
                return Colour.Transparent;

            // Get the nearest sample.
            Neighbour nearestSample = neighbours.First();

            // Calculate and remap the position from [-1,2] to [0,1] for the colour channels.
            const float oneThird = 1.0f / 3.0f;
            float factorX = oneThird + oneThird * nearestSample.X / target.Width;
            float factorY = oneThird + oneThird * nearestSample.Y / target.Height;

            // Return the position.
            return new Vector4(factorX, factorY, 1.0f - factorX, 1.0f - factorY);
        });
    }
}
