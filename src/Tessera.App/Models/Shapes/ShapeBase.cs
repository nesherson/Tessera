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
    private bool _isSelected;
    
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
    
    public abstract bool Intersects(Rect rect);
    public abstract bool HitTest(Point worldPoint, double tolerance);
    public abstract void Move(Vector delta);
    public abstract Rect GetBounds();
    
    protected Rect InflateForStroke(Rect bounds)
    {
        return bounds.Inflate(StrokeThickness / 2);
    }
}