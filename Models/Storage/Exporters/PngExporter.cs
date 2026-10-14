using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An exporter for PNG images.
/// </summary>
public class PngExporter : IExporter
{
    /// <summary>
    /// Gets the descriptor for this exporter.
    /// </summary>
    public static ExporterDescriptor Descriptor => new("PNG Image", typeof(PngExporter), FilePickerFileTypes.ImagePng);

    /// <inheritdoc/>
    public async Task ExportAsync(TextureBuffer texture, IStorageFile file)
    {
        using var stream = await file.OpenWriteAsync();
        var bitmap = texture.CreateBitmap(opaque: false);
        bitmap.Save(stream, PngBitmapEncoderOptions.Default);
    }
}
