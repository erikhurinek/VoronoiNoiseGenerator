using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Interface for a collection of samples.
/// Supports adding a sample and querying for neighbours within a distance.
/// </summary>
public interface ISampleCollection
{
    /// <summary>
    /// Adds a sample to the collection at the specified coordinates.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    public void Add(float x, float y);

    /// <summary>
    /// Returns an enumerable of neighbour samples within the specified distance from the given coordinates.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    /// <param name="distance">The distance within which to search for neighbour samples. How this distance is measured depends on the implementation.</param>
    /// <returns>An enumerable of neighbour samples.</returns>
    IEnumerable<Neighbour> Neighbours(float x, float y, float distance);
}