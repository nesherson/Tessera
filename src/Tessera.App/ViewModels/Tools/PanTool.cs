using Avalonia;
using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class PanTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    
    private bool _isDragging;
    private Point? _startPoint;
    private Matrix _originalMatrix;
    
    public PanTool(DrawingPageViewModel vm)
    {
        _vm = vm;
    }
    
    public void OnPointerPressed(Point p)
    {
        _startPoint = p;
        _originalMatrix = _vm.ViewMatrix;
        _isDragging = false;
        _vm.CurrentCursor = new Cursor(StandardCursorType.SizeAll);
    }

    public void OnPointerMoved(Point p)
    {
        if (_startPoint == null) return;
        
        var delta = p - _startPoint;
        
        if (!_isDragging && (System.Math.Abs(delta.Value.X) > 3 || System.Math.Abs(delta.Value.Y) > 3))
        {
            _isDragging = true;
        }

        if (!_isDragging) return;
        
        var translation = Matrix.CreateTranslation(delta.Value.X, delta.Value.Y);
        
        _vm.ViewMatrix = translation * _originalMatrix;
    }

    public void OnPointerReleased(Point p)
    {
        _startPoint = null;
        _isDragging = false;
        _vm.CurrentCursor = Cursor.Default;
    }
}