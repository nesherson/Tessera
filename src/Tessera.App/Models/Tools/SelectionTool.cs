using System.Linq;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class SelectionTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly RectangleShape _marqueeRect;
    
    private enum Mode { Undecided, Marquee, Moving }
    
    private Mode _mode;
    private Point? _pressPoint;
    private Point _lastPoint;
    private bool _pressedOnSelected;

    private const double HitTolerance = 6.0;
    private const double DragThreshold = 4.0;

    public SelectionTool(ICanvasContext canvasContext)
    {
        _canvasContext = canvasContext;
        
        _marqueeRect = new RectangleShape
        {
            StrokeColor = new SolidColorBrush(Colors.Gray, 0.7),
            StrokeThickness = 1,
            IsVisible = false,
            Color = new SolidColorBrush(Colors.Gray, 0.2)
        };
        _canvasContext.MarqueeRect = _marqueeRect;
    }
    
    public void OnPointerPressed(Point screenPoint)
    {
        var world = _canvasContext.Transform.ToWorld(screenPoint);
        _pressPoint = world;
        _lastPoint = world;
        _mode = Mode.Undecided;

        var hitShape = HitTestTopmost(world);

        if (hitShape == null)
        {
            // Pressed on empty space — will become marquee if dragged
            ClearSelection();
            
            _pressedOnSelected = false;
        }
        else if (hitShape.IsSelected)
        {
            // Pressed on already-selected shape — will become move if dragged
            _pressedOnSelected = true;
        }
        else
        {
            // Pressed on unselected shape — select it
            ClearSelection();
            
            hitShape.IsSelected = true;
            _pressedOnSelected = true;
        }
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_pressPoint is null)
            return;
        
        var world = _canvasContext.Transform.ToWorld(screenPoint);

        if (_mode == Mode.Undecided)
        {
            var delta = world - _pressPoint.Value;
            if (Math.Abs(delta.X) < DragThreshold && Math.Abs(delta.Y) < DragThreshold)
                return;

            // Now commit
            _mode = _pressedOnSelected ? Mode.Moving : Mode.Marquee;
        }

        if (_mode == Mode.Marquee)
        {
            UpdateMarquee(_pressPoint.Value, world);
        }
        else if (_mode == Mode.Moving)
        {
            var moveDelta = world - _lastPoint;
            
            foreach (var shape in _canvasContext.Shapes.Where(s => s.IsSelected))
            {
                shape.Move(moveDelta);
            }
        }

        _lastPoint = world;
    }

    public void OnPointerReleased(Point screenPoint)
    {
        if (_pressPoint is null)
            return;
        
        var world = _canvasContext.Transform.ToWorld(screenPoint);

        if (_mode == Mode.Marquee)
        {
            var rect = new Rect(
                new Point(Math.Min(_pressPoint.Value.X, world.X), Math.Min(_pressPoint.Value.Y, world.Y)),
                new Point(Math.Max(_pressPoint.Value.X, world.X), Math.Max(_pressPoint.Value.Y, world.Y)));

            foreach (var shape in _canvasContext.Shapes)
            {
                if (shape.Intersects(rect))
                    shape.IsSelected = true;
            }

            HideMarquee();
        }

        // If mode was Undecided, the press handler already handled click-selection
        // If mode was Moving, shapes are already in their new positions

        _mode = Mode.Undecided;
        _pressPoint = null;
    }
    
    private ShapeBase? HitTestTopmost(Point world)
    {
        for (var i = _canvasContext.Shapes.Count - 1; i >= 0; i--)
        {
            if (_canvasContext.Shapes[i].HitTest(world, HitTolerance))
                return _canvasContext.Shapes[i];
        }
        
        return null;
    }

    private void ClearSelection()
    {
        foreach (var shape in _canvasContext.Shapes)
            shape.IsSelected = false;
    }

    private void UpdateMarquee(Point startPoint, Point currentPoint)
    {
        var x = Math.Min(currentPoint.X, startPoint.X);
        var y = Math.Min(currentPoint.Y, startPoint.Y);
        var w = Math.Abs(currentPoint.X - startPoint.X);
        var h = Math.Abs(currentPoint.Y - startPoint.Y);

        _marqueeRect.IsVisible = true;
        _marqueeRect.X = x;
        _marqueeRect.Y = y;
        _marqueeRect.Width = w;
        _marqueeRect.Height = h;
    }

    private void HideMarquee()
    {
        _marqueeRect.IsVisible = false;
    }
}