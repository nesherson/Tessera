using Avalonia;
using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PanTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    
    private bool _isDragging;
    private Point? _startPoint;
    
    public PanTool(DrawingPageViewModel vm)
    {
        _vm = vm;
    }
    
    public void OnPointerPressed(Point screenPoint)
    {
        _startPoint = screenPoint;
        _isDragging = false;
        _vm.CurrentCursor = new Cursor(StandardCursorType.SizeAll);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_startPoint == null) return;
        
        var delta = screenPoint - _startPoint;
        
        if (!_isDragging && (System.Math.Abs(delta.Value.X) > 3 || System.Math.Abs(delta.Value.Y) > 3))
        {
            _isDragging = true;
        }

        if (!_isDragging) return;
        
        _vm.OffsetX += delta.Value.X;
        _vm.OffsetY += delta.Value.Y;
        _startPoint = screenPoint;
    }

    public void OnPointerReleased(Point screenPoint)
    {
        _startPoint = null;
        _isDragging = false;
        _vm.CurrentCursor = Cursor.Default;
    }
}