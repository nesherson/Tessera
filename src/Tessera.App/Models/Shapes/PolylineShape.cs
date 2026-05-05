using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class PolylineShape : ShapeBase
{
    [ObservableProperty]
    private ObservableCollection<Point> _points = [];

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
        Points = new ObservableCollection<Point>(
            Points.Select(point => new Point(point.X + delta.X, point.Y + delta.Y)));
    }
    
    public override Rect GetBounds()
    {
        if (Points.Count == 0)
            return default;

        var minX = Points.Min(p => p.X);
        var minY = Points.Min(p => p.Y);
        var maxX = Points.Max(p => p.X);
        var maxY = Points.Max(p => p.Y);

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    protected override void OnBoundsChanged(Rect oldBounds, Rect newBounds)
    {
        var originalPoints = Points.ToArray();
        var normalizedPoints = new List<Point>();
        var newPoints = new List<Point>();

        foreach (var originalPoint in originalPoints)
        {
            var normalizedPoint = new Point(
                (originalPoint.X - oldBounds.X) / oldBounds.Width * 100,
                (originalPoint.Y - oldBounds.Y) / oldBounds.Height * 100);
            
            normalizedPoints.Add(normalizedPoint);
        }

        foreach (var normalizedPoint in normalizedPoints)
        {
            var newPoint = new Point(
                newBounds.X + (normalizedPoint.X / 100) * newBounds.Width,
                newBounds.Y + (normalizedPoint.Y / 100) * newBounds.Height);
            
            newPoints.Add(newPoint);
        }
        
        Points = new ObservableCollection<Point>(newPoints);
    }
}