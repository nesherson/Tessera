using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class PolylineShape : ShapeBase
{
    private double MinWidth => StrokeThickness * 2.15;
    private double MinHeight => StrokeThickness * 2.15;

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

    private Rect _originalBoundingBox;
    private Point[] _originalPoints;

    public override void OnScaleStart(ResizePoint resizePoint, Vector delta)
    {
        // Keep the original bounding box and points
        // _originalBoundingBox = GetBounds();
        // _originalPoints = Points.ToArray();
    }

    public override void Scale(ResizePoint resizePoint, Vector delta)
    {
        _originalBoundingBox = GetBounds();
        _originalPoints = Points.ToArray();
        // Create new bounding box like in rectangle shape
        var newBoundingBox = new Rect();

        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                var newHeight = Math.Max(MinHeight, Height - delta.Y);
                var newWidth = Math.Max(MinWidth, Width - delta.X);
                var newX = X + (Width - newWidth);
                var newY = Y + (Height - newHeight);

                Width = newWidth;
                Height = newHeight;
                X = newX;
                Y = newY;

                break;
            }
            case ResizePoint.BottomLeft:
            {
                var newWidth = Math.Max(MinWidth, Width - delta.X);
                var newX = X + (Width - newWidth);
                var newHeight = Math.Max(MinHeight, Height + delta.Y);

                newBoundingBox = new Rect(newX, _originalBoundingBox.Y, newWidth, newHeight);

                break;
            }
            case ResizePoint.TopRight:
            {
                var newHeight = Math.Max(MinHeight, Height - delta.Y);
                var newY = Y + (Height - newHeight);

                Width = Math.Max(MinWidth, Width + delta.X);
                Height = newHeight;
                Y = newY;

                break;
            }
            case ResizePoint.BottomRight:
            {
                Width = Math.Max(MinWidth, Width + delta.X);
                Height = Math.Max(MinHeight, Height + delta.Y);

                break;
            }
            case ResizePoint.Bottom:
            {
                var bounds = GetBounds();
                var newHeight = Math.Max(MinHeight, bounds.Height + delta.Y);
                
                newBoundingBox = new Rect(bounds.X, bounds.Y, bounds.Width, newHeight);

                break;
            }
            case ResizePoint.Left:
            {
                var bounds = GetBounds();
                var newWidth = Math.Max(MinWidth, bounds.Width - delta.X);
                var newX = bounds.X + (bounds.Width - newWidth);
                
                newBoundingBox = new Rect(newX, bounds.Y, newWidth,
                    bounds.Height);

                break;
            }
            case ResizePoint.Right:
            {
                var bounds = GetBounds();
                var newWidth = Math.Max(MinWidth, bounds.Width + delta.X);

                newBoundingBox = new Rect(bounds.X, bounds.Y, newWidth,
                    bounds.Height);

                break;
            }
            case ResizePoint.Top:
            {
                var bounds = GetBounds();
                var newHeight = Math.Max(MinHeight, bounds.Height - delta.Y);
                var newY = bounds.Y + (bounds.Height - newHeight);
                
                newBoundingBox = new Rect(bounds.X, newY, bounds.Width,
                    newHeight);

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(resizePoint), resizePoint, null);
        }
        
        // For each point, figure out where it sits proportionally within the original bounding box — e.g.,
        // "this point is 30% from the left and 60% from the top"

        var normalizedPoints = new List<Point>();

        foreach (var originalPoint in _originalPoints)
        {
            var normalizedPoint = new Point(
                (originalPoint.X - _originalBoundingBox.X) / _originalBoundingBox.Width * 100,
                (originalPoint.Y - _originalBoundingBox.Y) / _originalBoundingBox.Height * 100);
            
            normalizedPoints.Add(normalizedPoint);
            
            // Debug.WriteLine($"Normalized x: {normalizedPoint.X}, y: {normalizedPoint.Y}");
        }
        
        var newPoints = new List<Point>();
        // Map that same proportion into the new bounding box
        foreach (var normalizedPoint in normalizedPoints)
        {
            var newPoint = new Point(
                newBoundingBox.X + (normalizedPoint.X / 100) * newBoundingBox.Width,
                newBoundingBox.Y + (normalizedPoint.Y / 100) * newBoundingBox.Height);
            
            newPoints.Add(newPoint);
        }
        
        Points = new ObservableCollection<Point>(newPoints);

    }

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
}