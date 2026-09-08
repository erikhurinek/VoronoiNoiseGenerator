using System;

namespace VoronoiNoiseGenerator.Models;

public sealed record RendererDescriptor(
    Type RendererType,
    string DisplayName
)
{
    public override string ToString() => DisplayName;
}
