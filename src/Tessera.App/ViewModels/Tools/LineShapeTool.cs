using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class LineShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    private readonly LineShapeToolSettings _settings;
    
    private LineShape? _line;
    
    public LineShapeTool(DrawingPageViewModel  drawingPageViewModel, LineShapeToolSettings settings)
    {
        _vm = drawingPageViewModel;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        if (!_vm.ViewMatrix.HasInverse)
            return;
        
        var currentPoint = _vm.ToWorld(p);
        
        _line = new LineShape
        {
            StartPoint = new Point(currentPoint.X, currentPoint.Y),
            EndPoint = new Point(currentPoint.X, currentPoint.Y),
            StrokeThickness = _settings.StrokeThickness,
            Color = new SolidColorBrush(_settings.StrokeColor),
        };

        _vm.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;
        
        if (!_vm.ViewMatrix.HasInverse)
            return;
        
        var currentPoint = _vm.ToWorld(p);
        
        _line.EndPoint = new Point(currentPoint.X, currentPoint.Y);
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
}