using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a colour with red, green, blue, and alpha components.
/// </summary>
public readonly struct Colour
{
    /// <summary>
    /// THe underlying vector4 representing this colour.
    /// </summary>
    private readonly Vector4 _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Colour"/> struct with the specified red, green, blue, and alpha components.
    /// </summary>
    /// <param name="red">The red channel.</param>
    /// <param name="green">The green channel.</param>
    /// <param name="blue">The blue channel.</param>
    /// <param name="alpha">The alpha channel.</param>
    public Colour(float red, float green, float blue, float alpha)
        => _value = new Vector4(red, green, blue, alpha);

    /// <summary>
    /// Initializes a new instance of the <see cref="Colour"/> struct with all components set to the specified value.
    /// </summary>
    /// <param name="value">The value of all components.</param>
    private Colour(Vector4 value) => _value = value;

    /// <summary>
    /// Gets the red channel of the colour.
    /// </summary>
    public float Red => _value.X;

    /// <summary>
    /// Gets the green channel of the colour.
    /// </summary>
    public float Green => _value.Y;

    /// <summary>
    /// Gets the blue channel of the colour.
    /// </summary>
    public float Blue => _value.Z;

    /// <summary>
    /// Gets the alpha channel of the colour.
    /// </summary>
    public float Alpha => _value.W;

    public static implicit operator Vector4(Colour colour) => colour._value;
    public static explicit operator Colour(Vector4 vector) => new(vector);

    /// <summary>
    /// Converts the colour to a tuple of bytes representing the RGBA channels.
    /// </summary>
    /// <returns>A tuple containing the red, green, blue, and alpha channels as bytes.</returns>
    public readonly (byte r, byte g, byte b, byte a) ToByteTuple()
    {
        Vector4 clamped = Vector4.Clamp(_value, Vector4.Zero, Vector4.One) * 255f;
        return ((byte)clamped.X, (byte)clamped.Y, (byte)clamped.Z, (byte)clamped.W);
    }

    /// <summary>
    /// Creates a <see cref="Colour"/> from byte values for the RGBA channels.
    /// Each channel is expected to be in the range [0, 255].
    /// </summary>
    /// <param name="r">The red channel.</param>
    /// <param name="g">The green channel.</param>
    /// <param name="b">The blue channel.</param>
    /// <param name="a">The alpha channel.</param>
    /// <returns>A new instance of <see cref="Colour"/> with the specified channel values.</returns>
    public static Colour FromByteRGBA(byte r, byte g, byte b, byte a) =>
        new(r / 255f, g / 255f, b / 255f, a / 255f);

    /// <summary>
    /// Gets a <see cref="Colour"/> representing black (0, 0, 0, 1).
    /// </summary>
    public static readonly Colour Black = new(0f, 0f, 0f, 1f);

    /// <summary>
    /// Gets a <see cref="Colour"/> representing white (1, 1, 1, 1).
    /// </summary>
    public static readonly Colour White = new(1f, 1f, 1f, 1f);

    /// <summary>
    /// Gets a <see cref="Colour"/> representing transparent (0, 0, 0, 0).
    /// </summary>
    public static readonly Colour Transparent = new(0f, 0f, 0f, 0f);

    /// <summary>
    /// Gets a <see cref="Colour"/> representing deep red (0.86, 0.08, 0.24, 1).
    /// </summary>
    public static readonly Colour DeepRed = new(0.86f, 0.08f, 0.24f, 1f);
}