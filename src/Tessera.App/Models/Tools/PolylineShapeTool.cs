using System.Collections.ObjectModel;
using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class PolylineShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly PolylineShapeToolSettings _settings;

    private PolylineShape? _line;

    public PolylineShapeTool(ICanvasContext canvasContext, PolylineShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);

        _line = new PolylineShape
        {
            StrokeThickness = _settings.StrokeThickness,
            StrokeColor = _settings.StrokeColor,
            StrokeType = _settings.StrokeType,
            Opacity = _settings.Opacity,
        };

        _line.Points.Add(currentPoint);
        _canvasContext.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;

        var currentPoint = _canvasContext.Transform.ToWorld(p);
        var newPoints = new ObservableCollection<Point>(_line.Points) { currentPoint };

        _line.Points = newPoints;
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
    
    public void OnActivated() { }
    public void OnDeactivated() { }
}