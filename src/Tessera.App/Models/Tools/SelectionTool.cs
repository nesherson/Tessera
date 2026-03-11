using System.Linq;
using Avalonia.Input;
using Tessera.App.Interfaces;
using Tessera.App.Managers;

namespace Tessera.App.Models;

public class SelectionTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly RectangleShape _marqueeRect;
    private readonly SelectionManager _selectionManager;
    
    private enum Mode { Undecided, Marquee, Moving }
    
    private Mode _mode;
    private Point? _pressPoint;
    private Point _lastPoint;
    private bool _pressedOnSelected;

    private const double HitTolerance = 6.0;
    private const double DragThreshold = 4.0;

    public SelectionTool(ICanvasContext canvasContext, SelectionManager selectionManager)
    {
        _canvasContext = canvasContext;
        _selectionManager = selectionManager;

        _marqueeRect = new RectangleShape
        {
            StrokeColor = new SolidColorBrush(Colors.Gray, 0.7),
            StrokeThickness = 1,
            IsVisible = false,
            Color = new SolidColorBrush(Colors.Gray, 0.2)
        };
        _canvasContext.MarqueeRect = _marqueeRect;
    }
    
    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        var worldPoint = _canvasContext.Transform.ToWorld(screenPoint);
        
        _pressPoint = worldPoint;
        _lastPoint = worldPoint;
        _mode = Mode.Undecided;
        _pressedOnSelected = false;
        
        var hitShape = HitTestTopmost(worldPoint);
        
        if (hitShape != null && _selectionManager.IsSelected(hitShape))
        {
            if (keyModifiers.HasFlag(KeyModifiers.Control))
            {
                _selectionManager.Toggle(hitShape);
            }
            else
            {
                _pressedOnSelected = true;
            }
            
            return;
        }
        
        if (hitShape != null)
        {
            if (!keyModifiers.HasFlag(KeyModifiers.Control))
                _selectionManager.Clear();

            _selectionManager.Toggle(hitShape);
            _pressedOnSelected = true;
            
            return;
        }
        
        if (_selectionManager.HasSelection && 
            _selectionManager.SelectionBounds.Contains(worldPoint))
        {
            _pressedOnSelected = true;
            
            return;
        }
        
        _selectionManager.Clear();
        
        // if (_selectionManager.SelectionBounds.Contains(world))
        // {
        //     if (keyModifiers.HasFlag(KeyModifiers.Control))
        //     {
        //         _selectionManager.Clear();
        //     }
        //     else
        //     {
        //         _pressedOnSelected = true;
        //     }
        //
        //     return;
        // }
        //
        // if (hitShape == null)
        // {
        //     _selectionManager.Clear();
        // }
        // else
        // {
        //     if (keyModifiers.HasFlag(KeyModifiers.Control))
        //     {
        //         _selectionManager.Toggle(hitShape);
        //     }
        //     else
        //     {
        //         _selectionManager.Clear();
        //         _selectionManager.Select(hitShape);
        //     }
        // }
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
            
            _mode = _pressedOnSelected ? Mode.Moving : Mode.Marquee;
        }

        if (_mode == Mode.Marquee)
        {
            UpdateMarquee(_pressPoint.Value, world);
        }
        else if (_mode == Mode.Moving)
        {
            var moveDelta = world - _lastPoint;
            
            foreach (var shape in _canvasContext.Shapes.Where(s => _selectionManager.IsSelected(s)))
            {
                shape.Move(moveDelta);
            }

            _selectionManager.MoveSelection(moveDelta);
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
            
            _selectionManager.SelectRange(_canvasContext.Shapes.Where(x => x.Intersects(rect)));

            HideMarquee();
        }
        
        _mode = Mode.Undecided;
        _pressedOnSelected = false;
        _pressPoint = null;
    }
    
    private ShapeBase? HitTestTopmost(Point world)
    {
        for (var i = _canvasContext.Shapes.Count - 1; i >= 0; i--)
            if (_canvasContext.Shapes[i].HitTest(world, HitTolerance))
                return _canvasContext.Shapes[i];
        
        return null;
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