namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a color with red, green, blue, and alpha components.
/// Colors are represented as double values in the range [0, 1].
/// </summary>
/// <param name="red">The red channel.</param>
/// <param name="green">The green channel.</param>
/// <param name="blue">The blue channel.</param>
/// <param name="alpha">The alpha channel.</param>
public readonly struct Colour(double red, double green, double blue, double alpha)
{
    /// <summary>
    /// The red channel of the color, in the range [0, 1].
    /// </summary>
    public double Red { get; } = red;

    /// <summary>
    /// The green channel of the color, in the range [0, 1].
    /// </summary>
    public double Green { get; } = green;

    /// <summary>
    /// The blue channel of the color, in the range [0, 1].
    /// </summary>
    public double Blue { get; } = blue;

    /// <summary>
    /// The alpha channel of the color, in the range [0, 1].
    /// </summary>
    public double Alpha { get; } = alpha;

    /// <summary>
    /// Converts the color to a tuple of bytes representing the RGBA channels.
    /// </summary>
    /// <returns>A tuple containing the red, green, blue, and alpha channels as bytes.</returns>
    public readonly (byte r, byte g, byte b, byte a) ToByteTuple() =>
        ((byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255), (byte)(Alpha * 255));

    /// <summary>
    /// Creates a new color from a tuple of bytes representing the RGBA channels.
    /// </summary>
    /// <param name="r">The red channel.</param>
    /// <param name="g">The green channel.</param>
    /// <param name="b">The blue channel.</param>
    /// <param name="a">The alpha channel.</param>
    /// <returns>A new color instance.</returns>
    public static Colour FromByteRGBA(byte r, byte g, byte b, byte a) =>
        new(r / 255.0, g / 255.0, b / 255.0, a / 255.0);

    /// <summary>
    /// A fully transparent color (0,0,0,0).
    /// </summary>
    public static readonly Colour Transparent = new(0, 0, 0, 0);

    /// <summary>
    /// Black color (0,0,0,1).
    /// </summary>
    public static readonly Colour Black = new(0, 0, 0, 1);

    /// <summary>
    /// White color (1,1,1,1).
    /// </summary>
    public static readonly Colour White = new(1, 1, 1, 1);
}