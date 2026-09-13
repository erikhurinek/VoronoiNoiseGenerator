using System;
using System.Collections.Generic;
using System.Linq;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Factory for creating instances of <see cref="IExporter"/>.
/// </summary>
public sealed class ExporterFactory : SingletonFactory<IExporter, ExporterDescriptor>
{
    /// <inheritdoc/>
    public override IEnumerable<ExporterDescriptor> Descriptors => [
        ExrExporter.Descriptor,
        PngExporter.Descriptor,
    ];

    /// <summary>
    /// Gets the first exporter that matches the given file extension.
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    public IExporter? GetMatchingExporterByExtension(string extension) => Descriptors
        .Where(descriptor => descriptor.FileType.Patterns?.Any(fileType => MatchesExtension(fileType ?? string.Empty, extension)) ?? false)
        .Select(Get)
        .FirstOrDefault();

    /// <summary>
    /// Determines whether the given file extension matches the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match against.</param>
    /// <param name="extension">The file extension to check.</param>
    /// <returns>True if the extension matches the pattern, false otherwise.</returns>
    private static bool MatchesExtension(string pattern, string extension)
    {
        string patternExtension = pattern.StartsWith("*.", StringComparison.Ordinal)
            ? pattern[2..]
            : pattern;

        return string.Equals(patternExtension, extension, StringComparison.OrdinalIgnoreCase);
    }
}