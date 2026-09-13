using System;
using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A factory for creating renderers.
/// </summary>
public sealed class RendererFactory : SingletonFactory<IRenderer, RendererDescriptor>
{
    /// <inheritdoc/>
    public override IEnumerable<RendererDescriptor> Descriptors => [
        SolidColourRenderer.Descriptor,
        VoronoiCellRenderer.Descriptor,
        VoronoiDistanceRenderer.Descriptor,
        VoronoiEdgeRenderer.Descriptor,
        VoronoiPositionRenderer.Descriptor,
    ];
}
