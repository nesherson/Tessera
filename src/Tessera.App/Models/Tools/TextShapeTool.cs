using Tessera.App.Interfaces;

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
            FontSize = _settings.Size.Thickness * 2,
            Color = _settings.Color,
            FontFamily = new FontFamily(_settings.FontFamily.Name)
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