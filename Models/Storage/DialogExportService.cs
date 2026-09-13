using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// An implementation of <see cref="IExportService"/> that uses a file save dialog to export textures.
/// </summary>
/// <param name="window">The main application window.</param>
/// <param name="exporterFactory">A collection of available exporters.</param>
public sealed class DialogExportService(Window window, ExporterFactory exporterFactory) : IExportService
{
    /// <summary>
    /// A list of file picker types.
    /// </summary>
    private readonly IReadOnlyList<FilePickerFileType> _filePickerFileTypes =
        exporterFactory.Descriptors.Select(d => d.FileType).ToList();

    /// <inheritdoc/>
    public async Task SaveFileAsync(TextureBuffer textureBuffer)
    {
        var options = new FilePickerSaveOptions
        {
            Title = "Save Rendered Texture",
            FileTypeChoices = _filePickerFileTypes,
        };

        var result = await window.StorageProvider.SaveFilePickerAsync(options);

        if (result is null)
            return;

        string extension = Path.GetExtension(result.Name).TrimStart('.');

        if (string.IsNullOrEmpty(extension))
        {
            await ShowUnsupportedFileTypeDialogAsync(result.Name);
            return;
        }

        IExporter? exporter = exporterFactory.GetMatchingExporterByExtension(extension);

        if (exporter is null)
        {
            await ShowUnsupportedFileTypeDialogAsync(extension);
            return;
        }

        await exporter.ExportAsync(textureBuffer, result);
    }

    /// <summary>
    /// Shows a dialog indicating that the specified file type is unsupported.
    /// </summary>
    /// <param name="extensionOrName">The file extension or name of the unsupported file type.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ShowUnsupportedFileTypeDialogAsync(string extensionOrName)
    {
        var dialog = new Window
        {
            Title = "Unsupported File Type",
            Width = 360,
            Height = 140,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        var okButton = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right };
        okButton.Click += (_, _) => dialog.Close();

        dialog.Content = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 12,
            Children =
            {
                new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Text = $"'{extensionOrName}' isn't a supported export format.",
                },
                okButton,
            },
        };

        await dialog.ShowDialog(window);
    }
}