namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Interface for a sampler that generates a collection of samples.
/// </summary>
public interface ISampler
{
    /// <summary>
    /// Generates a collection of samples based on the sampler's parameters.
    /// </summary>
    /// <returns>A collection of samples.</returns>
    public ISampleCollection GenerateSamples();
}