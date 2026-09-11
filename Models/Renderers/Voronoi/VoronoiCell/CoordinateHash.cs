using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Provides a method to hash 3D coordinates into a unique ulong value.
/// Useful for turning 2D coordinates and a colour channel into a unique value.
/// </summary>
public static class CoordinateHash
{
    // SplitMix64 finaliser - public domain (Vigna/Stafford), excellent avalanche behaviour
    /// <summary>
    /// Mixes a ulong value to provide a good distribution of bits.<br/>
    /// Based on the SplitMix64 algorithm from <see href="https://github.com/svaarala/duktape/blob/master/misc/splitmix64.c"/>.
    /// </summary>
    /// <param name="z">The ulong value to be mixed.</param>
    /// <returns>A mixed ulong value with a good distribution of bits.</returns>
    private static ulong Mix(ulong z)
    {
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    /// <summary>
    /// Hashes 3D vector (x, y, z) into a unique ulong value.
    /// </summary>
    /// <param name="x">First component.</param>
    /// <param name="y">Second component.</param>
    /// <param name="z">Third component.</param>
    /// <returns>A ulong hash value.</returns>
    public static ulong Hash(double x, double y, double z)
    {
        ulong hx = unchecked((ulong)BitConverter.DoubleToInt64Bits(x));
        ulong hy = unchecked((ulong)BitConverter.DoubleToInt64Bits(y));
        ulong hz = unchecked((ulong)BitConverter.DoubleToInt64Bits(z));

        ulong combined = hx * 0x9E3779B97F4A7C15UL
                        ^ hy * 0xC2B2AE3D27D4EB4FUL
                        ^ hz * 0xFF51AFD7ED558CCDUL;

        return Mix(combined);
    }

    /// <summary>
    /// Generates a double value in the range [0, 1] from a ulong value.
    /// </summary>
    /// <param name="h">The ulong value to convert.</param>
    /// <returns>A double value in the range [0, 1].</returns>
    public static double ToUnit(ulong h) => (h >> 11) * (1.0 / (1UL << 53));
}