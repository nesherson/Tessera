using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class LineToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _lineThickness = 3;

    [ObservableProperty]
    private Color _lineColor = Colors.Black;
}