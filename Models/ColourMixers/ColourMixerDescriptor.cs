using System;

namespace VoronoiNoiseGenerator.Models;

public sealed record ColourMixerDescriptor(
    string DisplayName,
    Type MixerType
)
{
    public override string ToString() => DisplayName;
}