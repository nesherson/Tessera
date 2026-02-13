using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PolylineShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvassContext;
    private readonly PolylineShapeToolSettings _settings;
    
    private PolylineShape? _line;

    public PolylineShapeTool(ICanvasContext canvasContext, PolylineShapeToolSettings settings)
    {
        _canvassContext = canvasContext;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        var currentPoint = _canvassContext.ToWorld(p);
        
        _line = new PolylineShape
        {
            StrokeThickness = _settings.StrokeThickness,
            StrokeColor = new SolidColorBrush(_settings.StrokeColor),
            StrokeJoin = _settings.SelectedStrokeJoin,
            StrokeCap = _settings.SelectedStrokeCap
        };

        _line.Points.Add(currentPoint);
        _canvassContext.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;

        var currentPoint = _canvassContext.ToWorld(p);
        var newPoints = new ObservableCollection<Point>(_line.Points) { currentPoint };

        _line.Points = newPoints;
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
}