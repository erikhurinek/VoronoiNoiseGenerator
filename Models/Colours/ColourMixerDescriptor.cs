using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Describes a colour mixer.
/// </summary>
/// <param name="DisplayName">The display name of the colour mixer.</param>
/// <param name="MixerType">The type of colour mixer. See <see cref="IColourMixer"/>.</param>
public sealed record ColourMixerDescriptor(
    string Name,
    Type DescribedType
) : IDescriptor
{
    /// <inheritdoc/>
    public override string ToString() => Name;
}