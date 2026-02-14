using System;
using Avalonia;
using Avalonia.Media;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class ShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly ShapeToolSettings _settings;
    private Point _startPoint;
    private ShapeBase? _previewShape;

    public ShapeTool(ICanvasContext canvasContext, ShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }
    public void OnPointerPressed(Point p)
    {
        _startPoint = _canvasContext.Transform.ToWorld(p);
        _previewShape = CreateShape(_settings.SelectedShapeType);
        _previewShape.X = p.X;
        _previewShape.Y = p.Y;
        _previewShape.Width = 0;
        _previewShape.Height = 0;
        _previewShape.StrokeColor = new SolidColorBrush(_settings.StrokeColor);
        _previewShape.Color = new SolidColorBrush(_settings.Color);
        _previewShape.StrokeThickness = _settings.Thickness;

        _canvasContext.Shapes.Add(_previewShape);
    }

    public void OnPointerMoved(Point p)
    {
        if (_previewShape == null) return;
        
        var currentPoint = _canvasContext.Transform.ToWorld(p);
        var x = Math.Min(currentPoint.X, _startPoint.X);
        var y = Math.Min(currentPoint.Y, _startPoint.Y);
        var w = Math.Abs(currentPoint.X - _startPoint.X);
        var h = Math.Abs(currentPoint.Y - _startPoint.Y);

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