using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Provides utilities for preparing a <see cref="WriteableBitmap"/> for display..
/// </summary>
public static class BitmapDisplay
{
    /// <summary>
    /// Prepares a <see cref="WriteableBitmap"/> for display. Ensure the alpha channel is opaque.
    /// </summary>
    /// <param name="bitmap">The bitmap to prepare.</param>
    /// <returns>A new WriteableBitmap that is ready for display.</returns>
    public static WriteableBitmap PrepareForDisplay(WriteableBitmap bitmap)
    {
        return CopyOpaque(bitmap);
    }

    /// <summary>
    /// Returns a copy of a given bitmap with the alpha channel by performing an OR mask on each pixel.
    /// </summary>
    /// <param name="source">The source bitmap to copy and modify.</param>
    /// <returns>A new, opaque <see cref="WriteableBitmap"/>.</returns>
    private static unsafe WriteableBitmap CopyOpaque(WriteableBitmap source)
    {
        // Create the destination bitmap.
        var destination = new WriteableBitmap(
            source.PixelSize,
            source.Dpi,
            PixelFormat.Rgba8888,
            AlphaFormat.Opaque);

        // Lock both the source and destination bitmaps.
        using ILockedFramebuffer srcFb = source.Lock();
        using ILockedFramebuffer dstFb = destination.Lock();

        int width = srcFb.Size.Width;
        int height = srcFb.Size.Height;
        int srcStride = srcFb.RowBytes;
        int dstStride = dstFb.RowBytes;
        byte* srcBase = (byte*)srcFb.Address;
        byte* dstBase = (byte*)dstFb.Address;

        const uint AlphaMask = 0xFF000000u;
        int vectorWidth = Vector<uint>.Count;
        var maskVector = new Vector<uint>(AlphaMask);

        Parallel.For(0, height, y =>
        {
            uint* srcRow = (uint*)(srcBase + y * srcStride);
            uint* dstRow = (uint*)(dstBase + y * dstStride);
            int x = 0;

            // Efficiently apply the mask using SIMD.
            for (; x <= width - vectorWidth; x += vectorWidth)
            {
                ref byte s = ref Unsafe.AsRef<byte>(srcRow + x);
                var pixels = Unsafe.ReadUnaligned<Vector<uint>>(ref s);
                Unsafe.WriteUnaligned(ref Unsafe.AsRef<byte>(dstRow + x), pixels | maskVector);
            }

            // Handle any remaining pixels that don't fit into a full vector.
            for (; x < width; x++)
                dstRow[x] = srcRow[x] | AlphaMask;
        });

        return destination;
    }
}