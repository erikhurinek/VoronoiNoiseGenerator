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
    protected override IEnumerable<IExporter> Instances =>
    [
        new PngExporter()
    ];

    /// <summary>
    /// Gets the first exporter that matches the given file extension.
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    public IExporter? GetMatchingExporterByExtension(string extension) =>
        Instances.FirstOrDefault(e =>
            e.Descriptor.FileType.Patterns?.Any(p => MatchesExtension(p, extension)) ?? false);

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