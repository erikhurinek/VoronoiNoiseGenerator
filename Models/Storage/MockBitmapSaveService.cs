using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A mock implementation of <see cref="IBitmapSaveService"/> that does nothing when saving files.
/// </summary>
public sealed class MockImageSaveService : IBitmapSaveService
{
    /// <inheritdoc/>
    public Task SaveFileAsync(Bitmap bitmap) => Task.CompletedTask;

    /// <inheritdoc/>
    public Task SaveFileAsync(Bitmap bitmap, FilePickerSaveOptions options) => Task.CompletedTask;
}