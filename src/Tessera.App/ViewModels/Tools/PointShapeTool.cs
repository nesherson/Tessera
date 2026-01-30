using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PointShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    private readonly PointShapeToolSettings _settings;
    
    public PointShapeTool(DrawingPageViewModel  drawingPageViewModel,  PointShapeToolSettings settings)
    {
        _vm = drawingPageViewModel;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        if (!_vm.ViewMatrix.HasInverse) return;
        
        var inverse = _vm.ViewMatrix.Invert();
        var worldPoint = p.Transform(inverse);
        var newPoint = new EllipseShape
        {
            X = worldPoint.X,
            Y = worldPoint.Y,
            Width = _settings.PointThickness,
            Height = _settings.PointThickness,
            Color = new SolidColorBrush(_settings.PointColor)
        };
        
        _vm.Shapes.Add(newPoint);
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