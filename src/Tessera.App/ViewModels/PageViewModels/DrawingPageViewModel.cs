using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    private IBrush _gridColor = Brushes.LightGray;
    
    [ObservableProperty]
    private Matrix _viewMatrix = Matrix.Identity;
    
    [ObservableProperty]
    private Cursor _currentCursor = Cursor.Default;
    
    private ICanvasTool CurrentTool => SelectedToolItem.Tool;

    [ObservableProperty] private Point _currentPoint;
    [ObservableProperty] private double _offsetX = 0;
    [ObservableProperty] private double _offsetY = 0;
    [ObservableProperty] private double _scale = 1;
    
    [ObservableProperty] private string _debugScreenPoint = "0, 0";
    [ObservableProperty] private string _debugWorldPoint = "0, 0";

    public void UpdateDebugInfo(Point screenPoint)
    {
        // Capture exactly what the screen sees
        DebugScreenPoint = $"{screenPoint.X:F0}, {screenPoint.Y:F0}";
    
        // Capture exactly what the math world calculates
        var world = ToWorld(screenPoint);
        DebugWorldPoint = $"{world.X:F1}, {world.Y:F1}";
    }
    
    public DrawingPageViewModel()
    {
        var pointSettings = new PointShapeToolSettings();
        var lineSettings = new LineShapeToolSettings();
        var shapeSettings = new ShapeToolSettings();
        var polylineSettings = new PolylineShapeToolSettings();
        
        PageName = ApplicationPageNames.Drawing;
        Tools = 
        [
            new ToolItem { Name = "Pan", Icon = "/Assets/Icons/hand-grabbing.svg", Tool = new PanTool(this), ToolSettings = new PanToolSettings()},
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
        // var world = !ViewMatrix.HasInverse ? screenPoint : screenPoint.Transform(ViewMatrix.Invert());
        // Debug.WriteLine($"ScreenPoint: {screenPoint},  World: {world}");
        // CurrentPoint = world;
        
        double worldX = (screenPoint.X - OffsetX) / Scale;
        double worldY = (screenPoint.Y - OffsetY) / Scale;
        return new Point(worldX, worldY);
        // return world;
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
    private void ResetView()
    {
        ViewMatrix = Matrix.Identity;
    }
    
    public void Zoom(Point point, double delta)
    {
        // var zoomFactor = delta > 0 ? 1.1 : 0.9;
        // var currentScale = ViewMatrix.M11;
        //
        // if (currentScale * zoomFactor < 0.1 || currentScale * zoomFactor > 10.0) 
        //     return;
        //
        //
        // ViewMatrix = ViewMatrix * Matrix.CreateTranslation(-point.X, -point.Y)
        //                  * Matrix.CreateScale(zoomFactor, zoomFactor)
        //                  * Matrix.CreateTranslation(point.X, point.Y);


        // var zoomMatrix = Matrix.CreateScale(zoomFactor, zoomFactor);

        // ViewMatrix = ViewMatrix *  zoomMatrix;
        
        double oldScale = Scale;
        double zoomFactor = delta > 0 ? 1.1 : 0.9;
        Scale *= zoomFactor;

        // Clamp the scale
        Scale = Math.Clamp(Scale, 0.1, 10.0);

        // Adjust Offset to zoom into the mouse position
        // Formula: Offset = ScreenPoint - (WorldPoint * NewScale)
        var worldPoint = ToWorld(point); 
    
        OffsetX = point.X - (worldPoint.X * Scale);
        OffsetY = point.Y - (worldPoint.Y * Scale);
    }
}