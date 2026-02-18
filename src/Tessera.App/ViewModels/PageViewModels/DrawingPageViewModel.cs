using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tessera.App.Data;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;
using Tessera.App.Messages;
using Tessera.App.Models;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel, ICanvasContext
{
    [ObservableProperty] 
    private ObservableCollection<ShapeBase> _shapes = [];
    
    [ObservableProperty] 
    private ToolItem _selectedToolItem;
    
    [ObservableProperty]
    private RectangleShape _eraserRect;
    
    [ObservableProperty] 
    private double _gridSpacing = 15;
    
    [ObservableProperty] 
    private GridType _gridType = GridType.Dots;
    
    [ObservableProperty]
    private IBrush _gridColor = Brushes.LightGray;
    
    [ObservableProperty]
    private Cursor _currentCursor = Cursor.Default;
    
    [ObservableProperty]
    private bool _isToolSettingsOpen;
    
    private ICanvasTool CurrentTool => SelectedToolItem.Tool;
    
    public DrawingPageViewModel()
    {
        var pointShapeSettings = new PointShapeToolSettings();
        var lineShapeSettings = new LineShapeToolSettings();
        var shapeSettings = new ShapeToolSettings();
        var polylineShapeSettings = new PolylineShapeToolSettings();
        var textShapeSettings = new TextShapeToolSettings();
        
        PageName = ApplicationPageNames.Drawing;
        Tools =
        [
            new ToolItem
            {
                Name = "Pan",
                Icon = "/Assets/Icons/hand-grabbing.svg",
                Tool = new PanTool(this),
                ToolSettings = new PanToolSettings()
            },
            new ToolItem
            {
                Name = "Point",
                Icon = "/Assets/Icons/point.svg",
                Tool = new PointShapeTool(this, pointShapeSettings),
                ToolSettings = pointShapeSettings
            },
            new ToolItem
            {
                Name = "Line",
                Icon = "/Assets/Icons/line.svg",
                Tool = new LineShapeTool(this, lineShapeSettings),
                ToolSettings = lineShapeSettings
            },
            new ToolItem
            {
                Name = "Free drawing",
                Icon = "/Assets/Icons/pen.svg",
                Tool = new PolylineShapeTool(this, polylineShapeSettings),
                ToolSettings = polylineShapeSettings
            },
            new ToolItem
            {
                Name = "Shape",
                Icon = "/Assets/Icons/shapes.svg",
                Tool = new ShapeTool(this, shapeSettings),
                ToolSettings = shapeSettings
            },
            new ToolItem
            {
                Name = "Text",
                Icon = "/Assets/Icons/text-t.svg",
                Tool = new TextShapeTool(this, textShapeSettings),
                ToolSettings = textShapeSettings
            },
            new ToolItem
            {
                Name = "Eraser",
                Icon = "/Assets/Icons/eraser.svg",
                Tool = new EraserTool(this),
                ToolSettings = new EraserToolSettings()
            },
        ];
        Transform = new CanvasTransform();
        
        ResetToolSelection();
    }
    
    public CanvasTransform Transform { get; }
    public ObservableCollection<ToolItem> Tools { get; }
    
    public void OnPointerPressed(Point screenPoint)
    {
        CurrentTool.OnPointerPressed(screenPoint);
    }
    
    public void OnPointerMoved(Point screenPoint)
    {
        CurrentTool.OnPointerMoved(screenPoint);
    }
    
    public void OnPointerReleased(Point screenPoint)
    {
        CurrentTool.OnPointerReleased(screenPoint);
    }
    
    public void ResetToolSelection()
    {
        SelectedToolItem = Tools[0];
    }
    
    [RelayCommand]
    private void ClearAll()
    {
        Shapes.Clear();
    }
    
    [RelayCommand]
    private async Task OpenOptions()
    {
        var result = await WeakReferenceMessenger
            .Default
            .Send(new ShowCanvasSettingsDialogMessage(GridSpacing, GridType, GridColor)).Tcs.Task;

        if (result is not null)
        {
            GridSpacing = result.GridSpacing;
            GridType = result.GridType;
            GridColor = result.GridColor;
        }
    }

    [RelayCommand]
    private void ResetView() => Transform.Reset();
}