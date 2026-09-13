using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

public abstract class SingletonFactory<T, TDescriptor> : IFactory<T, TDescriptor> where T : IFactoryInstantiable<TDescriptor> where TDescriptor : IDescriptor
{
    protected abstract IEnumerable<T> Instances { get; }
    protected Dictionary<Type, T> InstanceMap => Instances.ToDictionary(i => i.Descriptor.DescribedType, i => i);

    public IEnumerable<TDescriptor> Descriptors => Instances.Select(i => i.Descriptor);

    public T Create(TDescriptor descriptor)
    {
        var instance = InstanceMap.GetValueOrDefault(descriptor.DescribedType)
            ?? throw new ArgumentException($"No instance of type {typeof(T).Name} found for descriptor {descriptor.Name}.", nameof(descriptor));

        return instance;
    }
}