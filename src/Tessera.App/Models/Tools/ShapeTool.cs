using Avalonia.Input;
using Tessera.App.Constants;
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

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        _startPoint = _canvasContext.Transform.ToWorld(screenPoint);
        _previewShape = CreateShape(_settings.ShapeType);
        _previewShape.X = _startPoint.X;
        _previewShape.Y = _startPoint.Y;
        _previewShape.Width = 0;
        _previewShape.Height = 0;
        _previewShape.StrokeType = _settings.StrokeType;
        _previewShape.StrokeThickness = _settings.StrokeThickness;
        _previewShape.Opacity = _settings.Opacity;
        _previewShape.FillType = _settings.FillType; 

        switch (_settings.FillType)
        {
            case FillType.None:
                _previewShape.StrokeColor = _settings.StrokeColor;
                _previewShape.Color = Brushes.Transparent;

                break;
            case FillType.Semi:
                _previewShape.StrokeColor = _settings.StrokeColor;

                if (_settings.StrokeColor is SolidColorBrush scb)
                    _previewShape.Color = new SolidColorBrush(scb.Color, AppConstants.SemiFillOpacity);

                break;
            case FillType.Solid:
                _previewShape.StrokeColor = _settings.StrokeColor;
                _previewShape.Color = _settings.StrokeColor;

                break;
        }

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
    
    public void OnActivated() { }
    public void OnDeactivated() { }

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