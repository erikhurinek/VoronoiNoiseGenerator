using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for exporters that save texture buffers to files.
/// </summary>
public interface IExporter
{
    /// <summary>
    /// Exports the given texture buffer to a file.
    /// </summary>
    /// <param name="texture">The texture buffer to export.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ExportAsync(TextureBuffer texture, IStorageFile file);
}
