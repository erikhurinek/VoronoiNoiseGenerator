using System;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

public record struct ExporterDescriptor(
    string Name,
    Type DescribedType,
    FilePickerFileType FileType
) : IDescriptor
{
    /// <inheritdoc/>
    public override string ToString() => Name;
}