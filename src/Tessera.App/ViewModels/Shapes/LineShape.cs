using Avalonia;
using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class LineShape : ShapeBase
{
    [ObservableProperty]
    private Point _startPoint;

    [ObservableProperty]
    private Point _endPoint;

    [ObservableProperty]
    private double _thickness;

    public override bool Intersects(Rect rect)
    {
        return rect.Contains(StartPoint) || rect.Contains(EndPoint);
    }
}