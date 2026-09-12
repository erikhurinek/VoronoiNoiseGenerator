using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// A service for saving bitmaps to files, using Avalonia's storage provider.
/// </summary>
/// <param name="window">The Avalonia window used to access the storage provider.</param>
public sealed class BitmapSaveService(Window window) : IBitmapSaveService
{
    /// <summary>
    /// Gets the default file picker options.
    /// </summary>
    private static FilePickerSaveOptions DefaultSaveOptions => new()
    {
        DefaultExtension = "png",
        FileTypeChoices = [
            new FilePickerFileType("PNG Image")
            {
                Patterns = ["*.png"]
            }
        ],
        SuggestedFileName = "output.png"
    };

    /// <inheritdoc/>
    public async Task SaveFileAsync(Bitmap bitmap, FilePickerSaveOptions options)
    {
        // Open the file picker dialog.
        var file = await window.StorageProvider.SaveFilePickerAsync(options);

        // Return if no file was selected.
        if (file is null)
            return;

        // Save the bitmap.
        await using var stream = await file.OpenWriteAsync();
        bitmap.Save(stream, PngBitmapEncoderOptions.Default);
    }

    /// <inheritdoc/>
    public async Task SaveFileAsync(Bitmap bitmap)
    {
        // Save with the default options.
        await SaveFileAsync(bitmap, DefaultSaveOptions);
    }
}