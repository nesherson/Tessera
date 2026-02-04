using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tessera.App.Data;
using Tessera.App.Enumerations;
using Tessera.App.Interfaces;
using Tessera.App.Messages;
using Point = Avalonia.Point;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel
{
    [ObservableProperty] 
    private ObservableCollection<ShapeBase> _shapes = [];
    
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
    
    [ObservableProperty] 
    private double _gridSpacing = 15;
    
    [ObservableProperty] 
    private GridType _gridType = GridType.Dots;
    
    [ObservableProperty]
    private Matrix _viewMatrix = Matrix.Identity;
    
    [ObservableProperty]
    private Cursor _currentCursor = Cursor.Default;
    
    private ICanvasTool CurrentTool => SelectedToolItem.Tool;
    
    public DrawingPageViewModel()
    {
        var pointSettings = new PointShapeToolSettings();
        var lineSettings = new LineShapeToolSettings();
        var shapeSettings = new ShapeToolSettings();
        var polylineSettings = new PolylineShapeToolSettings();
        
        PageName = ApplicationPageNames.Drawing;
        Tools = 
        [
            new ToolItem { Name = "Pan", Icon = "/Assets/Icons/hand-grabbing.svg", Tool = new PanTool(this)},
            new ToolItem { Name = "Point", Icon = "/Assets/Icons/point.svg", Tool = new PointShapeTool(this, pointSettings), ToolSettings = pointSettings},
            new ToolItem { Name = "Line", Icon = "/Assets/Icons/line.svg", Tool = new LineShapeTool(this, lineSettings), ToolSettings = lineSettings},
            new ToolItem { Name = "Free drawing", Icon = "/Assets/Icons/pen.svg", Tool = new PolylineShapeTool(this, polylineSettings), ToolSettings = polylineSettings},
            new ToolItem { Name = "Shape", Icon = "/Assets/Icons/shapes.svg", Tool = new ShapeTool(this, shapeSettings), ToolSettings = shapeSettings},
            new ToolItem { Name = "Eraser", Icon = "/Assets/Icons/eraser.svg", Tool = new EraserTool(this), ToolSettings = new EraserToolSettings()},
        ];
        SelectedToolItem = Tools[0];
    }
    
    public ObservableCollection<ToolItem> Tools { get; }
    
    public void OnPointerPressed(Point point)
    {
        CurrentTool.OnPointerPressed(point);
    }
    
    public void OnPointerMoved(Point point)
    {
        CurrentTool.OnPointerMoved(point);
    }
    
    public void OnPointerReleased(Point point)
    {
        CurrentTool.OnPointerReleased(point);
    }
    
    public Point ToWorld(Point screenPoint)
    {
        return !ViewMatrix.HasInverse ? screenPoint : screenPoint.Transform(ViewMatrix.Invert());
    }
    
    [RelayCommand]
    private void ClearAll()
    {
        Shapes.Clear();
    }
    
    [RelayCommand]
    private async Task OpenOptions()
    {
        var result = await WeakReferenceMessenger.Default.Send(new ShowCanvasSettingsDialogMessage(GridSpacing, GridType)).Tcs.Task;

        if (result is not null)
        {
            GridSpacing = result.GridSpacing;
            GridType = result.GridType;
        }
    }
    
    [RelayCommand]
    private void ResetView()
    {
        ViewMatrix = Matrix.Identity;
    }
}