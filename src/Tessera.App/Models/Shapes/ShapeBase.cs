using System.Diagnostics;
using Avalonia.Collections;
using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public abstract partial class ShapeBase : ObservableObject, IShapeProperties
{
    [ObservableProperty]
    private double _x;

    [ObservableProperty]
    private double _y;

    [ObservableProperty]
    private double _width;

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private double _strokeThickness;

    [ObservableProperty]
    private IBrush _color = Brushes.Black;

    [ObservableProperty]
    private IBrush _strokeColor = Brushes.Black;

    [ObservableProperty]
    private bool _isVisible = true;

    [ObservableProperty]
    private double _opacity;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StrokeDashArray))]
    private StrokeType _strokeType;
    
    [ObservableProperty]
    private FillType _fillType;
    
    [ObservableProperty]
    private ShapeType _shapeType;
    
    public AvaloniaList<double> StrokeDashArray => StrokeType switch
    {
        StrokeType.Solid => [],
        StrokeType.Dashed => [1,2],
        StrokeType.Dotted => [4,4],
        _ => []
    };
    
    private double MinWidth => StrokeThickness * 2.15;
    private double MinHeight => StrokeThickness * 2.15;
    
    public abstract bool Intersects(Rect rect);
    public abstract bool HitTest(Point worldPoint, double tolerance);
    public abstract void Move(Vector delta);
    public abstract Rect GetBounds();
    public void Scale(ResizePoint resizePoint, Vector delta)
    {
        var originalBoundingBox = GetBounds();
        Rect newBoundingBox;

        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                var bounds = GetBounds();
                var newHeight = Math.Max(MinHeight, bounds.Height - delta.Y);
                var newWidth = Math.Max(MinWidth, bounds.Width - delta.X);
                var newX = bounds.X + (bounds.Width - newWidth);
                var newY = bounds.Y + (bounds.Height - newHeight);

                newBoundingBox = new Rect(newX, newY, newWidth, newHeight);

                break;
            }
            case ResizePoint.BottomLeft:
            {
                var bounds = GetBounds();
                var newWidth = Math.Max(MinWidth, bounds.Width - delta.X);
                var newX = bounds.X + (bounds.Width - newWidth);
                var newHeight = Math.Max(MinHeight, bounds.Height + delta.Y);

                newBoundingBox = new Rect(newX, originalBoundingBox.Y, newWidth, newHeight);

                break;
            }
            case ResizePoint.TopRight:
            {
                var bounds = GetBounds();
                var newHeight = Math.Max(MinHeight, bounds.Height - delta.Y);
                var newY = bounds.Y + (bounds.Height - newHeight);
                var newWidth = Math.Max(MinWidth, bounds.Width + delta.X);

                newBoundingBox = new Rect(bounds.X, newY, newWidth, newHeight);

                break;
            }
            case ResizePoint.BottomRight:
            {
                var bounds = GetBounds();
                var newWidth = Math.Max(MinWidth, bounds.Width + delta.X);
                var newHeight = Math.Max(MinHeight, bounds.Height + delta.Y);

                newBoundingBox = new Rect(bounds.X, bounds.Y, newWidth, newHeight);

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

                newBoundingBox = new Rect(newX, bounds.Y, newWidth, bounds.Height);

                break;
            }
            case ResizePoint.Right:
            {
                var bounds = GetBounds();
                var newWidth = Math.Max(MinWidth, bounds.Width + delta.X);

                newBoundingBox = new Rect(bounds.X, bounds.Y, newWidth, bounds.Height);

                break;
            }
            case ResizePoint.Top:
            {
                var bounds = GetBounds();
                var newHeight = Math.Max(MinHeight, bounds.Height - delta.Y);
                var newY = bounds.Y + (bounds.Height - newHeight);

                newBoundingBox = new Rect(bounds.X, newY, bounds.Width, newHeight);

                break;
            }
            default:
                newBoundingBox = GetBounds();
                break;
        }
        
        OnBoundsChanged(originalBoundingBox, newBoundingBox);
    }
    
    protected virtual void OnBoundsChanged(Rect oldBounds, Rect newBounds) { }
    
    protected Rect InflateForStroke(Rect bounds)
    {
        return bounds.Inflate(StrokeThickness / 2);
    }
}