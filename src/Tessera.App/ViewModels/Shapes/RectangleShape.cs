using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class RectangleShape : ShapeBase
{
    [ObservableProperty]
    private IBrush _strokeColor = Brushes.Black;
    [ObservableProperty]
    private double _strokeThickness;
    [ObservableProperty]
    private double _width;
    [ObservableProperty]
    private double _height;

    public override bool Intersects(Rect rect)
    {
        return false;
    }
}