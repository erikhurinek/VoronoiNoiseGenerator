using Avalonia.Controls;

namespace VoronoiNoiseGenerator.Views;

/// <summary>
/// A <see cref="UserControl"/> for editing the <see cref="Models.PassSettings"/> model.
/// </summary>
public partial class PassSettings : UserControl
{
    public PassSettings()
    {
        InitializeComponent();

        PointerEntered += (_, _) =>
        {
            if (DataContext is Models.PassSettings pass)
                pass.IsHovered = true;
        };

        PointerExited += (_, _) =>
        {
            if (DataContext is Models.PassSettings pass)
                pass.IsHovered = false;
        };
    }
}