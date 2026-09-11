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
/// <exception cref="InvalidOperationException">Thrown when <paramref name="availableRenderers"/> or <paramref name="availableColourMixers"/> is empty.</exception>
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
    public partial IRendererSettings RendererSettings { get; set; } = CreateRendererSettings(availableRenderers.First(), null);

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
    public partial IColourMixer ColourMixer { get; set; } = CreateColourMixer(availableColourMixers.First())
        ?? throw new InvalidOperationException($"Could not create an instance of {availableColourMixers.FirstOrDefault()?.MixerType.FullName}.");

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

    /// <summary>
    /// Called when the selected renderer changes.
    /// </summary>
    /// <param name="value">The new selected renderer.</param>
    partial void OnSelectedRendererChanged(RendererDescriptor? value)
    {
        if (value is null)
            return;

        RendererSettings = CreateRendererSettings(value, RendererSettings);
    }

    /// <summary>
    /// Creates a new instance of the renderer settings based on the provided descriptor and previous settings.
    /// </summary>
    /// <param name="descriptor">The renderer descriptor to create settings for.</param>
    /// <param name="previous">The previous renderer settings, if any.</param>
    /// <returns>The new renderer settings.</returns>
    private static IRendererSettings CreateRendererSettings(RendererDescriptor descriptor, IRendererSettings? previous)
        => RendererSettingsMigrator.Migrate(previous, descriptor.SettingsType);

    /// <summary>
    /// Creates a new instance of the colour mixer based on the provided descriptor.
    /// </summary>
    /// <param name="value">The colour mixer descriptor to create an instance for.</param>
    /// <exception cref="InvalidOperationException">Thrown when the colour mixer instance could not be created.</exception>
    partial void OnSelectedColourMixerChanged(ColourMixerDescriptor? value)
    {
        if (value is null)
            return;

        ColourMixer = CreateColourMixer(value) ?? throw new InvalidOperationException($"Could not create an instance of {value?.MixerType.FullName}.");
    }

    /// <summary>
    /// Creates a new instance of the colour mixer based on the provided descriptor.
    /// </summary>
    /// <param name="descriptor">The colour mixer descriptor to create an instance for.</param>
    /// <returns>The new colour mixer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the colour mixer instance could not be created.</exception>
    private static IColourMixer CreateColourMixer(ColourMixerDescriptor descriptor)
    {
        if (Activator.CreateInstance(descriptor.MixerType) is not IColourMixer mixer)
            throw new InvalidOperationException($"Could not create an instance of {descriptor.MixerType.FullName}.");

        return mixer;
    }
}