using System.Diagnostics;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;
using Tessera.App.Interfaces;

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

    public override void Scale(ResizePoint resizePoint, Vector delta)
    {
        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                if (StartPoint.X < EndPoint.X)
                {
                    var newX = Math.Min(StartPoint.X + delta.X, EndPoint.X - MinWidth);

                    StartPoint = new Point(newX, StartPoint.Y);
                }
                else
                {
                    var newX = Math.Min(EndPoint.X + delta.X, StartPoint.X - MinWidth);

                    EndPoint = new Point(newX, EndPoint.Y);
                }
                
                if (StartPoint.Y < EndPoint.Y)
                {
                    var newY = Math.Min(StartPoint.Y + delta.Y, EndPoint.Y - MinWidth);
                    
                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Min(EndPoint.Y + delta.Y, StartPoint.Y - MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
            case ResizePoint.BottomLeft:
            {
                if (StartPoint.X < EndPoint.X)
                {
                    var newX = Math.Min(StartPoint.X + delta.X, EndPoint.X - MinWidth);

                    StartPoint = new Point(newX, StartPoint.Y);
                }
                else
                {
                    var newX = Math.Min(EndPoint.X + delta.X, StartPoint.X - MinWidth);

                    EndPoint = new Point(newX, EndPoint.Y);
                }
                
                if (StartPoint.Y > EndPoint.Y)
                {
                    var newY = Math.Max(StartPoint.Y + delta.Y, EndPoint.Y + MinWidth);
                    
                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Max(EndPoint.Y + delta.Y, StartPoint.Y + MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
            case ResizePoint.TopRight:
            {
                if (StartPoint.X > EndPoint.X)
                {
                    var newX = Math.Max(StartPoint.X + delta.X, EndPoint.X + MinWidth);

                    StartPoint = new Point(newX, StartPoint.Y);
                }
                else
                {
                    var newX = Math.Max(EndPoint.X + delta.X, StartPoint.X + MinWidth);

                    EndPoint = new Point(newX, EndPoint.Y);
                }
                
                if (StartPoint.Y < EndPoint.Y)
                {
                    var newY = Math.Min(StartPoint.Y + delta.Y, EndPoint.Y - MinWidth);
                    
                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Min(EndPoint.Y + delta.Y, StartPoint.Y - MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
            case ResizePoint.BottomRight:
            {
                if (StartPoint.X > EndPoint.X)
                {
                    var newX = Math.Max(StartPoint.X + delta.X, EndPoint.X + MinWidth);

                    StartPoint = new Point(newX, StartPoint.Y);
                }
                else
                {
                    var newX = Math.Max(EndPoint.X + delta.X, StartPoint.X + MinWidth);

                    EndPoint = new Point(newX, EndPoint.Y);
                }
                
                if (StartPoint.Y > EndPoint.Y)
                {
                    var newY = Math.Max(StartPoint.Y + delta.Y, EndPoint.Y + MinWidth);
                    
                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Max(EndPoint.Y + delta.Y, StartPoint.Y + MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
            case ResizePoint.Bottom:
            {
                if (StartPoint.Y > EndPoint.Y)
                {
                    var newY = Math.Max(StartPoint.Y + delta.Y, EndPoint.Y + MinWidth);
                    
                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Max(EndPoint.Y + delta.Y, StartPoint.Y + MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
            case ResizePoint.Left:
            {
                var newX = Math.Min(StartPoint.X + delta.X, EndPoint.X - MinWidth);

                StartPoint = new Point(newX, StartPoint.Y);

                break;
            }
            case ResizePoint.Right:
            {
                var newX = Math.Max(EndPoint.X + delta.X, StartPoint.X + MinWidth);

                EndPoint = new Point(newX, EndPoint.Y);

                break;
            }
            case ResizePoint.Top:
            {
                if (StartPoint.Y < EndPoint.Y)
                {
                    var newY = Math.Min(StartPoint.Y + delta.Y, EndPoint.Y - MinWidth);

                    StartPoint = new Point(StartPoint.X, newY);
                }
                else
                {
                    var newY = Math.Min(EndPoint.Y + delta.Y, StartPoint.Y - MinWidth);
                    
                    EndPoint = new Point(EndPoint.X, newY);
                }
                
                break;
            }
        }
    }

    public override Rect GetBounds()
    {
        var x = Math.Min(StartPoint.X, EndPoint.X);
        var y = Math.Min(StartPoint.Y, EndPoint.Y);
        var width = Math.Abs(StartPoint.X - EndPoint.X);
        var height = Math.Abs(StartPoint.Y - EndPoint.Y);

        return InflateForStroke(new Rect(x, y, width, height));
    }
}