namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for colour mixers.
/// </summary>
public interface IColourMixer
{
    /// <summary>
    /// The descriptor of the colour mixer, providing its display name and type information.
    /// </summary>
    public ColourMixerDescriptor Descriptor { get; }

    /// <summary>
    /// Mixes two colours based on the specific mixing logic.
    /// </summary>
    /// <param name="foreground">The foreground colour.</param>
    /// <param name="background">The background colour.</param>
    /// <returns>The mixed colour.</returns>
    public Colour Mix(Colour foreground, Colour background);
}