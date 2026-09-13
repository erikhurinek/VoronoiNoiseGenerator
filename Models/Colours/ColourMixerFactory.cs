using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A factory for creating instances of <see cref="IColourMixer"/> implementations.
/// </summary>
public sealed class ColourMixerFactory : SingletonFactory<IColourMixer, ColourMixerDescriptor>
{
    /// <inheritdoc/>
    public override IEnumerable<ColourMixerDescriptor> Descriptors =>
    [
        ColourMixerChannel.Descriptor,
    ];
}