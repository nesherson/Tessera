using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Helpers;

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

    public override bool HitTest(Point worldPoint, double tolerance)
    {
        for (var i = 0; i < Points.Count - 1; i++)
        {
            if (GeometryHelpers.DistanceToSegment(Points[i], Points[i + 1], worldPoint) <= tolerance)
                return true;
        }
        return false;
    }

    public override void Move(Vector delta)
    {
        Points = new ObservableCollection<Point>(Points
            .Select(point => new Point(point.X + delta.X, point.Y + delta.Y)));
    }
}