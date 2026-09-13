using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public sealed class ColourMixerFactory : SingletonFactory<IColourMixer, ColourMixerDescriptor>
{
    protected override IEnumerable<IColourMixer> Instances =>
    [
        new ColourMixerChannel()
    ];
}