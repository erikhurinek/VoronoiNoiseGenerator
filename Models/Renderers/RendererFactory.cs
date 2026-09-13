using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Associates a renderer type with its implementation.
/// </summary>
public sealed class RendererFactory : SingletonFactory<IRenderer, RendererDescriptor>
{
    protected override IEnumerable<IRenderer> Instances => [
        new SolidColourRenderer(),
        new VoronoiCellRenderer(),
        new VoronoiDistanceRenderer(),
        new VoronoiEdgeRenderer(),
        new VoronoiPositionRenderer()
    ];
}
