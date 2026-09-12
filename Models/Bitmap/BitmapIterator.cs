using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Provides a utility for iterating over the pixels of a WriteableBitmap and applying a function to each pixel.
/// </summary>
public static class BitmapIterator
{
    /// <summary>
    /// Iterate over each pixel and apply a function.
    /// </summary>
    /// <param name="bitmap">The bitmap to iterate over.</param>
    /// <param name="colorMixer">The color mixer to use.</param>
    /// <param name="function">A function that takes x and y coordinates and returns an RGBA color.</param>
    public static unsafe void Iterate(WriteableBitmap bitmap, IColourMixer colorMixer, Func<int, int, Colour> function)
    {
        // Lock the bitmap for writing.
        using var framebuffer = bitmap.Lock();

        // Get a pointer to the pixel data.
        byte* pixels = (byte*)framebuffer.Address;

        // Iterate over each pixel in the bitmap.
        Parallel.For(0, framebuffer.Size.Height, y =>
        {
            // Start address of the current row.
            byte* row = pixels + y * framebuffer.RowBytes;
            for (int x = 0; x < framebuffer.Size.Width; x++)
            {
                // Address of the current pixel.
                byte* pixel = row + x * 4;

                // Mix the colours.
                Colour foreground = function(x, y);
                Colour background = Colour.FromByteRGBA(pixel[0], pixel[1], pixel[2], pixel[3]);
                Colour rgba = colorMixer.Mix(foreground, background);

                // Convert the colour to bytes and write it to the pixel.
                (byte r, byte g, byte b, byte a) = rgba.ToByteTuple();
                pixel[0] = r;
                pixel[1] = g;
                pixel[2] = b;
                pixel[3] = a;
            }
        });
    }
}