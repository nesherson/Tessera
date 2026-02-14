using Avalonia;
using Avalonia.Media;
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
    
    public void OnPointerPressed(Point p)
    {
        var currentPoint = _canvasContext.Transform.ToWorld(p);
        
        _line = new LineShape
        {
            StartPoint = new Point(currentPoint.X, currentPoint.Y),
            EndPoint = new Point(currentPoint.X, currentPoint.Y),
            StrokeThickness = _settings.StrokeThickness,
            Color = new SolidColorBrush(_settings.StrokeColor),
        };

        _canvasContext.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;
        
        var currentPoint = _canvasContext.Transform.ToWorld(p);
        
        _line.EndPoint = new Point(currentPoint.X, currentPoint.Y);
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
}