using Avalonia.Input;
using ExCSS;
using Tessera.App.Interfaces;
using Point = Avalonia.Point;

namespace Tessera.App.Models;

public class TextShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly TextShapeToolSettings _settings;
    
    private const double Tolerance = 1;
    
    private Point _startPoint;
    private TextShape? _shape;

    public TextShapeTool(ICanvasContext canvasContext, TextShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        _startPoint = _canvasContext.Transform.ToWorld(screenPoint);
        
        _shape = new TextShape
        {
            X = _startPoint.X,
            Y = _startPoint.Y,
            IsInitializing = true,
            FontSize = _settings.StrokeThickness * 2,
            StrokeColor = _settings.StrokeColor,
            Width = 1,
            Height = 1,
            Opacity = 1
        };
        
        _canvasContext.Shapes.Add(_shape);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_shape == null) return;
        
        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);
        var x = Math.Min(currentPoint.X, _startPoint.X);
        var y = Math.Min(currentPoint.Y, _startPoint.Y);
        var w = Math.Abs(currentPoint.X - _startPoint.X);
        var h = Math.Clamp(Math.Abs(currentPoint.Y - _startPoint.Y), _shape.MinHeight, _shape.MinHeight);

        _shape.X = x;
        _shape.Y = y;
        _shape.Width = w;
        _shape.Height = h;
    }

    public void OnPointerReleased(Point screenPoint)
    {
        if (_shape == null) return;

        if (Math.Abs(_shape.Width - 1) < Tolerance)
        {
            _shape.Width = 52;
            _shape.Height = 16;
        }
        
        _shape.IsInitializing = false;
        _shape.IsEditing = true;
        
        _shape = null;
    }
    
    public void OnActivated() { }
    public void OnDeactivated() { }
}