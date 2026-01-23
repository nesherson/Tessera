using System;
using Avalonia;
using Avalonia.Media;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class ShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    private readonly ShapeToolSettings _settings;
    private Point _startPoint;
    private ShapeBase? _previewShape;

    public ShapeTool(DrawingPageViewModel vm, ShapeToolSettings settings)
    {
        _vm = vm;
        _settings = settings;
    }
    public void OnPointerPressed(Point p)
    {
        _startPoint  = p;
        _previewShape = CreateShape(_settings.SelectedShapeType);
        _previewShape.X = p.X;
        _previewShape.Y = p.Y;
        _previewShape.Width = 0;
        _previewShape.Height = 0;
        _previewShape.StrokeColor = new SolidColorBrush(_settings.Color);
        _previewShape.StrokeThickness = _settings.Thickness;

        _vm.Shapes.Add(_previewShape);
    }

    public void OnPointerMoved(Point p)
    {
        if (_previewShape == null) return;
        
        var x = Math.Min(p.X, _startPoint.X);
        var y = Math.Min(p.Y, _startPoint.Y);
        var w = Math.Abs(p.X - _startPoint.X);
        var h = Math.Abs(p.Y - _startPoint.Y);

        _previewShape.X = x;
        _previewShape.Y = y;
        _previewShape.Width = w;
        _previewShape.Height = h;
    }

    public void OnPointerReleased(Point p)
    {
        _previewShape = null;
    }
    
    private ShapeBase CreateShape(ShapeType type)
    {
        return type switch
        {
            ShapeType.Rectangle => new RectangleShape(),
            ShapeType.Ellipse => new EllipseShape(),
            _ => new RectangleShape()
        };
    }
}