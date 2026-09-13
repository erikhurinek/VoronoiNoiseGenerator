using System;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A static helper class for preparing <see cref="TextureBuffer"/> instances for display in a UI.
/// </summary>
public static class TextureBufferExtensions
{
    /// <summary>
    /// Creates a <see cref="WriteableBitmap"/> from a <see cref="TextureBuffer"/>.
    /// </summary>
    /// <param name="source">The source bitmap to copy and modify.</param>
    /// <param name="opaque">Indicates whether the resulting bitmap should be opaque.</param>
    /// <returns>A new, opaque <see cref="WriteableBitmap"/>.</returns>
    public static unsafe WriteableBitmap CreateBitmap(this TextureBuffer source, bool opaque)
    {
        // Create the destination bitmap.
        var destination = new WriteableBitmap(
            new PixelSize(source.Width, source.Height),
            new Avalonia.Vector(96, 96),
            PixelFormat.Rgba8888,
            opaque ? AlphaFormat.Opaque : AlphaFormat.Premul
        );

        // Lock the destination.
        using var framebuffer = destination.Lock();
        byte* basePtr = (byte*)framebuffer.Address;
        int stride = framebuffer.RowBytes;

        // Iterate over columns.
        Parallel.For(0, source.Height, y =>
        {
            // Get the row.
            Span<Vector4> row = source.Row(y);

            // Get the pixel at the start of the row.
            byte* dstPixel = basePtr + y * stride;

            // Iterate over each pixel in the row.
            for (int x = 0; x < source.Width; x++)
            {
                // Clamp the pixel values to [0, 1] and convert to byte.
                Vector4 clamped = Vector4.Clamp(row[x], Vector4.Zero, Vector4.One);
                dstPixel[0] = (byte)(clamped.X * 255f + 0.5f);
                dstPixel[1] = (byte)(clamped.Y * 255f + 0.5f);
                dstPixel[2] = (byte)(clamped.Z * 255f + 0.5f);
                dstPixel[3] = opaque ? (byte)255 : (byte)(clamped.W * 255f + 0.5f);

                // Move to the next pixel.
                dstPixel += 4;
            }
        });

        return destination;
    }
}