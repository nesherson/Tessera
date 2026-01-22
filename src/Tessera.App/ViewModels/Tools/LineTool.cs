using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class LineTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    
    private LineShape? _line;
    
    public LineTool(DrawingPageViewModel  drawingPageViewModel)
    {
        _drawingPageViewModel = drawingPageViewModel;
    }
    
    public void OnPointerPressed(Point p)
    {
        _line = new LineShape
        {
            StartPoint = new Point(p.X, p.Y),
            EndPoint = new Point(p.X, p.Y),
            Color = new SolidColorBrush(_drawingPageViewModel.CurrentColor),
            Thickness = _drawingPageViewModel.CurrentThickness,
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