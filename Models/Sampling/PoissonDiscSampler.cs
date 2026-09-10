using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A sampler that generates samples using the Poisson Disc Sampling algorithm.
/// </summary>
/// <param name="width">The width of the sampling area.</param>
/// <param name="height">The height of the sampling area.</param>
/// <param name="radius">The minimum distance between samples.</param>
/// <param name="maxSamples">The maximum number of samples to generate.</param>
/// <param name="subsamples">The number of candidate samples to generate for each active sample.</param>
/// <param name="wrapped">Indicates whether the sampling area is wrapped (toroidal).</param>
public class PoissonDiscSampler(
    double width,
    double height,
    double radius,
    int maxSamples,
    int subsamples,
    bool wrapped
) : ISampler
{
    /// <summary>
    /// The width of the sampling area.
    /// </summary>
    public double Width { get; } = width;

    /// <summary>
    /// The height of the sampling area.
    /// </summary>
    public double Height { get; } = height;

    /// <summary>
    /// The minimum distance between samples.
    /// </summary>
    public double Radius { get; } = radius;

    /// <summary>
    /// The maximum number of samples to generate.
    /// </summary>
    public int MaxSamples { get; } = maxSamples;

    /// <summary>
    /// The number of candidate samples to generate for each active sample.
    /// </summary>
    public int Subsamples { get; } = subsamples;

    /// <summary>
    /// Indicates whether the sampling area is wrapped (toroidal).
    /// </summary>
    public bool Wrapped { get; } = wrapped;

    /// <summary>
    /// Generates a collection of samples based on the sampler's parameters.
    /// </summary>
    /// <returns>A collection of samples.</returns>
    public ISampleCollection GenerateSamples()
    {
        // Each Voronoi sample is given its own grid cell of size radius / sqrt(2).
        // The grid helps with efficient lookup of neighbouring samples.
        // Starting from an initial sample, we repeatedly generate candidate samples around the active sample.
        // If the candidate is far enough from other samples, it is added to the collection and becomes an active sample.

        // Calculate the grid cell size.
        int gridSize = (int)(Radius / Math.Sqrt(2));

        // Initialize the appropriate sample grid.
        ISampleGrid grid = Wrapped
            ? new WrappedSampleGrid(Width, Height, gridSize, gridSize)
            : new SampleGrid(Width, Height, gridSize, gridSize);

        // Randomly select the initial sample.
        (double, double) initialSample = (Random.Shared.NextDouble() * Width, Random.Shared.NextDouble() * Height);
        grid.Add(initialSample.Item1, initialSample.Item2);

        // Use a queue to track active samples for generating candidates.
        Queue<(double, double)> activeQueue = new();
        activeQueue.Enqueue(initialSample);

        // Iteratively generate samples.
        int sampleCount = 1;
        while (activeQueue.Count > 0 && sampleCount < MaxSamples)
        {
            // Dequeue the next active sample.
            var sample = activeQueue.Dequeue();

            // Generate candidate samples around the active sample.
            for (int i = 0; i < Subsamples; i++)
            {
                // Generate random angle and offset.
                double angle = Random.Shared.NextDouble() * 2 * Math.PI;
                double offset = Radius + Random.Shared.NextDouble() * Radius;

                var candidate = (
                    sample.Item1 + offset * Math.Cos(angle),
                    sample.Item2 + offset * Math.Sin(angle)
                );

                // Check if the candidate is too close to existing samples.
                var neighbours = grid.Neighbours(candidate.Item1, candidate.Item2, Radius);
                var tooClose = neighbours.Any(n => n.DistanceSquared < Radius * Radius);

                // Add the candidate if it's valid.
                if (!tooClose && !grid.Occupied(candidate.Item1, candidate.Item2))
                {
                    grid.Add(candidate.Item1, candidate.Item2);
                    activeQueue.Enqueue(candidate);
                    sampleCount++;
                }
            }
        }

        // Return the sample collection.
        return grid;
    }
}