using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class TriangleShape : ShapeBase
{
    [ObservableProperty]
    private ObservableCollection<Point> _points = [];

    public override bool Intersects(Rect rect)
    {
        if (Points.Any(rect.Contains)) return true;
        
        var a = Points[0];
        var b = Points[1];
        var c = Points[2];
        
        if (rect.Contains(a) || rect.Contains(b) || rect.Contains(c))
            return true;
        
        if (GeometryHelpers.PointInTriangle(rect.TopLeft, a, b, c)
            || GeometryHelpers.PointInTriangle(rect.TopRight, a, b, c)
            || GeometryHelpers.PointInTriangle(rect.BottomLeft, a, b, c)
            || GeometryHelpers.PointInTriangle(rect.BottomRight, a, b, c))
            return true;
        
        Point[] rectCorners = [rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft];
        Point[] triEdges = [a, b, c, a];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                if (GeometryHelpers.SegmentsIntersect(
                        triEdges[i], triEdges[i + 1],
                        rectCorners[j], rectCorners[(j + 1) % 4]))
                    return true;
            }
        }

        return false;
    }

    public override bool HitTest(Point worldPoint, double tolerance)
    {
        var firstPoint = Points[0];
        var secondPoint = Points[1];
        var thirdPoint = Points[2];

        return GeometryHelpers.Cross(firstPoint, secondPoint, worldPoint) <= 0
               && GeometryHelpers.Cross(secondPoint, thirdPoint, worldPoint) <= 0
               && GeometryHelpers.Cross(thirdPoint, firstPoint, worldPoint) <= 0;
    }

    public override void Move(Vector delta) =>
        Points = new ObservableCollection<Point>(
            Points.Select(point => new Point(point.X + delta.X, point.Y + delta.Y)));

    public override Rect GetBounds()
    {
        if (Points.Count == 0)
            return default;

        var minX = Points.Min(p => p.X);
        var minY = Points.Min(p => p.Y);
        var maxX = Points.Max(p => p.X);
        var maxY = Points.Max(p => p.Y);

        return InflateForStroke(new Rect(minX, minY, maxX - minX, maxY - minY));
    }

    public override void Scale(ResizePoint resizePoint, Vector delta)
    {
        switch (resizePoint)
        {
            case ResizePoint.Right:
            {
                var mostRightPoint = Points.MaxBy(p => p.X);
                var newRightPoint = new Point(mostRightPoint.X + delta.X, mostRightPoint.Y);

                var newPoints = Points.Where(p => p != mostRightPoint).ToList();
                
                newPoints.Add(newRightPoint);
                
                Points = new ObservableCollection<Point>(newPoints);
               
                
                break;
            }
        }
    }
}