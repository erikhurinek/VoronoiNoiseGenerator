using System;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

public record struct ExporterDescriptor(
    string DisplayName,
    Type ExporterType,
    FilePickerFileType FileType
)
{
    /// <inheritdoc/>
    public override string ToString() => DisplayName;
}