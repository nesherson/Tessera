using System;
using System.Linq;
using Avalonia;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class EraserTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    
    private Point _startPoint;

    public EraserTool(DrawingPageViewModel vm)
    {
        _vm = vm;
    }
    
    public void OnPointerPressed(Point p)
    {
        if (!_vm.ViewMatrix.HasInverse) return;
        
        _startPoint = p.Transform(_vm.ViewMatrix.Invert());
        _vm.SelectionX = _startPoint.X;
        _vm.SelectionY = _startPoint.Y;
        _vm.SelectionWidth = 0;
        _vm.SelectionHeight = 0;
        _vm.IsSelectionVisible = true;
    }

    public void OnPointerMoved(Point p)
    {
        if (!_vm.IsSelectionVisible) return;
        if (!_vm.ViewMatrix.HasInverse) return;
        
        var currentPoint = p.Transform(_vm.ViewMatrix.Invert());
        
        var x = Math.Min(currentPoint.X, _startPoint.X);
        var y = Math.Min(currentPoint.Y, _startPoint.Y);
        var w = Math.Abs(currentPoint.X - _startPoint.X);
        var h = Math.Abs(currentPoint.Y - _startPoint.Y);

        _vm.SelectionX = x;
        _vm.SelectionY = y;
        _vm.SelectionWidth = w;
        _vm.SelectionHeight = h;
    }

    public void OnPointerReleased(Point p)
    {
        var rect = new Rect(_vm.SelectionX, _vm.SelectionY, _vm.SelectionWidth, _vm.SelectionHeight);
        _vm.Shapes
            .Where(shape => shape.Intersects(rect))
            .ToList()
            .ForEach(x => _vm.Shapes.Remove(x));
        
        _vm.IsSelectionVisible = false;
    }
}