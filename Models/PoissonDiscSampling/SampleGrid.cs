using System;
using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public class SampleGrid(double width, double height, int gridWidth, int gridHeight) : ISampleGrid
{
    private readonly Dictionary<(int, int), (double, double)> _voxelMap = new Dictionary<(int, int), (double, double)>();

    public double Width { get; } = width;

    public double Height { get; } = height;

    public int GridWidth { get; } = gridWidth;

    public int GridHeight { get; } = gridHeight;

    private (int, int) SampleToVoxel(double x, double y) => ((int)x / GridWidth, (int)y / GridHeight);

    public IEnumerable<Neighbour> Neighbours(double x, double y, double distance)
    {
        var voxel = SampleToVoxel(x, y);
        int voxelDistanceX = (int)Math.Ceiling(distance / GridWidth);
        int voxelDistanceY = (int)Math.Ceiling(distance / GridHeight);
        for (int dx = -voxelDistanceX; dx <= voxelDistanceX; dx++)
        {
            for (int dy = -voxelDistanceY; dy <= voxelDistanceY; dy++)
            {
                var neighbourVoxel = (voxel.Item1 + dx, voxel.Item2 + dy);
                if (_voxelMap.TryGetValue(neighbourVoxel, out var sample))
                {
                    var distanceSquared = (sample.Item1 - x) * (sample.Item1 - x) + (sample.Item2 - y) * (sample.Item2 - y);
                    yield return new Neighbour(sample.Item1, sample.Item2, distanceSquared);
                }
            }
        }
    }

    public void Add(double x, double y)
    {
        var voxel = SampleToVoxel(x, y);
        _voxelMap[voxel] = (x, y);
    }

    public bool Occupied(double x, double y)
    {
        var voxel = SampleToVoxel(x, y);
        return _voxelMap.ContainsKey(voxel);
    }
}