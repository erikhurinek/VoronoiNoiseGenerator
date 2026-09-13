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
    public partial RendererDescriptor? SelectedRendererDescriptor { get; set; } = availableRenderers.FirstOrDefault();

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
    /// The currently selected colour mixer descriptor.
    /// </summary>
    [ObservableProperty]
    public partial ColourMixerDescriptor? SelectedColourMixerDescriptor { get; set; } = availableColourMixers.FirstOrDefault();

    /// <summary>
    /// The currently selected colour mixer instance.
    /// </summary>
    [ObservableProperty]
    public partial IColourMixer SelectedColourMixer { get; private set; } = CreateColourMixer(availableColourMixers.First())
        ?? throw new InvalidOperationException($"Could not create an instance of {availableColourMixers.FirstOrDefault()?.DescribedType.FullName}.");

    /// <summary>
    /// Whether this pass should be rendered.
    /// </summary>
    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;


    /// <summary>
    /// Whether this pass is currently hovered over in the UI by the cursor.
    /// </summary>
    [ObservableProperty]
    public partial bool IsHovered { get; set; } = false;

    /// <summary>
    /// Called when the selected renderer changes.
    /// </summary>
    /// <param name="value">The new selected renderer.</param>
    partial void OnSelectedRendererDescriptorChanged(RendererDescriptor? value)
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
    partial void OnSelectedColourMixerDescriptorChanged(ColourMixerDescriptor? value)
    {
        if (value is null)
            return;

        SelectedColourMixer = CreateColourMixer(value) ?? throw new InvalidOperationException($"Could not create an instance of {value?.DescribedType.FullName}.");
    }

    /// <summary>
    /// Creates a new instance of the colour mixer based on the provided descriptor.
    /// </summary>
    /// <param name="descriptor">The colour mixer descriptor to create an instance for.</param>
    /// <returns>The new colour mixer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the colour mixer instance could not be created.</exception>
    private static IColourMixer CreateColourMixer(ColourMixerDescriptor descriptor)
    {
        if (Activator.CreateInstance(descriptor.DescribedType) is not IColourMixer mixer)
            throw new InvalidOperationException($"Could not create an instance of {descriptor.DescribedType.FullName}.");

        return mixer;
    }
}