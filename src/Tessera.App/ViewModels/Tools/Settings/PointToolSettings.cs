using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class PointToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _pointThickness = 3;

    [ObservableProperty]
    private Color _pointColor = Colors.Black;
}