using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An interface for services that save <see cref="TextureBuffer"/> to files.
/// </summary>
public interface ISaveService
{
    /// <summary>
    /// Saves a <see cref="TextureBuffer"/> to a file.
    /// </summary>
    /// <param name="texture">The texture to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveFileAsync(TextureBuffer texture);

    /// <summary>
    /// Saves a <see cref="TextureBuffer"/> to a file, using custom file picker options.
    /// </summary>
    /// <param name="options">The file picker options to use when saving the texture.</param>
    /// <inheritdoc cref="SaveFileAsync(TextureBuffer)"/>
    Task SaveFileAsync(TextureBuffer texture, FilePickerSaveOptions options);
}