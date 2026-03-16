using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tessera.App.Data;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;
using Tessera.App.Interfaces;
using Tessera.App.Managers;
using Tessera.App.Messages;
using Tessera.App.Models;

namespace Tessera.App.ViewModels;

public partial class DrawingPageViewModel : PageViewModel, ICanvasContext
{
    [ObservableProperty]
    private double _viewportWidth;

    [ObservableProperty]
    private double _viewportHeight;

    [ObservableProperty]
    private ObservableCollection<ShapeBase> _shapes = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSelectionToolSelected))]
    private ToolItem _selectedToolItem;

    [ObservableProperty]
    private RectangleShape _eraserRect;
    
    [ObservableProperty]
    private RectangleShape _marqueeRect;

    [ObservableProperty]
    private double _gridSpacing = 15;

    [ObservableProperty]
    private GridType _gridType = GridType.Dots;

    [ObservableProperty]
    private Cursor _currentCursor = Cursor.Default;

    [ObservableProperty]
    private bool _isToolSettingsOpen;

    private ICanvasTool CurrentTool => SelectedToolItem.Tool;
    public bool IsSelectionToolSelected => SelectedToolItem.Tool is SelectionTool;

    public DrawingPageViewModel()
    {
        var pointShapeSettings = new PointShapeToolSettings();
        var lineShapeSettings = new LineShapeToolSettings();
        var shapeSettings = new ShapeToolSettings();
        var polylineShapeSettings = new PolylineShapeToolSettings();
        var textShapeSettings = new TextShapeToolSettings();
        
        PageName = ApplicationPageNames.Drawing;
        Transform = new CanvasTransform();
        SelectionManager = new SelectionManager(Shapes);
        
        Tools =
        [
            new ToolItem
            {
                Name = "Select",
                IconPath = Icons.Cursor,
                Tool = new SelectionTool(this, SelectionManager),
                Shortcut = new KeyGesture(Key.V)
            },
            new ToolItem
            {
                Name = "Pan",
                IconPath = Icons.HandGrabbing,
                Tool = new PanTool(this),
                Shortcut = new KeyGesture(Key.H)
            },
            new ToolItem
            {
                Name = "Point",
                IconPath = Icons.Point,
                Tool = new PointShapeTool(this, pointShapeSettings),
                ToolSettings = pointShapeSettings,
                Shortcut = new KeyGesture(Key.P)
            },
            new ToolItem
            {
                Name = "Line",
                IconPath = Icons.Line,
                Tool = new LineShapeTool(this, lineShapeSettings),
                ToolSettings = lineShapeSettings,
                Shortcut = new KeyGesture(Key.L)
            },
            new ToolItem
            {
                Name = "Free drawing",
                IconPath = Icons.Pen,
                Tool = new PolylineShapeTool(this, polylineShapeSettings),
                ToolSettings = polylineShapeSettings,
                Shortcut = new KeyGesture(Key.D)
            },
            new ToolItem
            {
                Name = "Shape",
                IconPath = Icons.Shapes,
                Tool = new ShapeTool(this, shapeSettings),
                ToolSettings = shapeSettings,
                Shortcut = new KeyGesture(Key.S)
            },
            new ToolItem
            {
                Name = "Text",
                IconPath = Icons.TextT,
                Tool = new TextShapeTool(this, textShapeSettings),
                ToolSettings = textShapeSettings,
                Shortcut = new KeyGesture(Key.T)
            },
            new ToolItem
            {
                Name = "Eraser",
                IconPath = Icons.Eraser,
                Tool = new EraserTool(this),
                Shortcut = new KeyGesture(Key.E)
            },
        ];

        ResetToolSelection();
    }

    public CanvasTransform Transform { get; }
    public SelectionManager SelectionManager { get; }
    public ObservableCollection<ToolItem> Tools { get; }

    public void OnPointerPressed(Point screenPoint, KeyModifiers keyModifiers)
    {
        CurrentTool.OnPointerPressed(screenPoint, keyModifiers);
    }

    public void OnPointerMoved(Point screenPoint)
    {
        CurrentTool.OnPointerMoved(screenPoint);
    }

    public void OnPointerReleased(Point screenPoint)
    {
        CurrentTool.OnPointerReleased(screenPoint);
    }

    public void OnPointerWheelChanged(Point screenPoint, double delta)
    {
        Zoom(screenPoint, delta);
    }

    private void ResetToolSelection() => SelectedToolItem = Tools[1];

    private void Zoom(Point screenPoint, double delta)
    {
        Transform.ZoomAt(screenPoint, delta);
    }

    private Point GetViewportCenter() => new(ViewportWidth / 2, ViewportHeight / 2);

    [RelayCommand]
    private void ClearAll() => Shapes.Clear();

    [RelayCommand]
    private async Task OpenOptions()
    {
        var result = await WeakReferenceMessenger
            .Default
            .Send(new ShowCanvasSettingsDialogMessage(GridSpacing, GridType)).Tcs.Task;

        if (result is not null)
        {
            GridSpacing = result.GridSpacing;
            GridType = result.GridType;
        }
    }

    [RelayCommand]
    private void ResetView() => Transform.Reset();

    [RelayCommand]
    private void ZoomIn() => Zoom(GetViewportCenter(), 1);

    [RelayCommand]
    private void ZoomOut() => Zoom(GetViewportCenter(), 0);

    [RelayCommand]
    private void ResetZoom()
    {
        Transform.ResetZoom(GetViewportCenter());
    }

    [RelayCommand]
    private void RemoveSelectedShapes()
    {
        Shapes
            .Where(x => SelectionManager.IsSelected(x))
            .ToList()
            .ForEach(x => Shapes.Remove(x));
        SelectionManager.Clear();
    }

    [RelayCommand]
    private void SelectAllShapes()
    {
        SelectedToolItem = Tools.First(x => x.Tool is SelectionTool);
        
        SelectionManager.Clear();
        SelectionManager.SelectRange(Shapes);
    }

    partial void OnSelectedToolItemChanging(ToolItem? oldValue, ToolItem newValue)
    {
        oldValue?.Tool.OnDeactivated();
    }

    partial void OnSelectedToolItemChanged(ToolItem value)
    {
        value.Tool.OnActivated();
    }
}