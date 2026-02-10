using Avalonia;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class TextShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;

    public TextShapeTool(DrawingPageViewModel drawingPageViewModel)
    {
        _vm = drawingPageViewModel;
    }

    public void OnPointerPressed(Point p)
    {
        var currentPoint = _vm.ToWorld(p);
        var newText = new TextShape
        {
            X = currentPoint.X,
            Y = currentPoint.Y,
            IsEditing = true 
        };
    
        _vm.Shapes.Add(newText);
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