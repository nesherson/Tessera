using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class LineShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly LineShapeToolSettings _settings;

    private LineShape? _line;

    public LineShapeTool(ICanvasContext canvasContext, LineShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);

        _line = new LineShape
        {
            StartPoint = new Point(currentPoint.X, currentPoint.Y),
            EndPoint = new Point(currentPoint.X, currentPoint.Y),
            StrokeThickness = _settings.StrokeThickness,
            StrokeColor = _settings.StrokeColor,
            Opacity = _settings.Opacity,
            StrokeType = _settings.StrokeType
        };

        _canvasContext.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_line == null) return;

        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);

        _line.EndPoint = new Point(currentPoint.X, currentPoint.Y);
    }

    public void OnPointerReleased(Point screenPoint)
    {
        _line = null;
    }
    
    public void OnActivated() { }
    public void OnDeactivated() { }
}