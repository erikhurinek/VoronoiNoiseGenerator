using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for services that save <see cref="TextureBuffer"/> to files.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Saves a <see cref="TextureBuffer"/> to a file.
    /// </summary>
    /// <param name="exporter">The exporter to use for saving the texture.</param>
    /// <param name="texture">The texture to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveFileAsync(TextureBuffer texture);
}