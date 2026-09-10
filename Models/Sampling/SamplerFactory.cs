namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Factory class for creating samplers.
/// </summary>
public static class SamplerFactory
{
    /// <summary>
    /// Creates an instance of <see cref="PoissonDiscSampler"/>.
    /// </summary>
    /// <param name="width">The width of the sampling area.</param>
    /// <param name="height">The height of the sampling area.</param>
    /// <param name="radius">The minimum distance between samples.</param>
    /// <param name="maxSamples">The maximum number of samples to generate.</param>
    /// <param name="subsamples">The number of candidate points to generate for each sample.</param>
    /// <param name="wrapped">Indicates whether the sampling area is wrapped (toroidal).</param>
    /// <param name="seed">The random seed.</param>
    /// <returns>An instance of <see cref="PoissonDiscSampler"/>.</returns>
    public static ISampler CreatePoissonDiscSampler(double width, double height, double radius, int maxSamples, int subsamples, bool wrapped, int seed) =>
        new PoissonDiscSampler(width, height, radius, maxSamples, subsamples, wrapped, seed);
}