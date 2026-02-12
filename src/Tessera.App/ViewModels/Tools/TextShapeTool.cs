using Avalonia;
using Avalonia.Media;
using Tessera.App.Interfaces;

namespace Tessera.App.ViewModels;

public class TextShapeTool : ICanvasTool
{
    private readonly DrawingPageViewModel _vm;
    private readonly TextShapeToolSettings _settings;

    public TextShapeTool(DrawingPageViewModel drawingPageViewModel, TextShapeToolSettings settings)
    {
        _vm = drawingPageViewModel;
        _settings = settings;
    }

    public void OnPointerPressed(Point p)
    {
        var currentPoint = _vm.ToWorld(p);
        var newText = new TextShape
        {
            X = currentPoint.X,
            Y = currentPoint.Y,
            IsEditing = true,
            FontSize = _settings.FontSize,
            Color = _settings.Color,
            FontFamily = new FontFamily(_settings.SelectedFontFamily)
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