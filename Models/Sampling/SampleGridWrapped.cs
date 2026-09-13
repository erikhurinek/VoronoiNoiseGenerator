using System;
using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A sample grid that handles wrapping around the edges.
/// </summary>
/// <param name="width">The width of the grid in sample space.</param>
/// <param name="height">The height of the grid in sample space.</param>
/// <param name="gridWidth">The width of each grid cell in sample space.</param>
/// <param name="gridHeight">The height of each grid cell in sample space.</param>
/// <param name="wrapBehaviour">The wrapping behavior for the sample grid.</param>
/// <remarks>
/// Initializes a new instance of the <see cref="SampleGridWrapped"/> class.
/// </remarks>
/// <param name="width">The width of the grid in sample space.</param>
/// <param name="height">The height of the grid in sample space.</param>
/// <param name="gridWidth">The width of each grid cell in sample space.</param>
/// <param name="gridHeight">The height of each grid cell in sample space.</param>
/// <param name="wrapBehaviour">The wrapping behavior for the sample grid.</param>
public sealed class SampleGridWrapped(float width, float height, int gridWidth, int gridHeight, WrapBehaviour wrapBehaviour) : ISampleGrid
{
    /// <summary>
    /// A dictionary that maps grid cell coordinates to the sample stored in that cell.
    /// </summary>
    private readonly Dictionary<(int X, int Y), (float X, float Y)> _samples = new();

    /// <inheritdoc/>
    public float Width { get; } = width;

    /// <inheritdoc/>
    public float Height { get; } = height;

    /// <inheritdoc/>
    public int GridWidth { get; } = gridWidth;

    /// <inheritdoc/>
    public int GridHeight { get; } = gridHeight;

    /// <summary>
    /// The wrapping behavior for the sample grid.
    /// </summary>
    public WrapBehaviour WrapBehaviour { get; } = wrapBehaviour;

    /// <summary>The number of grid cells in the X direction.</summary>
    private int VoxelCountX => (int)MathF.Ceiling(Width / GridWidth);

    /// <summary>The number of grid cells in the Y direction.</summary>
    private int VoxelCountY => (int)MathF.Ceiling(Height / GridHeight);

    /// <inheritdoc cref="SampleGridWrapped(float, float, int, int, WrapBehaviour)"/>
    public SampleGridWrapped(float width, float height, int gridWidth, int gridHeight)
        : this(width, height, gridWidth, gridHeight, WrapBehaviour.WrapDistanceUnwrapCoordinates)
    {
    }

    /// <summary>
    /// Calculates the floored (positive) modulus.
    /// </summary>
    /// <param name="value">The value to be modded.</param>
    /// <param name="modulus">The modulus.</param>
    /// <returns>The floored modulus of the value.</returns>
    private static int Mod(int value, int modulus) => (value % modulus + modulus) % modulus;

    /// <summary>
    /// Wraps the sample coordinates to ensure they are within the bounds of the grid.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    /// <returns>The wrapped sample coordinates.</returns>
    private (float X, float Y) WrapSample(float x, float y) => (
            Mod((int)MathF.Floor(x), (int)Width) + (x - MathF.Floor(x)),
            Mod((int)MathF.Floor(y), (int)Height) + (y - MathF.Floor(y))
        );

    /// <summary>
    /// Converts sample coordinates to grid cell coordinates, wrapping the sample.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    /// <returns>The grid cell coordinates.</returns>
    private (int X, int Y) SampleToVoxel(float x, float y)
    {
        var wrapped = WrapSample(x, y);

        return (
            Mod((int)(wrapped.X / GridWidth), VoxelCountX - 1),
            Mod((int)(wrapped.Y / GridHeight), VoxelCountY - 1)
        );
    }

    /// <summary>
    /// Wraps the grid cell coordinates to ensure they are within the bounds of the grid.
    /// </summary>
    /// <param name="x">Grid cell x-coordinate.</param>
    /// <param name="y">Grid cell y-coordinate.</param>
    /// <returns>The wrapped grid cell coordinates.</returns>
    private (int X, int Y) WrapVoxel(int x, int y) => (
            Mod(x, VoxelCountX),
            Mod(y, VoxelCountY)
        );

    /// <inheritdoc/>
    public void Add(float x, float y)
    {
        var sample = WrapSample(x, y);
        var voxel = SampleToVoxel(sample.X, sample.Y);

        _samples[voxel] = sample;
    }

    /// <inheritdoc/>
    public bool Occupied(float x, float y) => _samples.ContainsKey(SampleToVoxel(x, y));

    /// <param name="distance">The Manhattan distance to search for neighbours.</param>
    /// <inheritdoc/>
    public IEnumerable<Neighbour> Neighbours(
        float x,
        float y,
        float distance)
    {
        // Wrap the sample and calculate voxel coordinates.
        var sample = WrapSample(x, y);
        var voxel = SampleToVoxel(sample.X, sample.Y);

        // Calculate the limits of the search area in voxel space.
        var voxelDistanceX = (int)MathF.Ceiling(distance / GridWidth);
        var voxelDistanceY = (int)MathF.Ceiling(distance / GridHeight);

        var halfWidth = Width / 2.0f;
        var halfHeight = Height / 2.0f;

        // Iterate over the neighbouring voxels within the distance.
        for (var dx = -voxelDistanceX; dx <= voxelDistanceX; dx++)
        {
            for (var dy = -voxelDistanceY; dy <= voxelDistanceY; dy++)
            {
                // Calculate the neighbour voxel.
                var neighbourVoxel = WrapVoxel(
                    voxel.X + dx,
                    voxel.Y + dy);

                // If there's no sample in the neighbour voxel, skip to the next iteration.
                if (!_samples.TryGetValue(neighbourVoxel, out var neighbour))
                    continue;

                // Get the neighbour's coordinates.
                var neighbourX = neighbour.X;
                var neighbourY = neighbour.Y;

                // Calculate the distance to the neighbour, taking wrapping into account if necessary.
                float distanceX;
                float distanceY;

                // Calculate the correct wrapped distance and neighbour coordinates.
                if (WrapBehaviour == WrapBehaviour.WrapDistanceUnwrapCoordinates)
                {

                    if (neighbourX - sample.X > halfWidth)
                        neighbourX -= Width;
                    else if (sample.X - neighbourX > halfWidth)
                        neighbourX += Width;

                    if (neighbourY - sample.Y > halfHeight)
                        neighbourY -= Height;
                    else if (sample.Y - neighbourY > halfHeight)
                        neighbourY += Height;

                    distanceX = neighbourX - sample.X;
                    distanceY = neighbourY - sample.Y;
                }
                else
                {
                    distanceX = MathF.Abs(neighbour.X - sample.X);
                    distanceY = MathF.Abs(neighbour.Y - sample.Y);

                    distanceX = MathF.Min(distanceX, Width - distanceX);
                    distanceY = MathF.Min(distanceY, Height - distanceY);
                }

                var distanceSquared =
                    distanceX * distanceX +
                    distanceY * distanceY;

                // Return the unwrapped neighbour sample.
                yield return new Neighbour(
                    neighbourX,
                    neighbourY,
                    distanceSquared);
            }
        }
    }

    /// <summary>
    /// Always returns false, since this grid is wrapped.
    /// </summary>
    /// <inheritdoc/>
    public bool OutOfBounds(float x, float y) => false;
}