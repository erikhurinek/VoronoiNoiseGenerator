using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Represents the settings for a single rendering pass.
/// </summary>
/// <param name="availableRenderers">The list of available renderer descriptors.</param>
/// <param name="availableColourMixers">The list of available colour mixers.</param>
public partial class PassSettings(IEnumerable<RendererDescriptor> availableRenderers, IEnumerable<ColourMixerDescriptor> availableColourMixers) : ObservableObject
{
    /// <summary>
    /// List of available renderer descriptors to select from.
    /// </summary>
    public IReadOnlyList<RendererDescriptor> AvailableRenderers { get; } = availableRenderers.ToList();

    /// <summary>
    /// The currently selected renderer descriptor.
    /// </summary>
    [ObservableProperty]
    public partial RendererDescriptor? SelectedRenderer { get; set; } = availableRenderers.FirstOrDefault();

    /// <summary>
    /// The settings specific to the selected renderer.
    /// </summary>
    [ObservableProperty]
    public partial IRendererSettings? RendererSettings { get; set; } = CreateRendererSettings(availableRenderers.FirstOrDefault());

    /// <summary>
    /// List of available colour mixers to select from.
    /// </summary>
    public IReadOnlyList<ColourMixerDescriptor> AvailableColourMixers { get; } = availableColourMixers.ToList();

    /// <summary>
    /// The currently selected colour mixer.
    /// </summary>
    [ObservableProperty]
    public partial ColourMixerDescriptor? SelectedColourMixer { get; set; } = availableColourMixers.FirstOrDefault();

    /// <summary>
    /// The currently selected colour mixer instance.
    /// </summary>
    [ObservableProperty]
    public partial IColourMixer ColourMixer { get; set; } = CreateColourMixer(availableColourMixers.FirstOrDefault());

    /// <summary>
    /// Whether this pass should be rendered.
    /// </summary>
    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    /// <summary>
    /// The X offset for rendering this pass.
    /// </summary>
    [ObservableProperty]
    public partial int X { get; set; } = 0;

    /// <summary>
    /// The Y offset for rendering this pass.
    /// </summary>
    [ObservableProperty]
    public partial int Y { get; set; } = 0;

    partial void OnSelectedRendererChanged(RendererDescriptor? value)
    {
        RendererSettings = CreateRendererSettings(value);
    }

    private static IRendererSettings? CreateRendererSettings(RendererDescriptor? descriptor)
    {
        if (descriptor == null)
            return null;

        return Activator.CreateInstance(descriptor.SettingsType) as IRendererSettings;
    }

    partial void OnSelectedColourMixerChanged(ColourMixerDescriptor? value)
    {
        ColourMixer = CreateColourMixer(value) ?? throw new InvalidOperationException($"Could not create an instance of {value.MixerType.FullName}.");
    }

    private static IColourMixer? CreateColourMixer(ColourMixerDescriptor? descriptor)
    {
        if (descriptor == null)
            return null;

        return Activator.CreateInstance(descriptor.MixerType) as IColourMixer;
    }
}