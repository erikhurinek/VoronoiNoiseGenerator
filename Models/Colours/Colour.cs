using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

public readonly struct Colour
{
    private readonly Vector4 _value;

    public Colour(float red, float green, float blue, float alpha)
        => _value = new Vector4(red, green, blue, alpha);

    private Colour(Vector4 value) => _value = value;

    public float Red => _value.X;
    public float Green => _value.Y;
    public float Blue => _value.Z;
    public float Alpha => _value.W;

    public static implicit operator Vector4(Colour colour) => colour._value;
    public static explicit operator Colour(Vector4 vector) => new(vector);

    public readonly (byte r, byte g, byte b, byte a) ToByteTuple()
    {
        Vector4 clamped = Vector4.Clamp(_value, Vector4.Zero, Vector4.One) * 255f;
        return ((byte)clamped.X, (byte)clamped.Y, (byte)clamped.Z, (byte)clamped.W);
    }

    public static Colour FromByteRGBA(byte r, byte g, byte b, byte a) =>
        new(r / 255f, g / 255f, b / 255f, a / 255f);

    public static readonly Colour Black = new(0f, 0f, 0f, 1f);
    public static readonly Colour White = new(1f, 1f, 1f, 1f);
    public static readonly Colour Transparent = new(0f, 0f, 0f, 0f);
    public static readonly Colour DeepRed = new(0.86f, 0.08f, 0.24f, 1f);
}