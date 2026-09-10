using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VoronoiNoiseGenerator.Models;

namespace VoronoiNoiseGenerator.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private IEnumerable<IRenderer> _renderers;
    private IEnumerable<RendererDescriptor> _descriptors;
    private IEnumerable<ColourMixerDescriptor> _colourMixerDescriptors;
    private RendererRegistry _rendererRegistry;

    [ObservableProperty]
    public partial ObservableCollection<PassSettings> Passes { get; set; } = new ObservableCollection<PassSettings>();

    [ObservableProperty]
    public partial PassSettings? SelectedRenderPass { get; set; } = null;

    [ObservableProperty]
    public partial Bitmap? DisplayImage { get; private set; } = null;

    [ObservableProperty]
    public partial int ImageWidth { get; set; } = 128;

    [ObservableProperty]
    public partial int ImageHeight { get; set; } = 128;

    public MainViewModel()
    {
        _renderers = new List<IRenderer>();
        _descriptors = new List<RendererDescriptor>();
        _colourMixerDescriptors = new List<ColourMixerDescriptor>();
        _rendererRegistry = new RendererRegistry();
    }

    public MainViewModel(RendererRegistry rendererFactory, IEnumerable<IRenderer> renderers, IEnumerable<IColourMixer> colourMixers)
    {
        _renderers = renderers;
        _descriptors = _renderers.Select(r => r.Descriptor);
        _colourMixerDescriptors = colourMixers.Select(c => c.Descriptor);
        _rendererRegistry = rendererFactory;
    }

    [RelayCommand]
    public void Render()
    {
        var bitmap = new WriteableBitmap(
            new PixelSize(ImageWidth, ImageHeight),
            new Vector(96, 96),
            PixelFormat.Rgba8888,
            AlphaFormat.Opaque
        );

        foreach (PassSettings pass in Passes)
        {
            if (pass is null || !pass.IsEnabled)
                continue;

            var descriptor = pass.SelectedRenderer;

            if (descriptor is null)
                continue;

            IRenderer renderer = _rendererRegistry.Get(descriptor.RendererType);

            renderer.Render(bitmap, pass);
        }

        DisplayImage = bitmap;
    }

    [RelayCommand]
    public void AddPass()
    {
        Passes.Add(new PassSettings(_descriptors, _colourMixerDescriptors));
    }
}