using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class LineShape : ShapeBase
{
    private const double MinWidth = 20;

    [ObservableProperty]
    private Point _startPoint;

    [ObservableProperty]
    private Point _endPoint;

    public override bool Intersects(Rect rect)
    {
        if (rect.Contains(StartPoint) || rect.Contains(EndPoint))
            return true;

        var topLeft = rect.TopLeft;
        var topRight = rect.TopRight;
        var bottomLeft = rect.BottomLeft;
        var bottomRight = rect.BottomRight;

        return GeometryHelpers.SegmentsIntersect(StartPoint, EndPoint, topLeft, topRight) ||
               GeometryHelpers.SegmentsIntersect(StartPoint, EndPoint, bottomLeft, bottomRight) ||
               GeometryHelpers.SegmentsIntersect(StartPoint, EndPoint, topLeft, bottomLeft) ||
               GeometryHelpers.SegmentsIntersect(StartPoint, EndPoint, topRight, bottomRight);
    }

    public override bool HitTest(Point worldPoint, double tolerance)
    {
        return GeometryHelpers.DistanceToSegment(StartPoint, EndPoint, worldPoint) <= tolerance;
    }

    public override void Move(Vector delta)
    {
        StartPoint = new Point(StartPoint.X + delta.X, StartPoint.Y + delta.Y);
        EndPoint = new Point(EndPoint.X + delta.X, EndPoint.Y + delta.Y);
    }

    protected override void OnBoundsChanged(Rect oldBounds, Rect newBounds)
    {
        StartPoint = Remap(StartPoint, oldBounds, newBounds);
        EndPoint = Remap(EndPoint, oldBounds, newBounds);
    }

    public override Rect GetBounds()
    {
        var x = Math.Min(StartPoint.X, EndPoint.X);
        var y = Math.Min(StartPoint.Y, EndPoint.Y);
        var width = Math.Abs(StartPoint.X - EndPoint.X);
        var height = Math.Abs(StartPoint.Y - EndPoint.Y);

        return new Rect(x, y, width, height);
    }
}