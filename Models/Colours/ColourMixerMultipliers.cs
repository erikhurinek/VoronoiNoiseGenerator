using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents a colour mixer that multiplies both the foreground and background colours by a specified factor, and adds them together based on a channel selection.
/// </summary>
public partial class ColourMixerMultipliers : ObservableObject, IColourMixer
{
    /// <summary>
    /// Gets the descriptor for this colour mixer.
    /// </summary>
    public static ColourMixerDescriptor Descriptor => new("Multiply Add Mix", typeof(ColourMixerMultipliers));

    /// <summary>
    /// The factor to use for mixing the colours.
    /// </summary>
    [ObservableProperty]
    public partial float ForegroundMultiplier { get; set; } = 0.5f;

    /// <summary>
    /// The factor to use for mixing the colours.
    /// </summary>
    [ObservableProperty]
    public partial float BackgroundMultiplier { get; set; } = 0.5f;

    /// <summary>
    /// Whether to divide the result by the sum of the multipliers.
    /// </summary>
    [ObservableProperty]
    public partial bool Normalise { get; set; } = false;

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
    /// Adds the foreground scaled by the multiplier to the background, based on the channel selection.
    /// </summary>
    /// <inheritdoc/>
    public Vector4 Mix(Vector4 foreground, Vector4 background)
    {
        // Create a channel mask.
        Vector4 mask = new(
            MixRed ? 1.0f : 0.0f,
            MixGreen ? 1.0f : 0.0f,
            MixBlue ? 1.0f : 0.0f,
            MixAlpha ? 1.0f : 0.0f
        );

        // Calculate the result with the mask applied.
        // The inverse of the mask is applied to the background.
        Vector4 result = foreground * ForegroundMultiplier * mask
                       + background * BackgroundMultiplier * mask
                       + background * (Vector4.One - mask);

        // Normalise if requested.
        float total = ForegroundMultiplier + BackgroundMultiplier;
        if (Normalise && total > 0.0f)
            result /= (ForegroundMultiplier + BackgroundMultiplier);

        return result;
    }
}