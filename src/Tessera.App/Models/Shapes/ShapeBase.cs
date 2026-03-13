using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public abstract partial class ShapeBase : ObservableObject
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
    private AvaloniaList<double> _strokeDashArray;

    [ObservableProperty]
    private bool _isSelected;

    public abstract bool Intersects(Rect rect);
    public abstract bool HitTest(Point worldPoint, double tolerance);
    public abstract void Move(Vector delta);
    public abstract Rect GetBounds();
    
    protected Rect InflateForStroke(Rect bounds)
    {
        return bounds.Inflate(StrokeThickness / 2);
    }
}