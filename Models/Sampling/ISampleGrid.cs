namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Interface for a finite grid collection of samples.
/// See <see cref="ISampleCollection"/> for more details.
/// </summary>
public interface ISampleGrid : ISampleCollection
{
    /// <summary>
    /// The width of the grid in sample space.
    /// </summary>
    float Width { get; }

    /// <summary>
    /// The height of the grid in sample space.
    /// </summary>
    float Height { get; }

    /// <summary>
    /// The width of each grid cell in sample space.
    /// </summary>
    public int GridWidth { get; }

    /// <summary>
    /// The height of each grid cell in sample space.
    /// </summary>
    public int GridHeight { get; }

    /// <summary>
    /// Returns true if the grid cell corresponding to the given sample coordinates is occupied by a sample.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    /// <returns>Whether the grid cell is occupied by a sample.</returns>
    public bool Occupied(float x, float y);

    /// <summary>
    /// Returns true if the given sample coordinates are out of bounds of the grid.
    /// </summary>
    /// <param name="x">Sample x-coordinate.</param>
    /// <param name="y">Sample y-coordinate.</param>
    /// <returns>Whether the sample is out of bounds.</returns>
    public bool OutOfBounds(float x, float y);
}