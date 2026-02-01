using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PolylineShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    private readonly PolylineShapeToolSettings _settings;
    
    private PolylineShape? _line;

    public PolylineShapeTool(DrawingPageViewModel vm, PolylineShapeToolSettings settings)
    {
        _vm = vm;
        _settings = settings;
    }
    
    public void OnPointerPressed(Point p)
    {
        if (!_vm.ViewMatrix.HasInverse) return;
        
        var currentPoint = _vm.ToWorld(p);
        
        _line = new PolylineShape
        {
            StrokeThickness = _settings.StrokeThickness,
            StrokeColor = new SolidColorBrush(_settings.StrokeColor),
            StrokeJoin = _settings.SelectedStrokeJoin,
            StrokeCap = _settings.SelectedStrokeCap
        };

        _line.Points.Add(currentPoint);
        _vm.Shapes.Add(_line);
    }

    public void OnPointerMoved(Point p)
    {
        if (_line == null) return;
        if (!_vm.ViewMatrix.HasInverse) return;

        var currentPoint = _vm.ToWorld(p);
        var newPoints = new ObservableCollection<Point>(_line.Points) { currentPoint };

        _line.Points = newPoints;
    }

    public void OnPointerReleased(Point p)
    {
        _line = null;
    }
}