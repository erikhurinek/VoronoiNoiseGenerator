using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents the settings for a single rendering pass.
/// </summary>
/// <param name="availableRenderers">The list of available renderer descriptors.</param>
/// <param name="selectedDescriptor">The initially selected renderer descriptor.</param>
public partial class PassSettings(IEnumerable<RendererDescriptor> availableRenderers, RendererDescriptor selectedDescriptor) : ObservableObject
{
    public IReadOnlyList<RendererDescriptor> AvailableDescriptors { get; } = availableRenderers.ToList();

    [ObservableProperty]
    public partial RendererDescriptor SelectedDescriptor { get; set; } = selectedDescriptor;

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial int X { get; set; } = 0;

    [ObservableProperty]
    public partial int Y { get; set; } = 0;

    [ObservableProperty]
    public partial IRendererSettings? RendererSettings { get; set; } = CreateRendererSettings(selectedDescriptor);

    partial void OnSelectedDescriptorChanged(RendererDescriptor value)
    {
        RendererSettings = CreateRendererSettings(value);
    }

    private static IRendererSettings? CreateRendererSettings(RendererDescriptor descriptor)
    {
        return Activator.CreateInstance(descriptor.SettingsType) as IRendererSettings;
    }
}