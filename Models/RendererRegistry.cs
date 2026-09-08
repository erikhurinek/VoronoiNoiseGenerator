using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace VoronoiNoiseGenerator.Models;

public class RendererRegistry
{
    private Dictionary<RendererDescriptor, IRenderer> _renderers;
    public RendererRegistry()
    {
        _renderers = new Dictionary<RendererDescriptor, IRenderer>();
    }
    public RendererRegistry(IEnumerable<IRenderer> renderers)
    {
        _renderers = renderers.ToDictionary(r => r.Descriptor, r => r);
    }

    public IRenderer Get(RendererDescriptor descriptor)
    {
        return _renderers[descriptor];
    }
}
