using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class LineTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    private readonly LineToolSettings _settings;
    
    private LineShape? _line;
    
    public LineTool(DrawingPageViewModel  drawingPageViewModel, LineToolSettings settings)
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
            StrokeThickness = _settings.LineThickness,
            Color = new SolidColorBrush(_settings.LineColor),
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