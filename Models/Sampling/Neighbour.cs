using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a neighbour sample with its coordinates and distance from another reference sample.
/// </summary>
/// <param name="x">The x-coordinate of the neighbour sample.</param>
/// <param name="y">The y-coordinate of the neighbour sample.</param>
/// <param name="distanceSquared">The squared distance from the reference sample.</param>
public readonly struct Neighbour(double x, double y, double distanceSquared)
{
    /// <summary>
    /// The x-coordinate of the neighbour sample.
    /// </summary>
    public double X { get; } = x;

    /// <summary>
    /// The y-coordinate of the neighbour sample.
    /// </summary>
    public double Y { get; } = y;

    /// <summary>
    /// The squared distance from the reference sample.
    /// </summary>
    public double DistanceSquared { get; } = distanceSquared;

    /// <summary>
    /// The distance from the reference sample, calculated as the square root of the squared distance.
    /// </summary>
    public readonly double Distance => Math.Sqrt(DistanceSquared);
}