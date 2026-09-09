using System;
using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public record struct RGBA(byte R, byte G, byte B, byte A)
{
    public static RGBA FromRgba(int r, int g, int b, int a) => new((byte)r, (byte)g, (byte)b, (byte)a);
    public static readonly RGBA Transparent = new(0, 0, 0, 0);
    public static readonly RGBA Black = new(0, 0, 0, 255);
}

public static class PixelIterator
{
    public static void IteratePixels(WriteableBitmap bitmap, Func<int, int, RGBA> function)
    {
        using var framebuffer = bitmap.Lock();

        unsafe
        {
            byte* pixels = (byte*)framebuffer.Address;

            for (int y = 0; y < framebuffer.Size.Height; y++)
            {
                byte* row = pixels + y * framebuffer.RowBytes;

                for (int x = 0; x < framebuffer.Size.Width; x++)
                {
                    byte* pixel = row + x * 4;

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