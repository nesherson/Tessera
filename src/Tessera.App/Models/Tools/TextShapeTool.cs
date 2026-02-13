using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;
using Tessera.App.ViewModels;

namespace Tessera.App.Models;

public class TextShapeTool : ICanvasTool
{
    private readonly ICanvasContext _canvasContext;
    private readonly TextShapeToolSettings _settings;

    public TextShapeTool(ICanvasContext canvasContext, TextShapeToolSettings settings)
    {
        _canvasContext = canvasContext;
        _settings = settings;
    }

    public void OnPointerPressed(Point p)
    {
        var currentPoint = _canvasContext.Transform.ToWorld(p);
        var newText = new TextShape
        {
            X = currentPoint.X,
            Y = currentPoint.Y,
            IsEditing = true,
            FontSize = _settings.FontSize,
            Color = _settings.Color,
            FontFamily = new FontFamily(_settings.SelectedFontFamily)
        };
    
        _canvasContext.Shapes.Add(newText);
    }

    public void OnPointerMoved(Point p)
    {
        // Not used
    }

    public void OnPointerReleased(Point p)
    {
        // Not used
    }
}