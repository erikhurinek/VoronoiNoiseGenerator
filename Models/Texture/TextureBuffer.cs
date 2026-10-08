using System;
using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A buffer of texture data, represented as a 2D array of <see cref="Vector4"/> values.
/// </summary>
/// <param name="width">The texture width.</param>
/// <param name="height">The texture height.</param>
public sealed class TextureBuffer(int width, int height)
{
    /// <summary>
    /// The width of the texture buffer in pixels.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// The height of the texture buffer in pixels.
    /// </summary>
    public int Height { get; } = height;

    /// <summary>
    /// Internal row-major array of pixels, where each pixel is represented as a <see cref="Vector4"/> (RGBA).
    /// </summary>
    private readonly Vector4[] _pixels = new Vector4[width * height];

    /// <summary>
    /// Get a span representing a row of pixels in the texture buffer.
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public Span<Vector4> Row(int y) => _pixels.AsSpan(y * Width, Width);

    /// <summary>
    /// Get a reference to the pixel at the specified (x, y) coordinates in the texture buffer.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The pixel at (<paramref name="x"/>, <paramref name="y"/>).</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the coordinates are out of bounds.</exception>
    public ref Vector4 this[int x, int y] => ref _pixels[y * Width + x];

    /// <summary>
    /// Returns the entire pixel buffer as a span of <see cref="Vector4"/> values.
    /// </summary>
    /// <returns>A span representing the entire pixel buffer.</returns>
    public Span<Vector4> AsSpan() => _pixels;
}