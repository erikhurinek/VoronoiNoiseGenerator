using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using VoronoiNoiseGenerator.ViewModels;
using VoronoiNoiseGenerator.Views;

using Microsoft.Extensions.DependencyInjection;
using VoronoiNoiseGenerator.Models;

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
        var mainWindow = new MainWindow();
        var services = new ServiceCollection();

        // services.AddTransient<IRenderer, TestRenderer>();
        services.AddTransient<IRenderer, VoronoiDistanceRenderer>();
        services.AddTransient<IRenderer, VoronoiCellRenderer>();
        services.AddTransient<IRenderer, VoronoiEdgeRenderer>();
        services.AddTransient<IRenderer, VoronoiTrueEdgeRenderer>();
        services.AddTransient<IColourMixer, Models.ColourMixerChannel>();

        services.AddSingleton<RendererRegistry>();
        services.AddTransient<MainViewModel>();

        services.AddSingleton<IBitmapSaveService>(new BitmapSaveService(mainWindow));

        _services = services.BuildServiceProvider();

        mainWindow.DataContext = _services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
