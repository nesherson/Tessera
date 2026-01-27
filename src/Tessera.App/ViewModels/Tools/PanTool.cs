using System.Diagnostics;
using Avalonia;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PanTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    
    private Point? _startPoint;
    private double _startPanX;
    private double _startPanY;
    
    public PanTool(DrawingPageViewModel vm)
    {
        _vm = vm;
    }
    
    public void OnPointerPressed(Point p)
    {
        _startPoint = p;
        _startPanX = _vm.PanX;
        _startPanY = _vm.PanY;
    }

    public void OnPointerMoved(Point p)
    {
        if (_startPoint == null) return;
        
        var delta = p - _startPoint;
        
        
        _vm.PanX = _startPanX + delta?.X ?? 0;
        _vm.PanY = _startPanY + delta?.Y ?? 0;
        Debug.WriteLine($"PanX -> {_vm.PanX}");
        Debug.WriteLine($"PanY -> {_vm.PanY}");

        
    }

    public void OnPointerReleased(Point p)
    {
        _startPoint = null;
    }
}