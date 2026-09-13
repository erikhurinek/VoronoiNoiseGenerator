using System.Numerics;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for colour mixers.
/// </summary>
public interface IColourMixer : IFactoryInstantiable<ColourMixerDescriptor>
{
    /// <summary>
    /// Mixes two colours based on the specific mixing logic.
    /// </summary>
    /// <param name="foreground">The foreground colour.</param>
    /// <param name="background">The background colour.</param>
    /// <returns>The mixed colour.</returns>
    public Vector4 Mix(Vector4 foreground, Vector4 background);
}