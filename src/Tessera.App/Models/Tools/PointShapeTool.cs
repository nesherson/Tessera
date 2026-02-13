using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PointShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly PointShapeToolSettings _settings;
    
    public PointShapeTool(ICanvasContext  canvasContext,  PointShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        var currentPoint = _canvasContext.ToWorld(p);
        var newPoint = new EllipseShape
        {
            X = currentPoint.X,
            Y = currentPoint.Y,
            Width = _settings.PointThickness,
            Height = _settings.PointThickness,
            Color = new SolidColorBrush(_settings.PointColor)
        };
        
        _canvasContext.Shapes.Add(newPoint);
    }

    // Not used
    public void OnPointerMoved(Point p) {}

    // Not used
    public void OnPointerReleased(Point p) {}
}