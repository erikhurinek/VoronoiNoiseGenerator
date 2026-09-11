using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Describes an implementation of <see cref="IRenderer"/> 
/// </summary>
/// <param name="DisplayName">The display name to show in the UI.</param>
/// <param name="RendererType">The type of the renderer.</param>
/// <param name="SettingsType">The type of the renderer settings.</param>
public sealed record RendererDescriptor(
    string DisplayName,
    Type RendererType,
    Type SettingsType
)
{
    public override string ToString() => DisplayName;
}
