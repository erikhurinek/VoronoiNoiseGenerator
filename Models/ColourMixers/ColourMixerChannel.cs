using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

public partial class ColourMixerChannel : ObservableObject, IColourMixer
{
    public ColourMixerDescriptor Descriptor => new("Channel Mixer", typeof(ColourMixerChannel));

    [ObservableProperty]
    public partial double Factor { get; set; } = 0.5;

    [ObservableProperty]
    public partial bool MixRed { get; set; } = true;

    [ObservableProperty]
    public partial bool MixGreen { get; set; } = true;

    [ObservableProperty]
    public partial bool MixBlue { get; set; } = true;

    [ObservableProperty]
    public partial bool MixAlpha { get; set; } = true;
    public double RedFactor { get => MixRed ? Factor : 0; }
    public double GreenFactor { get => MixGreen ? Factor : 0; }
    public double BlueFactor { get => MixBlue ? Factor : 0; }
    public double AlphaFactor { get => MixAlpha ? Factor : 0; }

    public Colour Mix(Colour foreground, Colour background)
    {
        double red = (foreground.Red * RedFactor) + (background.Red * (1 - RedFactor));
        double green = (foreground.Green * GreenFactor) + (background.Green * (1 - GreenFactor));
        double blue = (foreground.Blue * BlueFactor) + (background.Blue * (1 - BlueFactor));
        double alpha = (foreground.Alpha * AlphaFactor) + (background.Alpha * (1 - AlphaFactor));

        return new Colour(red, green, blue, alpha);
    }
}