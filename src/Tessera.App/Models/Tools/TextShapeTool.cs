using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tessera.App.Interfaces;
using Tessera.App.Messages;

namespace Tessera.App.Models;

public class TextShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly TextShapeToolSettings _settings;
    
    private const double Tolerance = 1;
    
    private Point _startPoint;
    private TextShape? _previewShape;

    public TextShapeTool(ICanvasContext canvasContext, TextShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        _startPoint = _canvasContext.Transform.ToWorld(screenPoint);
        
        _previewShape = new TextShape
        {
            X = _startPoint.X,
            Y = _startPoint.Y,
            IsInitializing = true,
            FontSize = _settings.Size.Thickness * 3,
            Color = _settings.Color,
            Width = 1,
            Height = 1,
            FontFamily = new FontFamily(_settings.FontFamily.Name)
        };
        
        _canvasContext.Shapes.Add(_previewShape);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        if (_previewShape == null) return;
        
        var currentPoint = _canvasContext.Transform.ToWorld(screenPoint);
        var x = Math.Min(currentPoint.X, _startPoint.X);
        var y = Math.Min(currentPoint.Y, _startPoint.Y);
        var w = Math.Abs(currentPoint.X - _startPoint.X);
        var h = Math.Clamp(Math.Abs(currentPoint.Y - _startPoint.Y), 16, 16);

        _previewShape.X = x;
        _previewShape.Y = y;
        _previewShape.Width = w;
        _previewShape.Height = h;
    }

    public void OnPointerReleased(Point screenPoint)
    {
        if (_previewShape == null) return;

        if (Math.Abs(_previewShape.Width - 1) < Tolerance)
        {
            _previewShape.Width = 52;
            _previewShape.Height = 16;
        }
        
        _previewShape.IsInitializing = false;
        _previewShape.IsEditing = true;
        
        _previewShape = null;
    }
    
    public void OnActivated() { }
    public void OnDeactivated() { }
}