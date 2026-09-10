using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

public partial class VoronoiRendererSettings : ObservableObject, IRendererSettings
{
    [ObservableProperty]
    public partial int Radius { get; set; } = 10;

    [ObservableProperty]
    public partial int MaxSamples { get; set; } = 1000;

    [ObservableProperty]
    public partial int Subsamples { get; set; } = 30;

    [ObservableProperty]
    public partial int Seed { get; set; }

    [ObservableProperty]
    public partial bool Wrap { get; set; }
}