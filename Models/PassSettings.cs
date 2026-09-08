using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

public partial class PassSettings : ObservableObject
{
    public IReadOnlyList<RendererDescriptor> AvailableDescriptors { get; } = new List<RendererDescriptor>();

    [ObservableProperty]
    public partial RendererDescriptor? SelectedDescriptor { get; set; } = null;

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

    public PassSettings(IEnumerable<RendererDescriptor> renderers, RendererDescriptor selectedDescriptor)
    {
        AvailableDescriptors = renderers.ToList();
        SelectedDescriptor = selectedDescriptor;
    }
}
