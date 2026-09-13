using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface defining factories that create instances of <typeparamref name="TCreate"/> when given
/// descriptors of type <typeparamref name="TDescriptor"/>.
/// </summary>
/// <typeparam name="TCreate"></typeparam>
/// <typeparam name="TDescriptor"></typeparam>
public interface IFactory<out TCreate, TDescriptor> where TDescriptor : IDescriptor
{
    /// <summary>
    /// Gets the descriptors of all available instances of <typeparamref name="TCreate"/>.
    /// </summary>
    public IEnumerable<TDescriptor> Descriptors { get; }

    /// <summary>
    /// Creates an instance of <typeparamref name="TCreate"/> using the provided <paramref name="descriptor"/>.
    /// </summary>
    /// <param name="descriptor">The descriptor of the type to create.</param>
    /// <returns>An instance of <typeparamref name="TCreate"/>.</returns>
    TCreate Get(TDescriptor descriptor);
}