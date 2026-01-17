using System.Collections.ObjectModel;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel
{
    [ObservableProperty] 
    private ObservableCollection<ShapeBase> _shapes;

    public DrawingPageViewModel()
    {
        Shapes =
        [
            new PointShape(25, 25, 10, "#c3c3c3"),
            new PointShape(50, 50, 15, "#eb4034"),
            new PointShape(125, 125, 20, "#eb4034"),
            new PointShape(175, 175, 40, "#43eb34")
        ];
    }

    public void OnPointerPressed(PointerPoint point)
    {
        var newShape = new PointShape(point.Position.X, point.Position.Y, 6, "#c3c3c3");
        
        Shapes.Add(newShape);
    }
}