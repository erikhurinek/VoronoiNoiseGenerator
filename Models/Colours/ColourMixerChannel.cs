using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a colour mixer that linearly mixes two colours based on a specified factor and channel selection.
/// </summary>
public partial class ColourMixerChannel : ObservableObject, IColourMixer
{
    /// <inheritdoc/>
    public ColourMixerDescriptor Descriptor => new("Channel Mixer", GetType());

    /// <summary>
    /// The factor to use for mixing the colours.
    /// </summary>
    [ObservableProperty]
    public partial float Factor { get; set; } = 1.0f;

    /// <summary>
    /// Whether the red channel should be mixed.
    /// </summary>
    [ObservableProperty]
    public partial bool MixRed { get; set; } = true;

    /// <summary>
    /// Whether the green channel should be mixed.
    /// </summary>
    [ObservableProperty]
    public partial bool MixGreen { get; set; } = true;

    /// <summary>
    /// Whether the blue channel should be mixed.
    /// </summary>
    [ObservableProperty]
    public partial bool MixBlue { get; set; } = true;

    /// <summary>
    /// Whether the alpha channel should be mixed.
    /// </summary>
    [ObservableProperty]
    public partial bool MixAlpha { get; set; } = true;

    /// <summary>
    /// Helper property for the effective mixing factor for the red channel, based on whether it is enabled for mixing.
    /// </summary>
    private float RedFactor { get => MixRed ? Factor : 0.0f; }

    /// <summary>
    /// Helper property for the effective mixing factor for the green channel, based on whether it is enabled for mixing.
    /// </summary>
    private float GreenFactor { get => MixGreen ? Factor : 0.0f; }

    /// <summary>
    /// Helper property for the effective mixing factor for the blue channel, based on whether it is enabled for mixing.
    /// </summary>
    private float BlueFactor { get => MixBlue ? Factor : 0.0f; }

    /// <summary>
    /// Helper property for the effective mixing factor for the alpha channel, based on whether it is enabled for mixing.
    /// </summary>
    private float AlphaFactor { get => MixAlpha ? Factor : 0.0f; }

    /// <summary>
    /// Linearly interpolates two colours based on the factor and channel selection.
    /// A factor of 1 means the foreground is fully used.
    /// </summary>
    /// <inheritdoc/>
    public Vector4 Mix(Vector4 foreground, Vector4 background)
    {
        return Vector4.Lerp(
            background,
            foreground,
            new Vector4(RedFactor, GreenFactor, BlueFactor, AlphaFactor)
        );
    }
}