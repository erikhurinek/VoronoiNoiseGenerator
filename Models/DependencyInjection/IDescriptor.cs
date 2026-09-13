using System;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A descriptor that provides metadata about a type, such as its name and the type it describes. It is used as a key for lookup in factories.
/// </summary>
public interface IDescriptor
{
    /// <summary>
    /// The name of the descriptor.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type that this descriptor describes.
    /// </summary>
    Type DescribedType { get; }
}