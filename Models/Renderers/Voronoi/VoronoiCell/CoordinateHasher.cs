using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Provides a method to hash 2D coordinates into a unique colour value.
/// </summary>
public sealed class CoordinateHasher
{
    /// <summary>
    /// Singleton backing field.
    /// </summary>
    private CoordinateHasher? _instance = null;

    /// <summary>
    /// Gets the singleton instance of the <see cref="CoordinateHasher"/> class.
    /// </summary>
    public CoordinateHasher Instance => _instance ??= new CoordinateHasher();

    /// <summary>
    /// A thread-safe cache for storing computed colours based on 2D coordinates.
    /// </summary>
    private static readonly ConcurrentDictionary<(double X, double Y), Colour> _colorCache = new();

    /// <summary>
    /// Helper method to turn a 3D vector into a double value in the range [0, 1].<br/>
    /// </summary>
    /// <param name="x">First component.</param>
    /// <param name="y">Second component.</param>
    /// <param name="z">Third component.</param>
    /// <returns>A double value in the range [0, 1].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private double GetValue(double x, double y, double z)
        => CoordinateHash.ToUnit(CoordinateHash.Hash(x, y, z));

    /// <summary>
    /// Gets a hashed colour given 2D coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The hashed colour.</returns>
    public Colour GetColour(double x, double y)
    {
        return _colorCache.GetOrAdd((x, y), key => new(
            GetValue(key.X, key.Y, 0),
            GetValue(key.X, key.Y, 1),
            GetValue(key.X, key.Y, 2),
            GetValue(key.X, key.Y, 3)
        ));
    }
}