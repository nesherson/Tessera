using System.Collections.Generic;
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

        if (rect.Contains(a) || rect.Contains(b) || rect.Contains(c)) return true;

        if (GeometryHelpers.PointInTriangle(rect.TopLeft, a, b, c) ||
            GeometryHelpers.PointInTriangle(rect.TopRight, a, b, c) ||
            GeometryHelpers.PointInTriangle(rect.BottomLeft, a, b, c) ||
            GeometryHelpers.PointInTriangle(rect.BottomRight, a, b, c))
            return true;

        Point[] rectCorners = [rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft];
        Point[] triEdges = [a, b, c, a];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                if (GeometryHelpers.SegmentsIntersect(triEdges[i], triEdges[i + 1], rectCorners[j],
                        rectCorners[(j + 1) % 4]))
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

        return GeometryHelpers.Cross(firstPoint, secondPoint, worldPoint) <= 0 &&
               GeometryHelpers.Cross(secondPoint, thirdPoint, worldPoint) <= 0 &&
               GeometryHelpers.Cross(thirdPoint, firstPoint, worldPoint) <= 0;
    }

    public override void Move(Vector delta) =>
        Points = new ObservableCollection<Point>(
            Points.Select(point => new Point(point.X + delta.X, point.Y + delta.Y)));

    public override Rect GetBounds()
    {
        if (Points.Count == 0) return default;

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
            case ResizePoint.TopLeft:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);
                
                var newLeftPoint = new Point(
                    Math.Min(leftPoint.X + delta.X, rightPoint.X - 20),
                    leftPoint.Y);
                var newMidPoint = new Point((newLeftPoint.X + rightPoint.X) / 2, Math.Min(midPoint.Y + delta.Y, leftPoint.Y - 20));
                
                Points = new ObservableCollection<Point>([newLeftPoint, newMidPoint, rightPoint]);
                
                break;
            }
            case ResizePoint.TopRight:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);
                
                var newRightPoint = new Point(
                    Math.Max(rightPoint.X + delta.X, leftPoint.X + 20),
                    rightPoint.Y);
                var newMidPoint = new Point((leftPoint.X + newRightPoint.X) / 2, Math.Min(midPoint.Y + delta.Y, leftPoint.Y - 20));
                
                Points = new ObservableCollection<Point>([leftPoint, newMidPoint, newRightPoint]);
                
                break;
            }
            case ResizePoint.BottomLeft:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);
                
                var newLeftPoint = new Point(
                    Math.Min(leftPoint.X + delta.X, rightPoint.X - 20),
                    Math.Max(leftPoint.Y + delta.Y, midPoint.Y + 20));
                var newRightPoint = new Point(
                    rightPoint.X,
                    Math.Max(rightPoint.Y + delta.Y, midPoint.Y + 20));
                var newMidPoint = new Point((newLeftPoint.X + newRightPoint.X) / 2, midPoint.Y);
                
                Points = new ObservableCollection<Point>([newLeftPoint, newMidPoint, newRightPoint]);
                break;
            }
            case ResizePoint.BottomRight:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);

                var newRightPoint = new Point(
                    Math.Max(rightPoint.X + delta.X, leftPoint.X + 20),
                    Math.Max(rightPoint.Y + delta.Y, midPoint.Y + 20));
                var newLeftPoint = new Point(
                    leftPoint.X,
                    Math.Max(rightPoint.Y + delta.Y, midPoint.Y + 20));
                var newMidPoint = new Point((newLeftPoint.X + newRightPoint.X) / 2, midPoint.Y);
                
                Points = new ObservableCollection<Point>([newLeftPoint, newMidPoint, newRightPoint]);
                
                break;
            }
            case ResizePoint.Left:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);

                var newLeftPoint = new Point(Math.Min(leftPoint.X + delta.X, rightPoint.X - 20), leftPoint.Y);
                var newMidPoint = new Point((newLeftPoint.X + rightPoint.X) / 2, midPoint.Y);

                Points = new ObservableCollection<Point>([newLeftPoint, newMidPoint, rightPoint]);

                break;
            }
            case ResizePoint.Top:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);

                var newMidPoint = new Point(midPoint.X, Math.Min(midPoint.Y + delta.Y, leftPoint.Y - 20));

                Points = new ObservableCollection<Point>([leftPoint, newMidPoint, rightPoint]);

                break;
            }
            case ResizePoint.Right:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);

                var newRightPoint = new Point(Math.Max(rightPoint.X + delta.X, leftPoint.X + 20), rightPoint.Y);
                var newMidPoint = new Point((leftPoint.X + newRightPoint.X) / 2, midPoint.Y);

                Points = new ObservableCollection<Point>([leftPoint, newMidPoint, newRightPoint]);

                break;
            }
            case ResizePoint.Bottom:
            {
                var rightPoint = Points.MaxBy(p => p.X);
                var leftPoint = Points.MinBy(p => p.X);
                var midPoint = Points.First(p => p.X > leftPoint.X && p.X < rightPoint.X);

                var newLeftPoint = new Point(leftPoint.X, Math.Max(leftPoint.Y + delta.Y, midPoint.Y + 20));
                var newRightPoint = new Point(rightPoint.X, Math.Max(rightPoint.Y + delta.Y, midPoint.Y + 20));

                Points = new ObservableCollection<Point>([newLeftPoint, midPoint, newRightPoint]);

                break;
            }
        }
    }
}