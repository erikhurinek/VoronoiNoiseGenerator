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
        // Create the file picker options with the available file types.
        var options = new FilePickerSaveOptions
        {
            Title = "Save Rendered Texture",
            FileTypeChoices = _filePickerFileTypes,
        };

        // Show the save file picker dialog and get the result.
        var result = await window.StorageProvider.SaveFilePickerAsync(options);

        // If the operation was cancelled, return.
        if (result is null)
            return;

        // Get the file extension from the selected file name.
        string extension = Path.GetExtension(result.Name).TrimStart('.');

        // If the extension is empty, show an error dialog and return.
        if (string.IsNullOrEmpty(extension))
        {
            await ShowErrorDialogAsync(result.Name);
            return;
        }

        // Try to safely save the file.
        try
        {
            // Get the matching exporter for the file extension.
            IExporter? exporter = exporterFactory.GetMatchingExporterByExtension(extension);

            // If no exporter is found, show an error dialog and return.
            if (exporter is null)
            {
                await ShowErrorDialogAsync($"No exporter found for the file type '{extension}'.");
                return;
            }

            // Use the exporter to export the texture buffer to the selected file.
            await exporter.ExportAsync(textureBuffer, result);
        }
        catch (Exception ex)
        {
            // Display the error dialog if an exception occurs during the export process.
            await ShowErrorDialogAsync($"An error occurred while trying to find an exporter for the file type '{extension}': {ex.Message}");
        }
    }

    /// <summary>
    /// Shows a dialog indicating that the specified file type is unsupported.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ShowErrorDialogAsync(string message)
    {
        // Create a new window to display the error message.
        var dialog = new Window
        {
            Title = "Unsupported File Type",
            Width = 360,
            Height = 140,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        // Create an OK button to close the dialog.
        var okButton = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right };
        okButton.Click += (_, _) => dialog.Close();

        // Set the dialog's content.
        dialog.Content = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 12,
            Children =
            {
                new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Text = message,
                },
                okButton,
            },
        };

        await dialog.ShowDialog(window);
    }
}