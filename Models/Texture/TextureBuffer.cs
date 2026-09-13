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
    public int Width { get; } = width;
    public int Height { get; } = height;

    private readonly Vector4[] _pixels = new Vector4[width * height];

    public Span<Vector4> Row(int y) => _pixels.AsSpan(y * Width, Width);
    public ref Vector4 this[int x, int y] => ref _pixels[y * Width + x];
    public Span<Vector4> AsSpan() => _pixels;
}