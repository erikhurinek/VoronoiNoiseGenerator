using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A base class for factories that create singleton instances of a specific type based on a descriptor.
/// </summary>
/// <typeparam name="TCreate">The type of the instances to create.</typeparam>
/// <typeparam name="TDescriptor">The type of the descriptor used to identify instances.</typeparam>
public abstract class SingletonFactory<TCreate, TDescriptor> : IFactory<TCreate, TDescriptor> where TDescriptor : IDescriptor
{
    /// <inheritdoc/>
    public abstract IEnumerable<TDescriptor> Descriptors { get; }

    /// <summary>
    /// Gets the dictionary of instances managed by this factory.
    /// </summary>
    protected Dictionary<Type, TCreate> InstanceMap { get; } = new();

    /// <summary>
    /// Gets or creates an instance of <typeparamref name="TCreate"/> using the provided <paramref name="descriptor"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when no instance of type <typeparamref name="TCreate"/> is found for the provided descriptor.</exception>
    /// <inheritdoc/>
    public TCreate Get(TDescriptor descriptor)
    {
        // Check if an instance already exists for the given descriptor's described type
        if (InstanceMap.TryGetValue(descriptor.DescribedType, out var instance))
            return instance;

        // If not, check the descriptor is registered.
        var type = descriptor.DescribedType;
        if (!Descriptors.Any(d => d.DescribedType == type))
            throw new ArgumentException($"No instance of type {typeof(TCreate).Name} found for descriptor {descriptor}.", nameof(descriptor));

        // Create a new instance of the described type and store it in the instance map
        instance = (TCreate)Activator.CreateInstance(type)!;
        InstanceMap[type] = instance;
        return instance;
    }

    /// <summary>
    /// Gets all instances of <typeparamref name="TCreate"/> managed by this factory.
    /// Creates instances for any descriptor that does not already have an associated instance.
    /// </summary>
    /// <returns>>An enumerable of all instances of <typeparamref name="TCreate"/>.</returns>
    public IEnumerable<TCreate> GetAll() => Descriptors.Select(Get);
}