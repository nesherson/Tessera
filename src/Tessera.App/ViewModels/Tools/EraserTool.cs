using System;
using System.Collections.Generic;
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
        _startPoint = p;
        
        _vm.SelectionX = p.X;
        _vm.SelectionY = p.Y;
        _vm.SelectionWidth = 0;
        _vm.SelectionHeight = 0;
        _vm.IsSelectionVisible = true;
    }

    public void OnPointerMoved(Point p)
    {
        if (!_vm.IsSelectionVisible) return;
        
        var x = Math.Min(p.X, _startPoint.X);
        var y = Math.Min(p.Y, _startPoint.Y);
        var w = Math.Abs(p.X - _startPoint.X);
        var h = Math.Abs(p.Y - _startPoint.Y);

        _vm.SelectionX = x;
        _vm.SelectionY = y;
        _vm.SelectionWidth = w;
        _vm.SelectionHeight = h;
    }

    public void OnPointerReleased(Point p)
    {
        var rect = new Rect(_vm.SelectionX, _vm.SelectionY, _vm.SelectionWidth, _vm.SelectionHeight);
        var shapesToRemove = _vm.Shapes
            .Where(shape => shape.Intersects(rect))
            .ToList();
        
        foreach (var item in shapesToRemove)
        {
            _vm.Shapes.Remove(item);
        }
        
        _vm.IsSelectionVisible = false;
    }
}