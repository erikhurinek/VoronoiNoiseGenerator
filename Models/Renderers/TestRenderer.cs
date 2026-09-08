using Avalonia.Media.Imaging;

namespace VoronoiNoiseGenerator.Models;

public class TestRenderer : IRenderer
{
    public RendererDescriptor Descriptor => new(typeof(TestRenderer), "Test");

    public WriteableBitmap Render(WriteableBitmap source)
    {
        using var framebuffer = source.Lock();

        unsafe
        {
            byte* pixels = (byte*)framebuffer.Address;

            for (int y = 0; y < framebuffer.Size.Height; y++)
            {
                byte* row = pixels + y * framebuffer.RowBytes;

                for (int x = 0; x < framebuffer.Size.Width; x++)
                {
                    byte* pixel = row + x * 4;

                    pixel[0] = (byte)(x * 255 / framebuffer.Size.Height);
                    pixel[1] = (byte)(y * 255 / framebuffer.Size.Width);
                    pixel[2] = 0;   // B
                    pixel[3] = 255; // A
                }
            }
        }

        return source;
    }
}
