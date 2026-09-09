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
        var services = new ServiceCollection();

        services.AddTransient<IRenderer, TestRenderer>();
        services.AddTransient<IRenderer, VoronoiDistanceRenderer>();
        // services.AddTransient<IRenderer, VoronoiEdgeRenderer>();
        // services.AddTransient<IRenderer, VoronoiCellRenderer>();

        services.AddSingleton<RendererRegistry>();

        services.AddTransient<MainViewModel>();
        _services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
