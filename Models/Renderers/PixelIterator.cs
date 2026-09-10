using System;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a color in RGBA format.
/// </summary>
/// <param name="R">Red channel.</param>
/// <param name="G">Green channel.</param>
/// <param name="B">Blue channel.</param>
/// <param name="A">Alpha channel.</param>
public record struct RGBA(byte R, byte G, byte B, byte A)
{
    /// <summary>
    /// Creates an RGBA color from integer values, clamping them to the byte range.
    /// </summary>
    /// <param name="r">Red channel.</param>
    /// <param name="g">Green channel.</param>
    /// <param name="b">Blue channel.</param>
    /// <param name="a">Alpha channel.</param>
    /// <returns>The created RGBA color.</returns>
    public static RGBA FromRgba(int r, int g, int b, int a) => new((byte)r, (byte)g, (byte)b, (byte)a);

    /// <summary>
    /// Transparent color (0, 0, 0, 0).
    /// </summary>
    public static RGBA Transparent { get => new(0, 0, 0, 0); }

    /// <summary>
    /// Black color (0, 0, 0, 255).
    /// </summary>
    public static RGBA Black { get => new(0, 0, 0, 255); }
}

/// <summary>
/// Provides a utility for iterating over the pixels of a WriteableBitmap and applying a function to each pixel.
/// </summary>
public static class PixelIterator
{
    /// <summary>
    /// Iterate over each pixel and apply a function.
    /// </summary>
    /// <param name="bitmap">The bitmap to iterate over.</param>
    /// <param name="function">A function that takes x and y coordinates and returns an RGBA color.</param>
    public static void IteratePixels(WriteableBitmap bitmap, Func<int, int, RGBA> function)
    {
        // Lock the bitmap for writing.
        using var framebuffer = bitmap.Lock();

        unsafe
        {
            // Get a pointer to the pixel data.
            byte* pixels = (byte*)framebuffer.Address;

            // Iterate over each pixel in the bitmap.
            for (int y = 0; y < framebuffer.Size.Height; y++)
            {
                // Start address of the current row.
                byte* row = pixels + y * framebuffer.RowBytes;
                for (int x = 0; x < framebuffer.Size.Width; x++)
                {
                    // Address of the current pixel.
                    byte* pixel = row + x * 4;

                    // Modify the pixel.
                    var rgba = function(x, y);
                    pixel[0] = rgba.R;
                    pixel[1] = rgba.G;
                    pixel[2] = rgba.B;
                    pixel[3] = rgba.A;
                }
            }
        }
    }
}