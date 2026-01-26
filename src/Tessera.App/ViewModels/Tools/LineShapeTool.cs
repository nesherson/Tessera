using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class LineShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    private readonly LineShapeToolSettings _settings;
    
    private LineShape? _line;
    
    public LineShapeTool(DrawingPageViewModel  drawingPageViewModel, LineShapeToolSettings settings)
    {
        _drawingPageViewModel = drawingPageViewModel;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        _line = new LineShape
        {
            StartPoint = new Point(p.X, p.Y),
            EndPoint = new Point(p.X, p.Y),
            StrokeThickness = _settings.StrokeThickness,
            Color = new SolidColorBrush(_settings.StrokeColor),
        };
        
        _drawingPageViewModel.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;
        
        _line.EndPoint = new Point(p.X, p.Y);
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
}