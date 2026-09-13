using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using VoronoiNoiseGenerator.ViewModels;
using VoronoiNoiseGenerator.Views;

using Microsoft.Extensions.DependencyInjection;
using VoronoiNoiseGenerator.Models;
using Avalonia.Styling;

namespace VoronoiNoiseGenerator;

public partial class App : Application
{
    private ServiceProvider _services = null!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Create the main window and service collection.
        var mainWindow = new MainWindow();
        var services = new ServiceCollection();

        // Register renderers.
        services.AddSingleton<IRenderer, SolidColourRenderer>();
        services.AddSingleton<IRenderer, VoronoiDistanceRenderer>();
        services.AddSingleton<IRenderer, VoronoiCellRenderer>();
        services.AddSingleton<IRenderer, VoronoiEdgeRenderer>();
        services.AddSingleton<IRenderer, VoronoiPositionRenderer>();

        // Register colour mixers.
        services.AddSingleton<IColourMixer, Models.ColourMixerChannel>();

        // Register the renderer registry and view model.
        services.AddSingleton<RendererRegistry>();
        services.AddSingleton<MainViewModel>();
        // services.AddSingleton<ISaveService>(new TextureBufferSaveService(mainWindow));

        // Build the service provider.
        _services = services.BuildServiceProvider();

        // Set the main window's data context.
        mainWindow.DataContext = _services.GetRequiredService<MainViewModel>();

        // Set the main window as the application's main window.
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
