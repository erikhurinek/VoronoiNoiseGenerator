using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Describes an implementation of <see cref="IRenderer"/> 
/// </summary>
/// <param name="Name">The display name to show in the UI.</param>
/// <param name="DescribedType">The type of the renderer.</param>
/// <param name="SettingsType">The type of the renderer settings.</param>
public sealed record RendererDescriptor(
    string Name,
    Type DescribedType,
    Type SettingsType
) : IDescriptor
{
    public override string ToString() => Name;
}
