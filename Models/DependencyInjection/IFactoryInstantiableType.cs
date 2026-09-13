namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A type that can be instantiated by a factory.
/// </summary>
public interface IFactoryInstantiable<TDescriptor> where TDescriptor : IDescriptor
{
    TDescriptor Descriptor { get; }
}