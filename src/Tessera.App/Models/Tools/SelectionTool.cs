using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class SelectionTool : ICanvasTool
{
    private readonly ICanvasContext _context;

    private enum Mode { Undecided, Marquee, Moving }
    
    private Mode _mode;
    private Point _pressPoint;
    private Point _lastPoint;
    private bool _pressedOnSelected;

    private const double HitTolerance = 6.0;
    private const double DragThreshold = 4.0;
    
    public void OnPointerPressed(Point p)
    {
        throw new NotImplementedException();
    }

    public void OnPointerMoved(Point p)
    {
        throw new NotImplementedException();
    }

    public void OnPointerReleased(Point p)
    {
        throw new NotImplementedException();
    }
}