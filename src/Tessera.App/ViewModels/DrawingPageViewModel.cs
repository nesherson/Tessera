using System.Collections.ObjectModel;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Data;
using Tessera.App.Interfaces;
using Tessera.App.Models;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel
{
    [ObservableProperty] 
    private ObservableCollection<ShapeBase> _shapes;
    [ObservableProperty] 
    private ToolItem _selectedToolItem;
    [ObservableProperty] 
    private bool _isSelectionVisible;
    [ObservableProperty] 
    private double _selectionX;
    [ObservableProperty] 
    private double _selectionY;
    [ObservableProperty] 
    private double _selectionWidth;
    [ObservableProperty] 
    private double _selectionHeight;
    
    public ICanvasTool CurrentTool => SelectedToolItem.Tool;
    
    public DrawingPageViewModel()
    {
        var pointSettings = new PointToolSettings();
        var lineSettings = new LineToolSettings();
        var shapeSettings = new ShapeToolSettings();
        var polylineSettings = new PolylineToolSettings();
        
        PageName = ApplicationPageNames.Drawing;
        Shapes = [];
        Tools = 
        [
            new ToolItem { Name = "Point", Icon = "/Assets/Icons/point.svg", Tool = new PointTool(this, pointSettings), ToolSettings = pointSettings},
            new ToolItem { Name = "Line", Icon = "/Assets/Icons/line.svg", Tool = new LineTool(this, lineSettings), ToolSettings = lineSettings},
            new ToolItem { Name = "Free drawing", Icon = "/Assets/Icons/pen.svg", Tool = new PolylineShapeTool(this, polylineSettings), ToolSettings = polylineSettings},
            new ToolItem { Name = "Shape", Icon = "/Assets/Icons/shapes.svg", Tool = new ShapeTool(this, shapeSettings), ToolSettings = shapeSettings},
            new ToolItem { Name = "Eraser", Icon = "/Assets/Icons/eraser.svg", Tool = new EraserTool(this)},
        ];
        SelectedToolItem = Tools[0];
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

    [RelayCommand]
    private void ClearAll()
    {
        Shapes.Clear();
    }
}