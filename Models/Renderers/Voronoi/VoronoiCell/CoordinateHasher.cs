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
    private static readonly ConcurrentDictionary<(float X, float Y), Colour> _colorCache = new();

    /// <summary>
    /// Helper method to turn a 3D vector into a float value in the range [0, 1].<br/>
    /// </summary>
    /// <param name="x">First component.</param>
    /// <param name="y">Second component.</param>
    /// <param name="z">Third component.</param>
    /// <returns>A float value in the range [0, 1].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float GetValue(float x, float y, float z)
        => CoordinateHash.ToUnit(CoordinateHash.Hash(x, y, z));

    /// <summary>
    /// Gets a hashed colour given 2D coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The hashed colour.</returns>
    public Colour GetColour(float x, float y)
    {
        return _colorCache.GetOrAdd((x, y), key => new(
            GetValue(key.X, key.Y, 0.0f),
            GetValue(key.X, key.Y, 1.0f),
            GetValue(key.X, key.Y, 2.0f),
            GetValue(key.X, key.Y, 3.0f)
        ));
    }
}