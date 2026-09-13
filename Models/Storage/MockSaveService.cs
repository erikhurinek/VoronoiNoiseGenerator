using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A mock implementation of <see cref="IExportService"/> that does nothing when saving files.
/// </summary>
public sealed class MockExportService : IExportService
{
    /// <inheritdoc/>
    public Task SaveFileAsync(TextureBuffer texture) => Task.CompletedTask;
}