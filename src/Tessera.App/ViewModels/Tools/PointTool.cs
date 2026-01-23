using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PointTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    private readonly PointToolSettings _settings;
    
    public PointTool(DrawingPageViewModel  drawingPageViewModel,  PointToolSettings settings)
    {
        _drawingPageViewModel = drawingPageViewModel;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        var x = p.X - _settings.PointThickness / 2;
        var y = p.Y - _settings.PointThickness / 2;
        var newPoint = new EllipseShape
        {
            X = x,
            Y = y,
            Size = _settings.PointThickness,
            Color = new SolidColorBrush(_settings.PointColor)
        };
        
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