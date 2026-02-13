using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class PolylineShape : ShapeBase
{
    [ObservableProperty]
    private ObservableCollection<Point> _points = [];

    [ObservableProperty]
    private PenLineJoin _strokeJoin;

    [ObservableProperty]
    private PenLineCap _strokeCap;
    
    public override bool Intersects(Rect rect)
    {
        return Points.Any(rect.Contains);
    }
}