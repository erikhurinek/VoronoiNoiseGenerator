using System;

namespace VoronoiNoiseGenerator.Models;

public sealed record RendererDescriptor(
    string DisplayName,
    Type RendererType,
    Type SettingsType
)
{
    public override string ToString() => DisplayName;
}
