using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace VoronoiNoiseGenerator.Models;

public class RendererRegistry
{
    private Dictionary<Type, IRenderer> _renderers;
    public RendererRegistry()
    {
        _renderers = new();
    }
    public RendererRegistry(IEnumerable<IRenderer> renderers)
    {
        _renderers = renderers.ToDictionary(
            renderer => renderer.GetType(),
            renderer => renderer);
    }

    public IRenderer Get<TRenderer>(RendererDescriptor descriptor) where TRenderer : class, IRenderer
    {
        return _renderers[typeof(TRenderer)];
    }

    public IRenderer Get(Type rendererType)
    {
        return _renderers[rendererType];
    }
}
