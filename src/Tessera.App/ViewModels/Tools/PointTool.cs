using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PointTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    
    public PointTool(DrawingPageViewModel  drawingPageViewModel)
    {
        _drawingPageViewModel = drawingPageViewModel;
    }
    
    public void OnPointerPressed(Point p)
    {
        var x = p.X - _drawingPageViewModel.CurrentThickness / 2;
        var y = p.Y - _drawingPageViewModel.CurrentThickness / 2;
        var newPoint = new PointShape(x,
            y,
            _drawingPageViewModel.CurrentThickness,
            new SolidColorBrush(_drawingPageViewModel.CurrentColor));
        
        _drawingPageViewModel.Shapes.Add(newPoint);
    }

    public void OnPointerMoved(Point p)
    {
       // Not used
    }

    public void OnPointerReleased(Point p)
    {
        // Not used
    }
}