using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Settings for all Voronoi renderers.
/// </summary>
public partial class VoronoiRendererSettings : ObservableObject, IRendererSettings
{
    /// <summary>
    /// The minimum distance between Voronoi samples.
    /// </summary>
    [ObservableProperty]
    public partial int Radius { get; set; } = 10;

    /// <summary>
    /// The maximum number of samples to generate.
    /// </summary> 
    [ObservableProperty]
    public partial int MaxSamples { get; set; } = 1000;

    /// <summary>
    /// The number of candidate points to generate for each sample.
    /// </summary>
    [ObservableProperty]
    public partial int Subsamples { get; set; } = 20;

    /// <summary>
    /// The random seed.
    /// </summary>
    [ObservableProperty]
    public partial int Seed { get; set; } = 0;

    /// <summary>
    /// Whether the samples should wrap and the texture appear seamless.
    /// </summary>
    [ObservableProperty]
    public partial bool Wrap { get; set; } = false;
}