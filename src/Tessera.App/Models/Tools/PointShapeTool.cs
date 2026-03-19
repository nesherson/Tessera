using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class PointShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly PointShapeToolSettings _settings;

    public PointShapeTool(ICanvasContext canvasContext, PointShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);
        var newPoint = new EllipseShape
        {
            X = currentPoint.X - _settings.StrokeThickness / 2,
            Y = currentPoint.Y - _settings.StrokeThickness / 2,
            Width = _settings.StrokeThickness,
            Height = _settings.StrokeThickness,
            StrokeColor = _settings.StrokeColor,
            Color = _settings.StrokeColor,
            Opacity = _settings.Opacity,
            StrokeThickness = _settings.StrokeThickness
        };

        _canvasContext.Shapes.Add(newPoint);
    }

    // Not used
    public void OnPointerMoved(Point p) { }

    // Not used
    public void OnPointerReleased(Point p) { }
    
    public void OnActivated() { }
    public void OnDeactivated() { }
}