using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class LineToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _strokeThickness = 3;

    [ObservableProperty]
    private Color _strokeColor = Colors.Black;
}