using System;

namespace VoronoiNoiseGenerator.Models;

public readonly struct Neighbour(double x, double y, double distanceSquared)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double DistanceSquared { get; } = distanceSquared;
    public readonly double Distance => Math.Sqrt(DistanceSquared);
}