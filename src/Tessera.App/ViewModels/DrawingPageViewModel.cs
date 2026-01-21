using System.Collections.ObjectModel;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Data;
using Tessera.App.Interfaces;
using Tessera.App.Models;
using Tessera.App.ViewModels.Tools;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel
{
    [ObservableProperty] 
    private ObservableCollection<ShapeBase> _shapes;
    
    [ObservableProperty] 
    private ToolItem _selectedToolItem;

    [ObservableProperty] 
    private Color _currentColor;
    
    [ObservableProperty]
    private double _currentThickness = 2.0;
    
    public ICanvasTool CurrentTool => SelectedToolItem.Tool;
    
    public DrawingPageViewModel()
    {
        PageName = ApplicationPageNames.Drawing;
        Shapes = [];
        Tools = 
        [
            new ToolItem { Name = "Point", Icon = "/Assets/Icons/point.svg", Tool = new PointTool(this)},
            new ToolItem { Name = "Line", Icon = "/Assets/Icons/line.svg", Tool = new PointTool(this)},
        ];
        SelectedToolItem = Tools[0];
        CurrentColor = Colors.Black;
    }
    
    public ObservableCollection<ToolItem> Tools { get; }

    public void OnPointerPressed(PointerPoint pointerPoint)
    {
        CurrentTool.OnPointerPressed(pointerPoint.Position);
    }
    
    public void OnPointerMoved(PointerPoint pointerPoint)
    {
        CurrentTool.OnPointerMoved(pointerPoint.Position);
    }
    
    public void OnPointerReleased(PointerPoint pointerPoint)
    {
        CurrentTool.OnPointerReleased(pointerPoint.Position);
    }
}