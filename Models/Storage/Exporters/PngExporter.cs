using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

public class PngExporter : IExporter
{
    /// <inheritdoc/>
    public ExporterDescriptor Descriptor => new("PNG Image", GetType(), FilePickerFileTypes.ImagePng);

    /// <inheritdoc/>
    public async Task ExportAsync(TextureBuffer texture, IStorageFile file)
    {
        using var stream = await file.OpenWriteAsync();
        var bitmap = texture.CreateBitmap(opaque: false);
        bitmap.Save(stream, PngBitmapEncoderOptions.Default);
    }
}