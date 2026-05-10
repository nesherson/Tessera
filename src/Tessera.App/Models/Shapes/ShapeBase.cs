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
    
    public virtual double MinWidth => StrokeThickness * 2.15;
    public virtual double MinHeight => StrokeThickness * 2.15;
    
    public abstract bool Intersects(Rect rect);
    public abstract bool HitTest(Point worldPoint, double tolerance);
    public abstract void Move(Vector delta);
    public abstract Rect GetBounds();
    
    public void Scale(ResizePoint resizePoint, Vector delta)
    {
        var originalBounds = GetBounds();
        Rect newBounds;

        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                var newHeight = Math.Max(MinHeight, originalBounds.Height - delta.Y);
                var newWidth = Math.Max(MinWidth, originalBounds.Width - delta.X);
                var newX = originalBounds.X + (originalBounds.Width - newWidth);
                var newY = originalBounds.Y + (originalBounds.Height - newHeight);
            
                newBounds = new Rect(newX, newY, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.BottomLeft:
            {
                var newWidth = Math.Max(MinWidth, originalBounds.Width - delta.X);
                var newX = originalBounds.X + (originalBounds.Width - newWidth);
                var newHeight = Math.Max(MinHeight, originalBounds.Height + delta.Y);
            
                newBounds = new Rect(newX, originalBounds.Y, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.TopRight:
            {
                var newHeight = Math.Max(MinHeight, originalBounds.Height - delta.Y);
                var newY = originalBounds.Y + (originalBounds.Height - newHeight);
                var newWidth = Math.Max(MinWidth, originalBounds.Width + delta.X);
            
                newBounds = new Rect(originalBounds.X, newY, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.BottomRight:
            {
                var newWidth = Math.Max(MinWidth, originalBounds.Width + delta.X);
                var newHeight = Math.Max(MinHeight, originalBounds.Height + delta.Y);
            
                newBounds = new Rect(originalBounds.X, originalBounds.Y, newWidth, newHeight);
            
                break;
            }
            case ResizePoint.Bottom:
            {
                var newHeight = Math.Max(MinHeight, originalBounds.Height + delta.Y);
            
                newBounds = new Rect(originalBounds.X, originalBounds.Y, originalBounds.Width, newHeight);
            
                break;
            }
            case ResizePoint.Left:
            {
                var newWidth = Math.Max(MinWidth, originalBounds.Width - delta.X);
                var newX = originalBounds.X + (originalBounds.Width - newWidth);
            
                newBounds = new Rect(newX, originalBounds.Y, newWidth, originalBounds.Height);
            
                break;
            }
            case ResizePoint.Right:
            {
                var newWidth = Math.Max(MinWidth, originalBounds.Width + delta.X);

                newBounds = new Rect(originalBounds.X, originalBounds.Y, newWidth, originalBounds.Height);

                break;
            }
            case ResizePoint.Top:
            {
                var newHeight = Math.Max(MinHeight, originalBounds.Height - delta.Y);
                var newY = originalBounds.Y + (originalBounds.Height - newHeight);
            
                newBounds = new Rect(originalBounds.X, newY, originalBounds.Width, newHeight);
            
                break;
            }
            default:
                newBounds = originalBounds;
                break;
        }
        
        OnBoundsChanged(originalBounds, newBounds);
    }
    
    protected Rect InflateForStroke(Rect bounds)
    {
        return bounds.Inflate(StrokeThickness / 2);
    }
    
    protected virtual void OnBoundsChanged(Rect oldBounds, Rect newBounds) { }
}