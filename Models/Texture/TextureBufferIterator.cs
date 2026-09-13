using System;
using System.Numerics;
using System.Threading.Tasks;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A static helper class for iterating over a <see cref="TextureBuffer"/> and applying a colour mixing operation to each pixel.
/// </summary>
public static class TextureBufferIterator
{
    /// <summary>
    /// Iterate over each pixel and mix with the result of the provided function.
    /// </summary>
    /// <param name="target">The target <see cref="TextureBuffer"/> to iterate over.</param>
    /// <param name="colourMixer">The <see cref="IColourMixer"/> to use for mixing colours.</param>
    /// <param name="function">The function that takes the x and y coordinates and returns a <see cref="Vector4"/> colour to mix with the target pixel.</param>
    public static void Iterate(TextureBuffer target, IColourMixer colourMixer, Func<int, int, Vector4> function)
    {
        // Iterate over columns in parallel.
        Parallel.For(0, target.Height, y =>
        {
            // Iterate over each row and mix the colours.
            Span<Vector4> row = target.Row(y);
            for (int x = 0; x < target.Width; x++)
            {
                Vector4 foreground = function(x, y);
                row[x] = colourMixer.Mix(foreground, row[x]);
            }
        });
    }
}