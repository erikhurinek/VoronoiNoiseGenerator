using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Associates a renderer type with its implementation.
/// </summary>
public class RendererRegistry
{
    /// <summary>
    /// Maps renderer types to their corresponding renderer instances.
    /// </summary>
    private Dictionary<Type, IRenderer> _renderers;

    /// <summary>
    /// Initializes an empty renderer registry. 
    /// Useful for unit testing.
    /// </summary>
    public RendererRegistry()
    {
        _renderers = new();
    }

    /// <summary>
    /// Initializes a renderer registry with the given renderers.
    /// </summary>
    /// <param name="renderers">The renderers to initialize the registry with.</param>
    public RendererRegistry(IEnumerable<IRenderer> renderers)
    {
        _renderers = renderers.ToDictionary(
            renderer => renderer.GetType(),
            renderer => renderer);
    }

    /// <summary>
    /// Gets the renderer of the specified type from the registry.
    /// </summary>
    /// <typeparam name="TRenderer">The type of the renderer to retrieve.</typeparam>
    /// <returns>The renderer of the specified type.</returns>
    public IRenderer Get<TRenderer>() where TRenderer : class, IRenderer =>
        _renderers[typeof(TRenderer)];

    /// <summary>
    /// Gets the renderer of the specified type from the registry.
    /// </summary>
    /// <param name="rendererType">The type of the renderer to retrieve.</param>
    /// <returns>The renderer of the specified type.</returns>
    public IRenderer Get(Type rendererType) => _renderers[rendererType];
}
