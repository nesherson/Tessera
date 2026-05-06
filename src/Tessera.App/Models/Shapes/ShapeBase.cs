using Avalonia.Collections;
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
    
    public Rect Scale(Rect currentBounds, ResizePoint resizePoint, Vector delta)
    {
        Rect newBounds;

        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                var newHeight = Math.Max(MinHeight, currentBounds.Height - delta.Y);
                var newWidth = Math.Max(MinWidth, currentBounds.Width - delta.X);
                var newX = currentBounds.X + (currentBounds.Width - newWidth);
                var newY = currentBounds.Y + (currentBounds.Height - newHeight);
            
                newBounds = new Rect(newX, newY, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.BottomLeft:
            {
                var newWidth = Math.Max(MinWidth, currentBounds.Width - delta.X);
                var newX = currentBounds.X + (currentBounds.Width - newWidth);
                var newHeight = Math.Max(MinHeight, currentBounds.Height + delta.Y);
            
                newBounds = new Rect(newX, currentBounds.Y, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.TopRight:
            {
                var newHeight = Math.Max(MinHeight, currentBounds.Height - delta.Y);
                var newY = currentBounds.Y + (currentBounds.Height - newHeight);
                var newWidth = Math.Max(MinWidth, currentBounds.Width + delta.X);
            
                newBounds = new Rect(currentBounds.X, newY, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.BottomRight:
            {
                var newWidth = Math.Max(MinWidth, currentBounds.Width + delta.X);
                var newHeight = Math.Max(MinHeight, currentBounds.Height + delta.Y);
            
                newBounds = new Rect(currentBounds.X, currentBounds.Y, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.Bottom:
            {
                var newHeight = Math.Max(MinHeight, currentBounds.Height + delta.Y);
            
                newBounds = new Rect(currentBounds.X, currentBounds.Y, currentBounds.Width, newHeight);
            
                break;
            }
            case ResizePoint.Left:
            {
                var newWidth = Math.Max(MinWidth, currentBounds.Width - delta.X);
                var newX = currentBounds.X + (currentBounds.Width - newWidth);
            
                newBounds = new Rect(newX, currentBounds.Y, newWidth, currentBounds.Height);
            
                break;
            }
            case ResizePoint.Right:
            {
                // var testBounds = GetBounds();
                var newWidth = Math.Max(MinWidth, currentBounds.Width + delta.X);

                newBounds = new Rect(currentBounds.X, currentBounds.Y, newWidth, currentBounds.Height);

                break;
            }
            case ResizePoint.Top:
            {
                var newHeight = Math.Max(MinHeight, currentBounds.Height - delta.Y);
                var newY = currentBounds.Y + (currentBounds.Height - newHeight);
            
                newBounds = new Rect(currentBounds.X, newY, currentBounds.Width, newHeight);
            
                break;
            }
            default:
                newBounds = currentBounds;
                break;
        }
        
        OnBoundsChanged(currentBounds, newBounds);

        return newBounds;
    }
    
    protected Rect InflateForStroke(Rect bounds)
    {
        return bounds.Inflate(StrokeThickness / 2);
    }
    
    protected virtual void OnBoundsChanged(Rect oldBounds, Rect newBounds) { }
}