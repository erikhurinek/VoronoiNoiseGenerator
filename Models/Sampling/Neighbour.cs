using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a neighbour sample with its coordinates and distance from another reference point.
/// </summary>
/// <param name="x">The x-coordinate of this neighbour sample.</param>
/// <param name="y">The y-coordinate of this neighbour sample.</param>
/// <param name="distanceSquared">The squared distance from the reference sample.</param>
public readonly struct Neighbour(float x, float y, float distanceSquared)
{
    /// <summary>
    /// The x-coordinate of the neighbour sample.
    /// </summary>
    public float X { get; } = x;

    /// <summary>
    /// The y-coordinate of the neighbour sample.
    /// </summary>
    public float Y { get; } = y;

    /// <summary>
    /// The squared distance from the reference sample.
    /// </summary>
    public float DistanceSquared { get; } = distanceSquared;

    /// <summary>
    /// The distance from the reference sample, calculated as the square root of the squared distance.
    /// </summary>
    public readonly float Distance => MathF.Sqrt(DistanceSquared);
}