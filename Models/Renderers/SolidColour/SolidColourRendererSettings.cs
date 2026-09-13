using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Settings for the <see cref="SolidColourRenderer"/>, that fills the bitmap with a solid colour.
/// </summary>
public sealed partial class SolidColourRendererSettings : ObservableObject, IRendererSettings
{
    /// <summary>
    /// The red channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double R { get; set; }

    /// <summary>
    /// The green channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double G { get; set; }

    /// <summary>
    /// The blue channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double B { get; set; }

    /// <summary>
    /// The alpha channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double A { get; set; }

    /// <summary>
    /// Gets the solid colour as an <see cref="Avalonia.Media.Color"/> instance.
    /// </summary>
    public SolidColorBrush ColourBrush => new(new Color((byte)(A * 255), (byte)(R * 255), (byte)(G * 255), (byte)(B * 255)));

    /// <summary>
    /// Gets the solid colour as an <see cref="Avalonia.Media.Color"/> instance, with full opacity (alpha = 1).
    /// </summary>
    public SolidColorBrush ColourBrushOpaque => new(new Color(255, (byte)(R * 255), (byte)(G * 255), (byte)(B * 255)));

    /// <summary>
    /// Gets the solid colour as a <see cref="Colour"/> instance.
    /// </summary>
    public Colour Colour
    {
        get => new(R, G, B, A);
        set
        {
            R = value.Red;
            G = value.Green;
            B = value.Blue;
            A = value.Alpha;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolidColourRendererSettings"/> class with default values.
    /// </summary>
    public SolidColourRendererSettings()
    {
        Colour = Colour.DeepRed;
    }

    #region Property Change Handlers
    private void ColourChannelChanged()
    {
        OnPropertyChanged(nameof(ColourBrush));
        OnPropertyChanged(nameof(ColourBrushOpaque));
    }
    partial void OnRChanged(double value) => ColourChannelChanged();
    partial void OnGChanged(double value) => ColourChannelChanged();
    partial void OnBChanged(double value) => ColourChannelChanged();
    partial void OnAChanged(double value) => ColourChannelChanged();
    #endregion
}