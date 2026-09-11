using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for services that save bitmaps to files.
/// </summary>
public interface IBitmapSaveService
{
    /// <summary>
    /// Saves a bitmap to a file, using default options.
    /// </summary>
    /// <param name="bitmap">The bitmap to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveFileAsync(Bitmap bitmap);

    /// <summary>
    /// Saves a bitmap to a file, using the provided file picker options.<br/>
    /// </summary>
    /// <param name="options">The file picker options to use when saving the bitmap.</param>
    /// <inheritdoc cref="SaveFileAsync(Bitmap)"/>
    Task SaveFileAsync(Bitmap bitmap, FilePickerSaveOptions options);
}