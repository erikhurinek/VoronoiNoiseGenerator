using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public interface IFactory<out T, TDescriptor> where T : IFactoryInstantiable<TDescriptor> where TDescriptor : IDescriptor
{
    /// <summary>
    /// Gets the descriptors of all available instances of <typeparamref name="T"/>.
    /// </summary>
    public IEnumerable<TDescriptor> Descriptors { get; }

    /// <summary>
    /// Creates an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="descriptor">The descriptor of the type to create.</param>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    T Create(TDescriptor descriptor);
}