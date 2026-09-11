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
    public partial double Factor { get; set; } = 1;

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
    private double RedFactor { get => MixRed ? Factor : 0; }

    /// <summary>
    /// Helper property for the effective mixing factor for the green channel, based on whether it is enabled for mixing.
    /// </summary>
    private double GreenFactor { get => MixGreen ? Factor : 0; }

    /// <summary>
    /// Helper property for the effective mixing factor for the blue channel, based on whether it is enabled for mixing.
    /// </summary>
    private double BlueFactor { get => MixBlue ? Factor : 0; }

    /// <summary>
    /// Helper property for the effective mixing factor for the alpha channel, based on whether it is enabled for mixing.
    /// </summary>
    private double AlphaFactor { get => MixAlpha ? Factor : 0; }

    /// <summary>
    /// Linearly interpolates two colours based on the factor and channel selection.
    /// A factor of 1 means the foreground is fully used.
    /// </summary>
    /// <inheritdoc/>
    public Colour Mix(Colour foreground, Colour background)
    {
        double red = (foreground.Red * RedFactor) + (background.Red * (1 - RedFactor));
        double green = (foreground.Green * GreenFactor) + (background.Green * (1 - GreenFactor));
        double blue = (foreground.Blue * BlueFactor) + (background.Blue * (1 - BlueFactor));
        double alpha = (foreground.Alpha * AlphaFactor) + (background.Alpha * (1 - AlphaFactor));

        return new Colour(red, green, blue, alpha);
    }
}