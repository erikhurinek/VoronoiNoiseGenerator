using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

public class PoissonDiscSampler(
    int width,
    int height
)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public ISampleCollection GenerateSamples(int radius, int maxSamples, int subsamples, bool wrapped)
    {
        int cellSize = (int)(radius / Math.Sqrt(2));
        ISampleGrid grid = wrapped
            ? new WrappedSampleGrid(Width, Height, cellSize, cellSize)
            : new SampleGrid(Width, Height, cellSize, cellSize);

        Queue<(double, double)> activeQueue = new();
        var initialSample = (Width / 2.0, Height / 2.0);
        int sampleCount = 1;

        activeQueue.Enqueue(initialSample);
        grid.Add(initialSample.Item1, initialSample.Item2);

        while (activeQueue.Count > 0 && sampleCount < maxSamples)
        {
            var sample = activeQueue.Dequeue();

            for (int i = 0; i < subsamples; i++)
            {
                double angle = Random.Shared.NextDouble() * 2 * Math.PI;
                double distance = radius + Random.Shared.NextDouble() * radius;
                var candidate = (
                    sample.Item1 + distance * Math.Cos(angle),
                    sample.Item2 + distance * Math.Sin(angle)
                );
                var neighbours = grid.Neighbours(candidate.Item1, candidate.Item2, radius);
                var tooClose = neighbours.Any(n => n.DistanceSquared < radius * radius);

                if (!tooClose && !grid.Occupied(candidate.Item1, candidate.Item2))
                {
                    grid.Add(candidate.Item1, candidate.Item2);
                    activeQueue.Enqueue(candidate);
                    sampleCount++;
                }
            }
        }

        return grid;
    }
}