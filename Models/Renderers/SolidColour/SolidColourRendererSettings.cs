using System;
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
    public partial double R { get; set; } = 0.0;

    /// <summary>
    /// The green channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double G { get; set; } = 0.0;

    /// <summary>
    /// The blue channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double B { get; set; } = 0.0;

    /// <summary>
    /// The alpha channel of the solid colour, in the range [0, 1].
    /// </summary>
    [ObservableProperty]
    public partial double A { get; set; } = 1.0;

    /// <summary>
    /// Gets the solid colour as a <see cref="Colour"/> instance.
    /// </summary>
    public Colour Colour => new(R, G, B, A);
}