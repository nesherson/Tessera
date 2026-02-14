using Avalonia;
using Avalonia.Input;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class PanTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;

    private Point? _startPoint;
    
    public PanTool(ICanvasContext canvasContext)
    {
        _canvasContext = canvasContext;
    }
    
    public void OnPointerPressed(Point screenPoint)
    {
        _startPoint = screenPoint;
        _canvasContext.CurrentCursor = new Cursor(StandardCursorType.SizeAll);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_startPoint == null) return;
        
        var delta = screenPoint - _startPoint.Value;
        
        _canvasContext.Transform.Pan(delta.X, delta.Y);
        _startPoint = screenPoint;
    }

    public void OnPointerReleased(Point screenPoint)
    {
        _startPoint = null;
        _canvasContext.CurrentCursor = Cursor.Default;
    }
}