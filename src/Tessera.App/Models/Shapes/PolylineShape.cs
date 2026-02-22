using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class PolylineShape : ShapeBase
{
    [ObservableProperty]
    private ObservableCollection<Point> _points = [];

    [ObservableProperty]
    private AvaloniaList<double> _strokeDashArray;
    
    public override bool Intersects(Rect rect)
    {
        return Points.Any(rect.Contains);
    }
}