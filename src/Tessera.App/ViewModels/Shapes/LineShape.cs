using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class LineShape : ShapeBase
{
    [ObservableProperty]
    private Point _startPoint;

    [ObservableProperty]
    private Point _endPoint;

    [ObservableProperty]
    private double _strokeThickness;

    public override bool Intersects(Rect rect)
    {
        return rect.Contains(StartPoint) || rect.Contains(EndPoint);
    }
}