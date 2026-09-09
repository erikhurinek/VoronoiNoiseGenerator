using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

public partial class PassSettings(IEnumerable<RendererDescriptor> renderers, RendererDescriptor selectedDescriptor) : ObservableObject
{
    public IReadOnlyList<RendererDescriptor> AvailableDescriptors { get; } = renderers.ToList();

    [ObservableProperty]
    public partial RendererDescriptor? SelectedDescriptor { get; set; } = selectedDescriptor;

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial int X { get; set; } = 0;

    [ObservableProperty]
    public partial int Y { get; set; } = 0;

    [ObservableProperty]
    public partial int Width { get; set; } = 128;

    [ObservableProperty]
    public partial int Height { get; set; } = 128;

    [ObservableProperty]
    public partial int Radius { get; set; } = 10;
    [ObservableProperty]
    public partial int Samples { get; set; } = 1000;
    [ObservableProperty]
    public partial int Subsamples { get; set; } = 30;
    [ObservableProperty]
    public partial int Seed { get; set; } = 0;
    [ObservableProperty]
    public partial bool Wrap { get; set; } = false;
}
