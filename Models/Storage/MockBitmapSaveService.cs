using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A mock implementation of <see cref="ISaveService"/> that does nothing when saving files.
/// </summary>
public sealed class MockImageSaveService : ISaveService
{
    /// <inheritdoc/>
    public Task SaveFileAsync(TextureBuffer texture) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task SaveFileAsync(TextureBuffer texture, FilePickerSaveOptions options) => Task.CompletedTask;
}