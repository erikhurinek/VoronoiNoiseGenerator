using System;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Describes an exporter implementation.
/// </summary>
/// <param name="Name">The exporter name.</param>
/// <param name="DescribedType">The type of the exporter. See <see cref="IExporter"/>.</param>
/// <param name="FileType">The file type that the exporter supports.</param>
public record struct ExporterDescriptor(
    string Name,
    Type DescribedType,
    FilePickerFileType FileType
) : IDescriptor
{
    /// <inheritdoc/>
    public override string ToString() => Name;
}
