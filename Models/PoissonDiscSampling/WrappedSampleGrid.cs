using System;
using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public sealed class WrappedSampleGrid(
    double width,
    double height,
    int gridWidth,
    int gridHeight) : ISampleGrid
{
    private readonly Dictionary<(int X, int Y), (double X, double Y)> _samples = new();

    public double Width { get; } = width;
    public double Height { get; } = height;

    public int GridWidth { get; } = gridWidth;
    public int GridHeight { get; } = gridHeight;

    private int VoxelCountX => (int)Math.Ceiling(Width / GridWidth);
    private int VoxelCountY => (int)Math.Ceiling(Height / GridHeight);

    private static int Mod(int value, int modulus) => (value % modulus + modulus) % modulus;

    private (double X, double Y) WrapSample(double x, double y) => (
            Mod((int)Math.Floor(x), (int)Width) + (x - Math.Floor(x)),
            Mod((int)Math.Floor(y), (int)Height) + (y - Math.Floor(y))
        );

    private (int X, int Y) SampleToVoxel(double x, double y)
    {
        var wrapped = WrapSample(x, y);

        return (
            Math.Clamp((int)(wrapped.X / GridWidth), 0, VoxelCountX - 1),
            Math.Clamp((int)(wrapped.Y / GridHeight), 0, VoxelCountY - 1)
        );
    }

    private (int X, int Y) WrapVoxel(int x, int y) => (
            Mod(x, VoxelCountX),
            Mod(y, VoxelCountY)
        );

    public void Add(double x, double y)
    {
        var sample = WrapSample(x, y);
        var voxel = SampleToVoxel(sample.X, sample.Y);

        _samples[voxel] = sample;
    }

    public bool Occupied(double x, double y) => _samples.ContainsKey(SampleToVoxel(x, y));

    public IEnumerable<Neighbour> Neighbours(
        double x,
        double y,
        double distance)
    {
        var sample = WrapSample(x, y);
        var voxel = SampleToVoxel(sample.X, sample.Y);

        var radiusX = (int)Math.Ceiling(distance / GridWidth);
        var radiusY = (int)Math.Ceiling(distance / GridHeight);

        for (var dx = -radiusX; dx <= radiusX; dx++)
        {
            for (var dy = -radiusY; dy <= radiusY; dy++)
            {
                var neighbourVoxel = WrapVoxel(
                    voxel.X + dx,
                    voxel.Y + dy);

                if (!_samples.TryGetValue(neighbourVoxel, out var neighbour))
                    continue;

                var distanceX = Math.Abs(neighbour.X - sample.X);
                var distanceY = Math.Abs(neighbour.Y - sample.Y);

                distanceX = Math.Min(distanceX, Width - distanceX);
                distanceY = Math.Min(distanceY, Height - distanceY);

                var distanceSquared =
                    distanceX * distanceX +
                    distanceY * distanceY;

                if (distanceSquared <= distance * distance)
                {
                    yield return new Neighbour(
                        neighbour.X,
                        neighbour.Y,
                        distanceSquared);
                }
            }
        }
    }
}