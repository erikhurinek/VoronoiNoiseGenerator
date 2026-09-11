using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VoronoiNoiseGenerator.Models;

namespace VoronoiNoiseGenerator.ViewModels;

/// <summary>
/// The main view model for the application.
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// The list of available renderer descriptors.
    /// </summary>
    private IEnumerable<RendererDescriptor> _descriptors;

    /// <summary>
    /// The registry for managing renderer instances.
    /// </summary>
    private IEnumerable<ColourMixerDescriptor> _colourMixerDescriptors;

    /// <summary>
    /// Registry managing the available renderers.
    /// </summary>
    private RendererRegistry _rendererRegistry;

    /// <summary>
    /// The service responsible for saving images to disk.
    /// </summary>
    private IBitmapSaveService _imageSaveService;

    /// <summary>
    /// Collection of render passes. <br/>
    /// Each pass describes a modification to the generated texture.
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<PassSettings> Passes { get; set; } = new ObservableCollection<PassSettings>();

    /// <summary>
    /// The image that is currently displayed in the UI. <br/>
    /// </summary>
    [ObservableProperty]
    public partial Bitmap? DisplayImage { get; private set; } = null;

    /// <summary>
    /// The width of the texture to be rendered. <br/>
    /// </summary>
    [ObservableProperty]
    public partial int ImageWidth { get; set; } = 128;

    /// <summary>
    /// The height of the texture to be rendered. <br/>
    /// </summary>
    [ObservableProperty]
    public partial int ImageHeight { get; set; } = 128;

    /// <summary>
    /// The application information string, for debugging purposes.
    /// </summary>
    public string AppInfo
        => $"Avalonia version {typeof(AvaloniaObject).Assembly.GetName().Version?.ToString() ?? "Unknown Version"}";

    /// <summary>
    /// Parameterless constructor for preview.
    /// </summary>
    public MainViewModel()
    {
        _descriptors = new List<RendererDescriptor>();
        _colourMixerDescriptors = new List<ColourMixerDescriptor>();
        _imageSaveService = new MockImageSaveService();
        _rendererRegistry = new RendererRegistry();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class with the specified dependencies.
    /// </summary>
    /// <param name="rendererFactory">The factory for creating renderer instances.</param>
    /// <param name="renderers">The available renderers.</param>
    /// <param name="colourMixers">The available colour mixers.</param>
    /// <param name="fileSaveService">The service for saving images to disk.</param>
    public MainViewModel(
        RendererRegistry rendererFactory,
        IEnumerable<IRenderer> renderers,
        IEnumerable<IColourMixer> colourMixers,
        IBitmapSaveService fileSaveService
    )
    {
        _descriptors = renderers.Select(r => r.Descriptor);
        _colourMixerDescriptors = colourMixers.Select(c => c.Descriptor);
        _imageSaveService = fileSaveService;
        _rendererRegistry = rendererFactory;
    }

    /// <summary>
    /// Renders the texture based on the current pass settings and updates the <see cref="DisplayImage"/> property.
    /// </summary>
    [RelayCommand]
    private void Render()
    {
        // Create a new bitmap.
        var bitmap = new WriteableBitmap(
            new PixelSize(ImageWidth, ImageHeight),
            new Vector(96, 96),
            PixelFormat.Rgba8888,
            AlphaFormat.Opaque
        );

        // Iteratively apply the passes.
        foreach (PassSettings pass in Passes)
        {
            // Skip disabled passes.
            if (!pass.IsEnabled)
                continue;

            // Get the renderer descriptor.
            var descriptor = pass.SelectedRenderer;

            // Skip the pass if no renderer is selected.
            if (descriptor is null)
                continue;

            // Get the required renderer instance.
            IRenderer renderer = _rendererRegistry.Get(descriptor.RendererType);

            // Modify the bitmap.
            renderer.Render(bitmap, pass);
        }

        // Display the bitmap.
        DisplayImage = bitmap;
    }

    /// <summary>
    /// Adds a new pass to the list of passes. <br/>
    /// </summary>
    [RelayCommand]
    private void AddPass()
    {
        Passes.Add(new PassSettings(_descriptors, _colourMixerDescriptors));
    }

    /// <summary>
    /// Open a file dialog to save the currently displayed image. <br/>
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [RelayCommand]
    private async Task SaveImage()
    {
        if (DisplayImage is null)
            return;

        await _imageSaveService.SaveFileAsync(DisplayImage);
    }
}