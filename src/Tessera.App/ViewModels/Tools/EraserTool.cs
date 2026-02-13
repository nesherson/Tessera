using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class EraserTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly RectangleShape _eraserRect;
    
    private Point _startPoint;

    public EraserTool(ICanvasContext canvasContext)
    {
        _canvasContext = canvasContext;
        
        _eraserRect = new RectangleShape
        {
            StrokeColor = Brushes.Red,
            StrokeThickness = 1,
            Color = new SolidColorBrush(0x33FF0000),
            IsVisible = false
        };
        _canvasContext.EraserRect = _eraserRect;
    }
    
    public void OnPointerPressed(Point p)
    {
        _startPoint = _canvasContext.ToWorld(p);
        _eraserRect.X = _startPoint.X;
        _eraserRect.Y = _startPoint.Y;
        _eraserRect.Width = 0;
        _eraserRect.Height = 0;
        _eraserRect.IsVisible = true;
    }

    public void OnPointerMoved(Point p)
    {
        if (!_eraserRect.IsVisible)
            return;
        
        var currentPoint = _canvasContext.ToWorld(p);
        var x = Math.Min(currentPoint.X, _startPoint.X);
        var y = Math.Min(currentPoint.Y, _startPoint.Y);
        var w = Math.Abs(currentPoint.X - _startPoint.X);
        var h = Math.Abs(currentPoint.Y - _startPoint.Y);
        
        _eraserRect.X = x;
        _eraserRect.Y = y;
        _eraserRect.Width = w;
        _eraserRect.Height = h;
    }

    public void OnPointerReleased(Point p)
    {
        var rect = new Rect(_eraserRect.X, _eraserRect.Y, _eraserRect.Width, _eraserRect.Height);
        
        _canvasContext.Shapes
            .Where(shape => shape.Intersects(rect))
            .ToList()
            .ForEach(x => _canvasContext.Shapes.Remove(x));

        _eraserRect.IsVisible = false;
    }
}