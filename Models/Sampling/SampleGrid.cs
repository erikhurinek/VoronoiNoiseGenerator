using System;
using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A grid-based implementation of <see cref="ISampleGrid"/> that stores samples in a dictionary keyed by grid cell coordinates.
/// </summary>
/// <param name="width">The width of the grid in sample space.</param>
/// <param name="height">The height of the grid in sample space.</param>
/// <param name="gridWidth">The width of each grid cell in sample space.</param>
/// <param name="gridHeight">The height of each grid cell in sample space.</param>
public sealed class SampleGrid(double width, double height, int gridWidth, int gridHeight) : ISampleGrid
{
    /// <summary>
    /// A dictionary that maps grid cell coordinates to the sample stored in that cell.
    /// </summary>
    private readonly Dictionary<(int, int), (double, double)> _voxelMap = new();

    /// <inheritdoc/>
    public double Width { get; } = width;

    /// <inheritdoc/>
    public double Height { get; } = height;

    /// <inheritdoc/>
    public int GridWidth { get; } = gridWidth;

    /// <inheritdoc/>
    public int GridHeight { get; } = gridHeight;

    /// <summary>
    /// Helper method to convert sample coordinates to grid cell coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the sample.</param>
    /// <param name="y">The y-coordinate of the sample.</param>
    /// <returns>The grid cell coordinates.</returns>
    private (int X, int Y) SampleToVoxel(double x, double y) => ((int)x / GridWidth, (int)y / GridHeight);

    /// <param name="distance">The Manhattan distance to search for neighbours.</param>
    /// <inheritdoc/>
    public IEnumerable<Neighbour> Neighbours(double x, double y, double distance)
    {
        // Calculate the voxel coordinates.
        var (voxelX, voxelY) = SampleToVoxel(x, y);

        // Calculate the limits of the search area in voxel space.
        int voxelDistanceX = (int)Math.Ceiling(distance / GridWidth);
        int voxelDistanceY = (int)Math.Ceiling(distance / GridHeight);

        // Iterate over the neighbouring voxels within the distance.
        for (int dx = -voxelDistanceX; dx <= voxelDistanceX; dx++)
        {
            for (int dy = -voxelDistanceY; dy <= voxelDistanceY; dy++)
            {
                // Calculate the neighbour voxel.
                var neighbourVoxel = (voxelX + dx, voxelY + dy);

                // Return the neighbour sample if it exists.
                if (_voxelMap.TryGetValue(neighbourVoxel, out var sample))
                {
                    var distanceSquared = (sample.Item1 - x) * (sample.Item1 - x) + (sample.Item2 - y) * (sample.Item2 - y);
                    yield return new Neighbour(sample.Item1, sample.Item2, distanceSquared);
                }
            }
        }
    }

    /// <inheritdoc/>
    public void Add(double x, double y)
    {
        var voxel = SampleToVoxel(x, y);
        _voxelMap[voxel] = (x, y);
    }

    /// <summary>
    /// Returns true if the grid cell corresponding to the sample coordinates is occupied by a sample,
    /// or if the sample coordinates are out of bounds.
    /// </summary>
    /// <inheritdoc/>
    public bool Occupied(double x, double y)
    {
        var voxel = SampleToVoxel(x, y);
        return _voxelMap.ContainsKey(voxel);
    }

    /// <inheritdoc/>
    public bool OutOfBounds(double x, double y) => x < 0 || x >= Width || y < 0 || y >= Height;
}