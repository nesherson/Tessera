using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PointShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _drawingPageViewModel;
    private readonly PointShapeToolSettings _settings;
    
    public PointShapeTool(DrawingPageViewModel  drawingPageViewModel,  PointShapeToolSettings settings)
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
            Width = _settings.PointThickness,
            Height = _settings.PointThickness,
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